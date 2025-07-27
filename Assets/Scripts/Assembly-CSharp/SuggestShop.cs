using UnityEngine;

public class SuggestShop : MonoBehaviour
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
		if (PlayerAccount.Instance.ShowShopSuggestion() && Store.Instance.CanPurchaseItems())
		{
			GUI3DPopupManager.Instance.ShowPopup("ShopItemDescription", MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "SuggestShop_Description", "!BAD_TEXT!"), MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "SuggestShop_Title", "!BAD_TEXT!"), "icon-store", OnClose);
		}
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			GUI3DManager.Instance.SaveCurrentState();
			GUI3DManager.Instance.Activate("StoreGUI", true, true);
		}
	}
}
