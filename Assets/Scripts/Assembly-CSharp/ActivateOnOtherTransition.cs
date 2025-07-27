using UnityEngine;

public class ActivateOnOtherTransition : MonoBehaviour
{
	public GUI3DTransition OtherTransition;

	public GUI3DTransition Transition;

	private bool addedListener;

	private void Awake()
	{
		if (Transition == null)
		{
			Transition = GetComponent<GUI3DTransition>();
		}
	}

	private void OnEnable()
	{
		if (OtherTransition != null)
		{
			OtherTransition.TransitionStartEvent += OnStartTransition;
			addedListener = true;
		}
	}

	private void OnDisable()
	{
		if (addedListener)
		{
			OtherTransition.TransitionStartEvent -= OnStartTransition;
			addedListener = false;
		}
	}

	private void OnStartTransition(GUI3DOnTransitionStartEvent evt)
	{
		if (Transition != null)
		{
			Transition.StartTransition();
		}
	}
}
