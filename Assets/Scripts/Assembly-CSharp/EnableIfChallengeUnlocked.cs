using UnityEngine;

public class EnableIfChallengeUnlocked : MonoBehaviour
{
	public bool OnlyChallenge;

	public bool ChallengeUnlocked;

	private GUI3DText text;

	private void OnEnable()
	{
		text = GetComponent<GUI3DText>();
		if (text != null)
		{
			if (OnlyChallenge && PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Challenge)
			{
				text.SetDynamicText(string.Empty);
			}
			else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge && SceneParamsManager.Instance.GetBool("ChallengeUnlocked", false) != ChallengeUnlocked)
			{
				text.SetDynamicText(string.Empty);
			}
		}
	}
}
