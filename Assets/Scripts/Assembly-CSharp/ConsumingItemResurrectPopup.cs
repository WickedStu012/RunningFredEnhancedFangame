using UnityEngine;

public class ConsumingItemResurrectPopup : MonoBehaviour
{
	public GUI3DText consLeftCount;

	public GUI3DButton OkButton;

	private ItemInfo ii;

	private GUI3DPopFrontTransition transition;

	private GUI3DText okText;

	private void Awake()
	{
		if (OkButton != null)
		{
			okText = OkButton.GetComponentInChildren<GUI3DText>();
		}
		transition = GetComponent<GUI3DTransition>() as GUI3DPopFrontTransition;
		transition.TransitionStartEvent += onTransitionStart;
	}

	private void OnDestroy()
	{
		if ((bool)transition)
		{
			transition.TransitionStartEvent -= onTransitionStart;
		}
	}

	private void onTransitionStart(GUI3DOnTransitionStartEvent evt)
	{
		if (transition.CurrentState == GUI3DTransition.States.Intro)
		{
			updateUsesLeft();
		}
	}

	private void updateUsesLeft()
	{
		if (ii == null)
		{
			ii = Store.Instance.GetItem(120);
		}
		if (ii.Count > 0)
		{
			consLeftCount.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "UsesLeft", "!BAD_TEXT!"), ii.Count));
			okText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Yes", "!BAD_TEXT!"));
		}
		else
		{
			consLeftCount.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "UsesLeft", "!BAD_TEXT!"), "None"));
			okText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Yes", "!BAD_TEXT!"));
		}
	}
}
