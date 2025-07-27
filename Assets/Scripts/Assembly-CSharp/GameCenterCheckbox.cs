using UnityEngine;

public class GameCenterCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void Awake()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = ConfigParams.useLeaderboardAndAchievements;
		checkbox.Checked = checkbox.StartCheckStatus;
	}

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.CheckboxChangeEvent += OnChange;
	}

	private void OnDisable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.CheckboxChangeEvent -= OnChange;
	}

	private void OnChange(GUI3DOnCheckboxChangeEvent evt)
	{
		ConfigParams.useLeaderboardAndAchievements = evt.Checked;
		if (ConfigParams.useLeaderboardAndAchievements)
		{
			BeLord.Enable = true;
		}
		else
		{
			BeLord.Enable = false;
		}
	}
}
