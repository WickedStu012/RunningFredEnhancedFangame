using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsWrapper
{
	public delegate void OnWebDataStoreRes(bool result);

	public delegate void OnGameDataChange();

	private const int NoneMedal = 0;

	private const int BronzeMedal = 1;

	private const int SilverMedal = 2;

	private const int GoldMedal = 3;

	public static GameDataExtended_v2 gameData = new GameDataExtended_v2();

	public static bool IsInitializing = true;

	private static OnWebDataStoreRes onWebDataStoreRes = null;

	private static bool enableSaveOnServer = true;

	public static event OnGameDataChange OnGameDataChangeEvent;

	static PlayerPrefsWrapper()
	{
		PlayerPrefsWrapper.OnGameDataChangeEvent = null;
	}

	public static int GetCurrentAvatarId()
	{
		return gameData.CurrentAvatarId;
	}

	public static int GetCurrentChapterId()
	{
		if (gameData.CurrentChapterId == -1)
		{
			gameData.CurrentChapterId = gameData.AdventureChapterId;
		}
		return gameData.CurrentChapterId;
	}

	public static int GetCurrentLevel()
	{
		return gameData.CurrentLevel;
	}

	public static int GetCurrentChapterId(string gameMode)
	{
		if (gameMode == "Adventure")
		{
			return gameData.AdventureChapterId;
		}
		return gameData.SurvivalChapterId;
	}

	public static int GetCurrentLevel(string gameMode)
	{
		if (gameMode == "Adventure")
		{
			return gameData.AdventureLevel;
		}
		return gameData.SurvivalLevel;
	}

	public static void SetCurrentAvatar(int id)
	{
		gameData.CurrentAvatarId = (byte)id;
		Save();
	}

	public static void SetCurrentChapter(int id)
	{
		gameData.CurrentChapterId = (short)id;
		Save();
	}

	public static void SetCurrentLevel(int levelNum)
	{
		gameData.CurrentLevel = (byte)levelNum;
		Save();
	}

	public static void SetCurrentChapter(string gameMode, int id)
	{
		if (gameMode == "Adventure")
		{
			gameData.AdventureChapterId = (short)id;
		}
		else
		{
			gameData.SurvivalChapterId = (short)id;
		}
		Save();
	}

	public static void SetCurrentLevel(string gameMode, int levelNum)
	{
		if (gameMode == "Adventure")
		{
			gameData.AdventureLevel = (byte)levelNum;
		}
		else
		{
			gameData.SurvivalLevel = (byte)levelNum;
		}
		Save();
	}

	public static void UnlockLevel(string scenePrefix, int levelNum)
	{
		if (gameData.UnlockLevelChapter[gameData.ChaptersId[scenePrefix]] < levelNum)
		{
			gameData.UnlockLevelChapter[gameData.ChaptersId[scenePrefix]] = (byte)levelNum;
		}
		Save();
	}

	public static bool IsLevelUnlocked(string scenePrefix, int levelNum)
	{
		return gameData.UnlockLevelChapter[gameData.ChaptersId[scenePrefix]] >= (byte)levelNum;
	}

	public static int GetMedal(string scenePrefix, int levelNum)
	{
		return gameData.MedalsEarned[gameData.ChaptersId[scenePrefix], levelNum - 1];
	}

	public static void SetGoldMedal(string scenePrefix, int levelNum)
	{
		gameData.MedalsEarned[gameData.ChaptersId[scenePrefix], levelNum - 1] = 3;
		Save();
	}

	public static void SetSilverMedal(string scenePrefix, int levelNum)
	{
		gameData.MedalsEarned[gameData.ChaptersId[scenePrefix], levelNum - 1] = 2;
		Save();
	}

	public static void SetBronzeMedal(string scenePrefix, int levelNum)
	{
		gameData.MedalsEarned[gameData.ChaptersId[scenePrefix], levelNum - 1] = 1;
		Save();
	}

	public static int GetMoney()
	{
		return gameData.PlayerMoney;
	}

	public static void SetMoney(int money)
	{
		if (money > gameData.PlayerMoney)
		{
			gameData.Revision += money - gameData.PlayerMoney;
		}
		gameData.PlayerMoney = money;
		Save();
	}

	public static void SetHistoricalMoney(int historicalMoneyValue)
	{
		string text = string.Empty;
		if (ConfigParams.IsKongregate())
		{
			text = KongregateAPI.GetUserName();
		}
		PlayerPrefs.SetInt(text + "-hm", historicalMoneyValue);
		PlayerPrefs.Save();
	}

	public static int GetHistoricalMoney()
	{
		string text = string.Empty;
		if (ConfigParams.IsKongregate())
		{
			text = KongregateAPI.GetUserName();
		}
		return PlayerPrefs.GetInt(text + "-hm", 0);
	}

	public static void PurchaseItem(ItemInfo item)
	{
		if (gameData.PurchasedItems.Contains((short)item.Id))
		{
			int index = gameData.PurchasedItems.IndexOf((short)item.Id);
			gameData.ItemUpgrades[index] = (byte)item.Upgrades;
			gameData.ItemsCount[index] = (byte)item.Count;
		}
		else
		{
			gameData.PurchasedItems.Add((short)item.Id);
			gameData.ItemUpgrades.Add((byte)item.Upgrades);
			gameData.ItemsCount.Add((byte)item.Count);
		}
		Save();
	}

	public static bool IsItemPurchased(int id)
	{
		return gameData.PurchasedItems.Contains((short)id);
	}

	public static int ItemUpgrades(int id)
	{
		int num = gameData.PurchasedItems.IndexOf((short)id);
		if (num != -1)
		{
			return gameData.ItemUpgrades[num];
		}
		return 0;
	}

	public static int ItemCount(int id)
	{
		int num = gameData.PurchasedItems.IndexOf((short)id);
		if (num != -1)
		{
			return gameData.ItemsCount[num];
		}
		return 0;
	}

	public static int ConsumeItem(ItemInfo item)
	{
		int num = gameData.PurchasedItems.IndexOf((short)item.Id);
		int num2 = 0;
		if (num != -1)
		{
			num2 = gameData.ItemsCount[num] - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			item.Count = num2;
			gameData.ItemsCount[num] = (byte)num2;
		}
		Save();
		return num2;
	}

	public static int AddItem(ItemInfo item, int numToAdd)
	{
		int num = gameData.PurchasedItems.IndexOf((short)item.Id);
		int num2 = 0;
		if (num == -1)
		{
			PurchaseItem(item);
			num = gameData.PurchasedItems.IndexOf((short)item.Id);
		}
		if (num != -1)
		{
			num2 = (item.Count = gameData.ItemsCount[num] + numToAdd);
			gameData.ItemsCount[num] = (byte)num2;
		}
		Save();
		return num2;
	}

	public static int AddItem(ItemInfo item)
	{
		return AddItem(item, 1);
	}

	public static void SetMusicVolume(float volume)
	{
		PlayerPrefs.SetFloat("MusicVolume", volume);
	}

	public static float GetMusicVolume()
	{
		return PlayerPrefs.GetFloat("MusicVolume", 1f);
	}

	public static void SetFXVolume(float volume)
	{
		PlayerPrefs.SetFloat("FXVolume", volume);
	}

	public static float GetFXVolume()
	{
		return PlayerPrefs.GetFloat("FXVolume", 1f);
	}

	public static void SetGore(bool gore)
	{
		PlayerPrefs.SetInt("Gore", gore ? 1 : 0);
	}

	public static bool GetGore(bool defaultValue)
	{
		return PlayerPrefs.GetInt("Gore", defaultValue ? 1 : 0) == 1;
	}

	public static void SetGameCenter(bool enable)
	{
		PlayerPrefs.SetInt("GameCenter", enable ? 1 : 0);
	}

	public static bool GetGameCenter(bool defaultValue)
	{
		return PlayerPrefs.GetInt("GameCenter", defaultValue ? 1 : 0) == 1;
	}

	public static void SetICloud(bool enable)
	{
		PlayerPrefs.SetInt("iCloud", enable ? 1 : 0);
	}

	public static bool GetICloud(bool defaultValue)
	{
		return PlayerPrefs.GetInt("iCloud", defaultValue ? 1 : 0) == 1;
	}

	public static void SetiPodMusic(bool iPodMusic)
	{
		PlayerPrefs.SetInt("iPodMusic", iPodMusic ? 1 : 0);
	}

	public static bool GetiPodMusic()
	{
		return PlayerPrefs.GetInt("iPodMusic", 0) == 1;
	}

	public static bool GetTutorial()
	{
		return PlayerPrefs.GetInt("Tutorial", 1) == 1;
	}

	public static void SetTutorial(bool tutorial)
	{
		PlayerPrefs.SetInt("Tutorial", tutorial ? 1 : 0);
	}

	public static void SetSkiingFredAvailable(bool skfAvailable)
	{
		PlayerPrefs.SetInt("SkiingFredAvailable", skfAvailable ? 1 : 0);
	}

	public static bool GetSkiingFredAvailable()
	{
		return PlayerPrefs.GetInt("SkiingFredAvailable", 0) == 1;
	}

	public static void ClearGift()
	{
		gameData.ActiveGiftId = -1;
		Save();
	}

	public static void PickUpGift(int giftId)
	{
		gameData.ActiveGiftId = (sbyte)giftId;
		Save();
	}

	public static int GetGift()
	{
		return gameData.ActiveGiftId;
	}

	public static bool IsGiftActive()
	{
		return gameData.ActiveGiftId != -1;
	}

	public static void ActivatePiggyBank()
	{
		gameData.PiggyBankExpireDate = DateTime.Now.AddDays(1.0);
		Save();
	}

	public static bool IsPiggyBankActive()
	{
		if (DateTime.Now < gameData.PiggyBankExpireDate)
		{
			return true;
		}
		return false;
	}

	public static int GetMoneyInPiggyBank()
	{
		return gameData.MoneyInPiggyBank;
	}

	public static void SetMoneyInPiggyBank(int money)
	{
		gameData.MoneyInPiggyBank = money;
		Save();
	}

	public static void ActivateDoubleSkully()
	{
		gameData.DoubleSkully = true;
		Save();
	}

	public static bool IsDoubleSkullyActive()
	{
		if (gameData.DoubleSkully)
		{
			gameData.DoubleSkully = false;
			Save();
			return true;
		}
		return false;
	}

	public static void ActivateEarlyStart()
	{
		gameData.EarlyStart = true;
		Save();
	}

	public static bool IsEarlyStartActive()
	{
		if (gameData.EarlyStart)
		{
			gameData.EarlyStart = false;
			Save();
			return true;
		}
		return false;
	}

	public static void ActivateDrowsyReaper()
	{
		gameData.DrowsyReaper = true;
		Save();
	}

	public static bool IsDrowsyReaperActive()
	{
		if (gameData.DrowsyReaper)
		{
			gameData.DrowsyReaper = false;
			Save();
			return true;
		}
		return false;
	}

	public static void ActivateExtraLife()
	{
		gameData.ExtraLife = true;
		Save();
	}

	public static bool IsExtraLifeActive()
	{
		if (gameData.ExtraLife)
		{
			gameData.ExtraLife = false;
			Save();
			return true;
		}
		return false;
	}

	public static void ActivateDiscount()
	{
		gameData.DiscountExpireDate = DateTime.Now.AddDays(1.0);
		Save();
	}

	public static void DeactivateDiscount()
	{
		gameData.DiscountExpireDate = DateTime.MinValue;
		Save();
	}

	public static bool IsDiscountActive()
	{
		return DateTime.Now < gameData.DiscountExpireDate;
	}

	public static void ClearDiscount()
	{
		gameData.DiscountExpireDate = DateTime.MinValue;
		Save();
	}

	public static bool IsTreasureActive(int id, string scenePrefix, int levelNum)
	{
		return !gameData.ActiveChests[gameData.ChaptersId[scenePrefix], levelNum - 1].Contains((byte)id);
	}

	public static void PickupTreasure(int id, string scenePrefix, int levelNum)
	{
		if (!gameData.ActiveChests[gameData.ChaptersId[scenePrefix], levelNum - 1].Contains((byte)id))
		{
			gameData.ActiveChests[gameData.ChaptersId[scenePrefix], levelNum - 1].Add((byte)id);
		}
		Save();
	}

	public static bool IsGrimmyIdolTaken(int id)
	{
		if (gameData.grimmyIdols != null && id < gameData.grimmyIdols.Length)
		{
			if (gameData.grimmyIdols != null)
			{
				return gameData.grimmyIdols[id];
			}
			return false;
		}
		return false;
	}

	public static void PickupGrimmyIdol(int id)
	{
		if (gameData.grimmyIdols != null)
		{
			gameData.grimmyIdols[id] = true;
		}
		Save();
	}

	public static int GetGrimmyIdolPickedCount()
	{
		if (gameData.grimmyIdols == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < gameData.grimmyIdols.Length; i++)
		{
			if (gameData.grimmyIdols[i])
			{
				num++;
			}
		}
		return num;
	}

	public static void SetLastIncomeDate()
	{
		if ((DateTime.Now.Date - gameData.LastIncomeDate.Date).Days >= 0)
		{
			gameData.LastIncomeDate = DateTime.Now;
			Save();
		}
	}

	public static DateTime GetLastIncomeDateTime()
	{
		return gameData.LastIncomeDate;
	}

	public static string GetLastIncomeDate()
	{
		return gameData.LastIncomeDate.ToString();
	}

	public static int GetDaysFromLastStart()
	{
		return (DateTime.Now.Date - gameData.LastIncomeDate.Date).Days;
	}

	public static void SetDaysInARow(int days)
	{
		gameData.DaysInARow = (byte)days;
		Save();
	}

	public static int GetDaysInARow()
	{
		return gameData.DaysInARow;
	}

	public static void UnlockChallenge(int id)
	{
		gameData.UnlockedChallenges.Add((short)id);
		Save();
	}

	public static bool IsChallengeUnlocked(int id)
	{
		return gameData.UnlockedChallenges.Contains((short)id);
	}

	public static void SetTimeToLevel(string scenePrefix, int levelNum, int time)
	{
		gameData.TimePerLevel[gameData.ChaptersId[scenePrefix], levelNum - 1] = (byte)time;
		Save();
	}

	public static int GetTimeFromLevel(string scenePrefix, int levelNum)
	{
		return gameData.TimePerLevel[gameData.ChaptersId[scenePrefix], levelNum - 1];
	}

	public static void SetMetersToLevel(string scenePrefix, int levelNum, int meters)
	{
		int num = gameData.ChaptersId[scenePrefix] - gameData.AdventureChaptersCount;
		gameData.MetersPerEndlessLevel[num, levelNum - 1] = (short)meters;
		Save();
	}

	public static short GetMetersFromLevel(string scenePrefix, int levelNum)
	{
		int num = gameData.ChaptersId[scenePrefix] - gameData.AdventureChaptersCount;
		return gameData.MetersPerEndlessLevel[num, levelNum - 1];
	}

	public static void AddToTotalMeters(int meters)
	{
		gameData.TotalMetersPlayed += meters;
		Save();
	}

	public static int GetTotalMeters()
	{
		return gameData.TotalMetersPlayed;
	}

	public static void Save()
	{
		Save(false);
	}

	public static void Save(bool force)
	{
		SavePlayerPrefs();
		if (enableSaveOnServer && !IsInitializing && ConfigParams.useICloud)
		{
			string text = gameData.EncodeToBase64();
			if (text != null)
			{
				WebDataStore.WriteData(text, force);
			}
			else
			{
				Debug.Log("gameData.EncodeToBase64() returns null");
			}
		}
	}

	public static void SavePlayerPrefs()
	{
		string key = "GameData";
		string text = gameData.EncodeToBase64();
		if (text != null)
		{
			PlayerPrefs.SetString(key, text);
			PlayerPrefs.Save();
		}
	}

	public static bool Load()
	{
		string key = "GameData";
		return gameData.DecodeFromBase64(PlayerPrefs.GetString(key, string.Empty));
	}

	public static void DeleteAll()
	{
		gameData = new GameDataExtended_v2();
		PlayerPrefs.DeleteAll();
		Save(true);
	}

	public static void Sync(OnWebDataStoreRes onResult)
	{
		onWebDataStoreRes = onResult;
		if (ConfigParams.useICloud && Application.internetReachability != NetworkReachability.NotReachable)
		{
			WebDataStore.ReadData(OnReadData);
		}
		else if (onWebDataStoreRes != null)
		{
			onWebDataStoreRes(false);
		}
	}

	private static void OnReadData(bool res, string str)
	{
		if (res)
		{
			switch (gameData.IsSameVersion(str))
			{
			case GameDataBase.eVersionCheckResult.SAME_VERSION:
				enableSaveOnServer = true;
				switch (gameData.CheckRevision(str))
				{
				case GameDataBase.RevisionResult.Lower:
					if (gameData.DecodeFromBase64(str))
					{
						if (PlayerPrefsWrapper.OnGameDataChangeEvent != null)
						{
							PlayerPrefsWrapper.OnGameDataChangeEvent();
						}
						if (onWebDataStoreRes != null)
						{
							onWebDataStoreRes(true);
						}
						Save();
					}
					return;
				case GameDataBase.RevisionResult.Higher:
				{
					string text2 = gameData.EncodeToBase64();
					if (text2 != null)
					{
						WebDataStore.WriteData(text2, true);
						if (onWebDataStoreRes != null)
						{
							onWebDataStoreRes(false);
						}
						Save();
					}
					return;
				}
				}
				break;
			case GameDataBase.eVersionCheckResult.CLIENT_HAS_GREATER_VERSION:
			{
				enableSaveOnServer = true;
				GameDataExtended gameDataExtended = new GameDataExtended_v2();
				if (!gameDataExtended.DecodeFromBase64(str))
				{
					break;
				}
				if (gameDataExtended.Revision > gameData.Revision)
				{
					if (gameData.DecodeFromBase64(str))
					{
						if (PlayerPrefsWrapper.OnGameDataChangeEvent != null)
						{
							PlayerPrefsWrapper.OnGameDataChangeEvent();
						}
						if (onWebDataStoreRes != null)
						{
							onWebDataStoreRes(true);
						}
						Save();
					}
					return;
				}
				string text = gameData.EncodeToBase64();
				if (text != null)
				{
					WebDataStore.WriteData(text, true);
					if (onWebDataStoreRes != null)
					{
						onWebDataStoreRes(false);
					}
					Save();
				}
				return;
			}
			default:
				enableSaveOnServer = false;
				ConfigParams.showGameDataWrongVersionDialog = true;
				break;
			}
		}
		else
		{
			string message = string.Format("WebStore read error. enableSaveOnServer = false");
			Debug.Log(message);
			enableSaveOnServer = false;
		}
		if (onWebDataStoreRes != null)
		{
			onWebDataStoreRes(false);
		}
	}

	public static void IncrementSessions()
	{
		PlayerPrefs.SetInt("TotalSessions", GetTotalSessions() + 1);
	}

	public static int GetTotalSessions()
	{
		return PlayerPrefs.GetInt("TotalSessions", 0);
	}

	public static void SetFirstStartDate()
	{
		PlayerPrefs.SetString("FirstStart", DateTime.Now.ToString());
	}

	public static DateTime GetTimeFromStartDate()
	{
		if (!PlayerPrefs.HasKey("FirstStart"))
		{
			SetFirstStartDate();
		}
		return DateTime.Parse(PlayerPrefs.GetString("FirstStart", DateTime.Now.ToString()));
	}

	public static DateTime LastAskRate()
	{
		return DateTime.Parse(PlayerPrefs.GetString("LastAskRate", DateTime.Now.ToString()));
	}

	public static void SetLastAskRate()
	{
		PlayerPrefs.SetString("LastAskRate", DateTime.Now.ToString());
		PlayerPrefs.SetInt("Rate", 1);
	}

	public static bool WasRateAsked()
	{
		return PlayerPrefs.GetInt("Rate", 0) == 1;
	}

	public static bool CanRate()
	{
		return PlayerPrefs.GetInt("CanRate", 1) == 1;
	}

	public static void CanRate(bool canRate)
	{
		PlayerPrefs.SetInt("CanRate", canRate ? 1 : 0);
	}

	public static void SetPendingPayment(int pending)
	{
		gameData.PendingMoney = pending;
		Save(true);
	}

	public static int GetPendingPayment()
	{
		return gameData.PendingMoney;
	}

	public static void ClearPendings()
	{
		gameData.PendingMoney = 0;
	}

	public static int GetLastTapjoyMoney()
	{
		return gameData.LastTapjoyMoney;
	}

	public static void SetLastTapjoyMoney(int money)
	{
		gameData.LastTapjoyMoney = money;
	}

	public static void UnlockAchievement(string achievement)
	{
		if (!gameData.Achievements.Contains(achievement))
		{
			gameData.Achievements.Add(achievement);
		}
	}

	public static bool HasAchievement(string achievement)
	{
		return gameData.Achievements.Contains(achievement);
	}

	public static DateTime LastOfferShown()
	{
		return DateTime.Parse(PlayerPrefs.GetString("LastOfferShown", DateTime.MinValue.ToString()));
	}

	public static void SetLastOfferShown()
	{
		PlayerPrefs.SetString("LastOfferShown", DateTime.Now.Date.ToString());
	}

	public static short[] GetPurchasedItems()
	{
		return gameData.PurchasedItems.ToArray();
	}

	public static void RemovePurchasedItem(int id)
	{
		int num = gameData.PurchasedItems.IndexOf((short)id);
		Debug.Log(num);
		if (num >= 0)
		{
			gameData.PurchasedItems.RemoveAt(num);
			gameData.ItemsCount.RemoveAt(num);
			gameData.ItemUpgrades.RemoveAt(num);
		}
	}

	public static void RemoveUpgradeItem(int itemId)
	{
		int num = gameData.PurchasedItems.IndexOf((short)itemId);
		if (num >= 0)
		{
			if (gameData.ItemUpgrades[num] > 0)
			{
				List<byte> itemUpgrades;
				List<byte> list = (itemUpgrades = gameData.ItemUpgrades);
				int index2;
				int index = (index2 = num);
				byte b = itemUpgrades[index2];
				list[index] = (byte)(b - 1);
			}
			if (gameData.ItemUpgrades[num] == 0)
			{
				gameData.ItemsCount.RemoveAt(num);
				gameData.ItemUpgrades.RemoveAt(num);
				gameData.PurchasedItems.RemoveAt(num);
			}
		}
	}

	public static void RemoveConsumableItem(int id)
	{
		int num = gameData.PurchasedItems.IndexOf((short)id);
		if (num >= 0 && gameData.ItemsCount[num] > 0)
		{
			List<byte> itemsCount;
			List<byte> list = (itemsCount = gameData.ItemsCount);
			int index2;
			int index = (index2 = num);
			byte b = itemsCount[index2];
			list[index] = (byte)(b - 1);
			if (gameData.ItemsCount[num] == 0)
			{
				gameData.ItemsCount.RemoveAt(num);
				gameData.ItemUpgrades.RemoveAt(num);
				gameData.PurchasedItems.RemoveAt(num);
			}
		}
	}

	public static void SetTweetDate()
	{
		PlayerPrefs.SetString("LastTweetDate", DateTime.Now.Date.ToString());
	}

	public static DateTime GetTweetDate()
	{
		return DateTime.Parse(PlayerPrefs.GetString("LastTweetDate", DateTime.Now.Date.ToString()));
	}

	public static int GetDaysSinceLastTweet()
	{
		if (!PlayerPrefs.HasKey("LastTweetDate"))
		{
			return -1;
		}
		DateTime dateTime = DateTime.Parse(PlayerPrefs.GetString("LastTweetDate", DateTime.Now.Date.ToString()));
		DateTime date = DateTime.Now.Date;
		return (date - dateTime).Days;
	}

	public static bool HasCheat(long cheatId)
	{
		return PlayerPrefs.HasKey(string.Format("CHEAT-{0}", cheatId));
	}

	public static void SaveCheat(long cheatId)
	{
		PlayerPrefs.SetInt(string.Format("CHEAT-{0}", cheatId), 1);
	}
}
