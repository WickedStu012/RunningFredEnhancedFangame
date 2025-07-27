using UnityEngine;

public class RunningFredCapViewCode : MonoBehaviour
{
	public string popup;

	public string skiingFredURL;

	private GUI3DButton button;

	private ItemInfo ii;

	private void OnEnable()
	{
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
		GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
		GUI3DPopup currentPopup = GUI3DPopupManager.Instance.CurrentPopup;
		currentPopup.Close(GUI3DPopupManager.PopupResult.No);
		GUI3DPopupManager.Instance.ShowPopup(popup, onClose);
	}

	private void onClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Yes);
			Application.OpenURL(skiingFredURL);
		}
	}
}
