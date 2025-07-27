using System.Collections;
using UnityEngine;

public class BeLordInAppKongregateIAB : IBeLordInApp
{
	private IBeLordInAppOnSuccess onSuccess;

	private IBeLordInAppOnError onCancel;

	private string id;

	private int quantity;

	public void Init(string publicKey)
	{
	}

	public void UnInit()
	{
	}

	public bool CanMakePayments()
	{
		return true;
	}

	public void Buy(string id, int quantity, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel)
	{
		this.onSuccess = onSuccess;
		this.onCancel = onCancel;
		this.id = id;
		this.quantity = 1;
		FullScreenChecker.ChangeToFullScreen(false);
		KongregateAPI.PurchaseOption(id, PurchaseRes);
	}

	public string[] GetAllTransactions()
	{
		return null;
	}

	public string[] GetAllTransactionsReceipts()
	{
		return null;
	}

	public void RestorePurchases(IBeLordInApRestorePurchasesDone cb)
	{
	}

	public bool IsInAppPurchased(string id)
	{
		return false;
	}

	private void PurchaseRes(bool res, string marketId)
	{
		Debug.Log(string.Format("BeLordInAppKongregateIAB.PurchaseRes. res: {0} marketId: {1}", res, marketId));
		if (res)
		{
			onSuccess(id, string.Empty, quantity);
		}
		else
		{
			onCancel(string.Empty);
		}
	}

	private void onUseItemRes(bool res, Hashtable ht)
	{
		Debug.Log(string.Format("res: {0}", res));
		if (res && ht != null)
		{
			ArrayList arrayList = ht["items"] as ArrayList;
			for (int i = 0; i < arrayList.Count; i++)
			{
				ht = arrayList[i] as Hashtable;
				Debug.Log(string.Format("i: {0} id: {1} identifier: {2} name: {3}", i, ht["id"], ht["identifier"], ht["name"]));
			}
			Debug.Log("Now. I have to consume all the user items!");
			onSuccess(id, string.Empty, quantity);
		}
		else
		{
			onCancel(string.Empty);
		}
	}

	public void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError)
	{
		onProductInfo(null);
	}
}
