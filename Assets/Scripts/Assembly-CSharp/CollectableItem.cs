using UnityEngine;

public class CollectableItem : MonoBehaviour
{
	public ChallengeItemInfo ItemInfo;

	public Color UnlockedTextColor;

	public Color LockedTextColor;

	private GUI3DObject guiObject;

	private GUI3DText text;

	private string cardName;

	private Color textColor;

	private void Start()
	{
		text = GetComponentInChildren<GUI3DText>();
		text.GetComponent<Renderer>().material.color = textColor;
		text.SetDynamicText(cardName);
	}

	public void Refresh()
	{
		if (PlayerAccount.Instance.IsChallengeUnlocked(ItemInfo.Id))
		{
			Debug.Log("item unlocked!");
			guiObject.TextureName = ItemInfo.CollectPicture;
			cardName = ItemInfo.CollectName;
			textColor = UnlockedTextColor;
		}
		else
		{
			guiObject.TextureName = "Collectibles/DeathcardBack";
			cardName = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Locked", "!BAD_TEXT!");
			textColor = LockedTextColor;
		}
		guiObject.CreateOwnMesh = true;
		guiObject.CreateMesh();
	}

	private void OnEnable()
	{
		if (guiObject == null)
		{
			guiObject = GetComponent<GUI3DObject>();
		}
	}
}
