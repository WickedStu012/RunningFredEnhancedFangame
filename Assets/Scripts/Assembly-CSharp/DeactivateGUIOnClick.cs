using UnityEngine;

public class DeactivateGUIOnClick : MonoBehaviour
{
	public GUI3D[] DeactivateGUI;

	private GUI3DButton button;

	private GUI3DTransition transition;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
		if (transition == null && button != null)
		{
			transition = button.ActivateTransition;
		}
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
		if (!button.CancelActions)
		{
			if (transition != null)
			{
				transition.TransitionEndEvent += OnTransitionEnd;
			}
			else
			{
				DeactivateGUIs();
			}
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		DoAction();
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent e)
	{
		DeactivateGUIs();
		transition.TransitionEndEvent -= OnTransitionEnd;
	}

	private void DeactivateGUIs()
	{
		GUI3D[] deactivateGUI = DeactivateGUI;
		foreach (GUI3D gUI3D in deactivateGUI)
		{
			gUI3D.SetActive(false);
			gUI3D.SetVisible(false);
		}
	}
}
