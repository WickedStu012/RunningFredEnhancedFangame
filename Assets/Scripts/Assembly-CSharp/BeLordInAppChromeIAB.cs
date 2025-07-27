public class BeLordInAppChromeIAB : IBeLordInApp
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
		ChromeIAB.PurchaseOption(id, PurchaseRes);
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
		if (res)
		{
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
