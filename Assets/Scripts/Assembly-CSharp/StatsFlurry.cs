using System.Collections.Generic;
using UnityEngine;

public class StatsFlurry : iStatsService
{
	private string apiKey;

	public void SetApiKey(string key)
	{
		apiKey = key;
	}

	public void SessionStart()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			FlurryPlugin._startSession(apiKey);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			FlurryAndroid.onStartSession(apiKey);
		}
	}

	public void SessionEnd()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			FlurryPlugin._endSession();
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			FlurryAndroid.onEndSession();
		}
	}

	public void LogEvent(string sv)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			FlurryPlugin._logEvent(sv);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			FlurryAndroid.logEvent(sv);
		}
	}

	public void LogEvent(string sv, string param1Type, string param1)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			FlurryPlugin._logEventP1(sv, param1Type, param1);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[param1Type] = param1;
			FlurryAndroid.logEvent(sv, dictionary);
		}
	}

	public void LogEvent(string sv, string param1Type, string param1, string param2Type, string param2)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			FlurryPlugin._logEventP2(sv, param1Type, param1, param2Type, param2);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[param1Type] = param1;
			dictionary[param2Type] = param2;
			FlurryAndroid.logEvent(sv, dictionary);
		}
	}

	public void LogEvent(string sv, string param1Type, string param1, string param2Type, string param2, string param3Type, string param3)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			FlurryPlugin._logEventP3(sv, param1Type, param1, param2Type, param2, param3Type, param3);
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[param1Type] = param1;
			dictionary[param2Type] = param2;
			dictionary[param3Type] = param3;
			FlurryAndroid.logEvent(sv, dictionary);
		}
	}
}
