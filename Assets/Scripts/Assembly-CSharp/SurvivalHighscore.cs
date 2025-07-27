using UnityEngine;

public class SurvivalHighscore : MonoBehaviour
{
	private GUI3DText text;

	private void OnEnable()
	{
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			if (text == null)
			{
				text = GetComponent<GUI3DText>();
			}
			if (text != null)
			{
				text.SetDynamicText(PlayerAccount.Instance.GetMetersFromCurrentLevel().ToString());
			}
		}
	}
}
