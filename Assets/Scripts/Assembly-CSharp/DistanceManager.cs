using UnityEngine;

public class DistanceManager : MonoBehaviour
{
	public int BonusDistance = 100;

	public int DistanceToUnlockNormalLevel = 3000;

	public int DistanceToUnlockHardLevel = 1500;

	public int DistanceToUnlockNightmareLevel = 1500;

	public float CoinsPerDistance = 0.5f;

	public float CoinsPerDistanceEasy = 0.5f;

	public float CoinsPerDistanceNormal = 0.7f;

	public float CoinsPerDistanceHard = 0.9f;

	public float CoinsPerDistanceNightmare = 1.1f;

	public float ClampAt = 1.5f;

	public float ClampAtEasy = 1.5f;

	public float ClampAtNormal = 1.5f;

	public float ClampAtHard = 1.5f;

	public float ClampAtNightmare = 1.5f;

	public float IncrementPerLevel = 0.1f;

	public int[] Distances;

	public string[] LevelName;

	private float distance;

	private float maxDistance;

	private bool counting;

	private static DistanceManager instance;

	private GameObject player;

	private Vector3 lastPosition;

	private int lastDistance;

	private bool unlocked;

	private int currentLevelId;

	private bool justUnlockedNewLevel;

	private float origCoinsPerDistance = 0.5f;

	public static DistanceManager Instance
	{
		get
		{
			return instance;
		}
	}

	public int Distance
	{
		get
		{
			return (int)maxDistance;
		}
	}

	public bool LevelUnlocked
	{
		get
		{
			return justUnlockedNewLevel;
		}
	}

	private void Awake()
	{
		if (PlayerAccount.Instance == null)
		{
			base.enabled = false;
			return;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			if (PlayerAccount.Instance.CurrentLevelNum < 4)
			{
				unlocked = PlayerAccount.Instance.IsLevelUnlocked(PlayerAccount.Instance.CurrentLevelNum + 1);
			}
			else
			{
				unlocked = true;
			}
			GameEventDispatcher.AddListener("OnEndLessReset", EndLessReset);
			setCoinsPerDistance();
		}
		instance = this;
		distance = 0f;
		GameEventDispatcher.AddListener("OnLevelLoaded", OnStart);
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnStart);
		GameEventDispatcher.AddListener("OnLevelComplete", OnEnd);
		GameEventDispatcher.AddListener("OnPlayerDead", OnEnd);
		GameEventDispatcher.AddListener("PlayerDieFalling", OnEnd);
		GameEventDispatcher.AddListener("OnPlayerRespawn", OnEnd);
		GameEventDispatcher.AddListener("OnEndLessResurrect", EndLessResurrect);
	}

	private void Start()
	{
		player = CharHelper.GetPlayer();
		lastPosition = player.transform.position;
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			OnPlayerBonus.Instance.Multiplier = CoinsPerDistance;
			GameEventDispatcher.Dispatch(this, OnPlayerBonus.Instance);
		}
	}

	private void OnStart(object sender, GameEvent evt)
	{
		counting = true;
	}

	private void OnEnd(object sender, GameEvent evt)
	{
		counting = false;
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void Update()
	{
		if (!counting || !(player != null))
		{
			return;
		}
		distance += player.transform.position.z - lastPosition.z;
		if (distance > maxDistance)
		{
			maxDistance = distance;
		}
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			int num = Mathf.FloorToInt(distance);
			if (lastDistance != num)
			{
				if (currentLevelId < Distances.Length && num >= Distances[currentLevelId])
				{
					currentLevelId++;
				}
				if (num % BonusDistance == 0)
				{
					if (CoinsPerDistance < ClampAt)
					{
						OnPlayerBonus.Instance.Multiplier = CoinsPerDistance;
						GameEventDispatcher.Dispatch(this, OnPlayerBonus.Instance);
						CoinsPerDistance += IncrementPerLevel;
					}
					GameEventDispatcher.Dispatch(this, OnCoinPerDistance.Instance);
				}
			}
			lastDistance = num;
			switch (PlayerAccount.Instance.CurrentLevelNum)
			{
			case 1:
				if (distance >= (float)DistanceToUnlockNormalLevel && !unlocked)
				{
					UnlockNextLevel();
				}
				break;
			case 2:
				if (distance >= (float)DistanceToUnlockHardLevel && !unlocked)
				{
					UnlockNextLevel();
				}
				break;
			case 3:
				if (distance >= (float)DistanceToUnlockNightmareLevel && !unlocked)
				{
					UnlockNextLevel();
				}
				break;
			}
		}
		lastPosition = player.transform.position;
	}

	private void UnlockNextLevel()
	{
		Debug.Log("Unlocking next survival level.");
		PlayerAccount.Instance.UnlockNextLevel();
		unlocked = true;
		justUnlockedNewLevel = true;
	}

	private void EndLessReset(object sender, GameEvent evt)
	{
		if (PlayerAccount.Instance.CurrentLevelNum < 4)
		{
			unlocked = PlayerAccount.Instance.IsLevelUnlocked(PlayerAccount.Instance.CurrentLevelNum + 1);
		}
		else
		{
			unlocked = true;
		}
		counting = true;
		justUnlockedNewLevel = false;
		lastDistance = 0;
		maxDistance = 0f;
		distance = 0f;
		lastPosition = Vector3.zero;
		setCoinsPerDistance();
		currentLevelId = 0;
	}

	private void EndLessResurrect(object sender, GameEvent evt)
	{
		counting = true;
		lastPosition = player.transform.position;
	}

	private void setCoinsPerDistance()
	{
		switch (PlayerAccount.Instance.CurrentLevelNum)
		{
		case 1:
			origCoinsPerDistance = CoinsPerDistanceEasy;
			ClampAt = ClampAtEasy;
			break;
		case 2:
			origCoinsPerDistance = CoinsPerDistanceNormal;
			ClampAt = ClampAtNormal;
			break;
		case 3:
			origCoinsPerDistance = CoinsPerDistanceHard;
			ClampAt = ClampAtHard;
			break;
		case 4:
			origCoinsPerDistance = CoinsPerDistanceNightmare;
			ClampAt = ClampAtNightmare;
			break;
		}
		CoinsPerDistance = origCoinsPerDistance;
	}
}
