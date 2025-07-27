using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public enum MedalType
	{
		None = 0,
		Bronze = 1,
		Silver = 2,
		Gold = 3
	}

	public int SilverCoinValue = 10;

	private static ScoreManager instance;

	private int treasureGoldCoins;

	private int goldCoins;

	private int silverCoins;

	private int previousTotal;

	private List<int> treasureIds;

	private float multiplier = 0.5f;

	private float coinsPerDistance;

	private int lastCoinsEarned;

	public static ScoreManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "ScoreManager";
					instance = gameObject.AddComponent<ScoreManager>();
				}
			}
			return instance;
		}
	}

	public int TreasureGoldCoins
	{
		get
		{
			return treasureGoldCoins;
		}
	}

	public int GoldCoins
	{
		get
		{
			return goldCoins;
		}
	}

	public int SilverCoins
	{
		get
		{
			return silverCoins;
		}
	}

	public int PrevTotalMoney
	{
		get
		{
			return previousTotal;
		}
	}

	public int CoinsPerDistance
	{
		get
		{
			return (int)coinsPerDistance;
		}
	}

	public float CollectedPercentage
	{
		get
		{
			int num = SilverCoins + GoldCoins;
			if (GiftManager.Instance != null && GiftManager.Instance.CurrentActiveGift != null && GiftManager.Instance.CurrentActiveGift.Id == 1)
			{
				num /= 2;
			}
			int num2 = PickupManager.GetSilverCoinCount() + PickupManager.GetGoldCoinCount();
			return (float)num / (float)num2;
		}
	}

	public MedalType MedalGained
	{
		get
		{
			float collectedPercentage = CollectedPercentage;
			if (collectedPercentage >= 1f)
			{
				return MedalType.Gold;
			}
			if (collectedPercentage >= 0.75f)
			{
				return MedalType.Silver;
			}
			if (collectedPercentage >= 0.5f)
			{
				return MedalType.Bronze;
			}
			return MedalType.None;
		}
	}

	private void Awake()
	{
		instance = this;
		previousTotal = PlayerAccount.Instance.RetrieveMoney();
		GameEventDispatcher.AddListener("GoldCoinPickup", OnGoldCoinPickup);
		GameEventDispatcher.AddListener("SilverCoinPickup", OnSilverCoinPickup);
		GameEventDispatcher.AddListener("OnPlayerDead", SaveProgress);
		GameEventDispatcher.AddListener("OnLevelComplete", SaveProgress);
		GameEventDispatcher.AddListener("TreasurePickup", OnTreasurePickup);
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			GameEventDispatcher.AddListener("OnPlayerBonus", OnBonus);
			GameEventDispatcher.AddListener("OnCoinPerDistance", OnCoinEarnedPerDistance);
			GameEventDispatcher.AddListener("OnEndLessReset", EndLessReset);
		}
		PlayerAccount.Instance.MoneyChangeEvent += OnMoneyChange;
		treasureIds = new List<int>();
	}

	private void OnTreasurePickup(object sender, GameEvent evt)
	{
		TreasurePickup treasurePickup = (TreasurePickup)evt;
		treasureGoldCoins += treasurePickup.GoldCount;
		if (GiftManager.Instance.CurrentActiveGift != null && GiftManager.Instance.CurrentActiveGift.Id == 1)
		{
			treasureGoldCoins += treasurePickup.GoldCount;
		}
		treasureIds.Add(treasurePickup.Id);
		PlayerAccount.Instance.PickupTreasure(treasurePickup.Id);
	}

	private void OnGoldCoinPickup(object sender, GameEvent evt)
	{
		goldCoins++;
		if (GiftManager.Instance.CurrentActiveGift != null && GiftManager.Instance.CurrentActiveGift.Id == 1)
		{
			goldCoins++;
		}
	}

	private void OnSilverCoinPickup(object sender, GameEvent evt)
	{
		silverCoins++;
		if (GiftManager.Instance.CurrentActiveGift != null && GiftManager.Instance.CurrentActiveGift.Id == 1)
		{
			silverCoins++;
		}
	}

	private void OnCoinEarnedPerDistance(object sender, GameEvent evt)
	{
		coinsPerDistance += multiplier;
		if (lastCoinsEarned != (int)coinsPerDistance)
		{
			SoundManager.PlaySound(3);
			lastCoinsEarned = (int)coinsPerDistance;
		}
	}

	private void SaveProgress(object sender, GameEvent evt)
	{
		PlayerAccount.Instance.MoneyChangeEvent -= OnMoneyChange;
		int num = Instance.GoldCoins + Instance.SilverCoins / Instance.SilverCoinValue;
		num += treasureGoldCoins;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			num += CoinsPerDistance;
		}
		if (num > 0)
		{
			PlayerAccount.Instance.AddMoney(num);
			if (GiftManager.Instance.IsPiggyBankActive)
			{
				PlayerAccount.Instance.AddMoneyInPiggyBank(num);
			}
		}
		switch (MedalGained)
		{
		case MedalType.Gold:
			PlayerAccount.Instance.SetGoldMedal();
			break;
		case MedalType.Silver:
			PlayerAccount.Instance.SetSilverMedal();
			break;
		case MedalType.Bronze:
			PlayerAccount.Instance.SetBronzeMedal();
			break;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Adventure && evt.Name == "OnLevelComplete")
		{
			PlayerAccount.Instance.SetTimeToLevel(TimerManager.Instance.TotalSeconds);
		}
		else if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			PlayerAccount.Instance.SetMetersToLevel(DistanceManager.Instance.Distance);
		}
		PlayerAccount.Instance.AddToTotalMeters(DistanceManager.Instance.Distance);
		GameEventDispatcher.RemoveListener("OnPlayerDead", SaveProgress);
		GameEventDispatcher.RemoveListener("OnLevelComplete", SaveProgress);
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void OnMoneyChange(int money)
	{
		previousTotal = money;
	}

	private void OnBonus(object sender, GameEvent evt)
	{
		OnPlayerBonus onPlayerBonus = (OnPlayerBonus)evt;
		multiplier = onPlayerBonus.Multiplier;
	}

	private void EndLessReset(object sender, GameEvent evt)
	{
		coinsPerDistance = 0f;
		lastCoinsEarned = 0;
		previousTotal = PlayerAccount.Instance.RetrieveMoney();
		PlayerAccount.Instance.MoneyChangeEvent += OnMoneyChange;
		GameEventDispatcher.AddListener("OnPlayerDead", SaveProgress);
		GameEventDispatcher.AddListener("OnLevelComplete", SaveProgress);
	}
}
