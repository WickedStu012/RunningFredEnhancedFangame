using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TwitterBinding
{
#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterInit(string consumerKey, string consumerSecret);
#endif

	public static void init(string consumerKey, string consumerSecret)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterInit(consumerKey, consumerSecret);
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern bool _twitterIsLoggedIn();
#endif

	public static bool isLoggedIn()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterIsLoggedIn();
		}
#endif
		return false;
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern string _twitterLoggedInUsername();
#endif

	public static string loggedInUsername()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterLoggedInUsername();
		}
#endif
		return string.Empty;
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterLogin(string username, string password);
#endif

	public static void login(string username, string password)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterLogin(username, password);
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterShowOauthLoginDialog();
#endif

	public static void showOauthLoginDialog()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterShowOauthLoginDialog();
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterLogout();
#endif

	public static void logout()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterLogout();
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterPostStatusUpdate(string status);
#endif

	public static void postStatusUpdate(string status)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterPostStatusUpdate(status);
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterPostStatusUpdateWithImage(string status, string imagePath);
#endif

	public static void postStatusUpdate(string status, string pathToImage)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterPostStatusUpdateWithImage(status, pathToImage);
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterGetHomeTimeline();
#endif

	public static void getHomeTimeline()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterGetHomeTimeline();
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterPerformRequest(string methodType, string path, string parameters);
#endif

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterPerformRequest(methodType, path, (parameters == null) ? null : parameters.toJson());
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern bool _twitterIsTweetSheetSupported();
#endif

	public static bool isTweetSheetSupported()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterIsTweetSheetSupported();
		}
#endif
		return false;
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern bool _twitterCanUserTweet();
#endif

	public static bool canUserTweet()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _twitterCanUserTweet();
		}
#endif
		return false;
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _twitterShowTweetComposer(string status, string imagePath);
#endif

	public static void showTweetComposer(string status, string pathToImage)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_twitterShowTweetComposer(status, pathToImage);
		}
#endif
	}
}
