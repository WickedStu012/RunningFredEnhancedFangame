using UnityEngine;

public class iCadeCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void Awake()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = ConfigParams.useiCADE;
		checkbox.Checked = checkbox.StartCheckStatus;
	}

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.CheckboxChangeEvent += OnChange;
	}

	private void OnDisable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.CheckboxChangeEvent -= OnChange;
	}

	private void OnChange(GUI3DOnCheckboxChangeEvent evt)
	{
		if (evt.Checked)
		{
			GUI3DPopupManager.Instance.ShowPopup("ConfirmationiCade", OniCadeConfirmationClose);
			return;
		}
		ConfigParams.useiCADE = false;
		iCADEManager.DisableiCade();
	}

	private void OniCadeConfirmationClose(GUI3DPopupManager.PopupResult result)
	{
		if (result == GUI3DPopupManager.PopupResult.Yes)
		{
			ConfigParams.useiCADE = true;
			iCADEManager.EnableiCade();
		}
		else
		{
			checkbox.Checked = false;
		}
	}
}
