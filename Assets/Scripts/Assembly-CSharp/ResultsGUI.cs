using UnityEngine;

public class ResultsGUI : MonoBehaviour
{
	private enum State
	{
		CountSilver = 0,
		Wait1 = 1,
		CountGold = 2,
		Wait2 = 3,
		ConvertSilver = 4,
		Wait3 = 5,
		CountTotal = 6,
		Wait4 = 7
	}

	public float Timer = 2f;

	public float TimeBetweenCounts = 1f;

	public float CounterTimer = 0.2f;

	public GUI3DTransition Transition;

	public GUI3DText SilverCoins;

	public GUI3DText GoldCoins;

	public GUI3DText TotalCoins;

	public GUI3DText TimerCount;

	private int silverCoins;

	private int goldCoins;

	private int totalCoins;

	private float counterTimer;

	private float timeAccum;

	private bool isPlayerDead;

	private State state;

	private int silverCoinValue;

	private float originalVolume;

	private int lastTotal;

	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure)
		{
			silverCoinValue = ScoreManager.Instance.SilverCoinValue;
			GameEventDispatcher.AddListener("OnPlayerDead", OnDead);
			GameEventDispatcher.AddListener("OnLevelComplete", OnLevelCompleted);
			Transition = GetComponentInChildren<GUI3DTransition>();
			totalCoins = PlayerAccount.Instance.RetrieveMoney();
			lastTotal = totalCoins;
			SilverCoins.SetDynamicText("0");
			GoldCoins.SetDynamicText("0");
			TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
		}
	}

	private void OnLevelCompleted(object sender, GameEvent evt)
	{
		if (GiftManager.Instance.CollectedGift == null)
		{
		}
		SoundManager.PlaySound(47);
	}

	private void OnEnable()
	{
		state = State.CountSilver;
		timeAccum = 0f;
		totalCoins = ScoreManager.Instance.PrevTotalMoney;
		lastTotal = totalCoins;
		if (TotalCoins != null)
		{
			TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
		}
		if (TimerManager.Instance != null)
		{
			if (TimerManager.Instance.Minutes < 10)
			{
				TimerCount.SetDynamicText("0");
			}
			else
			{
				TimerCount.SetDynamicText(string.Empty);
			}
			TimerCount.SetDynamicText(TimerManager.Instance.Minutes + ":");
			if (TimerManager.Instance.Seconds < 10)
			{
				TimerCount.SetDynamicText("0");
			}
			TimerCount.SetDynamicText(TimerManager.Instance.Seconds);
		}
		goldCoins += ScoreManager.Instance.TreasureGoldCoins;
	}

	private void Update()
	{
		switch (state)
		{
		case State.CountSilver:
			CountSilver();
			break;
		case State.CountGold:
			CountGold();
			break;
		case State.ConvertSilver:
			ConvertSilver();
			break;
		case State.CountTotal:
			CountTotal();
			break;
		case State.Wait1:
			if (Wait(TimeBetweenCounts))
			{
				state = State.CountGold;
			}
			break;
		case State.Wait2:
			if (Wait(TimeBetweenCounts))
			{
				state = State.ConvertSilver;
			}
			break;
		case State.Wait3:
			if (Wait(TimeBetweenCounts))
			{
				state = State.CountTotal;
			}
			break;
		case State.Wait4:
			if (Wait(Timer))
			{
				if (isPlayerDead)
				{
					SendLevelFailedEvent();
				}
				else
				{
					SendLevelCompleteEvent();
				}
				base.enabled = false;
			}
			break;
		}
		if (!InputManager.GetJumpKeyDown())
		{
			return;
		}
		if (state != State.Wait4)
		{
			silverCoins = ScoreManager.Instance.SilverCoins;
			goldCoins = ScoreManager.Instance.GoldCoins + ScoreManager.Instance.SilverCoins / ScoreManager.Instance.SilverCoinValue;
			totalCoins = lastTotal + goldCoins + ScoreManager.Instance.TreasureGoldCoins;
			GoldCoins.SetDynamicText((goldCoins + ScoreManager.Instance.TreasureGoldCoins).ToString());
			SilverCoins.SetDynamicText("0");
			TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
			timeAccum = 0f;
			state = State.Wait4;
		}
		else
		{
			if (isPlayerDead)
			{
				SendLevelFailedEvent();
			}
			else
			{
				SendLevelCompleteEvent();
			}
			base.enabled = false;
		}
	}

	private void SendLevelCompleteEvent()
	{
		if ((Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) && BeLordTapJoy.IsReadyToUse && PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure && PlayerAccount.Instance.CurrentChapterInfo != null)
		{
			string text = string.Empty;
			switch (PlayerAccount.Instance.CurrentChapterInfo.Id)
			{
			case 1000:
				text = "GM";
				break;
			case 1002:
				text = "DC";
				break;
			case 1004:
				text = "HS";
				break;
			}
			TapjoyPlacementsManager.callbackOnDeactiveProcessing += OnDeactiveProcessing;
			if (PlayerAccount.Instance.IsChapterComplete)
			{
				Debug.Log("Chapter complete!");
				TapjoyPlacementsManager.PlacementLoadAndShow("level_completed_" + text + "_" + PlayerAccount.Instance.CurrentLevelNum);
			}
			else
			{
				Debug.Log("Chapter inccomplete...");
				TapjoyPlacementsManager.PlacementLoadAndShow("level_completed_" + text + "_" + (PlayerAccount.Instance.CurrentLevelNum - 1));
			}
		}
		else
		{
			Exit();
		}
	}

	private void SendLevelFailedEvent()
	{
		Exit();
	}

	private void OnDeactiveProcessing()
	{
		Exit();
	}

	private void Exit()
	{
		TapjoyPlacementsManager.callbackOnDeactiveProcessing -= OnDeactiveProcessing;
		Transition.TransitionEndEvent += OnTransitionEnd;
		Transition.StartTransition();
		base.enabled = false;
	}

	private void OnViewClosed()
	{
		Exit();
	}

	private void CountSilver()
	{
		silverCoins = ScoreManager.Instance.SilverCoins;
		SilverCoins.SetDynamicText(silverCoins.ToString());
		state = State.CountGold;
	}

	private void CountGold()
	{
		goldCoins = ScoreManager.Instance.GoldCoins + ScoreManager.Instance.TreasureGoldCoins;
		GoldCoins.SetDynamicText(goldCoins.ToString());
		state = State.Wait2;
	}

	private void ConvertSilver()
	{
		if (silverCoins <= 0)
		{
			state = State.Wait3;
			return;
		}
		counterTimer += GUI3DManager.Instance.DeltaTime;
		if (counterTimer >= CounterTimer)
		{
			SoundManager.PlaySound(SndId.SND_PICKUP_SILVER);
			counterTimer -= CounterTimer;
			silverCoins -= silverCoinValue;
			if (silverCoins < 0)
			{
				silverCoins = 0;
			}
			else
			{
				goldCoins++;
				GoldCoins.SetDynamicText(goldCoins.ToString());
			}
			SilverCoins.SetDynamicText(silverCoins.ToString());
		}
	}

	private void CountTotal()
	{
		if (goldCoins <= 0)
		{
			state = State.Wait4;
			return;
		}
		counterTimer += GUI3DManager.Instance.DeltaTime;
		if (counterTimer >= CounterTimer)
		{
			SoundManager.PlaySound(SndId.SND_PICKUP_GOLD);
			counterTimer -= CounterTimer;
			goldCoins--;
			if (goldCoins < 0)
			{
				goldCoins = 0;
			}
			else
			{
				totalCoins++;
				TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
			}
			GoldCoins.SetDynamicText(goldCoins.ToString());
		}
	}

	private bool Wait(float time)
	{
		if (timeAccum < time)
		{
			timeAccum += GUI3DManager.Instance.DeltaTime;
			if (timeAccum >= time)
			{
				timeAccum = 0f;
				return true;
			}
			return false;
		}
		return true;
	}

	private void OnTransitionEnd(GUI3DOnTransitionEndEvent evt)
	{
		if (isPlayerDead)
		{
			GUI3DManager.Instance.Activate("TryAgain", true, true);
		}
		else
		{
			switch (ScoreManager.Instance.MedalGained)
			{
			case ScoreManager.MedalType.Gold:
				SoundManager.PlaySound(49);
				GUI3DManager.Instance.Activate("GoldMedal", true, true);
				break;
			case ScoreManager.MedalType.Silver:
				SoundManager.PlaySound(50);
				GUI3DManager.Instance.Activate("SilverMedal", true, true);
				break;
			case ScoreManager.MedalType.Bronze:
				SoundManager.PlaySound(51);
				GUI3DManager.Instance.Activate("BronzeMedal", true, true);
				break;
			case ScoreManager.MedalType.None:
				GUI3DManager.Instance.Activate("Congratulations", true, true);
				break;
			}
		}
		Transition.TransitionEndEvent -= OnTransitionEnd;
	}

	private void OnDead(object sender, GameEvent evt)
	{
		isPlayerDead = true;
		SoundManager.PlaySound(46);
	}
}
