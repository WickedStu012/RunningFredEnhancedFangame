using System;
using System.Collections.Generic;
using UnityEngine;

public class FlurryAndroid
{
	private static AndroidJavaClass _flurryAgent;
	private static AndroidJavaObject _plugin;
	private static bool _initialized = false;

	static FlurryAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		
		try
		{
			Debug.Log("FlurryAndroid - Initializing");
			_flurryAgent = new AndroidJavaClass("com.flurry.android.FlurryAgent");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FlurryPlugin"))
			{
				if (androidJavaClass != null)
				{
					_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
					_initialized = _plugin != null;
				}
				else
				{
					Debug.LogWarning("Flurry plugin class is null");
					_initialized = false;
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Failed to initialize FlurryAndroid: {e.Message}");
			_flurryAgent = null;
			_plugin = null;
			_initialized = false;
		}
	}

	public static void onStartSession(string apiKey)
	{
		if (Application.platform != RuntimePlatform.Android || !_initialized)
		{
			Debug.Log("Flurry not available on this platform or not initialized");
			return;
		}
		
		try
		{
			if (_flurryAgent == null)
			{
				_flurryAgent = new AndroidJavaClass("com.flurry.android.FlurryAgent");
			}
			
			if (_plugin == null)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FlurryPlugin"))
				{
					if (androidJavaClass != null)
					{
						_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
					}
				}
			}
			
			if (_flurryAgent == null)
			{
				Debug.LogWarning("FlurryAgent is null");
				return;
			}
			if (_plugin == null)
			{
				Debug.LogWarning("Flurry plugin is null");
				return;
			}
			
			Debug.Log("Starting Flurry session");
			_plugin.Call("onStartSession", apiKey);
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Failed to start Flurry session: {e.Message}");
		}
	}

	public static void onEndSession()
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _plugin != null)
		{
			try
			{
				_plugin.Call("onEndSession");
			}
			catch (Exception e)
			{
				Debug.LogWarning($"Failed to end Flurry session: {e.Message}");
			}
		}
	}

	public static void setContinueSessionMillis(long milliseconds)
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _flurryAgent != null)
		{
			try
			{
				_flurryAgent.CallStatic("setContinueSessionMillis", milliseconds);
			}
			catch (Exception e)
			{
				Debug.LogWarning($"Failed to set Flurry continue session millis: {e.Message}");
			}
		}
	}

	public static void logEvent(string eventName)
	{
		logEvent(eventName, false);
	}

	public static void logEvent(string eventName, bool isTimed)
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _plugin != null)
		{
			try
			{
				if (isTimed)
				{
					_plugin.Call("logTimedEvent", eventName);
				}
				else
				{
					_plugin.Call("logEvent", eventName);
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning($"Failed to log Flurry event '{eventName}': {e.Message}");
			}
		}
	}

	public static void logEvent(string eventName, Dictionary<string, string> parameters)
	{
		logEvent(eventName, parameters, false);
	}

	public static void logEvent(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap"))
		{
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] array = new object[2];
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", parameter.Key))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", parameter.Value))
					{
						array[0] = androidJavaObject2;
						array[1] = androidJavaObject3;
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			if (isTimed)
			{
				_plugin.Call("logTimedEventWithParams", eventName, androidJavaObject);
			}
			else
			{
				_plugin.Call("logEventWithParams", eventName, androidJavaObject);
			}
		}
	}

	public static void endTimedEvent(string eventName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("endTimedEvent", eventName);
		}
	}

	public static void onPageView()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("onPageView");
		}
	}

	public static void onError(string errorId, string message, string errorClass)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("onError", errorId, message, errorClass);
		}
	}

	public static void setUserID(string userId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setUserID", userId);
		}
	}

	public static void setAge(int age)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setAge", age);
		}
	}

	public static void setLogEnabled(bool enable)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setLogEnabled", enable);
		}
	}
}
