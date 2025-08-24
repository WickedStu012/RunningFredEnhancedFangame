using System.Collections.Generic;
using UnityEngine;

public class BeLordInAppAndroid : IBeLordInApp
{
	public delegate void OnRefund(string id);

	private const bool debug = false;

	private IBeLordInAppOnSuccess onSuccess;

	private IBeLordInAppOnError onError;

	private IBeLordInAppOnError onCancel;

	private string id;

	private int quantity;

	private bool isConsumable;

	private string lastId = string.Empty;

	private float lastTimeCancel;

	private int consumeId;

	private List<string> productsIdToConsume;

	private int consumeCount;

	private List<GooglePurchase> inventory;

	private bool canMakePayments;

	private bool oneProductWasAtLeastConsumed;

	public event OnRefund OnRefundEvent;

	public void Init(string publicKey)
	{
		GoogleIABManager.billingSupportedEvent += BillingSupported;
		GoogleIABManager.billingNotSupportedEvent += BillingNotSupported;
		AndroidPluginBypass.SafeGoogleIABInit(publicKey);
	}

	public void UnInit()
	{
	}

	private void doQueryOfInventory(bool useCallbacks)
	{
		oneProductWasAtLeastConsumed = false;
		if (useCallbacks)
		{
			GoogleIABManager.queryInventorySucceededEvent += QueryInventorySucceeded;
			GoogleIABManager.queryInventoryFailedEvent += QueryInventoryFailed;
		}
		GoogleIAB.queryInventory(new string[9] { "com.dedalord.runningfred.nsk7m", "com.dedalord.runningfred.nsk8m", "com.dedalord.runningfred.nsk3m", "com.dedalord.runningfred.nsk6m", "com.dedalord.runningfred.nsk2m", "com.dedalord.runningfred.nsk4m", "com.dedalord.runningfred.valuepack1", "com.dedalord.runningfred.valuepack2", "com.dedalord.runningfred.valuepack3" });
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
		lastTimeCancel = Time.unscaledTime;
		this.onSuccess = onSuccess;
		this.onError = onError;
		this.onCancel = onCancel;
		this.id = id;
		quantity = 1;
		isConsumable = true;
		GoogleIABManager.purchaseSucceededEvent += PurchaseSuccess;
		GoogleIABManager.purchaseFailedEvent += PurchaseOnError;
		GoogleIAB.purchaseProduct(id);
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

	private void PurchaseSuccess(GooglePurchase gp)
	{
		if (id.IndexOf("com.dedalord.runningfred.sk") != -1 || id.IndexOf("com.dedalord.runningfred.nsk") != -1)
		{
			GoogleIABManager.consumePurchaseSucceededEvent += ConsumePurchaseSucceeded;
			GoogleIABManager.consumePurchaseFailedEvent += ConsumePurchaseFailed;
			GoogleIAB.consumeProduct(id);
		}
		GoogleIABManager.purchaseSucceededEvent -= PurchaseSuccess;
		GoogleIABManager.purchaseFailedEvent -= PurchaseOnError;
		PlayerPrefs.SetString(id, Encrypt(gp.productId + SystemInfo.deviceUniqueIdentifier));
		if (onSuccess != null)
		{
			onSuccess(gp.productId, string.Empty, quantity);
		}
	}

	private void PurchaseOnError(string error)
	{
		GoogleIABManager.purchaseSucceededEvent -= PurchaseSuccess;
		GoogleIABManager.purchaseFailedEvent -= PurchaseOnError;
		if (error.IndexOf("-1005") != -1)
		{
			consumeCount = 0;
			GoogleIABManager.consumePurchaseSucceededEvent += ConsumePurchaseSucceeded;
			GoogleIABManager.consumePurchaseFailedEvent += ConsumePurchaseFailed;
			GoogleIAB.consumeProduct(id);
			if (onCancel != null)
			{
				onCancel(error);
			}
		}
		else if (error.IndexOf(": 7:") != -1 || error.IndexOf("attempted to purchase an item that has already been purchased") != -1)
		{
			if (id.IndexOf("com.dedalord.runningfred.sk") != -1 || id.IndexOf("com.dedalord.runningfred.nsk") != -1)
			{
				if (consumeCount <= 1)
				{
					GoogleIABManager.consumePurchaseSucceededEvent += ConsumePurchaseSucceededTryToPurchaseAgain;
					GoogleIABManager.consumePurchaseFailedEvent += ConsumePurchaseFailedTryToPurchaseAgain;
					GoogleIAB.consumeProduct(id);
				}
				else
				{
					consumeCount = 0;
					doQueryOfInventory(true);
					if (onError != null)
					{
						onError(error);
					}
				}
			}
			else
			{
				consumeCount = 0;
				if (onSuccess != null)
				{
					onSuccess(id, string.Empty, quantity);
				}
			}
		}
		else
		{
			consumeCount = 0;
			if (onError != null)
			{
				onError(error);
			}
		}
		lastTimeCancel = Time.unscaledTime;
	}

	private void ConsumePurchaseSucceededTryToPurchaseAgain(GooglePurchase obj)
	{
		GoogleIABManager.consumePurchaseSucceededEvent -= ConsumePurchaseSucceededTryToPurchaseAgain;
		GoogleIABManager.consumePurchaseFailedEvent -= ConsumePurchaseFailedTryToPurchaseAgain;
		if (consumeCount == 0)
		{
			consumeCount++;
			Buy(id, onSuccess, onError, onCancel);
		}
		else
		{
			consumeCount = 0;
		}
	}

	private void ConsumePurchaseFailedTryToPurchaseAgain(string error)
	{
		GoogleIABManager.consumePurchaseSucceededEvent -= ConsumePurchaseSucceededTryToPurchaseAgain;
		GoogleIABManager.consumePurchaseFailedEvent -= ConsumePurchaseFailedTryToPurchaseAgain;
		if (consumeCount == 0)
		{
			consumeCount++;
			Buy(id, onSuccess, onError, onCancel);
		}
	}

	private void ConsumePurchaseSucceeded(GooglePurchase gp)
	{
		productsIdToConsume = null;
		GoogleIABManager.consumePurchaseSucceededEvent -= ConsumePurchaseSucceeded;
		GoogleIABManager.consumePurchaseFailedEvent -= ConsumePurchaseFailed;
		if (oneProductWasAtLeastConsumed)
		{
			doQueryOfInventory(true);
		}
	}

	private void ConsumePurchaseFailed(string error)
	{
		productsIdToConsume = null;
		GoogleIABManager.consumePurchaseSucceededEvent -= ConsumePurchaseSucceeded;
		GoogleIABManager.consumePurchaseFailedEvent -= ConsumePurchaseFailed;
		if (oneProductWasAtLeastConsumed)
		{
			doQueryOfInventory(false);
		}
	}

	private void BillingSupported()
	{
		canMakePayments = true;
		GoogleIABManager.billingSupportedEvent -= BillingSupported;
		GoogleIABManager.billingNotSupportedEvent -= BillingNotSupported;
		doQueryOfInventory(true);
	}

	private void BillingNotSupported(string error)
	{
		canMakePayments = false;
		GoogleIABManager.billingSupportedEvent -= BillingSupported;
		GoogleIABManager.billingNotSupportedEvent -= BillingNotSupported;
	}

	public string Encrypt(string str)
	{
		return str;
	}

	public void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError)
	{
		onProductInfo(null);
	}

