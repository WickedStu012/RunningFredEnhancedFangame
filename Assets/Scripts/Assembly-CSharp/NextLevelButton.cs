using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
	public GUI3DTransition ActivateTransition;

	private GUI3DButton button;

	private ItemInfo item;

	private int level;

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

	private void Update()
	{
		if (MogaInput.Instance.IsConnected() && GUI3DPopupManager.Instance.CurrentPopup == null && button.enabled && (MogaInput.Instance.GetButtonStartDown() || MogaInput.Instance.GetButtonADown()))
		{
			button.OnRelease();
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		level = PlayerAccount.Instance.CurrentLevelNum;
		item = Store.Instance.GetMandatoryItem(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, level);
		if (item != null)
		{
			ShowBuyPopup();
		}
		else if (level == 10 && PlayerAccount.Instance.IsChapterComplete)
		{
			Debug.Log("SelectNextLocation");
			GUI3DManager.Instance.Activate("SelectNextLocation", true, true);
		}
		else if (level <= 10 && PlayerAccount.Instance.IsLevelUnlocked(level))
		{
			if (ActivateTransition != null)
			{
				ActivateTransition.StartTransition();
			}
			button.enabled = false;
			CameraFade.Instance.FadeOut(OnFadeOut);
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

	private void OnFadeOut()
	{
		button.enabled = true;
		Time.timeScale = 1f;
		SoundManager.StopAll();
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			PlayerAccount.Instance.UnselectChallenge();
		}
		DedalordLoadLevel.LoadLevel("CharCreator");
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
			GUI3DManager.Instance.SaveCurrentState();
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
		item = Store.Instance.GetMandatoryItem(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, level);
		if (item == null)
		{
			if (ActivateTransition != null)
			{
				ActivateTransition.StartTransition();
			}
			CameraFade.Instance.FadeOut(OnFadeOut);
		}
		else
		{
			ShowBuyPopup();
		}
	}
}
