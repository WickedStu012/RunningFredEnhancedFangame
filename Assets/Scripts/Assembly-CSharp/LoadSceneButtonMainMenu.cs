using UnityEngine;

public class LoadSceneButtonMainMenu : MonoBehaviour
{
	private GUI3DButton button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.CheckEvents = true;
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
		CameraFade.Instance.FadeOut(OnFadeOut);
	}

	private void OnFadeOut()
	{
		Time.timeScale = 1f;
		SoundManager.StopAll();
		DedalordLoadLevel.LoadLevel(Levels.MainMenu);
	}
}
