using UnityEngine;

public class GrimmyIdolPopup : MonoBehaviour
{
	private enum State
	{
		NONE = 0,
		SHOWING = 1,
		GOING_OUT = 2
	}

	public GUI3DText consLeftCount;

	private State state;

	private GUI3DPopFrontTransition transition;

	private float time;

	private void Awake()
	{
		transition = base.gameObject.GetComponent<GUI3DTransition>() as GUI3DPopFrontTransition;
		transition.TransitionEndEvent += onTransitionEnd;
		state = State.NONE;
	}

	private void OnDestroy()
	{
		if (transition != null)
		{
			transition.TransitionEndEvent -= onTransitionEnd;
		}
	}

	private void Update()
	{
		State state = this.state;
		if (state == State.SHOWING && Time.time - time >= 1f)
		{
			time = Time.time;
			GetComponent<GUI3DPopup>().Close(GUI3DPopupManager.PopupResult.Yes);
			this.state = State.GOING_OUT;
		}
	}

	private void onTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		if (transition.CurrentState == GUI3DTransition.States.Show)
		{
			time = Time.time;
			state = State.SHOWING;
		}
	}
}
