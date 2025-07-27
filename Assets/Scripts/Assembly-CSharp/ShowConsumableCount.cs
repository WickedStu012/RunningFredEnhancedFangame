using UnityEngine;

public class ShowConsumableCount : MonoBehaviour
{
	public ShopItemId ItemId;

	private GUI3DText countText;

	private ItemInfo ii;

	private void Start()
	{
		countText = base.gameObject.GetComponent<GUI3DText>();
		ii = Store.Instance.GetItem((int)ItemId);
		countText.SetDynamicText(ii.Count.ToString());
	}

	private void Update()
	{
		countText.SetDynamicText(ii.Count.ToString());
	}
}
