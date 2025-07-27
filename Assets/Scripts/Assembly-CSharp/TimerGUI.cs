using UnityEngine;

public class TimerGUI : MonoBehaviour
{
	public DistanceManager DistanceManager;

	public TimerManager TimerMgr;

	public GUI3DText timerText;

	public GUI3DText SurivalHiScore;

	private int lastDistance;

	private int hiscore;

	private bool newRecord;

	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Adventure && PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Survival)
		{
			timerText.SetDynamicText(string.Empty);
			base.enabled = false;
		}
		newRecord = false;
	}

	private void OnEnable()
	{
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			hiscore = PlayerAccount.Instance.GetMetersFromCurrentLevel();
			if (hiscore == 0)
			{
				newRecord = true;
			}
			else if (DistanceManager.Instance != null && DistanceManager.Instance.Distance == 0)
			{
				newRecord = false;
			}
		}
	}

	private void FixedUpdate()
	{
		if (!(timerText != null))
		{
			return;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure && TimerMgr != null)
		{
			if (TimerMgr.Seconds != 0 || TimerMgr.Minutes != 0)
			{
				if (TimerMgr.Minutes < 10)
				{
					timerText.SetDynamicText("0");
				}
				else
				{
					timerText.SetDynamicText(string.Empty);
				}
				timerText.SetDynamicText(TimerMgr.Minutes + ":");
				if (TimerMgr.Seconds < 10)
				{
					timerText.SetDynamicText("0");
				}
				timerText.SetDynamicText(TimerMgr.Seconds);
			}
		}
		else
		{
			if (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Survival || !(DistanceManager != null) || lastDistance == DistanceManager.Instance.Distance)
			{
				return;
			}
			timerText.SetDynamicText(DistanceManager.Instance.Distance.ToString());
			lastDistance = DistanceManager.Instance.Distance;
			if (DistanceManager.Instance.Distance > hiscore)
			{
				if (SurivalHiScore != null)
				{
					SurivalHiScore.SetDynamicText(DistanceManager.Instance.Distance.ToString());
				}
				if (!newRecord)
				{
					GUI3DPopupManager.Instance.ShowPopup("NewRecord");
					newRecord = true;
				}
			}
		}
	}
}
