using UnityEngine;

public class BackToMenuButton : MonoBehaviour
{
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

	private void Update()
	{
		if (MogaInput.Instance.IsConnected() && button.enabled && MogaInput.Instance.GetButtonBDown())
		{
			button.OnRelease();
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		button.enabled = false;
		CameraFade.Instance.FadeOut(OnFadeOut);
	}

	private void OnFadeOut()
	{
		button.enabled = true;
		Time.timeScale = 1f;
		SoundManager.StopAll();
		PlayerAccount.Instance.UnselectChallenge();
		DedalordLoadLevel.LoadLevel(Levels.MainMenu);
	}
}
