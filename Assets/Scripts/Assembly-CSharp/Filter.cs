using UnityEngine;

public class Filter : MonoBehaviour
{
	public GUI3DCheckbox[] Filters;

	public GUI3DPageSlider PageSlider;

	private void OnEnable()
	{
		GUI3DCheckbox[] filters = Filters;
		foreach (GUI3DCheckbox gUI3DCheckbox in filters)
		{
			gUI3DCheckbox.CheckboxChangeEvent += OnChange;
		}
	}

	private void OnDisable()
	{
		GUI3DCheckbox[] filters = Filters;
		foreach (GUI3DCheckbox gUI3DCheckbox in filters)
		{
			gUI3DCheckbox.CheckboxChangeEvent -= OnChange;
		}
	}

	private void OnChange(GUI3DOnCheckboxChangeEvent evt)
	{
		string text = string.Empty;
		GUI3DCheckbox[] filters = Filters;
		foreach (GUI3DCheckbox gUI3DCheckbox in filters)
		{
			if (gUI3DCheckbox.Checked)
			{
				text = text + gUI3DCheckbox.Tag + ";";
			}
		}
		PageSlider.SetFilter(text);
	}
}
