using System;
using UnityEngine;

public class SFFPopupManager : GUI3DPopup
{
	private const string DO_NOT_SHOW_AGAIN_FLAG_VALUE = "SFFPopupDoNotShowAgainValue";

	private const string TIME_STAMP = "SFFPopupTimeStamp";

	private const string TIME_STAMP_FIRST_TIME = "SFFPopupTimeStampFirstTime";

	public const string SHOW_COUNTER = "SFFPopupShowCount";

	public static bool ShowPopupIfNecessary()
	{
		if (PlayerPrefs.GetInt("SFFPopupDoNotShowAgainValue", 0) == 1)
		{
			return false;
		}
		string text = PlayerPrefs.GetString("SFFPopupTimeStamp", string.Empty);
		bool flag = false;
		DateTime now = DateTime.Now;
		if (text != string.Empty)
		{
			DateTime dateTime = StringUtil.FromStringToDate(text);
			if (dateTime.Day != now.Day || dateTime.Month != now.Month || dateTime.Year != now.Year)
			{
				flag = true;
			}
		}
		else
		{
			PlayerPrefs.SetString("SFFPopupTimeStampFirstTime", StringUtil.FromDateToString(now));
			flag = true;
		}
		PlayerPrefs.SetString("SFFPopupTimeStamp", StringUtil.FromDateToString(now));
		if (flag)
		{
			GUI3DPopupManager.Instance.ShowPopup("SuperFallingFredPopup", OnGetItNow);
			return true;
		}
		return false;
	}

	private static void OnGetItNow(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			StatsManager.LogEvent(StatVar.SUPERFALLINGFRED_POPUP, "SHOWED_AND_CLICKED");
			PlayerPrefs.SetInt("SFFPopupDoNotShowAgainValue", 1);
			Application.OpenURL("market://details?id=com.dedalord.superfallingfred");
			return;
		}
		StatsManager.LogEvent(StatVar.SUPERFALLINGFRED_POPUP, "SHOWED_NOT_CLICKED");
		if (SFFPopupDoNotShowAgainCheckbox.DoNotShowAgainValue)
		{
			PlayerPrefs.SetInt("SFFPopupDoNotShowAgainValue", 1);
		}
	}
}
