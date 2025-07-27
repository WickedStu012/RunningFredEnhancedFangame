using System.Text;
using UnityEngine;

public class CmdDeleteAllCheats_CS
{
	private const string RES_BEGIN = "[res]";

	private const string RES_END = "[/res]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendDeleteAllCheatsRes_CS beRes;

	public static void DeleteAllCheats(string token, BackendDeleteAllCheatsRes_CS cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "dc");
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
					string resStr = text.Substring(num + "[res]".Length, text.IndexOf("[/res]") - num - "[/res]".Length + 1);
					beRes(true, resStr);
					return;
				}
				num = text.IndexOf("[err]");
				if (num != -1)
				{
					beRes(false, www.error);
				}
				else
				{
					beRes(false, string.Format("Error parsing the response from server. Response: {0} Error: {1}", text, www.error));
				}
			}
			else
			{
				beRes(false, www.error);
			}
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				waiting = false;
				beRes(false, www.error);
			}
		}
	}
}
