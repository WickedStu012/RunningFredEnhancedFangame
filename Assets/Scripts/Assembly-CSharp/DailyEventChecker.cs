using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyEventChecker : MonoBehaviour
{
	public float Timer = 2f;

	private int moneyEarned;

	private DailyOfferButton button;

	private void Start()
	{
		button = UnityEngine.Object.FindObjectOfType(typeof(DailyOfferButton)) as DailyOfferButton;
		moneyEarned = 0;
		if (!ConfigParams.isKindle)
		{
			PlayerAccount.CheckTapjoyEarnings();
		}
		if (PlayerAccount.Instance != null && !PlayerAccount.Instance.IsPiggyBankActive())
		{
			int moneyInPiggyBank = PlayerAccount.Instance.GetMoneyInPiggyBank();
			if (moneyInPiggyBank != 0)
			{
				moneyEarned = (int)((float)moneyInPiggyBank * 10f / 100f);
				PlayerAccount.Instance.AddMoney(moneyEarned);
				PlayerAccount.Instance.ClearMoneyInPiggyBank();
			}
		}
	}

	private void Update()
	{
		if (!(PlayerAccount.Instance != null))
		{
			return;
		}
		Timer -= Time.deltaTime;
		if (!(Timer <= 0f))
		{
			return;
		}
		bool flag = false;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			flag = checkAdsSystem();
		}
		bool flag2 = false;
		if (!flag)
		{
			if (ValuePack3Manager.IsShowable())
			{
				flag2 = ValuePack3Manager.ShowValuePackPopup();
			}
			else
			{
				bool flag3 = ValuePackManager.IsShowable();
				bool flag4 = ValuePack2Manager.IsShowable();
				List<int> list = new List<int>();
				if (flag3)
				{
					list.Add(1);
				}
				if (flag4)
				{
					list.Add(2);
				}
				if (list.Count > 0)
				{
					flag2 = ((UnityEngine.Random.Range(0, list.Count) != 0) ? ValuePack2Manager.ShowValuePackPopup() : ValuePackManager.ShowValuePackPopup());
				}
			}
			if (!PlayerAccount.Instance.DailyEarningsChecked && PlayerAccount.Instance.Days > 0)
			{
				PlayerAccount.Instance.DailyEarningsChecked = true;
				if (!flag2)
				{
					if (moneyEarned == 0)
					{
						GUI3DPopupManager.Instance.ShowPopup("DailyIncome", "+" + PlayerAccount.Instance.Earnings, OnClose);
					}
					else
					{
						GUI3DPopupManager.Instance.ShowPopup("DailyIncome", "+" + PlayerAccount.Instance.Earnings);
					}
				}
			}
			if (!flag2 && moneyEarned != 0)
			{
				GUI3DPopupManager.Instance.ShowPopup("PiggyBank", "+" + moneyEarned, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "DailyEventChecker_PiggyBank", "!BAD_TEXT!"), "PiggyBank", OnClose);
			}
		}
		if (!ConfigParams.isKindle && !flag2 && !flag && (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.isEditor))
		{
			bool flag5 = false;
			if (ConfigParams.skiingFredIsAvailable)
			{
				flag5 = SKFPopupManager.ShowPopupIfNecessary();
			}
			if (!flag5)
			{
				SFFPopupManager.ShowPopupIfNecessary();
			}
		}
		if (ConfigParams.showGameDataWrongVersionDialog)
		{
			GUI3DPopupManager.Instance.ShowPopup("GameDataWrongVersion", onGameDataWrongVersionPopupClose);
			ConfigParams.showGameDataWrongVersionDialog = false;
		}
		base.enabled = false;
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		if (Store.Instance != null && Store.Instance.GetCurrentSale() != null && (DateTime.Now.Date - PlayerPrefsWrapper.LastOfferShown()).Days >= 1)
		{
			PlayerPrefsWrapper.SetLastOfferShown();
			button.ShowOfferPopup();
		}
	}

	private static void onGameDataWrongVersionPopupClose(GUI3DPopupManager.PopupResult popupRes)
	{
		if (popupRes == GUI3DPopupManager.PopupResult.Yes)
		{
			Application.OpenURL(RateUs.Instance.URL_Android);
		}
		else
		{
			ConfigParams.useICloud = false;
		}
	}

	private bool checkAdsSystem()
	{
		if (AdManager.Instance == null || AdImageCacheManager.Instance == null || Application.internetReachability == NetworkReachability.NotReachable)
		{
			return false;
		}
		if (AdManager.Instance.IsScheduleCached())
		{
			if (!AdManager.Instance.GetLoadingAdImageFlag())
			{
				AdInfo adFromSchedule = AdManager.Instance.GetAdFromSchedule();
				if (adFromSchedule == null)
				{
					return false;
				}
				if (AdImageCacheManager.Instance.IsImageInCache(adFromSchedule.ImageURL))
				{
					AdManager.Instance.AdWasShowed();
					Material material = AdManager.Instance.GetMaterial();
					material.mainTexture = AdImageCacheManager.Instance.LoadImageFromCache(adFromSchedule.ImageURL);
					GUI3DPopupManager.Instance.ShowPopup("AdPopup");
					return true;
				}
				AdManager.Instance.SetLoadingAdImageFlag(true);
				AdImageCacheManager.Instance.LoadImageFromWeb(adFromSchedule.ImageURL, onAdImageLoaded);
			}
			else
			{
				AdManager.Instance.SetLoadingAdImageFlag(false);
				AdInfo currentAd = AdManager.Instance.GetCurrentAd();
				if (currentAd != null && AdImageCacheManager.Instance.IsImageInCache(currentAd.ImageURL))
				{
					AdManager.Instance.AdWasShowed();
					Material material2 = AdManager.Instance.GetMaterial();
					material2.mainTexture = AdImageCacheManager.Instance.LoadImageFromCache(currentAd.ImageURL);
					GUI3DPopupManager.Instance.ShowPopup("AdPopup");
					return true;
				}
			}
		}
		else
		{
			AdManager.Instance.GetAdScheduleFromWeb(onGetAdRes);
		}
		return false;
	}

	private void onGetAdRes(bool res, string str, List<AdInfo> adSchedule)
	{
		if (res)
		{
			AdInfo adFromSchedule = AdManager.Instance.GetAdFromSchedule();
			if (adFromSchedule != null && !AdImageCacheManager.Instance.IsImageInCache(adFromSchedule.ImageURL))
			{
				AdImageCacheManager.Instance.LoadImageFromWeb(adFromSchedule.ImageURL, onAdImageLoaded);
			}
		}
	}

	private void onAdImageLoaded(bool res, string str, Texture2D tex)
	{
	}
}
