using UnityEngine;

public class PauseButton : MonoBehaviour
{
	private SwitchToInGameMenu switchToInGameMenu;

	private ActivateTransitionsOnClick activatetrans;

	private GUI3DButton button;

	private OnPauseEvent onPauseEvent = new OnPauseEvent();

	private void OnEnable()
	{
		onPauseEvent.Paused = true;
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		if (activatetrans == null)
		{
			activatetrans = GetComponent<ActivateTransitionsOnClick>();
		}
		if (switchToInGameMenu == null)
		{
			switchToInGameMenu = GetComponent<SwitchToInGameMenu>();
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

	private void Update()
	{
		if (Time.timeScale != 0f && MogaInput.Instance.IsConnected() && button.enabled && MogaInput.Instance.GetButtonStartDown())
		{
			if (activatetrans != null)
			{
				activatetrans.DoAction();
			}
			if (switchToInGameMenu != null)
			{
				switchToInGameMenu.DoAction();
			}
			OnRelease(null);
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (DedalordLoadLevel.GetLevel() != "TutorialLoader")
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Cancel, true);
		}
		Time.timeScale = 0f;
		onPauseEvent.Paused = true;
		GameEventDispatcher.Dispatch(this, onPauseEvent);
	}
}
