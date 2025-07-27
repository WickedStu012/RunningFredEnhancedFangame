using UnityEngine;

public class ActivateOnGameEvent : MonoBehaviour
{
	public GUI3D ActivateGUI;

	public string Event = string.Empty;

	public string DeactivateEvent = string.Empty;

	public string DisableEvent = string.Empty;

	public GUI3DTransition DeactivateTransition;

	public bool DeactivateAllOthers;

	public PlayerAccount.GameMode GameMode;

	private bool disabled;

	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode == GameMode)
		{
			if (ActivateGUI == null)
			{
				ActivateGUI = GetComponent<GUI3D>();
			}
			if (Event != string.Empty)
			{
				GameEventDispatcher.AddListener(Event, OnEvent);
			}
			if (DeactivateEvent != string.Empty)
			{
				GameEventDispatcher.AddListener(DeactivateEvent, OnDeactivateEvent);
			}
			if (DisableEvent != string.Empty)
			{
				GameEventDispatcher.AddListener(DisableEvent, OnDisableEvent);
			}
			GameEventDispatcher.AddListener("OnPauseEvent", OnDisableEvent);
		}
	}

	private void OnEvent(object sender, GameEvent evt)
	{
		if (ActivateGUI != null)
		{
			if (GUI3DPopupManager.Instance.CurrentPopup != null)
			{
				GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Cancel, true);
				Invoke("showTryAgain", 1f);
			}
			else
			{
				showTryAgain();
			}
		}
	}

	private void showTryAgain()
	{
		if (DeactivateAllOthers)
		{
			GUI3DManager.Instance.Activate(ActivateGUI, true, true);
		}
		else
		{
			GUI3DManager.Instance.Activate(ActivateGUI, false, false);
		}
	}

	private void OnDeactivateEvent(object sender, GameEvent evt)
	{
		if (ActivateGUI != null)
		{
			if (DeactivateTransition != null)
			{
				DeactivateTransition.TransitionEndEvent += OnTransitionEnd;
				DeactivateTransition.StartTransition();
			}
			else if (DeactivateAllOthers)
			{
				GUI3DManager.Instance.RestoreLastState();
			}
			else
			{
				Debug.Log("Deactivating");
				ActivateGUI.SetActive(false);
				ActivateGUI.SetVisible(false);
			}
		}
	}

	private void OnDisableEvent(object sender, GameEvent evt)
	{
		if (disabled)
		{
			return;
		}
		bool flag = true;
		if (evt is OnPauseEvent)
		{
			flag = ((OnPauseEvent)evt).Paused;
		}
		else
		{
			disabled = true;
		}
		if (flag)
		{
			if (Event != string.Empty)
			{
				GameEventDispatcher.RemoveListener(Event, OnEvent);
			}
			if (DeactivateEvent != string.Empty)
			{
				GameEventDispatcher.RemoveListener(DeactivateEvent, OnDeactivateEvent);
			}
			if (DisableEvent != string.Empty)
			{
				GameEventDispatcher.RemoveListener(DisableEvent, OnDisableEvent);
			}
		}
		else
		{
			if (Event != string.Empty)
			{
				GameEventDispatcher.AddListener(Event, OnEvent);
			}
			if (DeactivateEvent != string.Empty)
			{
				GameEventDispatcher.AddListener(DeactivateEvent, OnDeactivateEvent);
			}
			if (DisableEvent != string.Empty)
			{
				GameEventDispatcher.AddListener(DisableEvent, OnDisableEvent);
			}
		}
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		if (DeactivateAllOthers)
		{
			GUI3DManager.Instance.RestoreLastState();
			return;
		}
		Debug.Log("Deactivating");
		ActivateGUI.SetActive(false);
		ActivateGUI.SetVisible(false);
	}
}
