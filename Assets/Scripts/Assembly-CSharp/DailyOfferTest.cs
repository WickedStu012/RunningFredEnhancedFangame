using System;
using UnityEngine;

public class DailyOfferTest : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUILayout.Button("Get Offer"))
		{
			DailyOfferBackEnd.Instance.GetOffer(onGetOfferRes);
		}
	}

	public void onGetOfferRes(bool res, DateTime dt, int discount, int itemId)
	{
		Debug.Log(string.Format("Date: {0} Discount: {1} Item: {2}", dt, discount, itemId));
	}
}
