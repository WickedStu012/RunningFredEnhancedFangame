using System.Text;
using UnityEngine;

public class CmdRemoveData
{
	private const string RES_OK = "[ok]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static BackendRes beRes;

	public static void RemoveData(string token, string username, BackendRes cbfn)
	{
		beRes = cbfn;
		string arg = StringUtil.EncodeTo64(username);
		string str = string.Format("{0}.{1}", token, arg);
		string arg2 = StringUtil.EncodeTo64(Hasher.Hash(str));
		string value = string.Format("{0}.{1}.{2}", token, arg, arg2);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "rm");
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
			string text = Encoding.ASCII.GetString(www.bytes);
			int num = text.IndexOf("[ok]");
			if (num != -1)
			{
				beRes(true, string.Empty);
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
