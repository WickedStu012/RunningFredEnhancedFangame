using UnityEngine;

public class CollectablesButton : MonoBehaviour
{
	private GUI3DButton button;

	public static bool collectablePress;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += ReleaseEvent;
	}

	private void OnDisable()
	{
		button.ReleaseEvent -= ReleaseEvent;
	}

	private void ReleaseEvent(GUI3DOnReleaseEvent evn)
	{
		collectablePress = true;
	}
}
