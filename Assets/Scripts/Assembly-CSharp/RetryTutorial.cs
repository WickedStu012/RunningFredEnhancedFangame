using UnityEngine;

public class RetryTutorial : MonoBehaviour
{
	public bool IsInGameMenu;

	private GUI3DButton button;

	private GUI3DPanel panel;

	private GUI3DTransition transition;

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
		if (button != null)
		{
			button.ReleaseEvent -= OnRelease;
		}
	}

	private void Update()
	{
		if (!IsInGameMenu && MogaInput.Instance.IsConnected() && button.enabled && (MogaInput.Instance.GetButtonStartDown() || MogaInput.Instance.GetButtonADown()))
		{
			OnRelease(null);
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (panel == null)
		{
			panel = button.GetPanel();
		}
		if (panel != null)
		{
			CameraFade.Instance.FadeOut(OnFadeOut);
			if (transition == null)
			{
				transition = panel.GetComponent<GUI3DTransition>();
			}
			if (transition != null)
			{
				SoundManager.FadeOutAll(1f, onFadeOutAll);
				transition.TransitionEndEvent += OnTransitionEnd;
			}
		}
	}

	private void onFadeOutAll()
	{
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		transition.TransitionEndEvent -= OnTransitionEnd;
	}

	private void OnFadeOut()
	{
		SoundManager.StopAll();
		Time.timeScale = 1f;
		CharHelper.GetCharStateMachine().SwitchTo(ActionCode.RUNNING);
		PlayerPrefsWrapper.SetTutorial(true);
		DedalordLoadLevel.LoadLevel("TutorialLoader");
	}
}
