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
		
		// For local-only mode, provide a mock token immediately
		bool isLocalMode = Application.platform != RuntimePlatform.Android && 
		                  Application.platform != RuntimePlatform.IPhonePlayer;
		
		if (isLocalMode)
		{
			Debug.Log("CmdGetTokenDO: Local mode - providing mock token");
			string mockToken = $"local_token_{System.DateTime.Now.Ticks % 100000}";
			beRes(true, mockToken);
			return;
		}
		
		try
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("op", "gt");
			
			// Use NetworkSecurityHelper for safe web requests
			www = NetworkSecurityHelper.SafeWebRequest("http://running-fred-do.appspot.com/running_fred_do", "POST");
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
				Debug.LogWarning("CmdGetTokenDO: Failed to create web request, using fallback");
				string fallbackToken = $"fallback_token_{System.DateTime.Now.Ticks % 100000}";
				beRes(true, fallbackToken);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogWarning($"CmdGetTokenDO: Exception during initialization: {e.Message}");
			string exceptionToken = $"exception_token_{System.DateTime.Now.Ticks % 100000}";
			beRes(true, exceptionToken);
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
				Debug.LogWarning($"CmdGetTokenDO: Web request failed: {www.error}, using fallback");
				string errorToken = $"error_token_{System.DateTime.Now.Ticks % 100000}";
				beRes(true, errorToken);
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
				Debug.Log("CmdGetTokenDO: Timeout, using fallback");
				waiting = false;
				string timeoutToken = $"timeout_token_{System.DateTime.Now.Ticks % 100000}";
				beRes(true, timeoutToken);
				
				// Properly dispose the UnityWebRequest to prevent memory leaks
				www.Dispose();
				www = null;
			}
		}
	}
}
