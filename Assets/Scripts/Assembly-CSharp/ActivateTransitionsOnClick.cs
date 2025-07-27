using UnityEngine;

public class ActivateTransitionsOnClick : MonoBehaviour
{
	public GUI3DTransition[] Transitions;

	private GUI3DButton button;

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

	public void DoAction()
	{
		if (button.CancelActions || Transitions == null || Transitions.Length <= 0)
		{
			return;
		}
		GUI3DTransition[] transitions = Transitions;
		foreach (GUI3DTransition gUI3DTransition in transitions)
		{
			if (gUI3DTransition != null)
			{
				gUI3DTransition.StartOutroTransition();
			}
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		DoAction();
	}
}
