using System.IO;
using UnityEngine;

public class GameDataExtended_v2 : GameDataExtended
{
	public new const byte VERSION = 2;

	private const int TOTAL_LEVEL_COUNT = 40;

	public bool[] grimmyIdols;

	public GameDataExtended_v2()
	{
		grimmyIdols = new bool[30];
		Version = 2;
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
		GameDataExtended_v2 gameDataExtended_v = new GameDataExtended_v2();
		if (gameDataExtended_v.DecodeFromBase64(base64))
		{
			if (gameDataExtended_v.PlayerMoney < PlayerMoney)
			{
				return RevisionResult.Lower;
			}
			if (gameDataExtended_v.PlayerMoney > PlayerMoney)
			{
				return RevisionResult.Higher;
			}
			return RevisionResult.Equal;
		}
		return RevisionResult.Error;
	}

	protected override bool OnConvert(string base64, int version)
	{
		Debug.Log("GameData conversion from GameDataExtended to GameDataExtended_v2");
		if (version == 1)
		{
			GameDataExtended gameDataExtended = new GameDataExtended();
			if (!gameDataExtended.DecodeFromBase64(base64))
			{
				return false;
			}
			Revision = gameDataExtended.Revision;
			CurrentAvatarId = gameDataExtended.CurrentAvatarId;
			CurrentChapterId = gameDataExtended.CurrentChapterId;
			CurrentLevel = gameDataExtended.CurrentLevel;
			AdventureChapterId = gameDataExtended.AdventureChapterId;
			SurvivalChapterId = gameDataExtended.SurvivalChapterId;
			AdventureLevel = gameDataExtended.AdventureLevel;
			SurvivalLevel = gameDataExtended.SurvivalLevel;
			UnlockLevelChapter = gameDataExtended.UnlockLevelChapter;
			MedalsEarned = gameDataExtended.MedalsEarned;
			PlayerMoney = gameDataExtended.PlayerMoney;
			PurchasedItems = gameDataExtended.PurchasedItems;
			ItemUpgrades = gameDataExtended.ItemUpgrades;
			ItemsCount = gameDataExtended.ItemsCount;
			PiggyBankExpireDate = gameDataExtended.PiggyBankExpireDate;
			DiscountExpireDate = gameDataExtended.DiscountExpireDate;
			MoneyInPiggyBank = gameDataExtended.MoneyInPiggyBank;
			ActiveGiftId = gameDataExtended.ActiveGiftId;
			ActiveChests = gameDataExtended.ActiveChests;
			LastIncomeDate = gameDataExtended.LastIncomeDate;
			DaysInARow = gameDataExtended.DaysInARow;
			UnlockedChallenges = gameDataExtended.UnlockedChallenges;
			DoubleSkully = gameDataExtended.DoubleSkully;
			EarlyStart = gameDataExtended.EarlyStart;
			DrowsyReaper = gameDataExtended.DrowsyReaper;
			ExtraLife = gameDataExtended.ExtraLife;
			TimePerLevel = gameDataExtended.TimePerLevel;
			MetersPerEndlessLevel = gameDataExtended.MetersPerEndlessLevel;
			TotalMetersPlayed = gameDataExtended.TotalMetersPlayed;
			PendingMoney = gameDataExtended.PendingMoney;
			LastTapjoyMoney = gameDataExtended.LastTapjoyMoney;
			Achievements = gameDataExtended.Achievements;
			AchievementsId = gameDataExtended.AchievementsId;
			AchievementsStrId = gameDataExtended.AchievementsStrId;
		}
		grimmyIdols = new bool[40];
		for (int i = 0; i < grimmyIdols.Length; i++)
		{
			grimmyIdols[i] = false;
		}
		return true;
	}
}
