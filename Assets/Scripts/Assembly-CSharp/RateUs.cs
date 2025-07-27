using UnityEngine;

public class RateUs : MonoBehaviour
{
	public static RateUs Instance;

	public float Timer = 3f;

	public int SessionsCount = 5;

	public int DaysCount = 1;

	public int RetryInDays = 1;

	public string URL_IPhone;

	public string URL_Android;

	public string URL_Kindle;

	private bool rateUs;

	private void Awake()
	{
		Instance = this;
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.CanRate() && Application.internetReachability != NetworkReachability.NotReachable && !ConfigParams.IsKongregate())
		{
			if (PlayerAccount.Instance.GetTotalSessions() >= SessionsCount && ((PlayerAccount.Instance.DaysFromFirstStart() >= DaysCount && !PlayerAccount.Instance.WasRateAsked()) || PlayerAccount.Instance.LastAskRate() >= RetryInDays))
			{
				rateUs = true;
			}
			else
			{
				base.enabled = false;
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	private void OnEnable()
	{
		Instance = this;
	}

	private void Update()
	{
		if (rateUs)
		{
			Timer -= Time.deltaTime;
			if (Timer <= 0f)
			{
				GUI3DPopupManager.Instance.ShowPopup("RateUs", OnClose, true);
				rateUs = false;
			}
		}
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		switch (result)
		{
		case GUI3DPopupManager.PopupResult.Yes:
			Application.OpenURL(URL_Android);
			PlayerAccount.Instance.CanRate(false);
			break;
		case GUI3DPopupManager.PopupResult.Cancel:
			PlayerAccount.Instance.SetLastAskRate();
			break;
		default:
			PlayerAccount.Instance.CanRate(false);
			break;
		}
	}
}
