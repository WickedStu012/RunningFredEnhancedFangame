public interface IBeLordInApp
{
	void Init(string publicKey);

	void UnInit();

	bool CanMakePayments();

	void Buy(string id, int quantity, IBeLordInAppOnSuccess onSuccess, IBeLordInAppOnError onError, IBeLordInAppOnError onCancel);

	string[] GetAllTransactions();

	string[] GetAllTransactionsReceipts();

	void RestorePurchases(IBeLordInApRestorePurchasesDone cb);

	bool IsInAppPurchased(string id);

	void RequestProductData(string[] pids, IBeLordInAppProductInfo onProductInfo, IBeLordInAppOnError onError);
}
