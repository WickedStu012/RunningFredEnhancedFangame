using System;
using System.Collections.Generic;
using UnityEngine;

public class TwitterAndroid
{
	private static AndroidJavaObject _plugin;
	private static bool _initialized = false;

	static TwitterAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.TwitterPlugin"))
			{
				_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
				_initialized = _plugin != null;
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Failed to initialize TwitterAndroid: {e.Message}");
			_plugin = null;
			_initialized = false;
		}
	}

	public static void init(string consumerKey, string consumerSecret)
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _plugin != null)
		{
			try
			{
				_plugin.Call("init", consumerKey, consumerSecret);
			}
			catch (Exception e)
			{
				Debug.LogWarning($"TwitterAndroid.init failed: {e.Message}");
			}
		}
	}

	public static bool isLoggedIn()
	{
		if (Application.platform != RuntimePlatform.Android || !_initialized || _plugin == null)
		{
			return false;
		}
		
		try
		{
			return _plugin.Call<bool>("isLoggedIn", new object[0]);
		}
		catch (Exception e)
		{
			Debug.LogWarning($"TwitterAndroid.isLoggedIn failed: {e.Message}");
			return false;
		}
	}

	public static void showLoginDialog()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showLoginDialog");
		}
	}

	public static void logout()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("logout");
		}
	}

	public static string getUsername()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return _plugin.Call<string>("username", new object[0]);
	}

	public static void postUpdate(string update)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("postUpdate", update);
		}
	}

	public static void postUpdateWithImage(string update, byte[] image)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("postUpdateWithImage", update, image);
		}
	}

	public static void getHomeTimeline()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("getHomeTimeline");
		}
	}

	public static void getFollowers()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("getFollowers");
		}
	}

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			if (parameters != null)
			{
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					array[0] = new AndroidJavaObject("java.lang.String", parameter.Key);
					array[1] = new AndroidJavaObject("java.lang.String", parameter.Value);
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
			_plugin.Call("performRequest", methodType, path, androidJavaObject);
		}
	}
}
