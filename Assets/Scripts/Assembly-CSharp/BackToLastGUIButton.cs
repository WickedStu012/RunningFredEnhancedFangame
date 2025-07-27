using UnityEngine;

public class BackToLastGUIButton : MonoBehaviour
{
	public GUI3DTransition Transition;

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

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (Transition == null)
		{
			GUI3DManager.Instance.RestoreLastState();
		}
		else
		{
			Transition.TransitionEndEvent += OnTransitionEnd;
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		Transition.TransitionEndEvent -= OnTransitionEnd;
		if (!SceneParamsManager.Instance.IsEmpty)
		{
			string text = (string)SceneParamsManager.Instance.Pop();
			if (text != null && string.Compare(text, "resurrect") == 0)
			{
				GameManager.BackToResurrectDialog();
			}
			else
			{
				GUI3DManager.Instance.RestoreLastState();
			}
		}
		else
		{
			GUI3DManager.Instance.RestoreLastState();
		}
	}
}
