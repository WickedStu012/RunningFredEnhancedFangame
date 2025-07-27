using UnityEngine;

public class SuggestBuyLives : MonoBehaviour
{
	public GUI3DTransition Transition;

	private void Awake()
	{
		if (Transition != null)
		{
			Transition.TransitionEndEvent += OnEndTransition;
		}
	}

	private void OnEndTransition(GUI3DOnTransitionEndEvent evt)
	{
		Transition.TransitionEndEvent -= OnEndTransition;
		PlayerAccount.Instance.IncrementDeathCounter();
		if (PlayerAccount.Instance.ShowBuyLivesSuggestion())
		{
			GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "SuggestBuyLives_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "SuggestBuyLives_Title", "!BAD_TEXT!"), "StoreItem_Skill_LifeSlot", OnClose);
		}
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			GUI3DManager.Instance.SaveCurrentState();
			GUI3D gUI3D = GUI3DManager.Instance.Activate("StoreGUI", true, true);
			GUI3DTabControl componentInChildren = gUI3D.GetComponentInChildren<GUI3DTabControl>();
			if (componentInChildren != null)
			{
				componentInChildren.SwitchToTab("Tab2");
			}
			else
			{
				Debug.Log("Can't find TabControl");
			}
		}
	}
}
