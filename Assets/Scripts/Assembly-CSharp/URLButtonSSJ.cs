using UnityEngine;

public class URLButtonSSJ : MonoBehaviour
{
	public string IPhoneURL;

	public string IPhoneURL_USA;

	public string AndroidURL;

	private GUI3DButton button;

	private void OnEnable()
	{
		if (AndroidURL == string.Empty)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (ConfigParams.skiingFredIsAvailable)
		{
			Application.OpenURL(AndroidURL);
		}
	}

	private bool IsInUSA()
	{
		bool result = false;
		string countryCode = PlayerAccount.Instance.GetCountryCode();
		if (countryCode != null)
		{
			result = countryCode.ToLower() == "us";
		}
		return result;
	}
}
