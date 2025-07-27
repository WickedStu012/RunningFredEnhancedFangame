using UnityEngine;

public class BeLordInAppiOS : IBeLordInApp
{
	private bool DebugMode;

	public void Init(string publicKey)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInAppiOS.Init()  publicKey: " + publicKey);
		}
	}

	public void UnInit()
	{
	}

	public bool CanMakePayments()
	{
		return false;
	}

	public void Buy(string id, int quantity, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInAppiOS.Buy()  id: " + id + " quantity: " + quantity);
		}
	}

	public string[] GetAllTransactions()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInAppiOS.GetAllTransactions()");
		}
		return null;
	}

	public string[] GetAllTransactionsReceipts()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInAppiOS.GetAllTransactionsReceipts()");
		}
		return null;
	}

	public void RestorePurchases(IBeLordInApRestorePurchasesDone cb)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInAppiOS.RestorePurchases()");
		}
	}

	private void onRestoreCompletedTransactions()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInAppiOS.onRestoreCompletedTransactions()");
		}
	}

	public bool IsInAppPurchased(string id)
	{
		return false;
	}

	public string Encrypt(string str)
	{
		return str;
	}

	public void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError)
	{
	}
}
