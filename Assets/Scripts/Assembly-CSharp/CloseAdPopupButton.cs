using UnityEngine;

public class CloseAdPopupButton : MonoBehaviour
{
	private GUI3DButton button;

	private void Start()
	{
		button = GetComponent<GUI3DButton>();
	}

	private void OnEnable()
	{
		if (button != null)
		{
			button.ClickEvent += OnClick;
		}
	}

	private void OnDisable()
	{
		if (button != null)
		{
			button.ClickEvent -= OnClick;
		}
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		AdManager.Instance.RemoveTextureFromMaterial();
	}
}
