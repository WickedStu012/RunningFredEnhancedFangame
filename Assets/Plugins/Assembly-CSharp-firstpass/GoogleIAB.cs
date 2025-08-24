using UnityEngine;
using System;

public class GoogleIAB
{
	private static AndroidJavaObject _plugin;
	private static bool _initialized = false;

	static GoogleIAB()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.GoogleIABPlugin"))
			{
				_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
				_initialized = _plugin != null;
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Failed to initialize GoogleIAB: {e.Message}");
			_plugin = null;
			_initialized = false;
		}
	}

	public static void enableLogging(bool shouldEnable)
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _plugin != null)
		{
			try
			{
				if (shouldEnable)
				{
					Debug.LogWarning("YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!");
				}
				_plugin.Call("enableLogging", shouldEnable);
			}
			catch (Exception e)
			{
				Debug.LogWarning($"GoogleIAB.enableLogging failed: {e.Message}");
			}
		}
	}

	public static void setAutoVerifySignatures(bool shouldVerify)
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _plugin != null)
		{
			try
			{
				_plugin.Call("setAutoVerifySignatures", shouldVerify);
			}
			catch (Exception e)
			{
				Debug.LogWarning($"GoogleIAB.setAutoVerifySignatures failed: {e.Message}");
			}
		}
	}

	public static void init(string publicKey)
	{
		if (Application.platform == RuntimePlatform.Android && _initialized && _plugin != null)
		{
			try
			{
				_plugin.Call("init", publicKey);
			}
			catch (Exception e)
			{
				Debug.LogWarning($"GoogleIAB.init failed: {e.Message}");
			}
		}
	}

	public static void unbindService()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("unbindService");
		}
	}

	public static bool areSubscriptionsSupported()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _plugin.Call<bool>("areSubscriptionsSupported", new object[0]);
	}

	public static void queryInventory(string[] skus)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("queryInventory", new object[1] { skus });
		}
	}

	public static void purchaseProduct(string sku)
	{
		purchaseProduct(sku, string.Empty);
	}

	public static void purchaseProduct(string sku, string developerPayload)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("purchaseProduct", sku, developerPayload);
		}
	}

	public static void consumeProduct(string sku)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("consumeProduct", sku);
		}
	}

	public static void consumeProducts(string[] skus)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("consumeProducts", new object[1] { skus });
		}
	}
}
