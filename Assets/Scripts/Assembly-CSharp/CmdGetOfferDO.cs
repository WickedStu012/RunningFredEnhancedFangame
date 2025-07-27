using System.Text;
using UnityEngine;

public class CmdGetOfferDO
{
	private const string RES_BEGIN = "[do]";

	private const string RES_END = "[/do]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendRes beRes;

	public static void GetOffer(string token, BackendRes cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "do");
		wWWForm.AddField("tk", token);
		www = new WWW("http://running-fred-do.appspot.com/running_fred_do", wWWForm);
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
					int num = text.IndexOf("[do]");
					if (num != -1)
					{
						string encodedData = text.Substring(num + "[do]".Length, text.IndexOf("[/do]") - num - "[/do]".Length + 1);
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
