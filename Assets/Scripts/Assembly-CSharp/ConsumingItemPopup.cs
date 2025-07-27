using UnityEngine;

public class ConsumingItemPopup : MonoBehaviour
{
	private enum State
	{
		NONE = 0,
		SHOWING_OLD = 1,
		SHOWING_LEFT = 2
	}

	public GUI3DText consLeftCount;

	public ShopItemId itemId;

	private State state;

	private ItemInfo ii;

	private GUI3DPopFrontTransition transition;

	private float time;

	private void Awake()
	{
		transition = base.gameObject.GetComponent<GUI3DTransition>() as GUI3DPopFrontTransition;
		transition.TransitionStartEvent += onTransitionStart;
		state = State.NONE;
		if (ii == null)
		{
			ii = Store.Instance.GetItem((int)itemId);
		}
		consLeftCount.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "UsesLeft", "!BAD_TEXT!"), ii.Count));
	}

	private void OnDestroy()
	{
		if (transition != null)
		{
			transition.TransitionStartEvent -= onTransitionStart;
			transition = null;
		}
	}

	private void Update()
	{
		switch (state)
		{
		case State.SHOWING_OLD:
			if (Time.time - time >= 1f)
			{
				if (ii == null)
				{
					ii = Store.Instance.GetItem((int)itemId);
				}
				if (ii.Count > 0)
				{
					Store.Instance.ConsumeItem(ii.Id);
					consLeftCount.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "UsesLeft", "!BAD_TEXT!"), ii.Count));
					state = State.SHOWING_LEFT;
				}
				else
				{
					GetComponent<GUI3DPopup>().Close(GUI3DPopupManager.PopupResult.Yes);
					time = Time.time;
				}
			}
			break;
		case State.SHOWING_LEFT:
			if (Time.time - time >= 2f)
			{
				GetComponent<GUI3DPopup>().Close(GUI3DPopupManager.PopupResult.Yes);
				time = Time.time;
			}
			break;
		}
	}

	private void onTransitionStart(GUI3DOnTransitionStartEvent evt)
	{
		if (transition.CurrentState == GUI3DTransition.States.Intro)
		{
			updateConsumableUsesLeft();
		}
	}

	private void updateConsumableUsesLeft()
	{
		time = Time.time;
		state = State.SHOWING_OLD;
		if (ii == null)
		{
			ii = Store.Instance.GetItem((int)itemId);
		}
		consLeftCount.SetDynamicText(string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "UsesLeft", "!BAD_TEXT!"), ii.Count));
	}
}
