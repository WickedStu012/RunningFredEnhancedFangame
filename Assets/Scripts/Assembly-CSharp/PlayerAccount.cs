using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccount : MonoBehaviour
{
	public enum GameMode
	{
		Adventure = 0,
		Survival = 1,
		Challenge = 2
	}

	public delegate void OnAvatarChange(ItemInfo avatar);

	public delegate void OnLevelChange(LocationItemInfo chapter, int level);

	public delegate void OnMoneyCountChange(int money);

	public const int NoneMedal = 0;

	public const int BronzeMedal = 1;

	public const int SilverMedal = 2;

	public const int GoldMedal = 3;

	public const int Earnings1 = 10;

	public const int Earnings2 = 20;

	public const int Earnings3 = 40;

	public const int Earnings4 = 80;

	public AvatarItemInfo CurrentAvatarInfo;

	public LocationItemInfo CurrentChapterInfo;

	public string CurrentChapter;

	public string CurrentLevel;

	public int CurrentLevelNum = 1;

	public string CurrentChallenge;

	public GameMode CurrentGameMode;

	public bool UnlockEverything;

	public bool UnlockLevels = true;

	private int Money;

	private int HistoricalMoney;

	public int Earnings;

	public int DaysInARow;

	public int Days;

	public bool DailyEarningsChecked;

	public int[] SuggestShopRange;

	public int[] SuggestBuyLivesRange;

	public bool IsChapterComplete;

	public int DeathsCount;

	private Dictionary<string, int> chaptersOrder = new Dictionary<string, int>();

	private string[] CavesLevels = new string[10];

	private string[] CastleLevels = new string[10];

	private string[] RooftopLevels = new string[10];

	private Dictionary<string, string[]> Chapters = new Dictionary<string, string[]>();

	private LocationItemInfo lastChapterInfo;

	private string lastChapter;

	private string lastLevel;

	private int lastLevelNum = 1;

	private int lastMoney;

	private GameMode lastGameMode;

	private static PlayerAccount instance;

	private static bool destroyed;

	private bool syncing;

	private PlayerPrefsWrapper.OnWebDataStoreRes onResultSync;

	public static PlayerAccount Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(PlayerAccount)) as PlayerAccount;
				if (instance == null && destroyed)
				{
				}
			}
			return instance;
		}
	}

	public event OnAvatarChange AvatarChangeEvent;

	public event OnLevelChange LevelChangeEvent;

	public event OnMoneyCountChange MoneyChangeEvent;

	private void InitLevels()
	{
		lastChapter = "Castle";
		lastLevel = "Castle-Level1";
		lastLevelNum = 1;
		CastleLevels[0] = "Castle-Level1";
		CastleLevels[1] = "Castle-Level2";
		CastleLevels[2] = "Castle-Level3";
		CastleLevels[3] = "Castle-Level4";
		CastleLevels[4] = "Castle-Level5";
		CastleLevels[5] = "Castle-Level6";
		CastleLevels[6] = "Castle-Level7";
		CastleLevels[7] = "Castle-Level8";
		CastleLevels[8] = "Castle-Level9";
		CastleLevels[9] = "Castle-Level10";
		CavesLevels[0] = "Caves-Level1";
		CavesLevels[1] = "Caves-Level2";
		CavesLevels[2] = "Caves-Level3";
		CavesLevels[3] = "Caves-Level4";
		CavesLevels[4] = "Caves-Level5";
		CavesLevels[5] = "Caves-Level6";
		CavesLevels[6] = "Caves-Level7";
		CavesLevels[7] = "Caves-Level8";
		CavesLevels[8] = "Caves-Level9";
		CavesLevels[9] = "Caves-Level10";
		RooftopLevels[0] = "Rooftop-Level1";
		RooftopLevels[1] = "Rooftop-Level2";
		RooftopLevels[2] = "Rooftop-Level3";
		RooftopLevels[3] = "Rooftop-Level4";
		RooftopLevels[4] = "Rooftop-Level5";
		RooftopLevels[5] = "Rooftop-Level6";
		RooftopLevels[6] = "Rooftop-Level7";
		RooftopLevels[7] = "Rooftop-Level8";
		RooftopLevels[8] = "Rooftop-Level9";
		RooftopLevels[9] = "Rooftop-Level10";
		Chapters["Caves"] = CavesLevels;
		Chapters["Castle"] = CastleLevels;
		Chapters["Rooftop"] = RooftopLevels;
		chaptersOrder["Castle"] = 0;
		chaptersOrder["Caves"] = 1;
		chaptersOrder["Rooftop"] = 2;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		instance = this;
		GetComponent<WebDataStore>().SetNewDataArrivedFromBackendCallback(onNewDataArrivedFromBackend);
		BeLordTapJoy.callbackOnEarnedCurrency += OnTapjoyEarnedCurrency;
		BeLordTapJoy.callbackOnSpendCurrencyResponse += OnSpendSuccess;
		BeLordTapJoy.callbackOnSpendCurrencyResponseFailure += OnSpendError;
		BeLordTapJoy.callbackOnViewDidClose += OnTapjoyClose;
		BeLordTapJoy.callbackOnGetCurrencyBalanceResponse += OnGetCurrencyBalanceResponse;
		BeLordTapJoy.callbackOnGetCurrencyBalanceResponseFailure += OnGetCurrencyBalanceResponseFailure;
	}

	private void OnEnable()
	{
		instance = this;
		destroyed = false;
	}

	private void OnDestroy()
	{
		instance = null;
		destroyed = true;
	}

	public void Initialize()
	{
		if (PlayerPrefsWrapper.GetTimeFromStartDate() == DateTime.Now)
		{
			PlayerPrefsWrapper.SetFirstStartDate();
		}
		PlayerPrefsWrapper.IncrementSessions();
		Load();
		InitLevels();
		int currentAvatarId = PlayerPrefsWrapper.GetCurrentAvatarId();
		int num = 1;
		ItemInfo item = Store.Instance.GetItem(currentAvatarId);
		if (item != null)
		{
			if (item.Purchased)
			{
				CurrentAvatarInfo = (AvatarItemInfo)item;
			}
			else if (currentAvatarId == 0)
			{
				Store.Instance.Purchase(0);
				CurrentAvatarInfo = (AvatarItemInfo)item;
			}
			else
			{
				CurrentAvatarInfo = (AvatarItemInfo)Store.Instance.GetItem(0);
			}
		}
		else
		{
			CurrentAvatarInfo = (AvatarItemInfo)Store.Instance.GetItem(0);
		}
		currentAvatarId = PlayerPrefsWrapper.GetCurrentChapterId();
		LocationItemInfo locationItemInfo = (LocationItemInfo)Store.Instance.GetItem(currentAvatarId);
		if (locationItemInfo != null)
		{
			if (locationItemInfo.Purchased || UnlockEverything)
			{
				CurrentChapterInfo = locationItemInfo;
			}
			else if (currentAvatarId == 1000)
			{
				Store.Instance.Purchase(currentAvatarId);
				CurrentChapterInfo = locationItemInfo;
			}
			else
			{
				CurrentChapterInfo = (LocationItemInfo)Store.Instance.GetItem(1000);
			}
		}
		else
		{
			CurrentChapterInfo = (LocationItemInfo)Store.Instance.GetItem(1000);
		}
		locationItemInfo = (LocationItemInfo)Store.Instance.GetItem(1001);
		if (!locationItemInfo.Purchased)
		{
			Store.Instance.Purchase(1001);
		}
		num = PlayerPrefsWrapper.GetCurrentLevel();
		SelectLevel(num);
		lastMoney = RetrieveMoney();
	}

	public void SelectAvatar(ItemInfo avatar)
	{
		if (avatar.Purchased || avatar.Id == 30)
		{
			CurrentAvatarInfo = (AvatarItemInfo)avatar;
			PlayerPrefsWrapper.SetCurrentAvatar(avatar.Id);
			if (this.AvatarChangeEvent != null)
			{
				this.AvatarChangeEvent(CurrentAvatarInfo);
			}
			return;
		}
		avatar = Store.Instance.GetItem(0);
		CurrentAvatarInfo = (AvatarItemInfo)avatar;
		PlayerPrefsWrapper.SetCurrentAvatar(avatar.Id);
		if (this.AvatarChangeEvent != null)
		{
			this.AvatarChangeEvent(CurrentAvatarInfo);
		}
	}

	public void ChangeGameMode(GameMode gameMode)
	{
		if (CurrentGameMode == gameMode)
		{
			return;
		}
		CurrentGameMode = gameMode;
		if (gameMode == GameMode.Adventure)
		{
			LocationItemInfo locationItemInfo = (LocationItemInfo)Store.Instance.GetItem(PlayerPrefsWrapper.GetCurrentChapterId("Adventure"));
			if (locationItemInfo == null)
			{
				locationItemInfo = (LocationItemInfo)Store.Instance.GetItem(1000);
			}
			SelectChapter(locationItemInfo);
			SelectLevel(PlayerPrefsWrapper.GetCurrentLevel("Adventure"));
		}
		else
		{
			LocationItemInfo locationItemInfo2 = (LocationItemInfo)Store.Instance.GetItem(PlayerPrefsWrapper.GetCurrentChapterId("Survival"));
			if (locationItemInfo2 == null)
			{
				locationItemInfo2 = (LocationItemInfo)Store.Instance.GetItem(1001);
			}
			SelectChapter(locationItemInfo2);
			SelectLevel(PlayerPrefsWrapper.GetCurrentLevel("Survival"));
		}
	}

	public void SelectChapter(LocationItemInfo chapter)
	{
		if (chapter != null && chapter.Purchased)
		{
			lastChapterInfo = CurrentChapterInfo;
			lastChapter = CurrentChapter;
			lastLevel = CurrentLevel;
			lastLevelNum = CurrentLevelNum;
			lastGameMode = CurrentGameMode;
			CurrentChapterInfo = chapter;
			PlayerPrefsWrapper.SetCurrentChapter(chapter.Id);
			if (chapter.Tag != "Survival")
			{
				CurrentGameMode = GameMode.Adventure;
			}
			else
			{
				CurrentGameMode = GameMode.Survival;
			}
			if (this.LevelChangeEvent != null)
			{
				this.LevelChangeEvent(CurrentChapterInfo, CurrentLevelNum);
			}
			return;
		}
		chapter = (LocationItemInfo)Store.Instance.GetItem(1000);
		CurrentLevelNum = 1;
		SelectLevel(1);
		CurrentChapterInfo = chapter;
		PlayerPrefsWrapper.SetCurrentChapter(chapter.Id);
		CurrentGameMode = GameMode.Adventure;
		if (this.LevelChangeEvent != null)
		{
			this.LevelChangeEvent(CurrentChapterInfo, CurrentLevelNum);
		}
		lastChapterInfo = CurrentChapterInfo;
		lastChapter = CurrentChapter;
		lastLevel = CurrentLevel;
		lastLevelNum = CurrentLevelNum;
		lastGameMode = CurrentGameMode;
	}

	public void SelectLevel(int levelNum)
	{
		if (CurrentGameMode != GameMode.Challenge && (levelNum == 1 || IsLevelUnlocked(levelNum)) && Chapters[CurrentChapterInfo.ScenePrefix][levelNum - 1] != null)
		{
			lastChapterInfo = CurrentChapterInfo;
			lastChapter = CurrentChapter;
			lastLevel = CurrentLevel;
			lastLevelNum = CurrentLevelNum;
			PlayerPrefsWrapper.SetCurrentLevel(levelNum);
			if (CurrentChapterInfo.Tag != "Survival")
			{
				CurrentGameMode = GameMode.Adventure;
				CurrentLevel = Chapters[CurrentChapterInfo.ScenePrefix][levelNum - 1];
				PlayerPrefsWrapper.SetCurrentLevel("Adventure", levelNum);
				PlayerPrefsWrapper.SetCurrentChapter("Adventure", CurrentChapterInfo.Id);
			}
			else
			{
				CurrentGameMode = GameMode.Survival;
				CurrentLevel = CurrentChapterInfo.ScenePrefix + "-Random";
				PlayerPrefsWrapper.SetCurrentLevel("Survival", levelNum);
				PlayerPrefsWrapper.SetCurrentChapter("Survival", CurrentChapterInfo.Id);
			}
			CurrentLevelNum = levelNum;
			if (this.LevelChangeEvent != null)
			{
				this.LevelChangeEvent(CurrentChapterInfo, CurrentLevelNum);
			}
		}
	}

	public void SelectChallenge(string challenge)
	{
		lastGameMode = CurrentGameMode;
		CurrentChallenge = challenge;
		CurrentGameMode = GameMode.Challenge;
	}

	public void UnselectChallenge()
	{
		if (CurrentGameMode == GameMode.Challenge)
		{
			CurrentGameMode = lastGameMode;
		}
	}

	public void UnlockNextLevel()
	{
		if (CurrentLevelNum < 10)
		{
			IsChapterComplete = false;
			if (!UnlockEverything && !UnlockLevels)
			{
				UnlockLevel(CurrentLevelNum + 1);
				if (CurrentGameMode != GameMode.Survival)
				{
					SelectLevel(CurrentLevelNum + 1);
				}
				return;
			}
			for (int i = CurrentLevelNum + 1; i <= 10; i++)
			{
				if (IsLevelAvailable(i))
				{
					SelectLevel(i);
					return;
				}
			}
			IsChapterComplete = true;
		}
		else
		{
			lastChapterInfo = CurrentChapterInfo;
			lastChapter = CurrentChapter;
			lastLevel = CurrentLevel;
			lastLevelNum = CurrentLevelNum;
			IsChapterComplete = true;
		}
	}

	public void UnlockLevel(int levelNum)
	{
		if (CurrentChapterInfo.Tag != "Survival")
		{
			PlayerPrefsWrapper.UnlockLevel(CurrentChapterInfo.ScenePrefix, levelNum);
		}
		else
		{
			PlayerPrefsWrapper.UnlockLevel(CurrentChapterInfo.ScenePrefix + "-Random", levelNum);
		}
	}

	public bool IsLevelUnlocked(int levelNum)
	{
		if (CurrentChapterInfo == null)
		{
			return false;
		}
		if (CurrentChapterInfo.Tag != "Survival")
		{
			if (!Chapters.ContainsKey(CurrentChapterInfo.ScenePrefix))
			{
				return false;
			}
			if (Chapters[CurrentChapterInfo.ScenePrefix][levelNum - 1] == null)
			{
				return false;
			}
		}
		if (UnlockEverything || UnlockLevels)
		{
			return true;
		}
		if (levelNum == 1)
		{
			return true;
		}
		if (CurrentChapterInfo.Tag != "Survival")
		{
			return PlayerPrefsWrapper.IsLevelUnlocked(CurrentChapterInfo.ScenePrefix, levelNum);
		}
		return PlayerPrefsWrapper.IsLevelUnlocked(CurrentChapterInfo.ScenePrefix + "-Random", levelNum);
	}

	private bool IsLevelAvailable(int levelNum)
	{
		return Chapters[CurrentChapterInfo.ScenePrefix][levelNum - 1] != null;
	}

	public void UndoLevelSelect()
	{
		CurrentChapterInfo = lastChapterInfo;
		CurrentChapter = lastChapter;
		CurrentLevel = lastLevel;
		CurrentLevelNum = lastLevelNum;
		CurrentGameMode = lastGameMode;
	}

	private int GetMedalNum()
	{
		return PlayerPrefsWrapper.GetMedal(CurrentChapterInfo.ScenePrefix, CurrentLevelNum);
	}

	public int GetMedal(string scenePrefix, int level)
	{
		return PlayerPrefsWrapper.GetMedal(scenePrefix, level);
	}

	public void SetGoldMedal()
	{
		int medal = GetMedal(CurrentChapterInfo.ScenePrefix, lastLevelNum);
		if (medal < 3)
		{
			PlayerPrefsWrapper.SetGoldMedal(CurrentChapterInfo.ScenePrefix, lastLevelNum);
		}
	}

	public void SetSilverMedal()
	{
		int medal = GetMedal(CurrentChapterInfo.ScenePrefix, lastLevelNum);
		if (medal < 2)
		{
			PlayerPrefsWrapper.SetSilverMedal(CurrentChapterInfo.ScenePrefix, lastLevelNum);
		}
	}

	public void SetBronzeMedal()
	{
		int medal = GetMedal(CurrentChapterInfo.ScenePrefix, lastLevelNum);
		if (medal < 1)
		{
			PlayerPrefsWrapper.SetBronzeMedal(CurrentChapterInfo.ScenePrefix, lastLevelNum);
		}
	}

	public int RetrieveMoney()
	{
		Money = PlayerPrefsWrapper.GetMoney();
		return Money;
	}

	public void AddMoney(int money)
	{
		if (lastMoney != money)
		{
			lastMoney = Money;
		}
		if (HistoricalMoney == 0)
		{
			HistoricalMoney = PlayerPrefsWrapper.GetHistoricalMoney();
		}
		if (HistoricalMoney < Money)
		{
			HistoricalMoney = Money;
		}
		HistoricalMoney += money;
		PlayerPrefsWrapper.SetHistoricalMoney(HistoricalMoney);
		if (ConfigParams.IsKongregate())
		{
			KongregateAPI.ReportBadgeHistoricalMoney(HistoricalMoney);
		}
		SetMoney(Money + money);
	}

	public void SubMoney(int money)
	{
		SetMoney(Money - money);
	}

	private void SetMoney(int money)
	{
		PlayerPrefsWrapper.SetMoney(money);
		Money = money;
		if (this.MoneyChangeEvent != null)
		{
			this.MoneyChangeEvent(Money);
		}
	}

	public void PickUpGift(int giftId)
	{
		PlayerPrefsWrapper.PickUpGift(giftId);
	}

	public int GetGift()
	{
		return PlayerPrefsWrapper.GetGift();
	}

	public bool IsGiftActive()
	{
		if (PlayerPrefsWrapper.IsGiftActive())
		{
			switch (PlayerPrefsWrapper.GetGift())
			{
			case 1:
				return PlayerPrefsWrapper.IsDoubleSkullyActive();
			case 0:
				return PlayerPrefsWrapper.IsPiggyBankActive();
			case 3:
				return PlayerPrefsWrapper.IsExtraLifeActive();
			case 2:
				return PlayerPrefsWrapper.IsDiscountActive();
			}
		}
		return false;
	}

	public void ClearGift()
	{
		PlayerPrefsWrapper.ClearGift();
	}

	public void ActivatePiggyBank()
	{
		PlayerPrefsWrapper.ActivatePiggyBank();
	}

	public bool IsPiggyBankActive()
	{
		return PlayerPrefsWrapper.IsPiggyBankActive();
	}

	public void ActivateDoubleSkully()
	{
		PlayerPrefsWrapper.ActivateDoubleSkully();
	}

	public void ActivateEarlyStart()
	{
		PlayerPrefsWrapper.ActivateEarlyStart();
	}

	public void ActivateDrowsyReaper()
	{
		PlayerPrefsWrapper.ActivateDrowsyReaper();
	}

	public void ActivateExtraLife()
	{
		PlayerPrefsWrapper.ActivateExtraLife();
	}

	public void ActivateDiscount()
	{
		PlayerPrefsWrapper.ActivateDiscount();
	}

	public void DeactivateDiscount()
	{
		PlayerPrefsWrapper.DeactivateDiscount();
	}

	public bool IsDiscountActive()
	{
		return PlayerPrefsWrapper.IsDiscountActive();
	}

	public void AddMoneyInPiggyBank(int money)
	{
		int moneyInPiggyBank = GetMoneyInPiggyBank();
		moneyInPiggyBank += money;
		PlayerPrefsWrapper.SetMoneyInPiggyBank(moneyInPiggyBank);
	}

	public int GetMoneyInPiggyBank()
	{
		return PlayerPrefsWrapper.GetMoneyInPiggyBank();
	}

	public void ClearMoneyInPiggyBank()
	{
		PlayerPrefsWrapper.SetMoneyInPiggyBank(0);
	}

	public void PickupTreasure(int id)
	{
		PlayerPrefsWrapper.PickupTreasure(id, CurrentChapterInfo.ScenePrefix, CurrentLevelNum);
	}

	public bool IsTreasureActive(int id)
	{
		return PlayerPrefsWrapper.IsTreasureActive(id, CurrentChapterInfo.ScenePrefix, CurrentLevelNum);
	}

	public void PickupGrimmyIdolInCurrentLevel()
	{
		if (LevelProps.Instance != null)
		{
			PlayerPrefsWrapper.PickupGrimmyIdol(LevelProps.Instance.LevelId);
		}
	}

	public bool IsGrimmyIdolTakenForCurrentLevel()
	{
		if (LevelProps.Instance != null)
		{
			return PlayerPrefsWrapper.IsGrimmyIdolTaken(LevelProps.Instance.LevelId);
		}
		return false;
	}

	public int GetGrimmyIdolPickedCount()
	{
		return PlayerPrefsWrapper.GetGrimmyIdolPickedCount();
	}

	public int MagnetLevel()
	{
		return PlayerPrefsWrapper.ItemUpgrades(102);
	}

	public int WallGrip()
	{
		return PlayerPrefsWrapper.ItemUpgrades(103);
	}

	public int ChickenFlaps()
	{
		return PlayerPrefsWrapper.ItemUpgrades(112);
	}

	public int Lives()
	{
		return PlayerPrefsWrapper.ItemUpgrades(111);
	}

	public bool DoubleJump()
	{
		return PlayerPrefsWrapper.IsItemPurchased(101);
	}

	public int WallBounce()
	{
		return PlayerPrefsWrapper.ItemUpgrades(100);
	}

	public bool FastRecovery()
	{
		return PlayerPrefsWrapper.IsItemPurchased(110);
	}

	public void CheckDayEarnings()
	{
		if (Application.platform == RuntimePlatform.NaCl)
		{
			return;
		}
		Earnings = 0;
		DaysInARow = 0;
		Days = PlayerPrefsWrapper.GetDaysFromLastStart();
		if (Days > 1)
		{
			Earnings = 10;
			DaysInARow = 0;
			PlayerPrefsWrapper.SetDaysInARow(0);
		}
		else if (Days == 1)
		{
			DaysInARow = PlayerPrefsWrapper.GetDaysInARow();
			switch (DaysInARow)
			{
			case 0:
				Earnings = 10;
				break;
			case 1:
				Earnings = 20;
				break;
			case 2:
				Earnings = 40;
				break;
			case 3:
				Earnings = 80;
				break;
			default:
				DaysInARow = 0;
				Earnings = 10;
				break;
			}
			PlayerPrefsWrapper.SetDaysInARow(DaysInARow + 1);
		}
		PlayerPrefsWrapper.SetLastIncomeDate();
		if (Earnings != 0)
		{
			AddMoney(Earnings);
		}
	}

	public int GetChapterOrder(string chapter)
	{
		if (chaptersOrder.ContainsKey(chapter))
		{
			return chaptersOrder[chapter];
		}
		return 0;
	}

	public bool ShowShopSuggestion()
	{
		if (SuggestShopRange == null || SuggestShopRange.Length == 0)
		{
			return false;
		}
		int[] suggestShopRange = SuggestShopRange;
		foreach (int num in suggestShopRange)
		{
			if (lastMoney < num && Money >= num)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShowBuyLivesSuggestion()
	{
		if (SuggestBuyLivesRange == null || SuggestBuyLivesRange.Length == 0 || Lives() > 0)
		{
			return false;
		}
		int[] suggestBuyLivesRange = SuggestBuyLivesRange;
		foreach (int num in suggestBuyLivesRange)
		{
			if (DeathsCount == num)
			{
				return true;
			}
		}
		return false;
	}

	public void IncrementDeathCounter()
	{
		DeathsCount++;
	}

	public bool IsChallengeUnlocked(int id)
	{
		return PlayerPrefsWrapper.IsChallengeUnlocked(id);
	}

	public void UnlockeChallenge(int id)
	{
		PlayerPrefsWrapper.UnlockChallenge(id);
	}

	public void SetTimeToLevel(int time)
	{
		bool flag = true;
		int num = 0;
		int timeFromLevel = GetTimeFromLevel(CurrentChapterInfo.ScenePrefix, (!IsChapterComplete) ? (CurrentLevelNum - 1) : CurrentLevelNum);
		if (timeFromLevel == 0 || timeFromLevel > time)
		{
			PlayerPrefsWrapper.SetTimeToLevel(CurrentChapterInfo.ScenePrefix, (!IsChapterComplete) ? (CurrentLevelNum - 1) : CurrentLevelNum, time);
		}
		for (int i = 1; i <= 10; i++)
		{
			int timeFromLevel2 = GetTimeFromLevel(CurrentChapterInfo.ScenePrefix, i);
			if (timeFromLevel2 == 0)
			{
				flag = false;
				break;
			}
			num += timeFromLevel2;
		}
		if (flag)
		{
			ReportScore(CurrentChapterInfo.ScenePrefix, (!IsChapterComplete) ? (CurrentLevelNum - 1) : CurrentLevelNum, num);
		}
	}

	public int GetTimeFromLevel(string scenePrefix, int levelNum)
	{
		return PlayerPrefsWrapper.GetTimeFromLevel(scenePrefix, levelNum);
	}

	public void SetMetersToLevel(int meters)
	{
		int metersFromLevel = GetMetersFromLevel(CurrentLevel, CurrentLevelNum);
		if (metersFromLevel < meters)
		{
			PlayerPrefsWrapper.SetMetersToLevel(CurrentLevel, CurrentLevelNum, meters);
		}
		if (ConfigParams.IsKongregate())
		{
			ReportScore(CurrentLevel, CurrentLevelNum, meters);
		}
		else
		{
			ReportScore(CurrentLevel, CurrentLevelNum, Mathf.Max(metersFromLevel, meters));
		}
	}

	public int GetMetersFromLevel(string scenePrefix, int levelNum)
	{
		return PlayerPrefsWrapper.GetMetersFromLevel(scenePrefix, levelNum);
	}

	public int GetMetersFromCurrentLevel()
	{
		return PlayerPrefsWrapper.GetMetersFromLevel(CurrentLevel, CurrentLevelNum);
	}

	public void ReportScore(string scenePrefix, int level, int score)
	{
		Debug.Log("Reporting score for: " + scenePrefix + " - L: " + level + " - Score: " + score);
		switch (scenePrefix)
		{
		case "Castle":
			BeLord.ReportScore("grislymanor", score);
			break;
		case "Caves":
			BeLord.ReportScore("dangercaves", score);
			break;
		case "Rooftop":
			BeLord.ReportScore("highstakes", score);
			break;
		case "Castle-Random":
			switch (level)
			{
			case 1:
				BeLord.ReportScore("endlessmanoreasy", score);
				break;
			case 2:
				BeLord.ReportScore("endlessmanornormal", score);
				break;
			case 3:
				BeLord.ReportScore("endlessmanorhard", score);
				break;
			case 4:
				BeLord.ReportScore("endlessmanornightmare", score);
				break;
			}
			break;
		case "Caves-Random":
			switch (level)
			{
			case 1:
				BeLord.ReportScore("endlesscaveeasy", score);
				break;
			case 2:
				BeLord.ReportScore("endlesscavenormal", score);
				break;
			case 3:
				BeLord.ReportScore("endlesscavehard", score);
				break;
			case 4:
				BeLord.ReportScore("endlesscavenightmare", score);
				break;
			}
			break;
		case "Rooftop-Random":
			switch (level)
			{
			case 1:
				BeLord.ReportScore("endlessstakeseasy", score);
				break;
			case 2:
				BeLord.ReportScore("endlessstakesnormal", score);
				break;
			case 3:
				BeLord.ReportScore("endlessstakeshard", score);
				break;
			case 4:
				BeLord.ReportScore("endlessstakesnightmare", score);
				break;
			}
			break;
		}
	}

	private void checkEndlessCheckpointKongregateBadge(int level, int currentScore)
	{
		if (level == 1 && currentScore >= 5000)
		{
			Debug.Log("Kongregate.ReportScore. Badge: reach_ckp_endless_easy");
			KongregateAPI.ReportScore("reach_ckp_endless_easy", 1);
		}
		else if (level == 2 && currentScore >= 3500)
		{
			Debug.Log("Kongregate.ReportScore. Badge: reach_ckp_endless_normal");
			KongregateAPI.ReportScore("reach_ckp_endless_normal", 1);
		}
		else if (level == 3 && currentScore >= 2500)
		{
			Debug.Log("Kongregate.ReportScore. Badge: reach_ckp_endless_hard");
			KongregateAPI.ReportScore("reach_ckp_endless_hard", 1);
		}
		else if (level == 4 && currentScore >= 1000)
		{
			Debug.Log("Kongregate.ReportScore. Badge: reach_ckp_endless_nightmare");
			KongregateAPI.ReportScore("reach_ckp_endless_nightmare", 1);
		}
	}

	public void AddToTotalMeters(int meters)
	{
		PlayerPrefsWrapper.AddToTotalMeters(meters);
	}

	public int GetTotalMeters()
	{
		return PlayerPrefsWrapper.GetTotalMeters();
	}

	public int GetTotalSessions()
	{
		return PlayerPrefsWrapper.GetTotalSessions();
	}

	public int DaysFromFirstStart()
	{
		return (DateTime.Now - PlayerPrefsWrapper.GetTimeFromStartDate()).Days;
	}

	public int LastAskRate()
	{
		return (DateTime.Now - PlayerPrefsWrapper.LastAskRate()).Days;
	}

	public void SetLastAskRate()
	{
		PlayerPrefsWrapper.SetLastAskRate();
	}

	public bool WasRateAsked()
	{
		return PlayerPrefsWrapper.WasRateAsked();
	}

	public bool CanRate()
	{
		return PlayerPrefsWrapper.CanRate();
	}

	public void CanRate(bool canRate)
	{
		PlayerPrefsWrapper.CanRate(canRate);
	}

	public static void SetLastTapjoyMoney(int money)
	{
		PlayerPrefsWrapper.SetLastTapjoyMoney(money);
	}

	public static int GetLastTapjoyMoney()
	{
		return PlayerPrefsWrapper.GetLastTapjoyMoney();
	}

	public static void CheckTapjoyEarnings()
	{
		if (BeLordTapJoy.Instance != null)
		{
			BeLordTapJoy.Instance.GetCurrencyBalance();
		}
	}

	private static void OnTapjoyEarnedCurrency(int points)
	{
		SpendCurrency(points);
	}

	private static void OnTapjoyClose()
	{
	}

	private static void OnGetCurrencyBalanceResponse(int points)
	{
		int lastTapjoyMoney = GetLastTapjoyMoney();
		if (BeLordTapJoy.DebugMode)
		{
			Debug.Log("Current Tapjoy money: " + points + " - lastTapjoyMoney: " + lastTapjoyMoney);
		}
		SpendCurrency(points);
	}

	private static void SpendCurrency(int points)
	{
		int lastTapjoyMoney = GetLastTapjoyMoney();
		if (points != 0)
		{
			if (BeLordTapJoy.Instance != null)
			{
				SetLastTapjoyMoney(points);
				BeLordTapJoy.Instance.SpendCurrency(points);
			}
		}
		else if (lastTapjoyMoney > 0)
		{
			if (BeLordTapJoy.DebugMode)
			{
				Debug.Log("SpendCurrency added lastTapjoyMoney: " + lastTapjoyMoney);
			}
			Instance.AddMoney(lastTapjoyMoney);
			Instance.Save(true);
			SetLastTapjoyMoney(0);
		}
	}

	private static void OnGetCurrencyBalanceResponseFailure()
	{
		if (BeLordTapJoy.DebugMode)
		{
			Debug.Log("OnGetTapPointsError function was called");
		}
	}

	private static void OnSpendSuccess(int balance)
	{
		int lastTapjoyMoney = GetLastTapjoyMoney();
		if (BeLordTapJoy.DebugMode)
		{
			Debug.Log("Money after spend: " + balance + " lastTapjoyMoney: " + lastTapjoyMoney);
		}
		Instance.AddMoney(lastTapjoyMoney);
		Instance.Save(true);
		SetLastTapjoyMoney(0);
		if (balance > 0)
		{
			SpendCurrency(balance);
		}
	}

	private static void OnSpendError()
	{
		if (BeLordTapJoy.DebugMode)
		{
			Debug.Log("OnSpendError function was called");
		}
	}

	public void Save()
	{
		Save(false);
	}

	public void Save(bool force)
	{
		PlayerPrefsWrapper.Save(force);
	}

	public void Load()
	{
		PlayerPrefsWrapper.Load();
	}

	public void Sync(PlayerPrefsWrapper.OnWebDataStoreRes onResult)
	{
		if (!syncing)
		{
			syncing = true;
			onResultSync = onResult;
			PlayerPrefsWrapper.Sync(OnResult);
			PlayerPrefsWrapper.IsInitializing = false;
		}
	}

	public void OnResult(bool result)
	{
		if (onResultSync != null)
		{
			onResultSync(result);
		}
		syncing = false;
	}

	public void DeleteAll()
	{
		if (!ConfigParams.isKindle)
		{
			CheckTapjoyEarnings();
		}
		PlayerPrefsWrapper.DeleteAll();
	}

	public void SyncCalledFromXCode()
	{
		if (!syncing)
		{
			syncing = true;
			PlayerPrefsWrapper.Sync(OnSync);
			PlayerPrefsWrapper.IsInitializing = false;
		}
	}

	public void SetCurrencyDecimalSeparatorFromXCode(string groupingSeparator)
	{
		ConfigParams.decSep = groupingSeparator;
	}

	public void SetCountryCodeFromXCode(string countryCode)
	{
		Debug.Log("countryCode :" + countryCode);
		PlayerPrefs.SetString("country_code", countryCode);
	}

	public string GetCountryCode()
	{
		return PlayerPrefs.GetString("country_code");
	}

	private void OnSync(bool overwrite)
	{
		if (overwrite && DedalordLoadLevel.GetLevel() == Levels.MainMenu)
		{
			Debug.Log("Loading syncronize scene...");
			DedalordLoadLevel.LoadSync();
		}
		syncing = false;
	}

	private void onNewDataArrivedFromBackend()
	{
		if (!syncing)
		{
			Debug.Log("New data arrived from backend, sync");
			syncing = true;
			PlayerPrefsWrapper.Sync(OnSync);
			PlayerPrefsWrapper.IsInitializing = false;
		}
	}
}
