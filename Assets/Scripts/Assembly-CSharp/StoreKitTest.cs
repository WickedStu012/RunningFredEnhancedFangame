using System.Collections.Generic;
using UnityEngine;

public class StoreKitTest : MonoBehaviour
{
	private void Awake()
	{
	}

	private void OnProductInfo(List<BeLordProductInfo> products)
	{
		Debug.Log("StoreKitTest.OnProductInfo() products is null" + (products == null));
		if (products == null)
		{
			return;
		}
		foreach (BeLordProductInfo product in products)
		{
			Debug.Log(string.Format("Looking for: {0}", product.Id));
		}
	}

	private void OnRequestProductError(string error)
	{
		Debug.Log("StoreKitTest.OnRequestProductError() error: " + error);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
