using UnityEngine;

public class GoToShopButtonMM : MonoBehaviour
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
		StatsManager.LogEvent(StatVar.MAIN_MENU_BUTTON, "SHOP");
		StatsManager.LogEventTimed(StatVar.SHOP, "MAIN_MENU");
	}
}
