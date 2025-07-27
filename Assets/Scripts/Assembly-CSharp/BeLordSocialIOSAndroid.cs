using System;
using System.Collections;
using System.Collections.Generic;

public class BeLordSocialIOSAndroid : IBelordSocial
{
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

	public void InitTwitter(string consumerKey, string consumerSecret)
	{
		TwitterAndroidManager.loginDidFailEvent += OnTwitterLoginFailed;
		TwitterAndroidManager.loginDidSucceedEvent += OnTwitterLogin;
		TwitterAndroidManager.requestFailedEvent += OnTwitterRequestDidFailEvent;
		TwitterAndroidManager.requestSucceededEvent += OnTwitterRequestDidFinishEvent;
		TwitterAndroid.init(consumerKey, consumerSecret);
	}

	public bool IsTwitterLoggedIn()
	{
		return TwitterAndroid.isLoggedIn();
	}

	public string LoggedInTwitterUsername()
	{
		return null;
	}

	public void LoginTwitter(string username, string password)
	{
		TwitterAndroid.init(username, password);
	}

	public void ShowTwitterOauthLoginDialog()
	{
		TwitterAndroid.showLoginDialog();
	}

	public void LogoutTwitter()
	{
		TwitterAndroid.logout();
	}

	public void PostTwitterStatusUpdate(string status)
	{
		TwitterAndroid.postUpdate(status);
	}

	public void PostTwitterStatusUpdate(string status, string pathToImage)
	{
	}

	public void GetTwitterHomeTimeline()
	{
		TwitterAndroid.getHomeTimeline();
	}

