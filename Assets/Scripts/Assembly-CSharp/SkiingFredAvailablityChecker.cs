using System;
using UnityEngine;

public class SkiingFredAvailablityChecker : MonoBehaviour
{
	private DateTime lastTimeCheck;

	private bool waitingForResponse;

	private void Start()
	{
		int num = PlayerPrefs.GetInt("SkiingFredAvailablityCheckerDate", 0);
		if (num == 0)
		{
			lastTimeCheck = DateTime.MinValue;
		}
		else
		{
			lastTimeCheck = DateUtil.ConvertToDateTime(num);
		}
		if ((DateTime.Now - lastTimeCheck).TotalHours > 24.0)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			checkSkiingFredAvailability();
		}
	}

	private void checkSkiingFredAvailability()
	{
		waitingForResponse = true;
		CmdStartupQuery.GetInfo(onStartupQueryRes);
	}

	private void updateLastTimeCheck()
	{
		lastTimeCheck = DateTime.Now;
		PlayerPrefs.SetInt("SkiingFredAvailablityCheckerDate", DateUtil.ConvertToInt32(lastTimeCheck));
	}

	private void Update()
	{
		if (waitingForResponse)
		{
			CmdStartupQuery.Update();
		}
	}

	private void onStartupQueryRes(bool res, string str)
	{
		waitingForResponse = false;
		if (res)
		{
			if (string.Compare(str, "true") == 0)
			{
				ConfigParams.skiingFredIsAvailable = true;
			}
			else
			{
				ConfigParams.skiingFredIsAvailable = false;
			}
			PlayerPrefsWrapper.SetSkiingFredAvailable(ConfigParams.skiingFredIsAvailable);
		}
		updateLastTimeCheck();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
