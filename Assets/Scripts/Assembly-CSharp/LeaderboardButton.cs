using UnityEngine;

public class LeaderboardButton : MonoBehaviour
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
		GUI3DPopupManager.Instance.ShowPopup("ComingSoon", "This feature is coming soon. Keep the game updated!", "Leaderboards", true);
	}

	private void OnConfirmation(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			ConfigParams.useLeaderboardAndAchievements = true;
			BeLord.Enable = true;
		}
	}
}
