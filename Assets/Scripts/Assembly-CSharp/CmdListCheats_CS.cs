using System.Text;
using UnityEngine;

public class CmdListCheats_CS
{
	private const string RES_BEGIN = "[res]";

	private const string RES_END = "[/res]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendListCheatsRes_CS beRes;

	public static void ListCheats(string token, BackendListCheatsRes_CS cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "lc");
		wWWForm.AddField("tk", token);
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
					if (text2.Length > 0)
					{
						string[] cheats = text2.Split(';');
						beRes(true, string.Empty, cheats);
					}
					else
					{
						beRes(true, "No cheats", null);
					}
				}
				else
				{
					num = text.IndexOf("[err]");
					if (num != -1)
					{
						string resStr = text.Substring(num + "[err]".Length, text.IndexOf("[/err]") - num - "[/err]".Length + 1);
						beRes(false, resStr, null);
					}
					else
					{
						beRes(false, string.Format("Error parsing the response from server. Response: {0} Error: {1}", text, www.error), null);
					}
				}
			}
			else
			{
				beRes(false, www.error, null);
			}
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				waiting = false;
				beRes(false, www.error, null);
			}
		}
	}
}
