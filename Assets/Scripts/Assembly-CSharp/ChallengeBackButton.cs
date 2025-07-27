using UnityEngine;

public class ChallengeBackButton : MonoBehaviour
{
	private GUI3DButton button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += ReleaseEvent;
	}

	private void OnDisable()
	{
		button.ReleaseEvent -= ReleaseEvent;
	}

	private void ReleaseEvent(GUI3DOnReleaseEvent evn)
	{
		if (CollectablesButton.collectablePress)
		{
			StatsManager.LogEvent(StatVar.CHALLENGE_SCREEN, "Yes");
		}
		else
		{
			StatsManager.LogEvent(StatVar.CHALLENGE_SCREEN, "No");
		}
		CollectablesButton.collectablePress = false;
	}
}
