using UnityEngine;

public class ResultsSurvivalGUI : MonoBehaviour
{
	private enum State
	{
		CountDistance = 0,
		Wait1 = 1,
		CountGold = 2,
		Wait2 = 3,
		ConvertDistance = 4,
		Wait3 = 5,
		CountTotal = 6,
		Wait4 = 7,
		End = 8
	}

	public float Timer = 2f;

	public float TimeBetweenCounts = 1f;

	public float CounterTimer = 0.1f;

	public GUI3DTransition Transition;

	public GUI3DText DistanceCount;

	public GUI3DText GoldCoins;

	public GUI3DText TotalCoins;

	private int distanceCount;

	private float goldCoins;

	private int totalCoins;

	private float counterTimer;

	private float timeAccum;

	private bool isPlayerDead;

	private State state;

	private int distanceValue;

	private int lastTotal;

	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			GameEventDispatcher.AddListener("OnPlayerDead", OnDead);
			GameEventDispatcher.AddListener("OnLevelComplete", OnLevelCompleted);
			GameEventDispatcher.AddListener("OnEndLessReset", OnReset);
		}
		Transition = GetComponentInChildren<GUI3DTransition>();
		Init();
	}

	private void Init()
	{
		state = State.CountDistance;
		distanceValue = 0;
		totalCoins = PlayerAccount.Instance.RetrieveMoney();
		lastTotal = totalCoins;
		DistanceCount.SetDynamicText("0");
		GoldCoins.SetDynamicText("0");
		TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
	}

	private void OnReset(object sender, GameEvent evt)
	{
		Init();
	}

	private void OnLevelCompleted(object sender, GameEvent evt)
	{
		if (GiftManager.Instance.CollectedGift == null)
		{
			GUI3DManager.Instance.Activate("ResultsSurival", false, false);
		}
	}

	private void OnEnable()
	{
		if (!isPlayerDead)
		{
			SoundManager.PlaySound(24);
		}
		state = State.CountDistance;
		totalCoins = ScoreManager.Instance.PrevTotalMoney;
		lastTotal = totalCoins;
		if (TotalCoins != null)
		{
			TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
		}
		goldCoins += ScoreManager.Instance.TreasureGoldCoins;
		if (DistanceManager.Instance != null)
		{
			if (ScoreManager.Instance != null && ScoreManager.Instance.CoinsPerDistance != 0)
			{
				distanceValue = (int)((float)DistanceManager.Instance.Distance / (float)ScoreManager.Instance.CoinsPerDistance);
			}
			else
			{
				distanceValue = DistanceManager.Instance.Distance + 1;
			}
		}
	}

	private void Update()
	{
		switch (state)
		{
		case State.CountDistance:
			CountDistance();
			break;
		case State.CountGold:
			CountGold();
			break;
		case State.ConvertDistance:
			ConvertDistance();
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
				state = State.ConvertDistance;
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
				Transition.TransitionEndEvent += OnTransitionEnd;
				Transition.StartTransition();
				base.enabled = false;
				state = State.End;
			}
			break;
		}
		if (InputManager.GetJumpKeyDown())
		{
			if (state != State.Wait4)
			{
				distanceCount = DistanceManager.Instance.Distance;
				goldCoins = distanceCount / distanceValue;
				totalCoins = lastTotal + (int)goldCoins;
				GoldCoins.SetDynamicText(goldCoins.ToString());
				DistanceCount.SetDynamicText("0");
				TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
				timeAccum = 0f;
				state = State.Wait4;
			}
			else
			{
				Transition.TransitionEndEvent += OnTransitionEnd;
				Transition.StartTransition();
				base.enabled = false;
			}
		}
	}

	private void CountDistance()
	{
		distanceCount = DistanceManager.Instance.Distance;
		DistanceCount.SetDynamicText(distanceCount.ToString());
		state = State.CountGold;
	}

	private void CountGold()
	{
		goldCoins = ScoreManager.Instance.GoldCoins + ScoreManager.Instance.TreasureGoldCoins;
		GoldCoins.SetDynamicText(goldCoins.ToString());
		state = State.Wait2;
	}

	private void ConvertDistance()
	{
		if (distanceCount <= 0)
		{
			state = State.Wait3;
			return;
		}
		counterTimer += GUI3DManager.Instance.DeltaTime;
		if (counterTimer >= CounterTimer)
		{
			SoundManager.PlaySound(SndId.SND_PICKUP_SILVER);
			counterTimer -= CounterTimer;
			distanceCount -= distanceValue;
			if (distanceCount < 0)
			{
				distanceCount = 0;
			}
			else
			{
				goldCoins += 1f;
				GoldCoins.SetDynamicText(((int)goldCoins).ToString());
			}
			DistanceCount.SetDynamicText(distanceCount.ToString());
		}
	}

	private void CountTotal()
	{
		if (goldCoins <= 0f)
		{
			state = State.Wait4;
			return;
		}
		counterTimer += GUI3DManager.Instance.DeltaTime;
		if (counterTimer >= CounterTimer)
		{
			SoundManager.PlaySound(SndId.SND_PICKUP_GOLD);
			counterTimer -= CounterTimer;
			goldCoins -= 1f;
			if (goldCoins < 0f)
			{
				goldCoins = 0f;
			}
			else
			{
				totalCoins++;
				TotalCoins.SetDynamicText(StringUtil.FormatNumbers(totalCoins));
			}
			GoldCoins.SetDynamicText(((int)goldCoins).ToString());
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
			if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
			{
				if (DistanceManager.Instance.LevelUnlocked)
				{
					string arg = string.Empty;
					string icon = string.Empty;
					switch (PlayerAccount.Instance.CurrentLevelNum)
					{
					case 1:
						arg = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Normal", "!BAD_TEXT!");
						icon = "SurvivalNormal";
						break;
					case 2:
						arg = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Hard", "!BAD_TEXT!");
						icon = "SurvivalHard";
						break;
					case 3:
						arg = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Nightmare", "!BAD_TEXT!");
						icon = "SurvivalHardcore";
						break;
					}
					string text = string.Format(MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "YouUnlockedLevel", "!BAD_TEXT!"), arg);
					GUI3DPopupManager.Instance.ShowPopup("LevelUnlock", text, MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("Unsorted", "Hooray", "!BAD_TEXT!"), icon, OnClose);
				}
				else
				{
					GUI3DManager.Instance.Activate("TryAgainSurvival", true, true);
				}
			}
			else
			{
				GUI3DManager.Instance.Activate("TryAgain", true, true);
			}
		}
		else
		{
			switch (ScoreManager.Instance.MedalGained)
			{
			case ScoreManager.MedalType.Gold:
				GUI3DManager.Instance.Activate("GoldMedal", true, true);
				break;
			case ScoreManager.MedalType.Silver:
				GUI3DManager.Instance.Activate("SilverMedal", true, true);
				break;
			case ScoreManager.MedalType.Bronze:
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
		base.enabled = true;
		state = State.CountDistance;
	}

	private void OnClose(GUI3DPopupManager.PopupResult result)
	{
		GUI3DManager.Instance.Activate("TryAgainSurvival", true, true);
	}
}
