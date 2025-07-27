using System;
using UnityEngine;

public class AndroidMarketAccount : IDisposable
{
	private AndroidJavaClass cls_AMGetUserName = new AndroidJavaClass("com.unity3d.Plugin.AMGetUserName");

	private string getUserName()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				return cls_AMGetUserName.CallStatic<string>("GetUserName", new object[1] { androidJavaObject });
			}
		}
	}

	public static string GetUserName()
	{
		AndroidMarketAccount androidMarketAccount = new AndroidMarketAccount();
		return androidMarketAccount.getUserName();
	}

	public void Dispose()
	{
		cls_AMGetUserName.Dispose();
	}
}
