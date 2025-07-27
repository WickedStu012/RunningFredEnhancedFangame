using UnityEngine;

public class ChallengeItem : MonoBehaviour
{
	public ChallengeItemInfo ItemInfo;

	private GUI3DText text;

	private GUI3DButton button;

	private bool disabled = true;

	private void Start()
	{
		text = GetComponentInChildren<GUI3DText>();
		text.SetDynamicText(ItemInfo.Name);
	}

	public void Refresh()
	{
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.IsChallengeUnlocked(ItemInfo.Id))
		{
			button.StartSegRollOverTexture = "green-hover-left";
			button.EndSegRollOverTexture = "green-hover-right";
			button.RollOverTexture = "green-hover-stretch";
			button.StartSegmentTexName = "green-normal-left";
			button.TextureName = "green-normal-stretch";
			button.EndSegmentTexName = "green-normal-right";
			button.StartSegPressedTexture = "green-down-left";
			button.EndSegPressedTexture = "green-down-right";
			button.PressedTexture = "green-down-stretch";
			disabled = false;
		}
		else if (!Store.Instance.GetItem(ItemInfo.Id).Purchased)
		{
			button.StartSegmentTexName = "disabled-left";
			button.TextureName = "disabled-stretch";
			button.EndSegmentTexName = "disabled-right";
			disabled = true;
		}
		else
		{
			button.StartSegRollOverTexture = "hover-left";
			button.EndSegRollOverTexture = "hover-right";
			button.RollOverTexture = "hover-stretch";
			button.StartSegmentTexName = "normal-left";
			button.TextureName = "normal-stretch";
			button.EndSegmentTexName = "normal-right";
			button.StartSegPressedTexture = "down-left";
			button.EndSegPressedTexture = "down-right";
			button.PressedTexture = "down-stretch";
			disabled = false;
		}
		button.CreateOwnMesh = true;
		button.CreateMesh();
		button.RefreshUVs();
	}

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
		if (disabled)
		{
			string text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "NotCompleteChallenge", "!BAD_TEXT!");
			string title = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "ChallengeLocked", "!BAD_TEXT!");
			GUI3DPopupManager.Instance.ShowPopup("ShopItemBuy", text, title, "ChallengeIcon", OnUnlockAllClose);
			BuyItemPopup buyItemPopup = (BuyItemPopup)GUI3DPopupManager.Instance.CurrentPopup;
			buyItemPopup.Price.SetDynamicText(StringUtil.FormatNumbers(ItemInfo.Price));
			buyItemPopup.YourSkullies.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
			buyItemPopup.SetOkText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "UnlockNow", "!BAD_TEXT!"));
		}
		else
		{
			CameraFade.Instance.FadeOut(OnFadeOut);
		}
	}

	private void OnUnlockAllClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			if (Store.Instance.CheckMoney(ItemInfo.Id))
			{
				Store.Instance.Purchase(ItemInfo.Id);
				CameraFade.Instance.FadeOut(OnFadeOut);
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
			GUI3D gUI3D = GUI3DManager.Instance.Activate("StoreGUI", true, false);
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

	private void OnFadeOut()
	{
		Time.timeScale = 1f;
		SoundManager.StopAll();
		PlayerAccount.Instance.SelectChallenge(ItemInfo.SceneName);
		SceneParamsManager.Instance.SetBool("ChallengeUnlocked", true);
		SceneParamsManager.Instance.SetObject("ChallengeItemInfo", ItemInfo);
		DedalordLoadLevel.LoadLevel("CharCreator");
	}
}
