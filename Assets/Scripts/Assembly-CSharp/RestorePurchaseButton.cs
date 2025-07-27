using System.Collections.Generic;
using UnityEngine;

public class RestorePurchaseButton : MonoBehaviour
{
	private class VPInfo
	{
		public int valuePackNumber;

		public string receiptB64;

		public VPInfo(int vpNum, string recB64)
		{
			valuePackNumber = vpNum;
			receiptB64 = recB64;
		}
	}

	private enum State
	{
		IDLE = 0,
		VALIDATING_VP1 = 1,
		VALIDATING_VP2 = 2,
		VALIDATING_VP3 = 3
	}

	private GUI3DButton button;

	private State state;

	private Queue<VPInfo> validateQueue = new Queue<VPInfo>();

	private bool buttonEnable;

	private void Awake()
	{
		if (state == State.IDLE && validateQueue != null)
		{
			state = State.IDLE;
		}
	}

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		if (!buttonEnable)
		{
			button.ReleaseEvent += ReleaseEvent;
			state = State.IDLE;
			buttonEnable = true;
		}
	}

	private void OnDisable()
	{
		button.ReleaseEvent -= ReleaseEvent;
		buttonEnable = false;
	}

	private void ReleaseEvent(GUI3DOnReleaseEvent evn)
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			BeLordInApp.Instance.RestorePurchases(onRestorePurchasesDone);
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("Error", "Internet connection unavailable", "Error");
		}
	}

	private void onRestorePurchasesDone()
	{
		string[] allTransactions = BeLordInApp.Instance.GetAllTransactions();
		if (allTransactions != null)
		{
			bool flag = false;
			for (int i = 0; i < allTransactions.Length; i++)
			{
				if (string.Compare(allTransactions[i], "com.dedalord.runningfred.valuepack1") == 0)
				{
					LocationItemInfo locationItemInfo = Store.Instance.GetItem(1012) as LocationItemInfo;
					if (!locationItemInfo.Purchased)
					{
						locationItemInfo.Purchased = true;
						PlayerPrefsWrapper.PurchaseItem(locationItemInfo);
						ValuePackManager.Unlock();
					}
					flag = true;
				}
				else if (string.Compare(allTransactions[i], "com.dedalord.runningfred.valuepack2") == 0)
				{
					LocationItemInfo locationItemInfo2 = Store.Instance.GetItem(1014) as LocationItemInfo;
					if (!locationItemInfo2.Purchased)
					{
						locationItemInfo2.Purchased = true;
						PlayerPrefsWrapper.PurchaseItem(locationItemInfo2);
						ValuePack2Manager.Unlock();
					}
					flag = true;
				}
				else if (string.Compare(allTransactions[i], "com.dedalord.runningfred.valuepack3") == 0)
				{
					AvatarItemInfo avatarItemInfo = Store.Instance.GetItem(1018) as AvatarItemInfo;
					if (!avatarItemInfo.Purchased)
					{
						avatarItemInfo.Purchased = true;
						PlayerPrefsWrapper.PurchaseItem(avatarItemInfo);
						ValuePack3Manager.Unlock();
					}
					flag = true;
				}
			}
			if (flag)
			{
				GUI3DPopupManager.Instance.ShowPopup("RestorePurchaseOK");
			}
			else
			{
				GUI3DPopupManager.Instance.ShowPopup("RestorePurchaseNothingToRestore");
			}
		}
		else
		{
			GUI3DPopupManager.Instance.ShowPopup("RestorePurchaseNothingToRestore");
		}
	}
}
