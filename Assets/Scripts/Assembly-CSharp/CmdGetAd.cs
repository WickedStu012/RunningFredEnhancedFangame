using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CmdGetAd : MonoBehaviour
{
	private const string RES_BEGIN = "[res]";
	private const string RES_END = "[/res]";

	private static UnityWebRequest www;
	private static bool waiting;
	private static float accumTime;
	private static BackendRes beRes;

	public static void GetAd(BackendRes cbfn)
	{
		beRes = cbfn;
		string empty = string.Empty;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			empty = "ios";
		else if (Application.platform == RuntimePlatform.Android)
			empty = "and";
		else if (Application.platform == RuntimePlatform.NaCl)
			empty = "cws";
		else if (Application.platform == RuntimePlatform.FlashPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
			empty = "web";
		else
			empty = "unk";
		string arg = StringUtil.EncodeTo64(SystemInfo.deviceUniqueIdentifier);
		string value = string.Format("rf.{0}.{1}.{2}", "180", empty, arg);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("op", "ga");
		wWWForm.AddField("dt", value);
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
						string encodedData = text.Substring(num + "[res]".Length, text.IndexOf("[/res]") - num - "[/res]".Length + 1);
						beRes(true, StringUtil.DecodeFrom64(encodedData));
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
					beRes(false, "The response was null");
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
