using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CmdStartupQuery : MonoBehaviour
{
	private const string RES_BEGIN = "[res]";
	private const string RES_END = "[/res]";

	private static UnityWebRequest www;
	private static bool waiting;
	private static float accumTime;
	private static BackendRes beRes;

	public static void GetInfo(BackendRes cbfn)
	{
		beRes = cbfn;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "sq");
		www = UnityWebRequest.Post("https://black-lord.appspot.com/blacklord", wWWForm);
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
					int num = text.IndexOf("[res]");
					if (num != -1)
					{
						string str = text.Substring(num + "[res]".Length, text.IndexOf("[/res]") - num - "[/res]".Length + 1);
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
							beRes(false, string.Format("Error parsing the response from server. Response: {0}", text));
						}
					}
				}
				else
				{
					beRes(false, string.Format("Response with error"));
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
				waiting = false;
				beRes(false, www.error);
				
				// Properly dispose the UnityWebRequest to prevent memory leaks
				www.Dispose();
				www = null;
			}
		}
	}
}
