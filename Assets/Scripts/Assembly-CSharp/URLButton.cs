using UnityEngine;

public class URLButton : MonoBehaviour
{
	public string IPhoneURL;

	public string IPhoneURLHD;

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
		Application.OpenURL(AndroidURL);
	}
}
