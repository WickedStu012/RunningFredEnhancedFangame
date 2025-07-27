using UnityEngine;

public class GoreCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void Awake()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = ConfigParams.useGore;
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
		ConfigParams.useGore = evt.Checked;
	}
}
