using UnityEngine;

public class ResumeGame : MonoBehaviour
{
	private GUI3DButton button;

	private OnPauseEvent onPauseEvent = new OnPauseEvent();

	private ActivateTransitionsOnClick activatetrans;

	private void OnEnable()
	{
		onPauseEvent.Paused = false;
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		if (activatetrans == null)
		{
			activatetrans = GetComponent<ActivateTransitionsOnClick>();
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
		if (MogaInput.Instance.IsConnected() && button.enabled && (MogaInput.Instance.GetButtonStartDown() || MogaInput.Instance.GetButtonADown()))
		{
			button.OnRelease();
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		InputManager.ResetSuperSprintTimer();
		Time.timeScale = 1f;
		onPauseEvent.Paused = false;
		GameEventDispatcher.Dispatch(this, onPauseEvent);
	}
}
