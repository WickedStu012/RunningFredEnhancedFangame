using UnityEngine;

public class RetryButton : MonoBehaviour
{
	public bool EnableOnButtonA = true;

	private GUI3DButton button;

	private ActivateTransitionsOnClick activatetrans;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		if (activatetrans == null)
		{
			activatetrans = GetComponent<ActivateTransitionsOnClick>();
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
		button.enabled = false;
		CameraFade.Instance.FadeOut(OnFadeOut);
	}

	private void Update()
	{
		if (MogaInput.Instance.IsConnected() && button.enabled && ((EnableOnButtonA && (MogaInput.Instance.GetButtonStartDown() || MogaInput.Instance.GetButtonADown())) || (!EnableOnButtonA && MogaInput.Instance.GetButtonBDown())))
		{
			button.OnRelease();
		}
	}

	private void OnFadeOut()
	{
		button.enabled = true;
		Time.timeScale = 1f;
		SoundManager.StopAll();
		if (Application.loadedLevelName == "TutorialLoader")
		{
			DedalordLoadLevel.LoadLevel("TutorialLoader");
			return;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			PlayerAccount.Instance.UndoLevelSelect();
		}
		DedalordLoadLevel.LoadLevel("CharCreator");
	}
}
