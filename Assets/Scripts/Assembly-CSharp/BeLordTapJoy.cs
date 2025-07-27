using System;
using TapjoyUnity;
using UnityEngine;

public class BeLordTapJoy : MonoBehaviour
{
	public enum StatusType
	{
		None = 0,
		PlacementToShow = 1,
		OfferWall = 2,
		ViewOpen = 3,
		PlayingVideo = 4,
		Resuming = 5
	}

	private string lastPlacementName;

	public float timeOut = 10f;

	public float timeOutGetTapjoyPoints = 30f;

	public static bool DebugMode;

	private float timeOutSetTime;

	private bool gettingTapPoints;

	private bool wattingForGetBalance;

	private TJPlacement lastPlacement;

	private static StatusType CurrentStatus;

	private static BeLordTapJoy instance;

	public static bool IsReadyToUse
	{
		get
		{
			bool result = false;
			if (instance != null)
			{
				result = Application.internetReachability != NetworkReachability.NotReachable && Tapjoy.IsConnected;
			}
			return result;
		}
	}

	public static BeLordTapJoy Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "BeLordTapJoy";
				instance = gameObject.AddComponent<BeLordTapJoy>();
			}
			return instance;
		}
	}

	public bool IsLoadingPlacement
	{
		get
		{
			return lastPlacement != null && !lastPlacement.IsContentReady();
		}
	}

	public bool ContentAvailableToShow
	{
		get
		{
			return lastPlacement != null && lastPlacement.IsContentReady();
		}
	}

	public static event Action callbackOnConnectSuccess;

	public static event Action callbackOnConnectFailure;

	public static event Action<int> callbackOnGetCurrencyBalanceResponse;

	public static event Action callbackOnGetCurrencyBalanceResponseFailure;

	public static event Action<int> callbackOnSpendCurrencyResponse;

	public static event Action callbackOnSpendCurrencyResponseFailure;

	public static event Action<int> callbackOnAwardCurrencyResponse;

	public static event Action callbackOnAwardCurrencyResponseFailure;

	public static event Action<int> callbackOnEarnedCurrency;

	public static event Action callbackOnOffersResponse;

	public static event Action callbackOnOffersResponseFailure;

	public static event Action callbackOnVideoStart;

	public static event Action callbackOnVideoError;

	public static event Action callbackOnVideoComplete;

	public static event Action callbackOnViewDidClose;

	public static event Action callbackOnViewDidOpen;

	public static event Action callbackOnViewWillClose;

	public static event Action callbackOnViewWillOpen;

	public static event Action callbackResume;

	private void Awake()
	{
		if (DebugMode)
		{
			Debug.Log("BeLordTapJoy.Awake()");
		}
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Tapjoy.OnConnectSuccess += TapjoyOnConnectSuccess;
		Tapjoy.OnConnectFailure += TapjoyOnConnectFailure;
		Tapjoy.OnGetCurrencyBalanceResponse += TapjoyOnGetCurrencyBalanceResponse;
		Tapjoy.OnGetCurrencyBalanceResponseFailure += TapjoyOnGetCurrencyBalanceResponseFailure;
		Tapjoy.OnSpendCurrencyResponse += TapjoyOnSpendCurrencyResponse;
		Tapjoy.OnSpendCurrencyResponseFailure += TapjoyOnSpendCurrencyResponseFailure;
		Tapjoy.OnAwardCurrencyResponse += TapjoyOnAwardCurrencyResponse;
		Tapjoy.OnAwardCurrencyResponseFailure += TapjoyOnAwardCurrencyResponseFailure;
		Tapjoy.OnEarnedCurrency += TapjoyOnEarnedCurrency;
		Tapjoy.OnOffersResponse += TapjoyOnOffersResponse;
		Tapjoy.OnOffersResponseFailure += TapjoyOnOffersResponseFailure;
		Tapjoy.OnVideoStart += TapjoyOnVideoStart;
		Tapjoy.OnVideoComplete += TapjoyOnVideoComplete;
		Tapjoy.OnVideoError += TapjoyOnVideoError;
		Tapjoy.OnViewDidClose += TapjoyOnViewDidClose;
		Tapjoy.OnViewDidOpen += TapjoyOnViewDidOpen;
		Tapjoy.OnViewWillClose += TapjoyOnViewWillClose;
		Tapjoy.OnViewWillOpen += TapjoyOnViewWillOpen;
		TJPlacement.OnRequestSuccess += TJPlacementOnRequestSuccess;
		TJPlacement.OnRequestFailure += TJPlacementOnRequestFailure;
		TJPlacement.OnContentReady += TJPlacementOnContentReady;
		TJPlacement.OnContentShow += TJPlacementOnContentShow;
		TJPlacement.OnContentDismiss += TJPlacementOnContentDismiss;
		TJPlacement.OnPurchaseRequest += TJPlacementOnPurchaseRequest;
		TJPlacement.OnRewardRequest += TJPlacementOnRewardRequest;
		TapjoyPlacementsManager.init();
	}

	private void Start()
	{
	}

	public void Connect()
	{
		if (!Tapjoy.IsConnected)
		{
			Tapjoy.OnConnectSuccess += OnFirstTimeConnect;
			Tapjoy.Connect();
		}
	}

	public void ActionComplete(string actionID)
	{
		Tapjoy.ActionComplete(actionID);
	}

	public void SetUserID(string userID)
	{
		Tapjoy.SetUserID(userID);
	}

	public void ShowOffers()
	{
		if (CurrentStatus == StatusType.None)
		{
			CurrentStatus = StatusType.OfferWall;
			timeOutSetTime = Time.unscaledTime;
			Tapjoy.ShowOffers();
		}
	}

	public void GetCurrencyBalance()
	{
		timeOutGetTapjoyPoints = Time.unscaledTime;
		gettingTapPoints = true;
		Tapjoy.GetCurrencyBalance();
	}

	public void SpendCurrency(int points)
	{
		gettingTapPoints = false;
		Tapjoy.SpendCurrency(points);
	}

	public void AwardCurrency(int points)
	{
		Tapjoy.AwardCurrency(points);
	}

	public void PlacementLoad(string placementName)
	{
		if (lastPlacement != null)
		{
			return;
		}
		TJPlacement tJPlacement = TJPlacement.CreatePlacement(placementName);
		if (tJPlacement != null)
		{
			tJPlacement.RequestContent();
			if (tJPlacement.IsContentAvailable() && tJPlacement.IsContentReady())
			{
				tJPlacement.ShowContent();
				OnPlayingVideo();
			}
		}
		lastPlacement = tJPlacement;
		lastPlacementName = placementName;
		timeOutSetTime = Time.unscaledTime;
	}

	public void PlacementLoadAndShow(string placementName)
	{
		if (CurrentStatus == StatusType.None)
		{
			CurrentStatus = StatusType.PlacementToShow;
			PlacementLoad(placementName);
		}
	}

	private void Update()
	{
		if (gettingTapPoints && Time.unscaledTime - timeOutGetTapjoyPoints >= timeOutGetTapjoyPoints)
		{
			gettingTapPoints = false;
			timeOutGetTapjoyPoints = 0f;
			if (BeLordTapJoy.callbackOnGetCurrencyBalanceResponseFailure != null)
			{
				BeLordTapJoy.callbackOnGetCurrencyBalanceResponseFailure();
			}
		}
		if (wattingForGetBalance && Time.unscaledTime - timeOutSetTime > timeOut / 4f)
		{
			wattingForGetBalance = false;
			GetCurrencyBalance();
		}
		if (CurrentStatus != StatusType.None && Time.unscaledTime - timeOutSetTime > timeOut)
		{
			switch (CurrentStatus)
			{
			case StatusType.PlacementToShow:
				OnOwnPlacementFailure();
				break;
			case StatusType.OfferWall:
			case StatusType.Resuming:
				OnResume();
				break;
			case StatusType.ViewOpen:
			case StatusType.PlayingVideo:
				break;
			}
		}
	}

	private void OnViewClosed()
	{
		CurrentStatus = StatusType.Resuming;
		lastPlacement = null;
		timeOutSetTime = Time.unscaledTime;
		wattingForGetBalance = true;
	}

	private void OnViewOpened()
	{
		CurrentStatus = StatusType.ViewOpen;
		timeOutSetTime = Time.unscaledTime + timeOut * 6f;
		wattingForGetBalance = false;
	}

	private void OnPlayingVideo()
	{
		CurrentStatus = StatusType.PlayingVideo;
		timeOutSetTime = Time.unscaledTime + timeOut * 6f;
		wattingForGetBalance = false;
	}

	private void OnOwnPlacementFailure()
	{
		lastPlacement = null;
		OnResume();
	}

	private void OnResume()
	{
		CurrentStatus = StatusType.None;
		wattingForGetBalance = false;
		if (BeLordTapJoy.callbackResume != null)
		{
			BeLordTapJoy.callbackResume();
		}
	}

	private void TapjoyOnConnectSuccess()
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnConnectSuccess");
		}
		if (BeLordTapJoy.callbackOnConnectSuccess != null)
		{
			BeLordTapJoy.callbackOnConnectSuccess();
		}
	}

	private void OnFirstTimeConnect()
	{
		if (DebugMode)
		{
			Debug.Log("FirstTimeConnect");
		}
		Tapjoy.OnConnectSuccess -= OnFirstTimeConnect;
		Tapjoy.GetCurrencyBalance();
	}

	private void TapjoyOnConnectFailure()
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnConnectFailure");
		}
		if (BeLordTapJoy.callbackOnConnectFailure != null)
		{
			BeLordTapJoy.callbackOnConnectFailure();
		}
	}

	private void TapjoyOnGetCurrencyBalanceResponse(string currencyName, int balance)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnGetCurrencyBalanceResponse currencyName: " + currencyName + " balance: " + balance);
		}
		gettingTapPoints = false;
		if (BeLordTapJoy.callbackOnGetCurrencyBalanceResponse != null)
		{
			BeLordTapJoy.callbackOnGetCurrencyBalanceResponse(balance);
		}
		if (CurrentStatus == StatusType.Resuming)
		{
			OnResume();
		}
	}

	private void TapjoyOnGetCurrencyBalanceResponseFailure(string errorMessage)
	{
		gettingTapPoints = false;
		if (DebugMode)
		{
			Debug.Log("TapjoyOnGetCurrencyBalanceResponseFailure errorMessage: " + errorMessage);
		}
		if (BeLordTapJoy.callbackOnGetCurrencyBalanceResponseFailure != null)
		{
			BeLordTapJoy.callbackOnGetCurrencyBalanceResponseFailure();
		}
	}

	private void TapjoyOnSpendCurrencyResponse(string currencyName, int balance)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnSpendCurrencyResponse currencyName: " + currencyName + " balance: " + balance);
		}
		if (BeLordTapJoy.callbackOnSpendCurrencyResponse != null)
		{
			BeLordTapJoy.callbackOnSpendCurrencyResponse(balance);
		}
	}

	private void TapjoyOnSpendCurrencyResponseFailure(string errorMessage)
	{
		if (DebugMode)
		{
			Debug.Log("TapJoyOnSpendCurrencyResponseFailure errorMessage: " + errorMessage);
		}
		if (BeLordTapJoy.callbackOnSpendCurrencyResponseFailure != null)
		{
			BeLordTapJoy.callbackOnSpendCurrencyResponseFailure();
		}
	}

	private void TapjoyOnAwardCurrencyResponse(string currencyName, int balance)
	{
		if (DebugMode)
		{
			Debug.Log("TapJoyOnAwardCurrencyResponse currencyName: " + currencyName + " balance: " + balance);
		}
		if (BeLordTapJoy.callbackOnAwardCurrencyResponse != null)
		{
			BeLordTapJoy.callbackOnAwardCurrencyResponse(balance);
		}
	}

	private void TapjoyOnAwardCurrencyResponseFailure(string errorMessage)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnAwardCurrencyResponseFailure errorMessage: " + errorMessage);
		}
		if (BeLordTapJoy.callbackOnAwardCurrencyResponseFailure != null)
		{
			BeLordTapJoy.callbackOnAwardCurrencyResponseFailure();
		}
	}

	private void TapjoyOnEarnedCurrency(string currencyName, int amount)
	{
		if (DebugMode)
		{
			Debug.Log("HandleTapPointsEarned currencyName: " + currencyName + " amount: " + amount);
		}
		if (BeLordTapJoy.callbackOnEarnedCurrency != null)
		{
			BeLordTapJoy.callbackOnEarnedCurrency(amount);
		}
		if (CurrentStatus == StatusType.Resuming)
		{
			OnResume();
		}
	}

	private void TapjoyOnOffersResponse()
	{
		if (DebugMode)
		{
			Debug.Log("TapJoyOnOffersResponse ");
		}
		if (BeLordTapJoy.callbackOnOffersResponse != null)
		{
			BeLordTapJoy.callbackOnOffersResponse();
		}
	}

	private void TapjoyOnOffersResponseFailure(string errorMessage)
	{
		if (DebugMode)
		{
			Debug.Log("TapJoyOnOffersResponseFailure errorMessage:" + errorMessage);
		}
		if (BeLordTapJoy.callbackOnOffersResponseFailure != null)
		{
			BeLordTapJoy.callbackOnOffersResponseFailure();
		}
	}

	private void TapjoyOnVideoStart()
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnVideoStart");
		}
		if (BeLordTapJoy.callbackOnVideoStart != null)
		{
			BeLordTapJoy.callbackOnVideoStart();
		}
	}

	private void TapjoyOnVideoError(string errorMessage)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnVideoError errorMessage: " + errorMessage);
		}
		if (BeLordTapJoy.callbackOnVideoError != null)
		{
			BeLordTapJoy.callbackOnVideoError();
		}
	}

	private void TapjoyOnVideoComplete()
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnVideoComplete");
		}
		if (BeLordTapJoy.callbackOnVideoComplete != null)
		{
			BeLordTapJoy.callbackOnVideoComplete();
		}
	}

	private void TapjoyOnViewDidClose(int viewType)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnViewDidClose viewType: " + viewType);
		}
		OnViewClosed();
		if (BeLordTapJoy.callbackOnViewDidClose != null)
		{
			BeLordTapJoy.callbackOnViewDidClose();
		}
	}

	private void TapjoyOnViewDidOpen(int viewType)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnViewDidOpen viewType: " + viewType);
		}
		OnViewOpened();
		if (BeLordTapJoy.callbackOnViewDidOpen != null)
		{
			BeLordTapJoy.callbackOnViewDidOpen();
		}
	}

	private void TapjoyOnViewWillClose(int viewType)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnViewWillClose viewType: " + viewType);
		}
		if (BeLordTapJoy.callbackOnViewWillClose != null)
		{
			BeLordTapJoy.callbackOnViewWillClose();
		}
	}

	private void TapjoyOnViewWillOpen(int viewType)
	{
		if (DebugMode)
		{
			Debug.Log("TapjoyOnViewWillOpen viewType: " + viewType);
		}
		if (BeLordTapJoy.callbackOnViewWillOpen != null)
		{
			BeLordTapJoy.callbackOnViewWillOpen();
		}
	}

	private void TJPlacementOnRequestSuccess(TJPlacement placement)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnRequestSuccess placement: " + placement.GetName());
		}
		if (placement.IsContentAvailable() && placement.GetName() == lastPlacementName)
		{
			placement.ShowContent();
			OnPlayingVideo();
		}
	}

	private void TJPlacementOnRequestFailure(TJPlacement placement, string error)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnRequestFailure placement: " + placement.GetName() + " error: " + error);
		}
		if (placement.GetName() == lastPlacementName)
		{
			OnOwnPlacementFailure();
		}
	}

	private void TJPlacementOnContentReady(TJPlacement placement)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnContentReady placement: " + placement.GetName());
		}
		if (!placement.IsContentAvailable())
		{
		}
	}

	private void TJPlacementOnContentShow(TJPlacement placement)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnContentShow placement: " + placement.GetName());
		}
	}

	private void TJPlacementOnContentDismiss(TJPlacement placement)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnContentDismiss placement: " + placement.GetName());
		}
	}

	private void TJPlacementOnPurchaseRequest(TJPlacement placement, TJActionRequest request, string productId)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnPurchaseRequest placement: " + placement.GetName());
		}
		request.Completed();
	}

	private void TJPlacementOnRewardRequest(TJPlacement placement, TJActionRequest request, string itemId, int quantity)
	{
		if (DebugMode)
		{
			Debug.Log("TJPlacementOnRewardRequest placement: " + placement.GetName());
		}
		request.Completed();
	}
}
