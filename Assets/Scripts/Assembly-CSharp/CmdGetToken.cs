using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CmdGetToken
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
		
		// For local-only mode, provide a mock token immediately
		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			Debug.Log("CmdGetToken: Local mode - providing mock token");
			beRes(true, "local_mock_token_12345");
			return;
		}
		
		try
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("op", "gt");
			
			// Use NetworkSecurityHelper for safe web requests
			www = NetworkSecurityHelper.SafeWebRequest("http://running-fred.appspot.com/running_fred", "POST");
			if (www != null)
			{
				www.uploadHandler = new UploadHandlerRaw(wWWForm.data);
				www.downloadHandler = new DownloadHandlerBuffer();
				www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
				www.SendWebRequest();
				waiting = true;
				accumTime = 0f;
			}
			else
			{
				Debug.LogWarning("CmdGetToken: Failed to create web request, using fallback");
				beRes(true, "fallback_token_67890");
			}
		}
		catch (System.Exception e)
		{
			Debug.LogWarning($"CmdGetToken: Exception during initialization: {e.Message}");
			beRes(true, "exception_token_11111");
		}
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
				Debug.LogWarning($"CmdGetToken: Web request failed: {www.error}, using fallback");
				beRes(true, "error_fallback_token_22222");
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
				Debug.Log("CmdGetToken: Timeout, using fallback");
				waiting = false;
				beRes(true, "timeout_fallback_token_33333");
				
				// Properly dispose the UnityWebRequest to prevent memory leaks
				www.Dispose();
				www = null;
			}
		}
	}
}
