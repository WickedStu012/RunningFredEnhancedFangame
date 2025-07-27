using UnityEngine;

public class BackFromStoreButton : MonoBehaviour
{
	public string DefaultGUI = Levels.MainMenu;

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
		StatsManager.LogEventEndTimed(StatVar.SHOP);
		Transition.TransitionEndEvent += OnTransitionEnd;
		Transition.StartTransition();
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent e)
	{
		Transition.TransitionEndEvent -= OnTransitionEnd;
		if (!SceneParamsManager.Instance.IsEmpty)
		{
			string guiName = (string)SceneParamsManager.Instance.Pop();
			GUI3DManager.Instance.Activate(guiName, true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate(DefaultGUI, true, true);
		}
	}
}
