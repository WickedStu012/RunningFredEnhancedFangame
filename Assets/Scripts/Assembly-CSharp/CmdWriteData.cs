using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CmdWriteData
{
	private const string RES_OK = "[ok]";
	private const string RES_END = "[/ok]";

	private static UnityWebRequest www;
	private static bool waiting;
	private static float accumTime;
	private static BackendRes beRes;

	public static void WriteData(string token, string username, string dataB64, BackendRes cbfn)
	{
		beRes = cbfn;
		string text = StringUtil.EncodeTo64(username);
		string text2 = StringUtil.EncodeTo64(Hasher.Hash(string.Format("{0}.{1}.{2}", token, text, dataB64)));
		string value = string.Format("{0}.{1}.{2}.{3}", token, text, dataB64, text2);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "wr");
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
			string empty = string.Empty;
			if (www.result != UnityWebRequest.Result.Success)
			{
				beRes(false, "Write operation failed.");
			}
			else
			{
				empty = www.downloadHandler.text;
				int num = empty.IndexOf("[ok]");
				if (num != -1)
				{
					Debug.Log(string.Format("WriteData: OK"));
					beRes(true, string.Empty);
				}
				else
				{
					num = empty.IndexOf("[err]");
					if (num != -1)
					{
						string str = empty.Substring(num + "[err]".Length, empty.IndexOf("[/err]") - num - "[/err]".Length + 1);
						beRes(false, str);
					}
					else
					{
						beRes(false, string.Format("Error parsing the response from server. Response: {0} Error: {1}", empty, www.error));
					}
				}
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
