using UnityEngine;

public class URLButtonSkiingFred : MonoBehaviour
{
	public string IPhoneURL;

	public string AndroidURL;

	public GUI3DObject ComingSoonObjs;

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
		ComingSoonObjs.gameObject.SetActive(!ConfigParams.skiingFredIsAvailable);
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
}
