using UnityEngine;

public class SwitchToSelectChapter : MonoBehaviour
{
	private GUI3DButton button;

	public GUI3DTransition transition;

	public GUI3D AdventureGUI;

	public GUI3D SurvivalGUI;

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
		if (transition != null)
		{
			transition.TransitionEndEvent += OnEndTransition;
		}
	}

	private void OnEndTransition(GUI3DOnTransitionEndEvent evt)
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			GUI3DManager.Instance.Activate(AdventureGUI, true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate(SurvivalGUI, true, true);
		}
		transition.TransitionEndEvent -= OnEndTransition;
	}
}
