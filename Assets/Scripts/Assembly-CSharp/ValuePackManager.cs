using System;
using UnityEngine;

public class ValuePackManager : MonoBehaviour
{
	private const string DO_NOT_SHOW_AGAIN_FLAG_VALUE = "ValuePackDoNotShowAgainValue";

	private const string TIME_STAMP = "ValuePackTimeStamp";

	private const string TIME_STAMP_FIRST_TIME = "ValuePackTimeStampFirstTime";

	private const string SHOW_COUNTER = "ValuePackShowCount";

	public GameObject store;

	public GameObject characterMainFrame;

	public GameObject selectLevel;

	private static ValuePackManager Instance;

	private static ValuePackManagerRes vpPurchaseOK;

	private void OnEnable()
	{
		Instance = this;
	}

	public static LocationItemInfo GetValuePackInfo(ItemInfo ii)
	{
		return ii as LocationItemInfo;
	}

	public static void Buy(ValuePackManagerRes vpManagerRes)
	{
		vpPurchaseOK = vpManagerRes;
		GUI3DPopupManager.Instance.ShowPopup("Processing");
		LocationItemInfo valuePackInfo = GetValuePackInfo(Store.Instance.GetItem(1012));
		Store.Instance.Purchase(valuePackInfo.Id, onPurchase);
	}

	public static void Buy()
	{
		Buy(null);
	}

	private static void onPurchase(Store.PurchaseResult res, string error)
	{
		switch (res)
		{
		case Store.PurchaseResult.Ok:
			Unlock();
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
			if (vpPurchaseOK != null)
			{
				vpPurchaseOK();
			}
			break;
		case Store.PurchaseResult.Error:
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
			if (error != null)
			{
				error = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "OperationFail", "!BAD_TEXT!");
				GUI3DPopupManager.Instance.ShowPopup("Error", error, "Error");
			}
			break;
		default:
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
			break;
		}
	}

	public static void Unlock()
	{
		StatsManager.LogEvent(StatVar.BUY_VALUE_PACK_BY_GROUP, "0");
		int id = Store.Instance.GetItem(1004).Id;
		Store.Instance.Purchase(id);
		Store.Instance.Purchase(Store.Instance.GetItem(28).Id);
		PlayerAccount.Instance.SelectAvatar(Store.Instance.GetItem(28));
		PlayerAccount.Instance.SelectChapter((LocationItemInfo)Store.Instance.GetItem(1004));
		PlayerPrefsWrapper.AddItem(Store.Instance.GetItem(121), 3);
		PlayerPrefsWrapper.AddItem(Store.Instance.GetItem(107), 3);
		PlayerPrefsWrapper.AddItem(Store.Instance.GetItem(122), 3);
		PlayerPrefsWrapper.AddItem(Store.Instance.GetItem(120), 3);
		if (Instance.store != null)
		{
			GUI3DPageSlider[] componentsInChildren = Instance.store.GetComponentsInChildren<GUI3DPageSlider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (string.Compare(componentsInChildren[i].transform.parent.gameObject.name, "Content1") == 0 || string.Compare(componentsInChildren[i].transform.parent.gameObject.name, "Content4") == 0)
				{
					PopulatePageSlider component = componentsInChildren[i].gameObject.GetComponent<PopulatePageSlider>();
					component.Clear(true);
					component.Populate();
				}
			}
		}
		if (Instance.characterMainFrame != null)
		{
			GUI3DPageSlider[] componentsInChildren2 = Instance.characterMainFrame.GetComponentsInChildren<GUI3DPageSlider>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				PopulatePageSlider component2 = componentsInChildren2[j].gameObject.GetComponent<PopulatePageSlider>();
				component2.Clear(true);
				component2.Populate();
			}
		}
		if (Instance.selectLevel != null)
		{
			GUI3DPageSlider[] componentsInChildren3 = Instance.selectLevel.GetComponentsInChildren<GUI3DPageSlider>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				PopulatePageSlider component3 = componentsInChildren3[k].gameObject.GetComponent<PopulatePageSlider>();
				component3.Clear(true);
				component3.Populate();
			}
		}
	}

	private static void OnValuePackBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				GUI3DPopupManager.Instance.ShowPopup("Error", "Internet connection unavailable", "Error");
			}
			else
			{
				Buy();
			}
		}
		else if (DoNotShowAgainCheckbox.DoNotShowAgainValue)
		{
			PlayerPrefs.SetInt("ValuePackDoNotShowAgainValue", 1);
		}
	}

	public static bool ShowValuePackPopup()
	{
		if (IsShowable())
		{
			DateTime now = DateTime.Now;
			string text = PlayerPrefs.GetString("ValuePackTimeStamp", string.Empty);
			if (text == string.Empty)
			{
				PlayerPrefs.SetString("ValuePackTimeStampFirstTime", StringUtil.FromDateToString(now));
			}
			PlayerPrefs.SetString("ValuePackTimeStamp", StringUtil.FromDateToString(now));
			LocationItemInfo valuePackInfo = GetValuePackInfo(Store.Instance.GetItem(1012));
			if (valuePackInfo != null)
			{
				int num = PlayerPrefs.GetInt("ValuePackShowCount", 0);
				PlayerPrefs.SetInt("ValuePackShowCount", num + 1);
				string price = ((!ConfigParams.IsKongregate()) ? valuePackInfo.PriceDollars : valuePackInfo.PriceKreds);
				GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyValuePackMenuPopup", valuePackInfo.Description, valuePackInfo.Name, valuePackInfo.Picture, price, OnValuePackBuy);
				return true;
			}
		}
		return false;
	}

	public static bool IsShowable()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			return false;
		}
		if (PlayerPrefs.GetInt("ValuePackDoNotShowAgainValue", 0) == 1)
		{
			return false;
		}
		string text = PlayerPrefs.GetString("ValuePackTimeStamp", string.Empty);
		if (text != string.Empty)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = StringUtil.FromStringToDate(text);
			if (dateTime.Day == now.Day || dateTime.Month == now.Month || dateTime.Year == now.Year)
			{
				return false;
			}
		}
		LocationItemInfo valuePackInfo = GetValuePackInfo(Store.Instance.GetItem(1012));
		if (valuePackInfo != null && valuePackInfo.Purchased)
		{
			return false;
		}
		return true;
	}
}
