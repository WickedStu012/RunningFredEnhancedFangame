using UnityEngine;

public class GoToShopButton : MonoBehaviour
{
	public GUI3DTransition ActivateTransition;

	public GUI3D SwitchToGUI;

	public string DefaultTabName = "Tab1";

	private GUI3DButton button;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= OnRelease;
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		StatsManager.LogEventTimed(StatVar.SHOP, "IGM");
		if (ActivateTransition != null)
		{
			if (SwitchToGUI != null)
			{
				GUI3DManager.Instance.SaveCurrentState();
			}
			ActivateTransition.StartTransition();
			ActivateTransition.TransitionEndEvent += OnEndTransition;
		}
	}

	private void OnEndTransition(GUI3DOnTransitionEndEvent evt)
	{
		ActivateTransition.TransitionEndEvent -= OnEndTransition;
		GUI3DManager.Instance.Activate(SwitchToGUI, true, true);
		GUI3DTabControl componentInChildren = SwitchToGUI.GetComponentInChildren<GUI3DTabControl>();
		if (componentInChildren != null)
		{
			componentInChildren.SwitchToTab(DefaultTabName);
		}
	}
}
