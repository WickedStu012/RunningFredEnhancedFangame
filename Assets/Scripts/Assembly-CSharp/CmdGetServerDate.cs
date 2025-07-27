using System.Text;
using UnityEngine;

public class CmdGetServerDate
{
	private const string RES_BEGIN = "[date]";

	private const string RES_END = "[/date]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendRes beRes;

	public static void GetDate(BackendRes cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "getDate");
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
				if (text != null)
				{
					int num = text.IndexOf("[date]");
					if (num != -1)
					{
						string str = text.Substring(num + "[date]".Length, text.IndexOf("[/date]") - num - "[/date]".Length + 1);
						beRes(true, str);
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
