using UnityEngine;

public class ItemButtonSelectScreen : MonoBehaviour
{
	private GUI3DButton button;

	private PageSliderItem item;

	private ShopItem shopItem;

	private void OnEnable()
	{
		if (item == null)
		{
			item = GetComponent<PageSliderItem>();
		}
		if (shopItem == null)
		{
			shopItem = GetComponent<ShopItem>();
		}
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		if (button != null)
		{
			button.ClickEvent += OnClick;
		}
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		if (button != null)
		{
			button.ClickEvent -= OnClick;
		}
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		if (item.Item.Purchased)
		{
			updatePageSliderWithSelectedLevel();
		}
		else
		{
			if (!(shopItem != null))
			{
				return;
			}
			if (shopItem.Item.Purchased && !shopItem.Item.Consumable)
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", shopItem.Item.Description, shopItem.Item.Name, shopItem.Item.Picture, shopItem.Item);
			}
			else if (shopItem.Item.Enabled)
			{
				if (!(shopItem.Item is MarketItemInfo))
				{
					OnSale currentSale = Store.Instance.GetCurrentSale();
					if (currentSale == null || currentSale.ItemId != shopItem.Item.Id)
					{
						if (!shopItem.Item.Consumable)
						{
							if (shopItem.Item.Id == 1012 || shopItem.Item.Id == 28)
							{
								GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyValuePack", shopItem.Item.Description, shopItem.Item.Name, shopItem.Item.Picture, shopItem.Item.PriceDollars, OnShopPackItemBuy);
							}
							else if (shopItem.Item.Id == 1014 || shopItem.Item.Id == 31)
							{
								GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyValuePack2", shopItem.Item.Description, shopItem.Item.Name, shopItem.Item.Picture, shopItem.Item.PriceDollars, OnShopPackItemBuy2);
							}
							else
							{
								GUI3DPopupManager.Instance.ShowPopup("ShopItemBuy", shopItem.Item.Description, shopItem.Item.Name, shopItem.Item.Picture, OnShopItemBuy);
							}
							BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
							if (!(buyItemPopup != null))
							{
								return;
							}
							if (shopItem.Item.Id == 30)
							{
								if (PlayerAccount.Instance.GetGrimmyIdolPickedCount() >= ConfigParams.IronFredGrimmyGoal)
								{
									buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Select", "!BAD_TEXT!"));
								}
								else
								{
									buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Ok", "!BAD_TEXT!"));
								}
								return;
							}
							buyItemPopup.Price.SetDynamicText(shopItem.Price.Text);
							if (buyItemPopup.YourSkullies != null)
							{
								buyItemPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
							}
							if (shopItem.Item.Upgradeable > 0)
							{
								buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Upgrade", "!BAD_TEXT!"));
							}
							else
							{
								buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
							}
						}
						else
						{
							GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyConsumable", shopItem.Item.Description, shopItem.Item.Name, shopItem.Item.Picture, OnShopItemBuy);
							BuyItemPopup buyItemPopup2 = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
							if (buyItemPopup2 != null)
							{
								buyItemPopup2.Price.SetDynamicText(shopItem.Item.Price.ToString());
								buyItemPopup2.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
								buyItemPopup2.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
							}
						}
						return;
					}
					GUI3DPopupManager.Instance.ShowPopup("DailyOffer", shopItem.Item.Description, shopItem.Item.Name, shopItem.Item.Picture, OnShopItemBuy);
					DailyOfferPopup dailyOfferPopup = (DailyOfferPopup)GUI3DPopupManager.Instance.CurrentPopup;
					if (dailyOfferPopup != null)
					{
						dailyOfferPopup.Discount.SetDynamicText(100f - currentSale.Discount * 100f + "%");
						dailyOfferPopup.Off.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Off", "!BAD_TEXT!"));
						dailyOfferPopup.Price.SetDynamicText(shopItem.Price.Text);
						if (PlayerAccount.Instance != null)
						{
							dailyOfferPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
						}
						if (shopItem.Item.Upgradeable > 0)
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
					if (shopItem.Item.Tag == "OpenURL")
					{
						GUI3DPopupManager.Instance.ShowPopup("Confirmation", shopItem.Item.Description, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetFreeSkullies", "!BAD_TEXT!"), shopItem.Item.Picture, OnShopItemBuy);
						GUI3DPopupManager.Instance.CurrentPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "LikeIt", "!BAD_TEXT!"));
						GUI3DPopupManager.Instance.CurrentPopup.SetCancelText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoThanks", "!BAD_TEXT!"));
					}
					else if (shopItem.Item.Tag == "Tweeter")
					{
						GUI3DPopupManager.Instance.ShowPopup("Confirmation", shopItem.Item.Description, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetFreeSkullies", "!BAD_TEXT!"), shopItem.Item.Picture, OnShopItemBuy);
						GUI3DPopupManager.Instance.CurrentPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Sure", "!BAD_TEXT!"));
						GUI3DPopupManager.Instance.CurrentPopup.SetCancelText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoThanks", "!BAD_TEXT!"));
					}
					else
					{
						GUI3DPopupManager.Instance.ShowPopup("Processing");
						Store.Instance.Purchase(shopItem.Item.Id, OnPurchase);
					}
				}
				else
				{
					GUI3DPopupManager.Instance.ShowPopup("Error", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoInternet", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Error", "!BAD_TEXT!"));
				}
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "ComingSoon", "!BAD_TEXT!"), shopItem.Item.Name, shopItem.Item.Picture);
			}
		}
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result != GUI3DPopupManager.PopupResult.Yes)
		{
			return;
		}
		if (!(shopItem.Item is MarketItemInfo))
		{
			if (Store.Instance.CheckMoney(shopItem.Item.Id))
			{
				Store.Instance.Purchase(shopItem.Item.Id);
				CharProps props = CharHelper.GetProps();
				if (string.Compare(shopItem.Item.Type, "avatar", true) == 0)
				{
					PlayerAccount.Instance.SelectAvatar(shopItem.Item);
				}
				if (props != null)
				{
					switch (shopItem.Item.Id)
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
				updatePageSliderWithSelectedLevel();
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			GUI3DPopupManager.Instance.ShowPopup("Processing");
			if (!Store.Instance.Purchase(shopItem.Item.Id, OnPurchase))
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("Error", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NoInternet", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Error", "!BAD_TEXT!"));
		}
	}

	private void OnShopPackItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			ValuePackManager.Buy(onValuePackBuyed);
		}
	}

	private void OnShopPackItemBuy2(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			ValuePack2Manager.Buy(onValuePackBuyed);
		}
	}

	private void onValuePackBuyed()
	{
		updatePageSliderWithSelectedLevel();
	}

	private void updatePageSliderWithSelectedLevel()
	{
		if (item.Item.Id == 1012)
		{
			item.Item = Store.Instance.GetItem(1004);
		}
		else if (item.Item.Id == 1014)
		{
			item.Item = Store.Instance.GetItem(1005);
		}
		PlayerAccount.Instance.SelectChapter((LocationItemInfo)item.Item);
		PlayerAccount.Instance.SelectLevel(1);
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			GUI3D gUI3DByName = GUI3DManager.Instance.GetGUI3DByName("SelectChapterAdventureEx");
			SelectChapterAdventureEx component = gUI3DByName.GetComponent<SelectChapterAdventureEx>();
			if (component != null)
			{
				GUI3DObject picture = component.Picture;
				picture.ObjectSize = Vector2.zero;
				picture.TextureName = item.Item.Picture;
				picture.CreateOwnMesh = true;
				picture.CreateMesh();
				component.Title.SetDynamicText(item.Item.Name);
				component.Description.SetDynamicText(item.Item.Description);
				component.CurrentLevel.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Level2", "!BAD_TEXT!"), PlayerAccount.Instance.CurrentLevelNum));
			}
			return;
		}
		GUI3D gUI3DByName2 = GUI3DManager.Instance.GetGUI3DByName("SelectChapterSurvivalEx");
		SelectChapterSurvivalEx component2 = gUI3DByName2.GetComponent<SelectChapterSurvivalEx>();
		if (component2 != null)
		{
			GUI3DObject picture2 = component2.Picture;
			picture2.ObjectSize = Vector2.zero;
			picture2.TextureName = item.Item.Picture;
			picture2.CreateOwnMesh = true;
			picture2.CreateMesh();
			component2.Title.SetDynamicText(item.Item.Name);
			component2.Description.SetDynamicText(item.Item.Description);
		}
		switch (PlayerAccount.Instance.CurrentLevelNum)
		{
		case 1:
			component2.DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Easy", "!BAD_TEXT!"));
			component2.DifficultyButtonIcon.TextureName = "SurvivalEasy";
			break;
		case 2:
			component2.DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Normal", "!BAD_TEXT!"));
			component2.DifficultyButtonIcon.TextureName = "SurvivalNormal";
			break;
		case 3:
			component2.DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Hard", "!BAD_TEXT!"));
			component2.DifficultyButtonIcon.TextureName = "SurvivalHard";
			break;
		case 4:
			component2.DifficultyButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Nightmare", "!BAD_TEXT!"));
			component2.DifficultyButtonIcon.TextureName = "SurvivalHardcore";
			break;
		}
		component2.DifficultyButtonIcon.CreateOwnMesh = true;
		component2.DifficultyButtonIcon.CreateMesh();
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
	}
}
