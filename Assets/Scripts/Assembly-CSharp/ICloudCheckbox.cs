using UnityEngine;

public class ICloudCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void Awake()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = ConfigParams.useICloud;
		checkbox.Checked = checkbox.StartCheckStatus;
	}

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = ConfigParams.useICloud;
		checkbox.Checked = checkbox.StartCheckStatus;
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
		ConfigParams.useICloud = evt.Checked;
		if (ConfigParams.useICloud && PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.Sync(null);
		}
	}
}
