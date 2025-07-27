using System;
using UnityEngine;

public class SKFPopupManager : GUI3DPopup
{
	private const string DO_NOT_SHOW_AGAIN_FLAG_VALUE = "SKFPopupDoNotShowAgainValue";

	private const string TIME_STAMP = "SKFPopupTimeStamp";

	private const string TIME_STAMP_FIRST_TIME = "SkFPopupTimeStampFirstTime";

	public const string SHOW_COUNTER = "SKFPopupShowCount";

	public static bool ShowPopupIfNecessary()
	{
		if (PlayerPrefs.GetInt("SKFPopupDoNotShowAgainValue", 0) == 1)
		{
			return false;
		}
		string text = PlayerPrefs.GetString("SKFPopupTimeStamp", string.Empty);
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
			PlayerPrefs.SetString("SkFPopupTimeStampFirstTime", StringUtil.FromDateToString(now));
			flag = true;
		}
		PlayerPrefs.SetString("SKFPopupTimeStamp", StringUtil.FromDateToString(now));
		PlayerPrefs.Save();
		if (flag)
		{
			GUI3DPopupManager.Instance.ShowPopup("SkiingFredPopup", OnGetItNow);
			return true;
		}
		return false;
	}

	private static void OnGetItNow(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			StatsManager.LogEvent(StatVar.SKIINGFRED_POPUP, "SHOWED_AND_CLICKED");
			PlayerPrefs.SetInt("SKFPopupDoNotShowAgainValue", 1);
			Application.OpenURL("market://details?id=com.dedalordnorth.skiingfred");
			return;
		}
		StatsManager.LogEvent(StatVar.SKIINGFRED_POPUP, "SHOWED_NOT_CLICKED");
		if (SFFPopupDoNotShowAgainCheckbox.DoNotShowAgainValue)
		{
			PlayerPrefs.SetInt("SKFPopupDoNotShowAgainValue", 1);
		}
	}
}
