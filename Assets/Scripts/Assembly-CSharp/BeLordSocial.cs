using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeLordSocial : MonoBehaviour
{
	private const float REFRESH_RATE = 10f;

	public string TwitterConsumerKey;

	public string TwitterConsumerSecret;

	public string FacebookAppId;

	private static BeLordSocial instance;

	private IBelordSocial beLordSocial;

	private bool lastInternetState;

	private float refreshTimeCounter;

	public static BeLordSocial Instance
	{
		get
		{
			return instance;
		}
	}

	public static event Action twitterLogin;

	public static event Action<string> twitterLoginFailed;

	public static event Action twitterPost;

	public static event Action<string> twitterPostFailed;

	public static event Action<ArrayList> twitterHomeTimelineReceived;

	public static event Action<string> twitterHomeTimelineFailed;

	public static event Action<object> twitterRequestDidFinishEvent;

	public static event Action<string> twitterRequestDidFailEvent;

	public static event Action facebookLoginSucceededEvent;

	public static event Action<string> facebookLoginFailedEvent;

	public static event Action facebookLoggedOutEvent;

	public static event Action<DateTime> facebookAccessTokenExtendedEvent;

	public static event Action facebookFailedToExtendTokenEvent;

	public static event Action facebookSessionInvalidatedEvent;

	public static event Action facebookDialogCompletedEvent;

	public static event Action<string> facebookDialogFailedEvent;

	public static event Action facebookDialogDidNotCompleteEvent;

	public static event Action<string> facebookDialogCompletedWithUrlEvent;

	public static event Action<object> facebookCustomRequestReceivedEvent;

	public static event Action<string> facebookCustomRequestFailedEvent;

	private BeLordSocial()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			beLordSocial = new BeLordSocialIOSAndroid();
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		bool flag = Application.internetReachability != NetworkReachability.NotReachable;
		if (flag)
		{
			InitTwitter();
			if (!ConfigParams.isKindle)
			{
				InitFacebook();
			}
		}
		lastInternetState = flag;
	}

	public bool IsSocialApiAvailable()
	{
		if (ConfigParams.isKindle)
		{
			return false;
		}
		return beLordSocial != null;
	}

	public void InitTwitter()
	{
		if (beLordSocial != null && TwitterConsumerKey != string.Empty && TwitterConsumerSecret != string.Empty)
		{
			BeLordSocialIOSAndroid.twitterHomeTimelineFailed -= OnTwitterHomeTimelineFailed;
			BeLordSocialIOSAndroid.twitterHomeTimelineReceived -= OnTwitterHomeTimelineReceived;
			BeLordSocialIOSAndroid.twitterLogin -= OnTwitterLogin;
			BeLordSocialIOSAndroid.twitterLoginFailed -= OnTwitterLoginFailed;
			BeLordSocialIOSAndroid.twitterPost -= OnTwitterPost;
			BeLordSocialIOSAndroid.twitterPostFailed -= OnTwitterPostFailed;
			BeLordSocialIOSAndroid.twitterRequestDidFailEvent -= OnTwitterRequestDidFailEvent;
			BeLordSocialIOSAndroid.twitterRequestDidFinishEvent -= OnTwitterRequestDidFinishEvent;
			BeLordSocialIOSAndroid.twitterHomeTimelineFailed += OnTwitterHomeTimelineFailed;
			BeLordSocialIOSAndroid.twitterHomeTimelineReceived += OnTwitterHomeTimelineReceived;
			BeLordSocialIOSAndroid.twitterLogin += OnTwitterLogin;
			BeLordSocialIOSAndroid.twitterLoginFailed += OnTwitterLoginFailed;
			BeLordSocialIOSAndroid.twitterPost += OnTwitterPost;
			BeLordSocialIOSAndroid.twitterPostFailed += OnTwitterPostFailed;
			BeLordSocialIOSAndroid.twitterRequestDidFailEvent += OnTwitterRequestDidFailEvent;
			BeLordSocialIOSAndroid.twitterRequestDidFinishEvent += OnTwitterRequestDidFinishEvent;
			beLordSocial.InitTwitter(TwitterConsumerKey, TwitterConsumerSecret);
		}
	}

	public bool IsTwitterLoggedIn()
	{
		if (beLordSocial != null)
		{
			return beLordSocial.IsTwitterLoggedIn();
		}
		return false;
	}

	public string LoggedInTwitterUsername()
	{
		if (beLordSocial != null)
		{
			return beLordSocial.LoggedInTwitterUsername();
		}
		return string.Empty;
	}

	public void LoginTwitter(string username, string password)
	{
		if (beLordSocial != null)
		{
			beLordSocial.LoginTwitter(username, password);
		}
		else if (BeLordSocial.twitterLoginFailed != null)
		{
			BeLordSocial.twitterLoginFailed("API not available");
		}
	}

	public void ShowTwitterOauthLoginDialog()
	{
		if (beLordSocial != null)
		{
			beLordSocial.ShowTwitterOauthLoginDialog();
		}
		else if (BeLordSocial.twitterLoginFailed != null)
		{
			BeLordSocial.twitterLoginFailed("API not available");
		}
	}

	public void LogoutTwitter()
	{
		if (beLordSocial != null)
		{
			beLordSocial.LogoutTwitter();
		}
	}

	public void PostTwitterStatusUpdate(string status)
	{
		if (beLordSocial != null)
		{
			beLordSocial.PostTwitterStatusUpdate(status);
		}
		else if (BeLordSocial.twitterPostFailed != null)
		{
			BeLordSocial.twitterPostFailed("API not available");
		}
	}

	public void PostTwitterStatusUpdate(string status, string pathToImage)
	{
		if (beLordSocial != null)
		{
			beLordSocial.PostTwitterStatusUpdate(status, pathToImage);
		}
		else if (BeLordSocial.twitterPostFailed != null)
		{
			BeLordSocial.twitterPostFailed("API not available");
		}
	}

	public void GetTwitterHomeTimeline()
	{
		if (beLordSocial != null)
		{
			beLordSocial.GetTwitterHomeTimeline();
		}
		else if (BeLordSocial.twitterHomeTimelineFailed != null)
		{
			BeLordSocial.twitterHomeTimelineFailed("API not available");
		}
	}

	public void PerformTwitterRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
		if (beLordSocial != null)
		{
			beLordSocial.PerformTwitterRequest(methodType, path, parameters);
		}
		else if (BeLordSocial.twitterRequestDidFailEvent != null)
		{
			BeLordSocial.twitterRequestDidFailEvent("API not available");
		}
	}

	public bool IsTweetSheetSupported()
	{
		if (beLordSocial != null)
		{
			return beLordSocial.IsTweetSheetSupported();
		}
		return false;
	}

	public bool CanUserTweet()
	{
		if (beLordSocial != null)
		{
			return beLordSocial.CanUserTweet();
		}
		return false;
	}

	public void ShowTweetComposer(string status, string pathToImage)
	{
		if (beLordSocial != null)
		{
			beLordSocial.ShowTweetComposer(status, pathToImage);
		}
	}

	public void InitFacebook()
	{
		if (beLordSocial != null && FacebookAppId != string.Empty)
		{
			BeLordSocialIOSAndroid.facebookAccessTokenExtendedEvent -= OnAccessTokenExtendedEvent;
			BeLordSocialIOSAndroid.facebookCustomRequestFailedEvent -= OnCustomRequestFailedEvent;
			BeLordSocialIOSAndroid.facebookCustomRequestReceivedEvent -= OnCustomRequestReceivedEvent;
			BeLordSocialIOSAndroid.facebookDialogCompletedEvent -= OnDialogCompletedEvent;
			BeLordSocialIOSAndroid.facebookDialogCompletedWithUrlEvent -= OnDialogCompletedWithUrlEvent;
			BeLordSocialIOSAndroid.facebookDialogDidNotCompleteEvent -= OnDialogDidNotCompleteEvent;
			BeLordSocialIOSAndroid.facebookDialogFailedEvent -= OnDialogFailedEvent;
			BeLordSocialIOSAndroid.facebookFailedToExtendTokenEvent -= OnFailedToExtendTokenEvent;
			BeLordSocialIOSAndroid.facebookLoggedOutEvent -= OnLoggedOutEvent;
			BeLordSocialIOSAndroid.facebookLoginFailedEvent -= OnLoginFailedEvent;
			BeLordSocialIOSAndroid.facebookLoginSucceededEvent -= OnLoginSucceededEvent;
			BeLordSocialIOSAndroid.facebookSessionInvalidatedEvent -= OnSessionInvalidatedEvent;
			BeLordSocialIOSAndroid.facebookAccessTokenExtendedEvent += OnAccessTokenExtendedEvent;
			BeLordSocialIOSAndroid.facebookCustomRequestFailedEvent += OnCustomRequestFailedEvent;
			BeLordSocialIOSAndroid.facebookCustomRequestReceivedEvent += OnCustomRequestReceivedEvent;
			BeLordSocialIOSAndroid.facebookDialogCompletedEvent += OnDialogCompletedEvent;
			BeLordSocialIOSAndroid.facebookDialogCompletedWithUrlEvent += OnDialogCompletedWithUrlEvent;
			BeLordSocialIOSAndroid.facebookDialogDidNotCompleteEvent += OnDialogDidNotCompleteEvent;
			BeLordSocialIOSAndroid.facebookDialogFailedEvent += OnDialogFailedEvent;
			BeLordSocialIOSAndroid.facebookFailedToExtendTokenEvent += OnFailedToExtendTokenEvent;
			BeLordSocialIOSAndroid.facebookLoggedOutEvent += OnLoggedOutEvent;
			BeLordSocialIOSAndroid.facebookLoginFailedEvent += OnLoginFailedEvent;
			BeLordSocialIOSAndroid.facebookLoginSucceededEvent += OnLoginSucceededEvent;
			BeLordSocialIOSAndroid.facebookSessionInvalidatedEvent += OnSessionInvalidatedEvent;
			beLordSocial.InitFacebook(FacebookAppId);
		}
	}

	public bool IsFacebookSessionValid()
	{
		if (!ConfigParams.isKindle && beLordSocial != null)
		{
			return beLordSocial.IsFacebookSessionValid();
		}
		return false;
	}

	public string GetFacebookAccessToken()
	{
		if (!ConfigParams.isKindle && beLordSocial != null)
		{
			return beLordSocial.GetFacebookAccessToken();
		}
		return string.Empty;
	}

	public void ExtendFacebookAccessToken()
	{
		if (!ConfigParams.isKindle && beLordSocial != null)
		{
			beLordSocial.ExtendFacebookAccessToken();
		}
	}

	public void LoginFacebook()
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.LoginFacebook();
			}
			else if (BeLordSocial.facebookLoginFailedEvent != null)
			{
				BeLordSocial.facebookLoginFailedEvent("API not available");
			}
		}
	}

	public void LoginFacebookWithRequestedPermissions(string[] permissions)
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.LoginFacebookWithRequestedPermissions(permissions);
			}
			else if (BeLordSocial.facebookLoginFailedEvent != null)
			{
				BeLordSocial.facebookLoginFailedEvent("API not available");
			}
		}
	}

	public void LoginFacebookWithRequestedPermissions(string[] permissions, string urlSchemeSuffix)
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.LoginFacebookWithRequestedPermissions(permissions, urlSchemeSuffix);
			}
			else if (BeLordSocial.facebookLoginFailedEvent != null)
			{
				BeLordSocial.facebookLoginFailedEvent("API not available");
			}
		}
	}

	public void LogoutFacebook()
	{
		if (!ConfigParams.isKindle && beLordSocial != null)
		{
			beLordSocial.LogoutFacebook();
		}
	}

	public void ShowFacebookPostMessageDialog()
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.ShowFacebookPostMessageDialog();
			}
			else if (BeLordSocial.facebookDialogFailedEvent != null)
			{
				BeLordSocial.facebookDialogFailedEvent("API not available");
			}
		}
	}

	public void ShowFacebookPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.ShowFacebookPostMessageDialogWithOptions(link, linkName, linkToImage, caption);
			}
			else if (BeLordSocial.facebookDialogFailedEvent != null)
			{
				BeLordSocial.facebookDialogFailedEvent("API not available");
			}
		}
	}

	public void ShowFacebookDialog(string dialogType, Dictionary<string, string> options)
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.ShowFacebookDialog(dialogType, options);
			}
			else if (BeLordSocial.facebookDialogFailedEvent != null)
			{
				BeLordSocial.facebookDialogFailedEvent("API not available");
			}
		}
	}

	public void FacebookRestRequest(string restMethod, string httpMethod, Hashtable keyValueHash)
	{
		if (!ConfigParams.isKindle)
		{
			if (beLordSocial != null)
			{
				beLordSocial.FacebookRestRequest(restMethod, httpMethod, keyValueHash);
			}
			else if (BeLordSocial.facebookCustomRequestFailedEvent != null)
			{
				BeLordSocial.facebookCustomRequestFailedEvent("API not available");
			}
		}
	}

	private void OnTwitterLogin()
	{
		if (BeLordSocial.twitterLogin != null)
		{
			BeLordSocial.twitterLogin();
		}
	}

	private void OnTwitterLoginFailed(string error)
	{
		if (BeLordSocial.twitterLoginFailed != null)
		{
			BeLordSocial.twitterLoginFailed(error);
		}
	}

	private void OnTwitterPost()
	{
		if (BeLordSocial.twitterPost != null)
		{
			BeLordSocial.twitterPost();
		}
	}

	private void OnTwitterPostFailed(string error)
	{
		if (BeLordSocial.twitterPostFailed != null)
		{
			BeLordSocial.twitterPostFailed(error);
		}
	}

	private void OnTwitterHomeTimelineReceived(ArrayList data)
	{
		if (BeLordSocial.twitterHomeTimelineReceived != null)
		{
			BeLordSocial.twitterHomeTimelineReceived(data);
		}
	}

	private void OnTwitterHomeTimelineFailed(string error)
	{
		if (BeLordSocial.twitterHomeTimelineFailed != null)
		{
			BeLordSocial.twitterHomeTimelineFailed(error);
		}
	}

	private void OnTwitterRequestDidFinishEvent(object obj)
	{
		if (BeLordSocial.twitterRequestDidFinishEvent != null)
		{
			BeLordSocial.twitterRequestDidFinishEvent(obj);
		}
	}

	private void OnTwitterRequestDidFailEvent(string error)
	{
		if (BeLordSocial.twitterRequestDidFailEvent != null)
		{
			BeLordSocial.twitterRequestDidFailEvent(error);
		}
	}

	private void OnLoginSucceededEvent()
	{
		if (BeLordSocial.facebookLoginSucceededEvent != null)
		{
			BeLordSocial.facebookLoginSucceededEvent();
		}
	}

	private void OnLoginFailedEvent(string error)
	{
		if (BeLordSocial.facebookLoginFailedEvent != null)
		{
			BeLordSocial.facebookLoginFailedEvent(error);
		}
	}

	private void OnLoggedOutEvent()
	{
		if (BeLordSocial.facebookLoggedOutEvent != null)
		{
			BeLordSocial.facebookLoggedOutEvent();
		}
	}

	private void OnAccessTokenExtendedEvent(DateTime date)
	{
		if (BeLordSocial.facebookAccessTokenExtendedEvent != null)
		{
			BeLordSocial.facebookAccessTokenExtendedEvent(date);
		}
	}

	private void OnFailedToExtendTokenEvent()
	{
		if (BeLordSocial.facebookFailedToExtendTokenEvent != null)
		{
			BeLordSocial.facebookFailedToExtendTokenEvent();
		}
	}

	private void OnSessionInvalidatedEvent()
	{
		if (BeLordSocial.facebookSessionInvalidatedEvent != null)
		{
			BeLordSocial.facebookSessionInvalidatedEvent();
		}
	}

	private void OnDialogCompletedEvent()
	{
		if (BeLordSocial.facebookDialogCompletedEvent != null)
		{
			BeLordSocial.facebookDialogCompletedEvent();
		}
	}

	private void OnDialogFailedEvent(string error)
	{
		if (BeLordSocial.facebookDialogFailedEvent != null)
		{
			BeLordSocial.facebookDialogFailedEvent(error);
		}
	}

	private void OnDialogDidNotCompleteEvent()
	{
		if (BeLordSocial.facebookDialogDidNotCompleteEvent != null)
		{
			BeLordSocial.facebookDialogDidNotCompleteEvent();
		}
	}

	private void OnDialogCompletedWithUrlEvent(string error)
	{
		if (BeLordSocial.facebookDialogCompletedWithUrlEvent != null)
		{
			BeLordSocial.facebookDialogCompletedWithUrlEvent(error);
		}
	}

	private void OnCustomRequestReceivedEvent(object data)
	{
		if (BeLordSocial.facebookCustomRequestReceivedEvent != null)
		{
			BeLordSocial.facebookCustomRequestReceivedEvent(data);
		}
	}

	private void OnCustomRequestFailedEvent(string error)
	{
		if (BeLordSocial.facebookCustomRequestFailedEvent != null)
		{
			BeLordSocial.facebookCustomRequestFailedEvent(error);
		}
	}

	private void Update()
	{
		if (refreshTimeCounter > 10f)
		{
			bool flag = Application.internetReachability != NetworkReachability.NotReachable;
			if (flag != lastInternetState)
			{
				if (flag)
				{
					InitTwitter();
					if (!ConfigParams.isKindle)
					{
						InitFacebook();
					}
				}
				lastInternetState = flag;
			}
			refreshTimeCounter = 0f;
		}
		refreshTimeCounter += Time.deltaTime;
	}
}
