using UnityEngine;

public class SelectLocationButton : MonoBehaviour
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

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge)
		{
			PlayerAccount.Instance.UnselectChallenge();
		}
		button.enabled = false;
		CameraFade.Instance.FadeOut(OnFadeOut);
	}

	private void OnFadeOut()
	{
		button.enabled = true;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			SceneParamsManager.Instance.Push("SelectChapterAdventureEx");
		}
		else
		{
			SceneParamsManager.Instance.Push("SelectChapterSurvivalEx");
		}
		DedalordLoadLevel.LoadLevel(Levels.MainMenu);
	}

	protected void Update()
	{
		if (MogaInput.Instance.IsConnected() && button.enabled)
		{
			if (MogaInput.Instance.GetButtonADown())
			{
				button.OnRelease();
			}
			else if (MogaInput.Instance.GetButtonBDown())
			{
				button.OnRelease();
			}
		}
	}
}
