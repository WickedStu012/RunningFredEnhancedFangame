using System;
using UnityEngine;

public class DailyOfferPopup : BuyItemPopup
{
	public GUI3DText Discount;

	public GUI3DText Off;

	public GUI3DText Ends;

	private OnSale sale;

	private int lastDiff;

	private new void OnEnable()
	{
		sale = Store.Instance.GetCurrentSale();
	}

	private new void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Close(GUI3DPopupManager.PopupResult.Cancel);
		}
		if (!(Ends != null) || sale == null)
		{
			return;
		}
		DateTime dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Utc);
		TimeSpan timeSpan = sale.ToDate - dateTime;
		if (timeSpan.Seconds == lastDiff)
		{
			return;
		}
		if (timeSpan.Hours < 0 || timeSpan.Minutes < 0 || timeSpan.Seconds < 0)
		{
			Ends.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "OfferExpired", "!BAD_TEXT!"));
			return;
		}
		string text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "ExpiresIn", "!BAD_TEXT!");
		string text2 = string.Empty;
		if (timeSpan.Hours < 10)
		{
			text2 += "0";
		}
		text2 = text2 + timeSpan.Hours + ":";
		if (timeSpan.Minutes < 10)
		{
			text2 += "0";
		}
		text2 = text2 + timeSpan.Minutes + ":";
		if (timeSpan.Seconds < 10)
		{
			text2 += "0";
		}
		text2 += timeSpan.Seconds;
		Ends.SetDynamicText(string.Format(text, text2));
		lastDiff = timeSpan.Seconds;
	}
}
