using UnityEngine;

public class SelectAvatarButtonSelectScreen : MonoBehaviour
{
	public string GUIToSwitch;

	private GUI3DButton button;

	private GUI3DPanel panel;

	private PageSliderItem item;

	private void OnEnable()
	{
		if (item == null)
		{
			item = GetComponent<PageSliderItem>();
		}
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		button.ClickEvent += OnClick;
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
			PlayerAccount.Instance.SelectAvatar(item.Item);
			GUI3D gUI3DByName = GUI3DManager.Instance.GetGUI3DByName("SelectAvatarEx");
			SelectAvatarEx component = gUI3DByName.GetComponent<SelectAvatarEx>();
			if (component != null)
			{
				GUI3DObject picture = component.Picture;
				picture.ObjectSize = Vector2.zero;
				picture.TextureName = item.Item.Picture;
				picture.CreateOwnMesh = true;
				picture.CreateMesh();
				component.Title.SetDynamicText(item.Item.Name);
				component.Description.SetDynamicText(item.Item.Description);
			}
		}
		else if (string.Compare(item.Item.CoinType, "grimmies") == 0)
		{
			if (PlayerAccount.Instance.GetGrimmyIdolPickedCount() >= ConfigParams.IronFredGrimmyGoal)
			{
				PlayerAccount.Instance.SelectAvatar(item.Item);
				CloseAvatarSelect();
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemIronFredBuy", OnIronFredPopupClose);
			}
		}
		else if (item.Item.Id == 1018)
		{
			GUI3DPopupManager.Instance.ShowPopup("ShopItemBuyValuePack3", item.Item.Description, item.Item.Name, item.Item.Picture, item.Item.PriceDollars, OnShopPackItemBuy3);
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("ShopItemBuy", item.Item.Description, item.Item.Name, item.Item.Picture, OnShopItemBuy);
			BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
			if (buyItemPopup != null)
			{
				buyItemPopup.Price.SetDynamicText(((ShopItem)item).Price.Text);
				buyItemPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
				buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
			}
		}
	}

	private void CloseAvatarSelect()
	{
		PlayerAccount.Instance.SelectAvatar(item.Item);
		GUI3D gUI3DByName = GUI3DManager.Instance.GetGUI3DByName("SelectAvatarEx");
		SelectAvatarEx component = gUI3DByName.GetComponent<SelectAvatarEx>();
		if (component != null)
		{
			GUI3DObject picture = component.Picture;
			picture.ObjectSize = Vector2.zero;
			picture.TextureName = item.Item.Picture;
			picture.CreateOwnMesh = true;
			picture.CreateMesh();
			component.Title.SetDynamicText(item.Item.Name);
			component.Description.SetDynamicText(item.Item.Description);
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		if (GUIToSwitch != null && GUIToSwitch != string.Empty)
		{
			GUI3DManager.Instance.Activate(GUIToSwitch, true, true);
		}
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result != GUI3DPopupManager.PopupResult.Yes)
		{
			return;
		}
		if (!(item.Item is MarketItemInfo))
		{
			if (Store.Instance.CheckMoney(item.Item.Id))
			{
				Store.Instance.Purchase(item.Item.Id);
				PlayerAccount.Instance.SelectAvatar(item.Item);
				CloseAvatarSelect();
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
		else if (!Store.Instance.Purchase(item.Item.Id))
		{
			GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
		}
	}

	private void OnCloseNotEnoughMoney(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (!GUI3DManager.Instance.IsActive("StoreGUI"))
			{
				SceneParamsManager.Instance.Push("SelectAvatarEx");
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

	private void OnIronFredPopupClose(GUI3DPopupManager.PopupResult result)
	{
	}

	private void OnShopPackItemBuy3(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				GUI3DPopupManager.Instance.ShowPopup("Error", "Internet connection unavailable", "Error");
			}
			else
			{
				ValuePack3Manager.Buy();
			}
		}
	}
}
