using System;
using UnityEngine;

public class AndroidMarketAccount : IDisposable
{
	private AndroidJavaClass cls_AMGetUserName;

	private string getUserName()
	{
		try
		{
			if (cls_AMGetUserName == null)
			{
				cls_AMGetUserName = new AndroidJavaClass("com.unity3d.Plugin.AMGetUserName");
			}

			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					return cls_AMGetUserName.CallStatic<string>("GetUserName", new object[1] { androidJavaObject });
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Failed to get Android username: {e.Message}");
			return "Player"; // Fallback username
		}
	}

	public static string GetUserName()
	{
		try
		{
			AndroidMarketAccount androidMarketAccount = new AndroidMarketAccount();
			return androidMarketAccount.getUserName();
		}
		catch (Exception e)
		{
			Debug.LogWarning($"AndroidMarketAccount.GetUserName failed: {e.Message}");
			return "Player"; // Fallback username
		}
	}

	public void Dispose()
	{
		if (cls_AMGetUserName != null)
		{
			cls_AMGetUserName.Dispose();
			cls_AMGetUserName = null;
		}
	}
}
