using UnityEngine;

public class KonPriceChanger : MonoBehaviour
{
	public string priceToPlace = "19 Kreds";

	public GUI3DText priceText;

	private void OnEnable()
	{
		if (priceText == null)
		{
			priceText = GetComponent<GUI3DText>();
		}
		if (ConfigParams.IsKongregate() && priceText != null)
		{
			priceText.SetDynamicText(priceToPlace);
		}
	}
}
