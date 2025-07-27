using System.Text;
using UnityEngine;

public class CmdGetInfo : MonoBehaviour
{
	private const string RES_BEGIN = "[res]";

	private const string RES_END = "[/res]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendRes beRes;

	public static void GetInfo(BackendRes cbfn)
	{
		beRes = cbfn;
		string empty = string.Empty;
		empty = ((Application.platform == RuntimePlatform.IPhonePlayer) ? "ios" : ((Application.platform == RuntimePlatform.Android) ? "and" : ((Application.platform == RuntimePlatform.NaCl) ? "cws" : ((Application.platform != RuntimePlatform.FlashPlayer && Application.platform != RuntimePlatform.WebGLPlayer) ? "unk" : "web"))));
		string value = string.Format("sfi.rf.{0}.{1}", "180", empty);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "gi");
		wWWForm.AddField("dt", value);
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
				if (text != null)
				{
					int num = text.IndexOf("[res]");
					if (num != -1)
					{
						string encodedData = text.Substring(num + "[res]".Length, text.IndexOf("[/res]") - num - "[/res]".Length + 1);
						beRes(true, StringUtil.DecodeFrom64(encodedData));
						return;
					}
					num = text.IndexOf("[err]");
					if (num != -1)
					{
						string str = text.Substring(num + "[err]".Length, text.IndexOf("[/err]") - num - "[/err]".Length + 1);
						beRes(false, str);
					}
					else
					{
						beRes(false, string.Format("Error parsing the response from server. Response: {0} Error: {1}", text, www.error));
					}
				}
				else
				{
					beRes(false, "The response was null");
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
				beRes(false, www.error);
			}
		}
	}
}
