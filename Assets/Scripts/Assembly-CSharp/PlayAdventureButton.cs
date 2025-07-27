using UnityEngine;

public class PlayAdventureButton : MonoBehaviour
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
		StatsManager.LogEvent(StatVar.MAIN_MENU_BUTTON, "ADVENTURE");
		PlayerAccount.Instance.ChangeGameMode(PlayerAccount.GameMode.Adventure);
	}
}
