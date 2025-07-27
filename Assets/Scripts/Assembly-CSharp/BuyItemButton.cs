using UnityEngine;

public class BuyItemButton : MonoBehaviour
{
	public GUI3DTransition ShopTransition;

	public ShopItemId ItemId;

	private GUI3DButton button;

	private ItemInfo ii;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		if (button != null)
		{
			button.ReleaseEvent += OnRelease;
		}
		ii = Store.Instance.GetItem((int)ItemId);
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		if (button != null)
		{
			button.ReleaseEvent -= OnRelease;
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (ii.Purchased && !ii.Consumable)
		{
			GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", ii.Description, ii.Name, ii.Picture);
		}
		else if (ii.Enabled)
		{
			if (!(ii is MarketItemInfo))
			{
				OnSale currentSale = Store.Instance.GetCurrentSale();
				if (currentSale == null || currentSale.ItemId != ii.Id)
				{
					if (!ii.Consumable)
					{
						GUI3DPopupManager.Instance.ShowPopup("ShopItemBuy", ii.Description, ii.Name, ii.Picture, OnShopItemBuy);
						BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
						if (buyItemPopup != null)
						{
							Debug.Log(string.Format("Set price to: {0}", ii.GetPrice()));
							buyItemPopup.Price.SetDynamicText(ii.Price.ToString());
							buyItemPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
							if (ii.Upgradeable > 0)
							{
								buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Upgrade", "!BAD_TEXT!"));
							}
							else
							{
								buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
							}
						}
					}
					else
					{
						GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyConsumable", ii.Description, ii.Name, ii.Picture, OnShopItemBuy);
						BuyItemPopup buyItemPopup2 = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
						if (buyItemPopup2 != null)
						{
							buyItemPopup2.Price.SetDynamicText(ii.Price.ToString());
							buyItemPopup2.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
							buyItemPopup2.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
						}
					}
					return;
				}
				GUI3DPopupManager.Instance.ShowPopup("DailyOffer", ii.Description, ii.Name, ii.Picture, OnShopItemBuy);
				DailyOfferPopup dailyOfferPopup = (DailyOfferPopup)GUI3DPopupManager.Instance.CurrentPopup;
				if (dailyOfferPopup != null)
				{
					dailyOfferPopup.Discount.SetDynamicText(100f - currentSale.Discount * 100f + "%");
					dailyOfferPopup.Off.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Off", "!BAD_TEXT!"));
					dailyOfferPopup.Price.SetDynamicText(ii.GetPrice().ToString());
					if (PlayerAccount.Instance != null)
					{
						dailyOfferPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
					}
					if (ii.Upgradeable > 0)
					{
						dailyOfferPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Upgrade", "!BAD_TEXT!"));
					}
					else
					{
						dailyOfferPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
					}
				}
			}
			else if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				if (ii.Tag == "OpenURL")
				{
					GUI3DPopupManager.Instance.ShowPopup("Confirmation", ii.Description, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetFreeSkullies", "!BAD_TEXT!"), ii.Picture, OnShopItemBuy);
					GUI3DPopupManager.Instance.CurrentPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "LikeIt", "!BAD_TEXT!"));
					GUI3DPopupManager.Instance.CurrentPopup.SetCancelText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoThanks", "!BAD_TEXT!"));
				}
				else if (ii.Tag == "Tweeter")
				{
					GUI3DPopupManager.Instance.ShowPopup("Confirmation", ii.Description, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetFreeSkullies", "!BAD_TEXT!"), ii.Picture, OnShopItemBuy);
					GUI3DPopupManager.Instance.CurrentPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Sure", "!BAD_TEXT!"));
					GUI3DPopupManager.Instance.CurrentPopup.SetCancelText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoThanks", "!BAD_TEXT!"));
				}
				else
				{
					GUI3DPopupManager.Instance.ShowPopup("Processing");
					Store.Instance.Purchase(ii.Id, OnPurchase);
				}
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("Error", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoInternet", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Error", "!BAD_TEXT!"));
			}
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "ComingSoon", "!BAD_TEXT!"), ii.Name, ii.Picture);
		}
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (!(ii is MarketItemInfo))
			{
				if (Store.Instance.CheckMoney(ii.Id))
				{
					Store.Instance.Purchase(ii.Id);
					CharProps props = CharHelper.GetProps();
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
				GUI3DPopupManager.Instance.ShowPopup("Error", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoInternet", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Error", "!BAD_TEXT!"));
			}
		}
		else if (BeLordTapJoy.IsReadyToUse)
		{
			TapjoyPlacementsManager.PlacementLoadAndShow("abandon_in_app_purchase");
		}
	}

	private void OnPurchase(Store.PurchaseResult res, string error)
	{
		switch (res)
		{
		case Store.PurchaseResult.Ok:
			Debug.Log("PurchaseOK");
			if (string.Compare(ii.Type, "avatar", true) == 0)
			{
				Debug.Log("Select Avatar");
				PlayerAccount.Instance.SelectAvatar(ii);
			}
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
			break;
		case Store.PurchaseResult.Error:
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
			if (error != null)
			{
				error = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "OperationNotComplete", "!BAD_TEXT!");
				GUI3DPopupManager.Instance.ShowPopup("Error", error, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Error", "!BAD_TEXT!"));
			}
			break;
		default:
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
			break;
		}
	}

	private void OnCloseNotEnoughMoney(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
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
		else if (BeLordTapJoy.IsReadyToUse)
		{
			TapjoyPlacementsManager.PlacementLoadAndShow("insufficient_currency_to_purchase");
		}
	}
}
