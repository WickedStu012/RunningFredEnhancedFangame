using System.IO;
using UnityEngine;

public class GameData_v2 : GameData
{
	public new const byte VERSION = 2;

	private const int TOTAL_LEVEL_COUNT = 40;

	public bool[] grimmyIdols;

	public GameData_v2()
	{
		Version = 2;
		grimmyIdols = new bool[40];
		for (int i = 0; i < grimmyIdols.Length; i++)
		{
			grimmyIdols[i] = false;
		}
	}

	protected override void OnWrite(BinaryWriter bw)
	{
		base.OnWrite(bw);
		if (grimmyIdols != null)
		{
			bw.Write(grimmyIdols.Length);
			for (int i = 0; i < grimmyIdols.Length; i++)
			{
				bw.Write(grimmyIdols[i]);
			}
		}
		else
		{
			bw.Write(0);
		}
	}

	protected override void OnRead(BinaryReader br)
	{
		base.OnRead(br);
		int num = br.ReadInt32();
		if (num > 0)
		{
			grimmyIdols = new bool[num];
			for (int i = 0; i < grimmyIdols.Length; i++)
			{
				grimmyIdols[i] = br.ReadBoolean();
			}
		}
		else
		{
			grimmyIdols = new bool[40];
			for (int j = 0; j < 40; j++)
			{
				grimmyIdols[j] = false;
			}
		}
	}

	protected override RevisionResult OnCheckRevision(string base64)
	{
		GameData_v2 gameData_v = new GameData_v2();
		if (gameData_v.DecodeFromBase64(base64))
		{
			if (gameData_v.PlayerMoney < PlayerMoney)
			{
				return RevisionResult.Lower;
			}
			if (gameData_v.PlayerMoney > PlayerMoney)
			{
				return RevisionResult.Higher;
			}
			return RevisionResult.Equal;
		}
		return RevisionResult.Error;
	}

	protected override bool OnConvert(string base64, int version)
	{
		Debug.Log("GameData conversion from GameData to GameData_v2");
		if (version == 1)
		{
			GameData gameData = new GameData();
			if (!gameData.DecodeFromBase64(base64))
			{
				return false;
			}
			Revision = gameData.Revision;
			CurrentAvatarId = gameData.CurrentAvatarId;
			CurrentChapterId = gameData.CurrentChapterId;
			CurrentLevel = gameData.CurrentLevel;
			AdventureChapterId = gameData.AdventureChapterId;
			SurvivalChapterId = gameData.SurvivalChapterId;
			AdventureLevel = gameData.AdventureLevel;
			SurvivalLevel = gameData.SurvivalLevel;
			UnlockLevelChapter = gameData.UnlockLevelChapter;
			MedalsEarned = gameData.MedalsEarned;
			PlayerMoney = gameData.PlayerMoney;
			PurchasedItems = gameData.PurchasedItems;
			ItemUpgrades = gameData.ItemUpgrades;
			ItemsCount = gameData.ItemsCount;
			PiggyBankExpireDate = gameData.PiggyBankExpireDate;
			DiscountExpireDate = gameData.DiscountExpireDate;
			MoneyInPiggyBank = gameData.MoneyInPiggyBank;
			ActiveGiftId = gameData.ActiveGiftId;
			ActiveChests = gameData.ActiveChests;
			LastIncomeDate = gameData.LastIncomeDate;
			DaysInARow = gameData.DaysInARow;
			UnlockedChallenges = gameData.UnlockedChallenges;
			DoubleSkully = gameData.DoubleSkully;
			EarlyStart = gameData.EarlyStart;
			DrowsyReaper = gameData.DrowsyReaper;
			ExtraLife = gameData.ExtraLife;
			TimePerLevel = gameData.TimePerLevel;
			MetersPerEndlessLevel = gameData.MetersPerEndlessLevel;
			TotalMetersPlayed = gameData.TotalMetersPlayed;
			PendingMoney = gameData.PendingMoney;
			LastTapjoyMoney = gameData.LastTapjoyMoney;
		}
		grimmyIdols = new bool[40];
		for (int i = 0; i < grimmyIdols.Length; i++)
		{
			grimmyIdols[i] = false;
		}
		return true;
	}
}
