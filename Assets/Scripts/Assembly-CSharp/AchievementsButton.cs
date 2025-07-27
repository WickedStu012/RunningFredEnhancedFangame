using UnityEngine;

public class AchievementsButton : MonoBehaviour
{
	private GUI3DButton button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ClickEvent += OnClick;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ClickEvent -= OnClick;
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		StatsManager.LogEvent(StatVar.MAIN_MENU_BUTTON, "ACHIEVEMENTS");
		button.CheckEvents = false;
		CameraFade.Instance.FadeOut(OnFadeOut);
	}

	private void OnConfirmation(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			ConfigParams.useLeaderboardAndAchievements = true;
			BeLord.Enable = true;
		}
	}

	private void OnFadeOut()
	{
		DedalordLoadLevel.LoadLevel("Achievements");
	}
}
