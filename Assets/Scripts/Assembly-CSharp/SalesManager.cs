using System;
using System.Text;
using UnityEngine;

public class SalesManager : MonoBehaviour
{
	public delegate void OnCheckOnSales(OnSale sale);

	private OnSale currentSale;

	private static SalesManager instance;

	private string key = "LAIjso8f8ays98&(*&^*&%*(SG";

	private DateTime lastRequest;

	public OnSale CurrentSale
	{
		get
		{
			return currentSale;
		}
	}

	public static SalesManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(SalesManager)) as SalesManager;
			}
			return instance;
		}
	}

	public event OnCheckOnSales OnSaleRequest;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(instance);
	}

	public void GetCurrentSale()
	{
		int totalSessions = PlayerPrefsWrapper.GetTotalSessions();
		if (totalSessions > 1)
		{
			if (!LoadCurrentSale() && Application.internetReachability != NetworkReachability.NotReachable && CheckLastRequest())
			{
				RequestOffer();
			}
			else if (this.OnSaleRequest != null)
			{
				this.OnSaleRequest(currentSale);
			}
		}
		else if (this.OnSaleRequest != null)
		{
			this.OnSaleRequest(null);
		}
	}

	private bool CheckLastRequest()
	{
		string text = PlayerPrefs.GetString("LastOfferRequestDate", string.Empty);
		if (text != string.Empty)
		{
			DateTime dateTime = DateTime.Parse(text);
			if (lastRequest < dateTime.Date)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private void RequestOffer()
	{
		if (DailyOfferBackEnd.Instance != null)
		{
			DailyOfferBackEnd.Instance.GetOffer(OnGetOfferRes);
		}
		else if (this.OnSaleRequest != null)
		{
			this.OnSaleRequest(currentSale);
		}
	}

	private void OnGetOfferRes(bool res, DateTime dt, int discount, int itemId)
	{
		if (res)
		{
			lastRequest = DateTime.UtcNow.Date;
			PlayerPrefs.SetString("LastOfferRequestDate", lastRequest.ToShortDateString());
			if (itemId == -2)
			{
				return;
			}
			currentSale = new OnSale(dt, discount, itemId);
			SaveCurrentSale();
		}
		if (this.OnSaleRequest != null)
		{
			this.OnSaleRequest(currentSale);
		}
	}

	private bool LoadCurrentSale()
	{
		currentSale = null;
		string text = PlayerPrefs.GetString("CurrentOffer", string.Empty);
		if (text != string.Empty)
		{
			byte[] data = StringUtil.DecodeFrom64ToByteArray(text);
			byte[] bytes = XOREncryption.Decrypt(data, key);
			string text2 = Encoding.ASCII.GetString(bytes);
			string[] array = text2.Split(new char[1] { ';' }, StringSplitOptions.None);
			string text3 = array[0];
			string text4 = array[1];
			string text5 = array[2];
			if (text3 != null && text3 != string.Empty)
			{
				DateTime dateTime = DateTime.Parse(text3);
				if (dateTime == DateTime.UtcNow.Date)
				{
					float num = 1f;
					int itemId = -1;
					if (text4 != null && text4 != string.Empty)
					{
						num = int.Parse(text4);
					}
					if (text5 != null && text5 != string.Empty)
					{
						itemId = int.Parse(text5);
					}
					if (num != 1f)
					{
						currentSale = new OnSale(dateTime, num, itemId);
						base.enabled = false;
						return true;
					}
				}
			}
		}
		return false;
	}

	private void SaveCurrentSale()
	{
		string text = currentSale.FromDate.Date.ToShortDateString();
		text = text + ";" + currentSale.RealDiscount;
		text = text + ";" + currentSale.ItemId;
		byte[] bytes = Encoding.ASCII.GetBytes(text);
		byte[] toEncode = XOREncryption.Encrypt(bytes, key);
		string value = StringUtil.EncodeTo64(toEncode);
		PlayerPrefs.SetString("CurrentOffer", value);
	}
}
