using UnityEngine;

public class ResetGame : MonoBehaviour
{
	public bool IsInGameMenu;

	public bool DisableInChallenge;

	private GUI3DButton button;

	private ActivateTransitionsOnClick activatetrans;

	private DeactivateGUIOnClick deactivateGui;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += OnRelease;
		if (deactivateGui == null)
		{
			deactivateGui = GetComponent<DeactivateGUIOnClick>();
		}
		if (activatetrans == null)
		{
			activatetrans = GetComponent<ActivateTransitionsOnClick>();
		}
		if (DisableInChallenge && PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge && !PlayerAccount.Instance.IsChallengeUnlocked(ChallengesManager.Instance.SelectedChallenge.Id) && !Store.Instance.GetItem(ChallengesManager.Instance.SelectedChallenge.Id).Purchased)
		{
			GUI3DText componentInChildren = GetComponentInChildren<GUI3DText>();
			componentInChildren.SetDynamicText(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Forfeit", "!BAD_TEXT!"));
		}
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
		if (!IsInGameMenu && MogaInput.Instance.IsConnected() && button.enabled && (MogaInput.Instance.GetButtonStartDown() || MogaInput.Instance.GetButtonADown()))
		{
			button.OnRelease();
		}
	}

	private void OnRelease(GUI3DOnReleaseEvent evt)
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1000)
			{
				StatsManager.LogEvent(StatVar.RESTART_LEVEL_CASTLE, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1002)
			{
				StatsManager.LogEvent(StatVar.RESTART_LEVEL_CAVES, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1004)
			{
				StatsManager.LogEvent(StatVar.RESTART_LEVEL_ROOFTOP, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
		}
		else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1001)
			{
				StatsManager.LogEvent(StatVar.RESTART_ENDLESS_CASTLE, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1003)
			{
				StatsManager.LogEvent(StatVar.RESTART_ENDLESS_CAVES, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
			else if (PlayerAccount.Instance.CurrentChapterInfo.Id == 1005)
			{
				StatsManager.LogEvent(StatVar.RESTART_ENDLESS_ROOFTOP, PlayerAccount.Instance.CurrentLevelNum.ToString());
			}
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			Time.timeScale = 1f;
			OnPauseEvent onPauseEvent = new OnPauseEvent();
			onPauseEvent.Paused = false;
			GameEventDispatcher.Dispatch(this, onPauseEvent);
			GameManager.ResetGameForEndless(true);
		}
		else
		{
			button.enabled = false;
			CameraFade.Instance.FadeOut(OnFadeOut);
		}
	}

	private void OnFadeOut()
	{
		button.enabled = true;
		Time.timeScale = 1f;
		SoundManager.StopAll();
		if (DisableInChallenge && PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Challenge && !PlayerAccount.Instance.IsChallengeUnlocked(ChallengesManager.Instance.SelectedChallenge.Id) && !Store.Instance.GetItem(ChallengesManager.Instance.SelectedChallenge.Id).Purchased)
		{
			PlayerAccount.Instance.UnselectChallenge();
		}
		if (Application.loadedLevelName == "TutorialLoader")
		{
			DedalordLoadLevel.LoadLevel("TutorialLoader");
		}
		else
		{
			DedalordLoadLevel.LoadLevel("CharCreator");
		}
	}
}
