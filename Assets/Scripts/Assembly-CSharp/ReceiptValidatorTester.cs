using UnityEngine;

public class ReceiptValidatorTester : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUILayout.Button("Send Receipt"))
		{
			ReceiptValidator.Instance.ValidateReceipt(string.Empty, true, onValidateReceipt);
		}
	}

	private void onValidateReceipt(bool res, bool receiptIsValid)
	{
		Debug.Log(string.Format("onValidateReceipt: res: {0}", receiptIsValid));
	}
}
