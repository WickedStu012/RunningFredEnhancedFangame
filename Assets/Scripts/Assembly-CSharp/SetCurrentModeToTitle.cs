using UnityEngine;

public class SetCurrentModeToTitle : MonoBehaviour
{
	private GUI3DText text;

	private void OnEnable()
	{
		if (text == null)
		{
			text = GetComponent<GUI3DText>();
		}
		if (text != null && PlayerAccount.Instance != null)
		{
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
			{
				text.SetDynamicText(string.Format("Survival"));
			}
			else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
			{
				text.SetDynamicText(string.Format("Adventure"));
			}
		}
	}
}
