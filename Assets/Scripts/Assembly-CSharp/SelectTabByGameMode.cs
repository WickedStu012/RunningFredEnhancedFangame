using UnityEngine;

public class SelectTabByGameMode : MonoBehaviour
{
	private GUI3DTabControl tabControl;

	private void OnEnable()
	{
		tabControl = GetComponent<GUI3DTabControl>();
		if (tabControl != null && PlayerAccount.Instance != null)
		{
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
			{
				tabControl.SwitchToTab("AdventureTab");
			}
			else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
			{
				tabControl.SwitchToTab("SurvivalTab");
			}
		}
	}
}
