using UnityEngine;

public class ChallengeButton : MonoBehaviour
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
		StatsManager.LogEvent(StatVar.MAIN_MENU_BUTTON, "CHALLENGE");
	}
}