	public void PerformTwitterRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
		TwitterAndroid.performRequest(methodType, path, parameters);
	}

	public bool IsTweetSheetSupported()
	{
		return false;
	}

	public bool CanUserTweet()
	{
		return false;
	}

	public void ShowTweetComposer(string status, string pathToImage)
	{
	}

	public void InitFacebook(string applicationId)
	{
		FacebookManager.accessTokenExtendedEvent += OnAccessTokenExtendedEvent;
		FacebookManager.customRequestFailedEvent += OnCustomRequestFailedEvent;
		FacebookManager.customRequestReceivedEvent += OnCustomRequestReceivedEvent;
		FacebookManager.dialogCompletedEvent += OnDialogCompletedEvent;
		FacebookManager.dialogCompletedWithUrlEvent += OnDialogCompletedWithUrlEvent;
		FacebookManager.dialogDidNotCompleteEvent += OnDialogDidNotCompleteEvent;
		FacebookManager.dialogFailedEvent += OnDialogFailedEvent;
		FacebookManager.failedToExtendTokenEvent += OnFailedToExtendTokenEvent;
		FacebookManager.loggedOutEvent += OnLoggedOutEvent;
		FacebookManager.loginFailedEvent += OnLoginFailedEvent;
		FacebookManager.loginSucceededEvent += OnLoginSucceededEvent;
		FacebookManager.sessionInvalidatedEvent += OnSessionInvalidatedEvent;
		FacebookAndroid.init(applicationId);
	}

	public bool IsFacebookSessionValid()
	{
		return FacebookAndroid.isSessionValid();
	}

	public string GetFacebookAccessToken()
	{
		return FacebookAndroid.getAccessToken();
	}

	public void ExtendFacebookAccessToken()
	{
		FacebookAndroid.extendAccessToken();
	}

	public void LoginFacebook()
	{
		FacebookAndroid.login();
	}

	public void LoginFacebookWithRequestedPermissions(string[] permissions)
	{
		FacebookAndroid.loginWithRequestedPermissions(permissions);
	}

	public void LoginFacebookWithRequestedPermissions(string[] permissions, string urlSchemeSuffix)
	{
		FacebookAndroid.loginWithRequestedPermissions(permissions, urlSchemeSuffix);
	}

	public void LogoutFacebook()
	{
		FacebookAndroid.logout();
	}

	public void ShowFacebookPostMessageDialog()
	{
		FacebookAndroid.showPostMessageDialog();
	}

	public void ShowFacebookPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{
		FacebookAndroid.showPostMessageDialogWithOptions(link, linkName, linkToImage, caption);
	}

	public void ShowFacebookDialog(string dialogType, Dictionary<string, string> options)
	{
		FacebookAndroid.showDialog(dialogType, options);
	}

	public void FacebookRestRequest(string restMethod, string httpMethod, Hashtable keyValueHash)
	{
	}

	private void OnTwitterLogin(string obj)
	{
		if (BeLordSocialIOSAndroid.twitterLogin != null)
		{
			BeLordSocialIOSAndroid.twitterLogin();
		}
	}

	private void OnTwitterLoginFailed(string error)
	{
		if (BeLordSocialIOSAndroid.twitterLoginFailed != null)
		{
			BeLordSocialIOSAndroid.twitterLoginFailed(error);
		}
	}

	private void OnTwitterPost()
	{
		if (BeLordSocialIOSAndroid.twitterPost != null)
		{
			BeLordSocialIOSAndroid.twitterPost();
		}
	}

	private void OnTwitterPostFailed(string error)
	{
		if (BeLordSocialIOSAndroid.twitterPostFailed != null)
		{
			BeLordSocialIOSAndroid.twitterPostFailed(error);
		}
	}

	private void OnTwitterHomeTimelineReceived(ArrayList data)
	{
		if (BeLordSocialIOSAndroid.twitterHomeTimelineReceived != null)
		{
			BeLordSocialIOSAndroid.twitterHomeTimelineReceived(data);
		}
	}

	private void OnTwitterHomeTimelineFailed(string error)
	{
		if (BeLordSocialIOSAndroid.twitterHomeTimelineFailed != null)
		{
			BeLordSocialIOSAndroid.twitterHomeTimelineFailed(error);
		}
	}

	private void OnTwitterRequestDidFinishEvent(object obj)
	{
		if (BeLordSocialIOSAndroid.twitterRequestDidFinishEvent != null)
		{
			BeLordSocialIOSAndroid.twitterRequestDidFinishEvent(obj);
		}
		if (BeLordSocialIOSAndroid.twitterPost != null)
		{
			BeLordSocialIOSAndroid.twitterPost();
		}
	}

	private void OnTwitterRequestDidFailEvent(string error)
	{
		if (BeLordSocialIOSAndroid.twitterRequestDidFailEvent != null)
		{
			BeLordSocialIOSAndroid.twitterRequestDidFailEvent(error);
		}
		if (BeLordSocialIOSAndroid.twitterPostFailed != null)
		{
			BeLordSocialIOSAndroid.twitterPostFailed(error);
		}
	}

	private void OnLoginSucceededEvent()
	{
		if (BeLordSocialIOSAndroid.facebookLoginSucceededEvent != null)
		{
			BeLordSocialIOSAndroid.facebookLoginSucceededEvent();
		}
	}

	private void OnLoginFailedEvent(string error)
	{
		if (BeLordSocialIOSAndroid.facebookLoginFailedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookLoginFailedEvent(error);
		}
	}

	private void OnLoggedOutEvent()
	{
		if (BeLordSocialIOSAndroid.facebookLoggedOutEvent != null)
		{
			BeLordSocialIOSAndroid.facebookLoggedOutEvent();
		}
	}

	private void OnAccessTokenExtendedEvent(DateTime date)
	{
		if (BeLordSocialIOSAndroid.facebookAccessTokenExtendedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookAccessTokenExtendedEvent(date);
		}
	}

	private void OnFailedToExtendTokenEvent()
	{
		if (BeLordSocialIOSAndroid.facebookFailedToExtendTokenEvent != null)
		{
			BeLordSocialIOSAndroid.facebookFailedToExtendTokenEvent();
		}
	}

	private void OnSessionInvalidatedEvent()
	{
		if (BeLordSocialIOSAndroid.facebookSessionInvalidatedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookSessionInvalidatedEvent();
		}
	}

	private void OnDialogCompletedEvent()
	{
		if (BeLordSocialIOSAndroid.facebookDialogCompletedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookDialogCompletedEvent();
		}
	}

	private void OnDialogFailedEvent(string error)
	{
		if (BeLordSocialIOSAndroid.facebookDialogFailedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookDialogFailedEvent(error);
		}
	}

	private void OnDialogDidNotCompleteEvent()
	{
		if (BeLordSocialIOSAndroid.facebookDialogDidNotCompleteEvent != null)
		{
			BeLordSocialIOSAndroid.facebookDialogDidNotCompleteEvent();
		}
	}

	private void OnDialogCompletedWithUrlEvent(string error)
	{
		if (BeLordSocialIOSAndroid.facebookDialogCompletedWithUrlEvent != null)
		{
			BeLordSocialIOSAndroid.facebookDialogCompletedWithUrlEvent(error);
		}
	}

	private void OnCustomRequestReceivedEvent(object data)
	{
		if (BeLordSocialIOSAndroid.facebookCustomRequestReceivedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookCustomRequestReceivedEvent(data);
		}
	}

	private void OnCustomRequestFailedEvent(string error)
	{
		if (BeLordSocialIOSAndroid.facebookCustomRequestFailedEvent != null)
		{
			BeLordSocialIOSAndroid.facebookCustomRequestFailedEvent(error);
		}
	}
}
