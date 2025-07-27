using UnityEngine;

public class FullScreenCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = ConfigParams.fullScreen;
		checkbox.Checked = checkbox.StartCheckStatus;
	}

	private void OnDisable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.CheckboxChangeEvent += OnChange;
	}

	private void OnChange(GUI3DOnCheckboxChangeEvent evt)
	{
		if (evt.Checked)
		{
			FullScreenChecker.ChangeToFullScreen(true);
		}
		else
		{
			FullScreenChecker.ChangeToFullScreen(false);
		}
	}
}
