using UnityEngine;

public class SelectDifficultyButtonSelectScreen : MonoBehaviour
{
	public int Level = 1;

	private GUI3DButton button;

	private GUI3DPanel panel;

	private bool disabled;

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
		RefreshButton();
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

	private void RefreshButton()
	{
		if (!(PlayerAccount.Instance != null))
		{
			return;
		}
		if (!PlayerAccount.Instance.IsLevelUnlocked(Level))
		{
			if (button != null)
			{
				button.CancelActions = true;
				button.CheckEvents = true;
				button.StartSegmentTexName = "disabled-left";
				button.TextureName = "disabled-stretch";
				button.EndSegmentTexName = "disabled-right";
				button.RefreshUVs();
				disabled = true;
			}
		}
		else if (button != null)
		{
			button.CancelActions = false;
			button.CheckEvents = true;
			button.StartSegmentTexName = "normal-left";
			button.TextureName = "normal-stretch";
			button.EndSegmentTexName = "normal-right";
			button.RefreshUVs();
			disabled = false;
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (disabled)
		{
			string icon = string.Empty;
			string text = string.Empty;
			string title = string.Empty;
			switch (Level)
			{
			case 2:
				title = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NormalLevel", "!BAD_TEXT!");
				icon = "SurvivalNormal";
				text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NormalLevelDesc", "!BAD_TEXT!");
				item = Store.Instance.GetItem(PlayerAccount.Instance.CurrentChapterInfo.UnlockNormalId);
				break;
			case 3:
				title = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "HardLevel", "!BAD_TEXT!");
				icon = "SurvivalHard";
				text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "HardLevelDesc", "!BAD_TEXT!");
				item = Store.Instance.GetItem(PlayerAccount.Instance.CurrentChapterInfo.UnlockHardId);
				break;
			case 4:
				title = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NightmareLevel", "!BAD_TEXT!");
				icon = "SurvivalHardcore";
				text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NightmareLevelDesc", "!BAD_TEXT!");
				item = Store.Instance.GetItem(PlayerAccount.Instance.CurrentChapterInfo.UnlockNightmareId);
				break;
			}
			GUI3DPopupManager.Instance.ShowPopup("UnlockSurvival", text, title, icon, OnShopItemBuy);
			BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
			buyItemPopup.Price.SetDynamicText(StringUtil.FormatNumbers(item.Price));
		}
		else
		{
			SelectLevel();
		}
	}

	private void SelectLevel()
	{
		PlayerAccount.Instance.UnselectChallenge();
		PlayerAccount.Instance.SelectLevel(Level);
		if (SelectChapterSurvivalEx.Instance != null)
		{
			SelectChapterSurvivalEx.Instance.RefreshSelection();
		}
		GUI3DManager.Instance.Activate("SelectChapterSurvivalEx", true, true);
	}

	private void OnShopItemBuy(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (Store.Instance.CheckMoney(item.Id))
			{
				Store.Instance.Purchase(item.Id);
				PlayerAccount.Instance.UnlockLevel(Level);
				RefreshButton();
				SelectLevel();
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("NotEnoughMoney", OnCloseNotEnoughMoney);
			}
		}
	}

	private void OnCloseNotEnoughMoney(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (!GUI3DManager.Instance.IsActive("StoreGUI"))
			{
				SceneParamsManager.Instance.Push("SelectDifficultyEx");
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
}
