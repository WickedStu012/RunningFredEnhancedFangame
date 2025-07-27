using UnityEngine;

public class DailyOfferButton : MonoBehaviour
{
	public GUI3DTransition Transition;

	public GUI3DTransition[] MainMenuTransitions;

	private GUI3DButton button;

	private ItemInfo item;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ClickEvent += OnClick;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ClickEvent -= OnClick;
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		ShowOfferPopup(true);
	}

	public void ShowOfferPopup()
	{
		ShowOfferPopup(false);
	}

	public void ShowOfferPopup(bool force)
	{
		OnSale currentSale = Store.Instance.GetCurrentSale();
		if (currentSale == null)
		{
			return;
		}
		item = Store.Instance.GetItem(currentSale.ItemId);
		if (item == null)
		{
			return;
		}
		if (item.Purchased)
		{
			if (force)
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", item.Description, item.Name, item.Picture);
			}
			return;
		}
		GUI3DPopupManager.Instance.ShowPopup("DailyOffer", item.Description, item.Name, item.Picture, OnShopItemBuy);
		DailyOfferPopup dailyOfferPopup = (DailyOfferPopup)GUI3DPopupManager.Instance.CurrentPopup;
		if (!(dailyOfferPopup != null))
		{
			return;
		}
		dailyOfferPopup.Discount.SetDynamicText(currentSale.RealDiscount + "%");
		dailyOfferPopup.Off.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Off", "!BAD_TEXT!"));
		if (PlayerAccount.Instance != null)
		{
			dailyOfferPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
		}
		if (item.Upgradeable > 0)
		{
			switch (item.Upgrades)
			{
			case 0:
				dailyOfferPopup.Price.SetDynamicText(item.Price.ToString());
				break;
			case 1:
				dailyOfferPopup.Price.SetDynamicText(item.Price1.ToString());
				break;
			case 2:
				dailyOfferPopup.Price.SetDynamicText(item.Price2.ToString());
				break;
			case 3:
				dailyOfferPopup.Price.SetDynamicText(item.Price3.ToString());
				break;
			case 4:
				dailyOfferPopup.Price.SetDynamicText(item.Price4.ToString());
				break;
			}
			dailyOfferPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Upgrade", "!BAD_TEXT!"));
		}
		else
		{
			dailyOfferPopup.Price.SetDynamicText(item.Price.ToString());
			dailyOfferPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
		}
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (Store.Instance.CheckMoney(item.Id))
			{
				Store.Instance.Purchase(item.Id);
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
	}

	private void OnCloseNotEnoughMoney(GUI3DPopupManager.PopupResult result)
	{
		if (result != GUI3DPopupManager.PopupResult.Yes)
		{
			return;
		}
		if (GUI3DManager.Instance.IsActive("StoreGUI"))
		{
			ShowStore();
		}
		else if (MainMenuTransitions != null && MainMenuTransitions.Length > 0)
		{
			MainMenuTransitions[0].TransitionEndEvent += OnTransitionEnd;
			GUI3DTransition[] mainMenuTransitions = MainMenuTransitions;
			foreach (GUI3DTransition gUI3DTransition in mainMenuTransitions)
			{
				gUI3DTransition.StartTransition();
			}
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		MainMenuTransitions[0].TransitionEndEvent -= OnTransitionEnd;
		ShowStore();
	}

	private void ShowStore()
	{
		if (!GUI3DManager.Instance.IsActive("StoreGUI"))
		{
			SceneParamsManager.Instance.Push(Levels.MainMenu);
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
