using System.Collections.Generic;
using UnityEngine;

public class BeLordInAppKindle : IBeLordInApp
{
	private IBeLordInAppOnSuccess onSuccess;

	private IBeLordInAppOnError onError;

	private IBeLordInAppOnError onCancel;

	private string id;

	private int quantity;

	private bool isConsumable;

	private bool canMakePayments;

	private List<BeLordProductInfo> productInfoList;

	public void Init(string publicKey)
	{
	}

	public void UnInit()
	{
	}

	public bool CanMakePayments()
	{
		return canMakePayments;
	}

	public void Buy(string id, int quantity, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel)
	{
		Buy(id, onSuccess, onError, onCancel);
	}

	public void Buy(string id, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel)
	{
	}

	public bool IsInAppPurchased(string id)
	{
		if (PlayerPrefs.HasKey(id))
		{
			string text = PlayerPrefs.GetString(id);
			if (text == Encrypt(id + SystemInfo.deviceUniqueIdentifier))
			{
				return true;
			}
		}
		return false;
	}

	private string Encrypt(string str)
	{
		return str;
	}

	public void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError)
	{
		onProductInfo(null);
	}

	public void RestorePurchases(IBeLordInApRestorePurchasesDone cb)
	{
		if (cb != null)
		{
			cb();
		}
	}

	public string[] GetAllTransactions()
	{
		return null;
	}

	public string[] GetAllTransactionsReceipts()
	{
		return null;
	}
}
