using UnityEngine;

public class GoToConsumableInstructions : MonoBehaviour
{
	public string popup;

	private GUI3DButton button;

	private ItemInfo ii;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
		GUI3DPopup currentPopup = GUI3DPopupManager.Instance.CurrentPopup;
		if (string.Compare(currentPopup.Title.Text, "Resurrect", true) == 0)
		{
			currentPopup.Close(GUI3DPopupManager.PopupResult.No);
			ii = Store.Instance.GetItem(120);
			GUI3DPopupManager.Instance.ShowPopup(popup, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_Resurrect_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_Resurrect_Title", "!BAD_TEXT!"), "StoreItem_Consumable_Resurrect", OnShopItemBuy);
		}
		else if (string.Compare(currentPopup.Title.Text, "Afterburner", true) == 0)
		{
			currentPopup.Close(GUI3DPopupManager.PopupResult.No);
			ii = Store.Instance.GetItem(121);
			GUI3DPopupManager.Instance.ShowPopup(popup, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_Afterburner_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_Afterburner_Title", "!BAD_TEXT!"), "Help-AfterBurner-1", OnShopItemBuy);
		}
		else if (string.Compare(currentPopup.Title.Text, "Shield", true) == 0)
		{
			currentPopup.Close(GUI3DPopupManager.PopupResult.No);
			ii = Store.Instance.GetItem(107);
			GUI3DPopupManager.Instance.ShowPopup(popup, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_Shield_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_Shield_Title", "!BAD_TEXT!"), "StoreItem_Consumable_ProtectiveVest", OnShopItemBuy);
		}
		else if (string.Compare(currentPopup.Title.Text, "Safety Spring", true) == 0)
		{
			currentPopup.Close(GUI3DPopupManager.PopupResult.No);
			ii = Store.Instance.GetItem(122);
			GUI3DPopupManager.Instance.ShowPopup(popup, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_SafetySpring_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_SafetySpring_Title", "!BAD_TEXT!"), "StoreItem_Consumable_SafetySpring", OnShopItemBuy);
		}
		else if (string.Compare(currentPopup.Title.Text, "Panic Power", true) == 0)
		{
			currentPopup.Close(GUI3DPopupManager.PopupResult.No);
			ii = Store.Instance.GetItem(104);
			GUI3DPopupManager.Instance.ShowPopup(popup, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_PanicPower_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GoToConsumableInstructions_PanicPower_Title", "!BAD_TEXT!"), "StoreItem_Consumable_PanicPower", OnShopItemBuy);
		}
		else
		{
			currentPopup.Close(GUI3DPopupManager.PopupResult.No);
		}
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result != GUI3DPopupManager.PopupResult.Yes)
		{
			return;
		}
		if (!(ii is MarketItemInfo))
		{
			if (Store.Instance.CheckMoney(ii.Id))
			{
				Store.Instance.Purchase(ii.Id);
				CharProps props = CharHelper.GetProps();
				if (string.Compare(ii.Type, "avatar", true) == 0)
				{
					PlayerAccount.Instance.SelectAvatar(ii);
				}
				if (props != null)
				{
					switch (ii.Id)
					{
					case 112:
						CharHelper.GetProps().ChickenFlaps++;
						break;
					case 102:
						CharHelper.GetProps().MagnetLevel++;
						break;
					case 103:
						CharHelper.GetProps().WallGrip++;
						break;
					case 111:
						CharHelper.GetProps().Lives++;
						break;
					case 101:
						CharHelper.GetProps().SuccesiveJumpCount = 2;
						break;
					case 100:
						CharHelper.GetProps().WallBounce++;
						break;
					}
				}
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			GUI3DPopupManager.Instance.ShowPopup("Processing");
			if (!Store.Instance.Purchase(ii.Id, OnPurchase))
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("Error", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoInternet", "!BAD_TEXT!"), "Error");
		}
	}

	private void OnPurchase(Store.PurchaseResult res, string error)
	{
		switch (res)
		{
		case Store.PurchaseResult.Ok:
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
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

	private void OnCloseNotEnoughMoney(GUI3DPopupManager.PopupResult result)
	{
		if (result != GUI3DPopupManager.PopupResult.Yes)
		{
			return;
		}
		if (!GUI3DManager.Instance.IsActive("StoreGUI"))
		{
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
			{
				SceneParamsManager.Instance.Push("SelectChapterAdventureEx");
			}
			else
			{
				SceneParamsManager.Instance.Push("SelectChapterSurvivalEx");
			}
		}
		GUI3D gUI3D = GUI3DManager.Instance.Activate("StoreGUI", true, true);
		GUI3DTabControl componentInChildren = gUI3D.GetComponentInChildren<GUI3DTabControl>();
		if (componentInChildren != null)
		{
			componentInChildren.SwitchToTab("Tab5");
		}
		else
		{
			Debug.Log("Can't find TabControl");
		}
	}
}
