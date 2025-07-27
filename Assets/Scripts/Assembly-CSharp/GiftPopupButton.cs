using UnityEngine;

public class GiftPopupButton : MonoBehaviour
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
		if (GrimmyIdol.shouldShowUnlock)
		{
			GrimmyIdol.shouldShowUnlock = false;
			GUI3DManager.Instance.Activate("IronFredUnlocked", true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate("Results", false, false);
		}
	}
}
