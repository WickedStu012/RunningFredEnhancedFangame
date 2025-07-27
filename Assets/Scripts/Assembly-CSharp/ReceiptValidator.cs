using System.Collections;
using UnityEngine;

public class ReceiptValidator : MonoBehaviour
{
	public static ReceiptValidator Instance;

	private bool waitingResponse;

	private BlackLordReceiptValidatorRes cb;

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		Instance = this;
	}

	private void OnDisable()
	{
		Instance = null;
	}

	private void Update()
	{
		if (waitingResponse)
		{
			blValidateReceipt.Update();
		}
	}

	public void ValidateReceipt(string receiptBase64, bool isTest, BlackLordReceiptValidatorRes ber)
	{
		waitingResponse = true;
		cb = ber;
		blValidateReceipt.ValidateReceipt(receiptBase64, isTest, validateReceiptRes);
	}

	private void validateReceiptRes(bool res, string str)
	{
		waitingResponse = false;
		if (res)
		{
			if (cb != null)
			{
				cb(true, IsReceiptResponseValid(str));
			}
			return;
		}
		Debug.Log(string.Format("Receipt Validation server returns false"));
		if (cb != null)
		{
			cb(false, false);
		}
	}

	public bool IsReceiptResponseValid(string receiptResponse)
	{
		if (receiptResponse.Contains("do_not_check"))
		{
			return true;
		}
		Hashtable hashtable = MiniJSON.jsonDecode(receiptResponse) as Hashtable;
		if (hashtable == null || !hashtable.Contains("receipt") || !hashtable.Contains("status"))
		{
			return false;
		}
		double num = (double)hashtable["status"];
		return num == 0.0;
	}
}
