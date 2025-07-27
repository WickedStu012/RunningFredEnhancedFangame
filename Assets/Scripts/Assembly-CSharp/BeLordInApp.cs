using System.Collections.Generic;
using UnityEngine;

public class BeLordInApp
{
	public delegate void OnRefund(string id);

	private static BeLordInApp instance;

	private IBeLordInApp belordInApp;

	private bool productDataRequested;

	private bool isRequestingProductData;

	private IBeLordInAppProductInfo onRequestProductData;

	private IBeLordInAppOnError onRequestProductDataError;

	private bool alreadyInitialized;

	private static bool DebugMode;

	public static BeLordInApp Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BeLordInApp();
			}
			return instance;
		}
	}

	public bool IsProductDataRequested
	{
		get
		{
			return productDataRequested;
		}
	}

	public bool IsRequestingProductData
	{
		get
		{
			return isRequestingProductData;
		}
	}

	public event OnRefund OnRefundEvent;

	protected BeLordInApp()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (DebugMode)
			{
				Debug.Log("BeLordInApp.BeLordInApp(): Creating BeLordInAppiOS");
			}
			belordInApp = new BeLordInAppiOS();
		}
		else if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			if (DebugMode)
			{
				Debug.Log("BeLordInApp.BeLordInApp(): Creating BeLordInAppMac");
			}
			belordInApp = new BeLordInAppMac();
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			if (!ConfigParams.isKindle)
			{
				if (DebugMode)
				{
					Debug.Log("BeLordInApp.BeLordInApp(): Creating BeLordInAppAndroid");
				}
				belordInApp = new BeLordInAppAndroid();
			}
			else
			{
				if (DebugMode)
				{
					Debug.Log("BeLordInApp.BeLordInApp(): Creating BeLordInAppKindle");
				}
				belordInApp = new BeLordInAppKindle();
			}
		}
		else if (Application.platform == RuntimePlatform.NaCl)
		{
			if (DebugMode)
			{
				Debug.Log("BeLordInApp.BeLordInApp(): Creating BeLordInAppChromeIAB");
			}
			belordInApp = new BeLordInAppChromeIAB();
		}
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (DebugMode)
			{
				Debug.Log("BeLordInApp.BeLordInApp(): Creating BeLordInAppKongregateIAB");
			}
			belordInApp = new BeLordInAppKongregateIAB();
		}
		else
		{
			if (DebugMode)
			{
				Debug.Log("BeLordInApp.BeLordInApp(): belordInApp = null");
			}
			belordInApp = null;
		}
	}

	public void Init(string publicKey)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.Init() publicKey: " + publicKey + " belordInApp is null: " + (belordInApp == null));
		}
		if (!alreadyInitialized)
		{
			alreadyInitialized = true;
			if (belordInApp != null)
			{
				belordInApp.Init(publicKey);
			}
		}
	}

	private void onProductInfoRes(List<BeLordProductInfo> productInfo)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.onProductInfoRes()");
		}
		Debug.Log("---> onProductInfoRes");
		for (int i = 0; i < productInfo.Count; i++)
		{
			Debug.Log(string.Format("Product. Id: {0} Price: {1}", productInfo[i].Id, productInfo[i].Price));
		}
	}

	private void onProductInfoErrorRes(string error)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.onProductInfoErrorRes() error: " + error);
		}
		Debug.Log("---> onProductInfoErrorRes");
	}

	public void UnInit()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.UnInit() belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			belordInApp.UnInit();
		}
	}

	public bool CanMakePayments()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.CanMakePayments() belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			return belordInApp.CanMakePayments();
		}
		return true;
	}

	public void Buy(string id, int quantity, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.Buy() id: " + id + " quantity: " + quantity + " belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			belordInApp.Buy(id, quantity, onSuccess, onError, onCancel);
		}
		else if (!Application.isEditor)
		{
			onError("In-app purchases not available!");
		}
		else
		{
			onSuccess(id, string.Empty, quantity);
		}
	}

	public string[] GetAllTransactions()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.GetAllTransactions() belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			return belordInApp.GetAllTransactions();
		}
		return null;
	}

	public string[] GetAllTransactionsReceipts()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.GetAllTransactionsReceipts() belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			return belordInApp.GetAllTransactionsReceipts();
		}
		return null;
	}

	public void RestorePurchases(IBeLordInApRestorePurchasesDone cb)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.RestorePurchases() belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			belordInApp.RestorePurchases(cb);
		}
		else if (cb != null)
		{
			cb();
		}
	}

	public bool IsInAppPurchased(string id)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.IsInAppPurchased() id: " + id + "belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			belordInApp.IsInAppPurchased(id);
		}
		return false;
	}

	public void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.RequestProductData() belordInApp is null: " + (belordInApp == null));
		}
		if (belordInApp != null)
		{
			onRequestProductData = onProductInfo;
			onRequestProductDataError = onError;
			isRequestingProductData = true;
			belordInApp.RequestProductData(pids, OnRequestProductData, OnRequestProductDataError);
		}
	}

	private void OnRequestProductData(List<BeLordProductInfo> productInfo)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.OnRequestProductData()");
		}
		productDataRequested = true;
		if (onRequestProductData != null)
		{
			onRequestProductData(productInfo);
		}
		isRequestingProductData = false;
	}

	private void OnRequestProductDataError(string error)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.OnRequestProductDataError()");
		}
		if (onRequestProductDataError != null)
		{
			onRequestProductDataError(error);
		}
		isRequestingProductData = false;
	}

	private void OnCancelAndRefund(string id)
	{
		if (DebugMode)
		{
			Debug.Log("BeLordInApp.OnCancelAndRefund()");
		}
		if (this.OnRefundEvent != null)
		{
			this.OnRefundEvent(id);
		}
	}
}
