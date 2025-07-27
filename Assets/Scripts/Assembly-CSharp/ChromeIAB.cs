using UnityEngine;

public class ChromeIAB : MonoBehaviour
{
	private static PurchaseRes_s purchaseResult;

	private static string marketId;

	private void Awake()
	{
		if (Application.platform != RuntimePlatform.NaCl)
		{
			Object.Destroy(this);
		}
	}

	private void Update()
	{
		GetPayLoad.Update();
	}

	private void pres1(string opt)
	{
		Debug.Log(string.Format("purchaseOk"));
		if (purchaseResult != null)
		{
			purchaseResult(true, marketId);
		}
	}

	private void pres2(string opt)
	{
		Debug.Log(string.Format("purchaseFail"));
		if (purchaseResult != null)
		{
			purchaseResult(false, marketId);
		}
	}

	public static void PurchaseOption(string marketId, PurchaseRes_s purchaseRes)
	{
		purchaseResult = purchaseRes;
		ChromeIAB.marketId = marketId;
		string res = GetPayLoadLocal.GetRes(marketId);
		purchase(res);
	}

	private static void payloadRes(bool res, string str)
	{
		Debug.Log(string.Format("PayloadRes. res: {0} str: {1}", res, str));
		if (res)
		{
			purchase(str);
		}
		else if (purchaseResult != null)
		{
			purchaseResult(false, marketId);
		}
	}

	private static void purchase(string opt)
	{
		Application.ExternalEval("var sh = function(purchaseAction) { document.getElementById('UnityEmbed').postMessage(\"PlayerAccount.pres1()\");};var fh = function(purchaseActionError) { document.getElementById('UnityEmbed').postMessage(\"PlayerAccount.pres2()\");};goog.payments.inapp.buy({  'jwt'     : \"" + opt + "\",  'success' : sh,  'failure' : fh });");
	}
}
