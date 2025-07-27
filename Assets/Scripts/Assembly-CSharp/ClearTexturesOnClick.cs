using UnityEngine;

public class ClearTexturesOnClick : MonoBehaviour
{
	private GUI3DInteractiveObject button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DInteractiveObject>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DInteractiveObject>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
	}
}
