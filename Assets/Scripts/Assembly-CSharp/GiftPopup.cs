using UnityEngine;

public class GiftPopup : MonoBehaviour
{
	public GUI3DText Description1;

	public GUI3DText Description2;

	public GUI3DObject Picture;

	private void OnEnable()
	{
		if (GiftManager.Instance != null)
		{
			GiftManager.GiftType collectedGift = GiftManager.Instance.CollectedGift;
			if (collectedGift != null)
			{
				Description1.SetDynamicText(collectedGift.Name);
				Description2.SetDynamicText(collectedGift.Description);
				Picture.TextureName = collectedGift.Picture;
				Picture.ObjectSize = Vector2.zero;
				Picture.RefreshMaterial(collectedGift.Picture);
			}
		}
	}
}
