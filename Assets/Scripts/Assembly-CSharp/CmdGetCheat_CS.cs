using System.Text;
using UnityEngine;

public class CmdGetCheat_CS
{
	private const string RES_BEGIN = "[res]";

	private const string RES_END = "[/res]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendGetCheatRes_CS beRes;

	public static void GetCheat(string token, string cheatCode, BackendGetCheatRes_CS cbfn)
	{
		beRes = cbfn;
		string value = string.Format("{0}.{1}.{2}", token, 3, cheatCode);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "gc");
		wWWForm.AddField("dt", value);
		www = new WWW("http://running-fred-cheat-server.appspot.com/running_fred_cheat_server", wWWForm);
		waiting = true;
		accumTime = 0f;
	}

	public static void Update()
	{
		if (!waiting)
		{
			return;
		}
		if (www.isDone)
		{
			waiting = false;
			if (www.error == null)
			{
				string text = Encoding.ASCII.GetString(www.bytes);
				int num = text.IndexOf("[res]");
				if (num != -1)
				{
					string text2 = text.Substring(num + "[res]".Length, text.IndexOf("[/res]") - num - "[/res]".Length + 1);
					string[] array = text2.Split(',');
					if (array != null && array.Length == 3)
					{
						beRes(true, string.Empty, int.Parse(array[0]), int.Parse(array[1]), long.Parse(array[2]));
					}
					else
					{
						beRes(true, "Incorrect number of arguments in the sever response.", 0, 0, 0L);
					}
				}
				else
				{
					num = text.IndexOf("[err]");
					if (num != -1)
					{
						string resStr = text.Substring(num + "[err]".Length, text.IndexOf("[/err]") - num - "[/err]".Length + 1);
						beRes(false, resStr, 0, 0, 0L);
					}
					else
					{
						beRes(false, string.Format("Error parsing the response from server."), 0, 0, 0L);
					}
				}
			}
			else
			{
				beRes(false, "An error has ocurred. Try again later.", 0, 0, 0L);
			}
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				waiting = false;
				beRes(false, "An error has ocurred. Try again later.", 0, 0, 0L);
			}
		}
	}
}
