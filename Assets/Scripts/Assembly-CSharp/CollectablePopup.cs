using UnityEngine;

public class CollectablePopup : MonoBehaviour
{
	public GUI3DText Description1;

	public GUI3DText Description2;

	public GUI3DObject Picture;

	private void OnEnable()
	{
		if (ChallengesManager.Instance != null)
		{
			ChallengeItemInfo selectedChallenge = ChallengesManager.Instance.SelectedChallenge;
			if (selectedChallenge != null && selectedChallenge.CollectName != null && selectedChallenge.CollectPicture != null)
			{
				Description1.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "CollectableUnlocked", "!BAD_TEXT!"));
				Description2.SetDynamicText(selectedChallenge.CollectName);
				Picture.TextureName = selectedChallenge.CollectPicture;
				Picture.RefreshMaterial(selectedChallenge.CollectPicture);
			}
		}
	}
}
