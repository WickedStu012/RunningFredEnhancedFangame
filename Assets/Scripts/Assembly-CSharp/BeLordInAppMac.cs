using System.Threading;
using UnityEngine;

public class BeLordInAppMac : IBeLordInApp
{
	public void Init(string publicKey)
	{
		Debug.Log(string.Format("BeLordInAppMac.Init. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId));
	}

	public void UnInit()
	{
	}

	public bool CanMakePayments()
	{
		Debug.Log("BeLordInAppMac.CanMakePayments");
		return false;
	}

	public void Buy(string id, int quantity, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel)
	{
		Debug.Log(string.Format("BeLordInAppMac.Buy. id: {0}. ThreadId: {1}", id, Thread.CurrentThread.ManagedThreadId));
	}

	public string[] GetAllTransactions()
	{
		return null;
	}

	public string[] GetAllTransactionsReceipts()
	{
		Debug.Log(string.Format("BeLordInAppMac.GetAllTransactionsReceipts"));
		return null;
	}

	public void RestorePurchases(IBeLordInApRestorePurchasesDone cb)
	{
		Debug.Log(string.Format("BeLordInAppMac.RestorePurchases"));
	}

	private void onRestoreCompletedTransactions()
	{
		Debug.Log(string.Format("BeLordInAppMac.onRestoreCompletedTransactions"));
	}

	public bool IsInAppPurchased(string id)
	{
		Debug.Log(string.Format("BeLordInAppMac.IsInAppPurchased. id: {0}", id));
		return false;
	}

	public string Encrypt(string str)
	{
		return str;
	}

	public void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError)
	{
		Debug.Log(string.Format("BeLordInAppMac.RequestProductData"));
	}
}
