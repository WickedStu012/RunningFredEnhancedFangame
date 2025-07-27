using UnityEngine;

public class ButtonOK : MonoBehaviour
{
	public GUI3DPopup popup;

	public GUI3DText okButtonText;

	private GUI3DButton button;

	private int frameCount;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
		frameCount = 0;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void Update()
	{
		if (frameCount < 2)
		{
			frameCount++;
			if (frameCount == 2)
			{
				changedButtonOKText();
			}
		}
	}

	private void changedButtonOKText()
	{
		ItemInfo itemInfo = popup.GetCustomData() as ItemInfo;
		if (itemInfo != null)
		{
			if (string.Compare(itemInfo.Type, "avatar", true) == 0)
			{
				okButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Select", "!BAD_TEXT!"));
			}
			else
			{
				okButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Ok", "!BAD_TEXT!"));
			}
		}
		else
		{
			okButtonText.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Ok", "!BAD_TEXT!"));
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		ItemInfo itemInfo = popup.GetCustomData() as ItemInfo;
		if (itemInfo != null && string.Compare(itemInfo.Type, "avatar", true) == 0)
		{
			PlayerAccount.Instance.SelectAvatar(itemInfo);
		}
	}
}
