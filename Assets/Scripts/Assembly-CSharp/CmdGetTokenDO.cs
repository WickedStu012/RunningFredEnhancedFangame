using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CmdGetTokenDO
{
	private const string RES_BEGIN = "[tk]";
	private const string RES_END = "[/tk]";

	private static UnityWebRequest www;
	private static bool waiting;
	private static float accumTime;
	private static BackendRes beRes;

	public static void GetToken(BackendRes cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "gt");
		www = UnityWebRequest.Post("http://running-fred-do.appspot.com/running_fred_do", wWWForm);
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
				if (text != null)
				{
					int num = text.IndexOf("[tk]");
					if (num != -1)
					{
						string str = text.Substring(num + "[tk]".Length, text.IndexOf("[/tk]") - num - "[/tk]".Length + 1);
						beRes(true, str);
					}
					else
					{
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
				}
				else
				{
					beRes(false, "The response is null");
				}
			}
			else
			{
				beRes(false, www.error);
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
				Debug.Log("Timeout");
				waiting = false;
				beRes(false, www.error);
				
				// Properly dispose the UnityWebRequest to prevent memory leaks
				www.Dispose();
				www = null;
			}
		}
	}
}
