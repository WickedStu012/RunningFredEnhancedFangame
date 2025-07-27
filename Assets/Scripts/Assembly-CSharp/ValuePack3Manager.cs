using System;
using UnityEngine;

public class ValuePack3Manager : MonoBehaviour
{
	private const string DO_NOT_SHOW_AGAIN_FLAG_VALUE = "ValuePackDoNotShowAgainValueVP3";

	private const string TIME_STAMP = "ValuePackTimeStampVP3";

	private const string TIME_STAMP_FIRST_TIME = "ValuePackTimeStampFirstTimeVP3";

	private const string SHOW_COUNTER = "ValuePackShowCountVP3";

	public GameObject store;

	public GameObject characterMainFrame;

	private static ValuePack3Manager Instance;

	private static ValuePack3ManagerRes vpPurchaseOK;

	private void OnEnable()
	{
		Instance = this;
	}

	public static AvatarItemInfo GetValuePackInfo(ItemInfo ii)
	{
		return ii as AvatarItemInfo;
	}

	public static void Buy(ValuePack3ManagerRes vpManagerRes)
	{
		vpPurchaseOK = vpManagerRes;
		GUI3DPopupManager.Instance.ShowPopup("Processing");
		ItemInfo item = Store.Instance.GetItem(1018);
		if (item != null)
		{
			Store.Instance.Purchase(item.Id, onPurchase);
		}
		else
		{
			Debug.LogError("Cannot find the product ShopItemId.Value_Pack_3 in the Store");
		}
	}

	public static void Buy()
	{
		Buy(null);
	}

	private static void onPurchase(Store.PurchaseResult res, string error)
	{
		Debug.Log(string.Format("Value Pack3 was purchased. res: {0}", res));
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
		Store.Instance.Purchase(Store.Instance.GetItem(32).Id);
		PlayerAccount.Instance.SelectAvatar(Store.Instance.GetItem(32));
		ItemInfo item = Store.Instance.GetItem(2001);
		PlayerAccount.Instance.AddMoney(item.PackCount);
		if (Instance.store != null)
		{
			GUI3DPageSlider[] componentsInChildren = Instance.store.GetComponentsInChildren<GUI3DPageSlider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (string.Compare(componentsInChildren[i].transform.parent.gameObject.name, "Content1") == 0)
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
		else if (DoNotShowAgainCheckboxVP3.DoNotShowAgainValue)
		{
			PlayerPrefs.SetInt("ValuePackDoNotShowAgainValueVP3", 1);
		}
	}

	public static bool ShowValuePackPopup()
	{
		if (IsShowable())
		{
			DateTime now = DateTime.Now;
			string text = PlayerPrefs.GetString("ValuePackTimeStampVP3", string.Empty);
			if (text == string.Empty)
			{
				PlayerPrefs.SetString("ValuePackTimeStampFirstTimeVP3", StringUtil.FromDateToString(now));
			}
			PlayerPrefs.SetString("ValuePackTimeStampVP3", StringUtil.FromDateToString(now));
			AvatarItemInfo valuePackInfo = GetValuePackInfo(Store.Instance.GetItem(1018));
			if (valuePackInfo != null)
			{
				int num = PlayerPrefs.GetInt("ValuePackShowCountVP3", 0);
				PlayerPrefs.SetInt("ValuePackShowCountVP3", num + 1);
				string price = ((!ConfigParams.IsKongregate()) ? valuePackInfo.PriceDollars : valuePackInfo.PriceKreds);
				GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyValuePackMenuPopup3", valuePackInfo.Description, "Value Pack 3", valuePackInfo.Picture, price, OnValuePackBuy);
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
		if (PlayerPrefs.GetInt("ValuePackDoNotShowAgainValueVP3", 0) == 1)
		{
			return false;
		}
		string text = PlayerPrefs.GetString("ValuePackTimeStampVP3", string.Empty);
		if (text != string.Empty)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = StringUtil.FromStringToDate(text);
			if (dateTime.Day == now.Day || dateTime.Month == now.Month || dateTime.Year == now.Year)
			{
				return false;
			}
		}
		AvatarItemInfo valuePackInfo = GetValuePackInfo(Store.Instance.GetItem(1018));
		if (valuePackInfo != null && valuePackInfo.Purchased)
		{
			return false;
		}
		return true;
	}
}