	private void OnCancelAndRefund(string id, string developerPayload)
	{
		if (!(Time.unscaledTime - lastTimeCancel < 30f) || !(lastId == id))
		{
			lastTimeCancel = Time.unscaledTime;
			lastId = id;
			if (this.OnRefundEvent != null)
			{
				this.OnRefundEvent(id);
			}
		}
	}

	private void QueryInventorySucceeded(List<GooglePurchase> gps, List<GoogleSkuInfo> gskis)
	{
		consumeId = 0;
		inventory = gps;
		productsIdToConsume = new List<string>();
		for (int i = 0; i < inventory.Count; i++)
		{
			if (inventory[i].purchaseState == GooglePurchase.GooglePurchaseState.Purchased && (inventory[i].productId.IndexOf("com.dedalord.runningfred.sk") != -1 || inventory[i].productId.IndexOf("com.dedalord.runningfred.nsk") != -1))
			{
				productsIdToConsume.Add(inventory[i].productId);
			}
		}
		if (productsIdToConsume.Count > 0)
		{
			GoogleIABManager.consumePurchaseSucceededEvent += ConsumePurchaseSucceeded;
			GoogleIABManager.consumePurchaseFailedEvent += ConsumePurchaseFailed;
			oneProductWasAtLeastConsumed = true;
			GoogleIAB.consumeProducts(productsIdToConsume.ToArray());
		}
		GoogleIABManager.queryInventorySucceededEvent -= QueryInventorySucceeded;
		GoogleIABManager.queryInventoryFailedEvent -= QueryInventoryFailed;
	}

	private void QueryInventoryFailed(string error)
	{
		GoogleIABManager.queryInventorySucceededEvent -= QueryInventorySucceeded;
		GoogleIABManager.queryInventoryFailedEvent -= QueryInventoryFailed;
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
		if (inventory != null && inventory.Count > 0)
		{
			int num = 0;
			for (int i = 0; i < inventory.Count; i++)
			{
				if (inventory[i].purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
				{
					num++;
				}
			}
			if (num > 0)
			{
				string[] array = new string[num];
				int num2 = 0;
				for (int j = 0; j < inventory.Count; j++)
				{
					if (inventory[j].purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
					{
						array[num2++] = inventory[j].productId;
					}
				}
				return array;
			}
		}
		return null;
	}

	public string[] GetAllTransactionsReceipts()
	{
		return null;
	}
}
