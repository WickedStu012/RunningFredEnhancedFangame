using UnityEngine;

public class PlayButton : MonoBehaviour
{
	public bool Tutorial;

	protected GUI3DButton button;

	private GUI3DPanel panel;

	private GUI3DTransition transition;

	private ItemInfo item;

	protected virtual void OnEnable()
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
		if (button != null)
		{
			button.ReleaseEvent -= OnRelease;
		}
	}

	protected virtual void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (panel == null)
		{
			panel = button.GetPanel();
		}
		if (!(panel != null))
		{
			return;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			item = Store.Instance.GetMandatoryItem(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, PlayerAccount.Instance.CurrentLevelNum);
		}
		if (item != null)
		{
			ShowBuyPopup();
		}
		else if (!Tutorial)
		{
			Tutorial = PlayerPrefsWrapper.GetTutorial();
			if (Tutorial)
			{
				GUI3DPopupManager.Instance.ShowPopup("WantToPlayTutorial", OnTutorialPopupClose);
			}
			else
			{
				Play();
			}
		}
		else
		{
			PlayerAccount.Instance.UnselectChallenge();
			Play();
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

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
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
		else if (string.IsNullOrEmpty(GUI3DPopupManager.Instance.CurrentPopupName()))
		{
			GUI3DManager.Instance.Activate("SelectChapterAdventureEx", true, true);
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
		item = Store.Instance.GetMandatoryItem(PlayerAccount.Instance.CurrentChapterInfo.ScenePrefix, PlayerAccount.Instance.CurrentLevelNum);
		if (item == null)
		{
			Play();
		}
		else
		{
			ShowBuyPopup();
		}
	}

	private void Play()
	{
		StatsManager.LogEvent(StatVar.SELECTED_CHARACTER, PlayerAccount.Instance.CurrentAvatarInfo.Name);
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1000)
			{
				StatsManager.LogEvent(StatVar.START_LEVEL_CASTLE, PlayerAccount.Instance.CurrentAvatarInfo.Name, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1002)
			{
				StatsManager.LogEvent(StatVar.START_LEVEL_CAVES, PlayerAccount.Instance.CurrentAvatarInfo.Name, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1004)
			{
				StatsManager.LogEvent(StatVar.START_LEVEL_ROOFTOP, PlayerAccount.Instance.CurrentAvatarInfo.Name, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
		}
		else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			StatsManager.LogEvent(StatVar.START_CHALLENGE, PlayerAccount.Instance.CurrentChallenge.ToString());
		}
		else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1001)
			{
				StatsManager.LogEvent(StatVar.START_ENDLESS_CASTLE, PlayerAccount.Instance.CurrentAvatarInfo.Name, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1003)
			{
				StatsManager.LogEvent(StatVar.START_ENDLESS_CAVES, PlayerAccount.Instance.CurrentAvatarInfo.Name, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1005)
			{
				StatsManager.LogEvent(StatVar.START_ENDLESS_ROOFTOP, PlayerAccount.Instance.CurrentAvatarInfo.Name, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
		}
		PlayerAccount.Instance.UnselectChallenge();
		CameraFade.Instance.FadeOut(OnFadeOut);
		SceneParamsManager.Instance.GetBool("LaunchCinematic", false, true);
		SceneParamsManager.Instance.SetBool("LaunchCinematic", true);
		if (transition == null)
		{
			transition = panel.GetComponent<GUI3DTransition>();
		}
		if (transition != null)
		{
			SoundManager.FadeOutAll(1f, onFadeOutAll);
			transition.TransitionEndEvent += OnTransitionEnd;
		}
	}

	private void onFadeOutAll()
	{
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		transition.TransitionEndEvent -= OnTransitionEnd;
	}

	private void OnFadeOut()
	{
		if (Tutorial)
		{
			PlayerPrefsWrapper.SetTutorial(true);
			DedalordLoadLevel.LoadLevel("TutorialLoader");
		}
		else
		{
			DedalordLoadLevel.LoadLevel("CharCreator");
		}
	}

	private void OnTutorialPopupClose(GUI3DPopupManager.PopupResult result)
	{
		Tutorial = result == GUI3DPopupManager.PopupResult.Yes;
		PlayerPrefsWrapper.SetTutorial(Tutorial);
		Play();
	}
}
