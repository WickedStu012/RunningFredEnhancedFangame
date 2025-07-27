using UnityEngine;

public class SwitchToInGameMenu : MonoBehaviour
{
	public GUI3DTransition ActivateTransition;

	public GUI3D SwitchToGUIAdventure;

	public GUI3D SwitchToGUIEndless;

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
		if (ActivateTransition != null)
		{
			GUI3DManager.Instance.SaveCurrentState();
			ActivateTransition.StartOutroTransition();
			ActivateTransition.TransitionEndEvent += OnEndTransition;
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		DoAction();
	}

	private void OnEndTransition(GUI3DOnTransitionEndEvent evt)
	{
		ActivateTransition.TransitionEndEvent -= OnEndTransition;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure || PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			GUI3DManager.Instance.Activate(SwitchToGUIAdventure, true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate(SwitchToGUIEndless, true, true);
		}
	}
}
