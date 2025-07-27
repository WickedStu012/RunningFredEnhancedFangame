using UnityEngine;

public class SFFPopupDoNotShowAgainCheckbox : MonoBehaviour
{
	public static bool DoNotShowAgainValue;

	private GUI3DCheckbox checkbox;

	private void Awake()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = false;
		checkbox.Checked = false;
		if (PlayerPrefs.GetInt("SFFPopupShowCount", 0) < 2)
		{
			MeshRenderer component = GetComponent<MeshRenderer>();
			component.GetComponent<Renderer>().enabled = false;
			MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].GetComponent<Renderer>().enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = false;
		checkbox.Checked = false;
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
		DoNotShowAgainValue = evt.Checked;
	}
}
