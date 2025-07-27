using UnityEngine;

public class StoreTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Buy Option 1d"))
		{
		}
		if (GUILayout.Button("Buy Option 2"))
		{
		}
		if (GUILayout.Button("Buy Option 3"))
		{
		}
		if (GUILayout.Button("Buy Option 4"))
		{
		}
		if (GUILayout.Button("Buy Option 5"))
		{
		}
		if (!GUILayout.Button("Buy Option 6"))
		{
		}
	}

	private void PurchaseRes(bool res, int storeId)
	{
		Debug.Log(string.Format("PurchaseRes. res: {0}", res));
	}
}
