using System.Text;
using UnityEngine;

public class blGetToken
{
	private const string RES_BEGIN = "[res]";

	private const string RES_END = "[/res]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BlackLordRes beRes;

	public static void GetToken(BlackLordRes cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "gt");
		www = new WWW("https://black-lord.appspot.com/blacklord", wWWForm);
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
					string str = text.Substring(num + "[res]".Length, text.IndexOf("[/res]") - num - "[/res]".Length + 1);
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
				Debug.Log("BlackLord - Timeout");
				waiting = false;
				beRes(false, "Timeout");
			}
		}
	}
}
