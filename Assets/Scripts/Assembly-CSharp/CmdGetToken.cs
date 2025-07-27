using System.Text;
using UnityEngine;

public class CmdGetToken
{
	private const string RES_BEGIN = "[tk]";

	private const string RES_END = "[/tk]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendRes beRes;

	public static void GetToken(BackendRes cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "gt");
		www = new WWW("http://running-fred.appspot.com/running_fred", wWWForm);
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
				int num = text.IndexOf("[tk]");
				if (num != -1)
				{
					string str = text.Substring(num + "[tk]".Length, text.IndexOf("[/tk]") - num - "[/tk]".Length + 1);
					beRes(true, str);
					return;
				}
				num = text.IndexOf("[err]");
				if (num != -1)
				{
					string str2 = text.Substring(num + "[err]".Length, text.IndexOf("[/err]") - num - "[/err]".Length + 1);
					beRes(false, str2);
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
				Debug.Log("Timeout");
				waiting = false;
				beRes(false, "Timeout");
			}
		}
	}
}
