using System.Text;
using UnityEngine;

public class CmdReadData
{
	private const string RES_BEGIN = "[ok]";

	private const string RES_END = "[/ok]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendRes beRes;

	public static void ReadData(string token, string username, BackendRes cbfn)
	{
		beRes = cbfn;
		string arg = StringUtil.EncodeTo64(username);
		string str = string.Format("{0}.{1}", token, arg);
		string arg2 = StringUtil.EncodeTo64(Hasher.Hash(str));
		string value = string.Format("{0}.{1}.{2}", token, arg, arg2);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "rd");
		wWWForm.AddField("dt", value);
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
			if (www.error != null)
			{
				beRes(false, "Read operation failed.");
				return;
			}
			string text = Encoding.ASCII.GetString(www.bytes);
			int num = text.IndexOf("[ok]");
			if (num != -1)
			{
				string text2 = text.Substring(num + "[ok]".Length, text.IndexOf("[/ok]") - num - "[/ok]".Length + 1);
				if (text2.CompareTo("null") != 0)
				{
					beRes(true, text2);
				}
				else
				{
					beRes(true, string.Empty);
				}
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
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				waiting = false;
				beRes(false, www.error);
			}
		}
	}
}
