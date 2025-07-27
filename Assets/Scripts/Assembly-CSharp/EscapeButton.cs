using UnityEngine;

public class EscapeButton : MonoBehaviour
{
	private void Update()
	{
		if (GUI3DManager.Instance.IsActive("MainMenu") && GUI3DPopupManager.Instance.CurrentPopup == null && Input.GetKeyUp(KeyCode.Escape))
		{
			GUI3DPopupManager.Instance.ShowPopup("Confirmation", "Are you sure you want to exit the game?", "Are you sure?", "icon-error", OnClose);
			GUI3DPopup currentPopup = GUI3DPopupManager.Instance.CurrentPopup;
			currentPopup.SetOkText("Yes");
			currentPopup.SetCloseText("No");
			currentPopup.SetCancelText("No");
		}
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			Application.Quit();
		}
	}
}
