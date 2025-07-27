using UnityEngine;

public class SelectLevelButton : MonoBehaviour
{
	public string GUIToSwitch;

	public int Level = 1;

	public GUI3DText Text;

	public GUI3DObject[] Blocked;

	public GUI3DObject Picture;

	public GUI3DObject Medal;

	private GUI3DButton button;

	private GUI3DPanel panel;

	private GUI3DTransition transition;

	private ItemInfo item;

	private void Awake()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
	}

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponentInChildren<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
		if (Text == null)
		{
			Text = GetComponentInChildren<GUI3DText>();
		}
		if (!(PlayerAccount.Instance != null))
		{
			return;
		}
		if (!PlayerAccount.Instance.IsLevelUnlocked(Level))
		{
			if (button != null)
			{
				button.CheckEvents = false;
				button.StartSegmentTexName = "level-selector-side-disabled";
				button.TextureName = "level-selector-center-disabled";
				button.RefreshUVs();
			}
		}
		else if (button != null)
		{
			button.CheckEvents = true;
			button.StartSegmentTexName = "level-selector-side-normal";
			button.TextureName = "level-selector-center-normal";
			button.RefreshUVs();
		}
		if (Text != null)
		{
			Text.SetDynamicText(Level.ToString());
		}
		if (Picture != null)
		{
			if (Picture.GetComponent<Renderer>() == null)
			{
				Picture.TextureName = PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix + Level;
				Picture.CreateOwnMesh = true;
				Picture.CreateMesh();
			}
			else
			{
				string textureName = PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix + Level;
				Picture.TextureName = textureName;
				Picture.RefreshMaterial(textureName);
				Picture.RefreshUV();
			}
		}
		if (!(Medal != null))
		{
			return;
		}
		switch (PlayerAccount.Instance.GetMedal(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, Level))
		{
		case 1:
			Medal.RefreshMaterial("Bronze2D");
			if (Medal.GetComponent<Renderer>() != null)
			{
				Medal.GetComponent<Renderer>().enabled = true;
			}
			break;
		case 2:
			Medal.RefreshMaterial("Silver2D");
			if (Medal.GetComponent<Renderer>() != null)
			{
				Medal.GetComponent<Renderer>().enabled = true;
			}
			break;
		case 3:
			Medal.RefreshMaterial("Gold2D");
			if (Medal.GetComponent<Renderer>() != null)
			{
				Medal.GetComponent<Renderer>().enabled = true;
			}
			break;
		default:
			if (Medal.GetComponent<Renderer>() != null && Medal.GetComponent<Renderer>() != null)
			{
				Medal.GetComponent<Renderer>().enabled = false;
			}
			break;
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
			button.ReleaseEvent -= OnRelease;
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		item = Store.Instance.GetMandatoryItem(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, Level);
		if (item != null)
		{
			ShowBuyPopup();
		}
		else
		{
			SelectLevel();
		}
	}

	private void ShowBuyPopup()
	{
		GUI3DPopupManager.Instance.ShowPopup("ShopItemBuy", item.MandatoryText, item.Name, item.Picture, OnShopItemBuy);
		BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
		if (buyItemPopup != null)
		{
			buyItemPopup.Price.SetDynamicText(StringUtil.FormatNumbers(item.Price));
			buyItemPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
			if (item.Upgradeable > 0)
			{
				buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Upgrade", "!BAD_TEXT!"));
			}
			else
			{
				buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "GetItNow", "!BAD_TEXT!"));
			}
		}
	}

	private void SelectLevel()
	{
		PlayerAccount.Instance.UnselectChallenge();
		PlayerAccount.Instance.SelectLevel(Level);
		if (panel == null)
		{
			panel = button.GetPanel();
		}
		if (panel != null)
		{
			if (transition == null)
			{
				transition = panel.GetComponent<GUI3DTransition>();
			}
			if (transition != null)
			{
				transition.TransitionEndEvent += OnTransitionEnd;
				transition.StartTransition();
			}
			else
			{
				GUI3DManager.Instance.Activate(GUIToSwitch, true, true);
			}
		}
		if (SelectChapterAdventureEx.Instance != null)
		{
			SelectChapterAdventureEx.Instance.RefreshSelection();
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		transition.TransitionEndEvent -= OnTransitionEnd;
		GUI3DManager.Instance.Activate(GUIToSwitch, true, true);
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result != GUI3DPopupManager.PopupResult.Yes)
		{
			return;
		}
		if (Store.Instance.CheckMoney(item.Id))
		{
			Store.Instance.Purchase(item.Id);
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", item.InstructionsMobile, item.Name, item.Picture, OnCloseDescription);
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", item.InstructionsDesktop, item.Name, item.Picture, OnCloseDescription);
			}
		}
		else
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
				SceneParamsManager.Instance.Push("SelectLevelEx");
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

	private void OnCloseDescription(GUI3DPopupManager.PopupResult result)
	{
		item = Store.Instance.GetMandatoryItem(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, Level);
		if (item == null)
		{
			SelectLevel();
		}
		else
		{
			ShowBuyPopup();
		}
	}
}
