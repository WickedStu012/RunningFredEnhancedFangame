using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CmdReadData
{
	private const string RES_BEGIN = "[ok]";
	private const string RES_END = "[/ok]";

	private static UnityWebRequest www;
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
		www = UnityWebRequest.Post("http://running-fred.appspot.com/running_fred", wWWForm);
		www.SendWebRequest();
		waiting = true;
		accumTime = 0f;
	}

	public static void Update()
	{
		if (!waiting || www == null)
		{
			return;
		}
		
		if (www.isDone)
		{
			waiting = false;
			if (www.result == UnityWebRequest.Result.Success)
			{
				string text = www.downloadHandler.text;
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
				}
				else
				{
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
			}
			else
			{
				beRes(false, "Read operation failed.");
			}
			
			// Properly dispose the UnityWebRequest to prevent memory leaks
			www.Dispose();
			www = null;
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				waiting = false;
				beRes(false, www.error);
				
				// Properly dispose the UnityWebRequest to prevent memory leaks
				www.Dispose();
				www = null;
			}
		}
	}
}
