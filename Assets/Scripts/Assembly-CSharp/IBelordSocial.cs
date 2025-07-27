using System.Collections;
using System.Collections.Generic;

internal interface IBelordSocial
{
	void InitTwitter(string consumerKey, string consumerSecret);

	bool IsTwitterLoggedIn();

	string LoggedInTwitterUsername();

	void LoginTwitter(string username, string password);

	void ShowTwitterOauthLoginDialog();

	void LogoutTwitter();

	void PostTwitterStatusUpdate(string status);

	void PostTwitterStatusUpdate(string status, string pathToImage);

	void GetTwitterHomeTimeline();

	void PerformTwitterRequest(string methodType, string path, Dictionary<string, string> parameters);

	bool IsTweetSheetSupported();

	bool CanUserTweet();

	void ShowTweetComposer(string status, string pathToImage);

	void InitFacebook(string applicationId);

	bool IsFacebookSessionValid();

	string GetFacebookAccessToken();

	void ExtendFacebookAccessToken();

	void LoginFacebook();

	void LoginFacebookWithRequestedPermissions(string[] permissions);

	void LoginFacebookWithRequestedPermissions(string[] permissions, string urlSchemeSuffix);

	void LogoutFacebook();

	void ShowFacebookPostMessageDialog();

	void ShowFacebookPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption);

	void ShowFacebookDialog(string dialogType, Dictionary<string, string> options);

	void FacebookRestRequest(string restMethod, string httpMethod, Hashtable keyValueHash);
}
