using UnityEngine;

public class SwitchToSettings : MonoBehaviour
{
	private GUI3DInteractiveObject button;

	public GUI3DTransition Transition;

	public string SettingPageEditor;

	public string SettingPageiOS;

	public string SettingPageAndroid;

	public string SettingPageWeb;

	public string SettingPageMac;

	public string SettingPageElse;

	private void Start()
	{
	}

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
		Transition.TransitionEndEvent += OnTransitionEnd;
		Transition.StartTransition();
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		Transition.TransitionEndEvent -= OnTransitionEnd;
		if (Application.isEditor)
		{
			GUI3DManager.Instance.Activate(SettingPageEditor, true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate(SettingPageAndroid, true, true);
		}
	}
}
