using System;
using System.Collections.Generic;
using System.IO;

public class GameData : GameDataBase
{
	public const byte VERSION = 1;

	private const string ENCRYPT_KEY = "slkdfjLKJLLA(U8as98yha9fh*Y(*Y";

	public byte CurrentAvatarId;

	public short CurrentChapterId = -1;

	public byte CurrentLevel = 1;

	public short AdventureChapterId = 1000;

	public short SurvivalChapterId = 1001;

	public byte AdventureLevel = 1;

	public byte SurvivalLevel = 1;

	public byte[] UnlockLevelChapter;

	public byte[,] MedalsEarned;

	public int PlayerMoney;

	public List<short> PurchasedItems = new List<short>();

	public List<byte> ItemUpgrades = new List<byte>();

	public List<byte> ItemsCount = new List<byte>();

	public DateTime PiggyBankExpireDate = DateTime.Now;

	public DateTime DiscountExpireDate = DateTime.Now;

	public int MoneyInPiggyBank;

	public sbyte ActiveGiftId = -1;

	public List<byte>[,] ActiveChests;

	public DateTime LastIncomeDate = DateTime.Now;

	public byte DaysInARow;

	public List<short> UnlockedChallenges = new List<short>();

	public bool DoubleSkully;

	public bool EarlyStart;

	public bool DrowsyReaper;

	public bool ExtraLife;

	public byte[,] TimePerLevel;

	public short[,] MetersPerEndlessLevel;

	public int TotalMetersPlayed;

	public int PendingMoney;

	public int LastTapjoyMoney;

	public Dictionary<string, int> ChaptersId = new Dictionary<string, int>();

	public int ChaptersCount = 10;

	public int AdventureChaptersCount = 5;

	public int SurvivalChaptersCount = 5;

	public GameData()
	{
		Version = 1;
		ChaptersId["Castle"] = 0;
		ChaptersId["Caves"] = 1;
		ChaptersId["Rooftop"] = 2;
		ChaptersId["Sector51"] = 3;
		ChaptersId["Reserved-1"] = 4;
		ChaptersId["Castle-Random"] = 5;
		ChaptersId["Caves-Random"] = 6;
		ChaptersId["Rooftop-Random"] = 7;
		ChaptersId["Sector51-Random"] = 8;
		ChaptersId["Reserved-2"] = 9;
		TimePerLevel = new byte[AdventureChaptersCount, 10];
		MetersPerEndlessLevel = new short[SurvivalChaptersCount, 4];
		UnlockLevelChapter = new byte[ChaptersCount];
		MedalsEarned = new byte[ChaptersCount, 10];
		ActiveChests = new List<byte>[ChaptersCount, 10];
		for (int i = 0; i < ChaptersCount; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				ActiveChests[i, j] = new List<byte>();
			}
		}
		for (int k = 0; k < AdventureChaptersCount; k++)
		{
			for (int l = 0; l < 10; l++)
			{
				TimePerLevel[k, l] = 0;
			}
		}
		for (int m = 0; m < SurvivalChaptersCount; m++)
		{
			for (int n = 0; n < 4; n++)
			{
				MetersPerEndlessLevel[m, n] = 0;
			}
		}
	}

	protected override void OnWrite(BinaryWriter bw)
	{
		bw.Write(ChaptersCount);
		for (int i = 0; i < ChaptersCount; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				int count = ActiveChests[i, j].Count;
				bw.Write(count);
				for (int k = 0; k < count; k++)
				{
					bw.Write(ActiveChests[i, j][k]);
				}
			}
		}
		bw.Write(ActiveGiftId);
		bw.Write(AdventureChapterId);
		bw.Write(AdventureLevel);
		bw.Write(CurrentAvatarId);
		bw.Write(CurrentChapterId);
		bw.Write(CurrentLevel);
		bw.Write(DaysInARow);
		bw.Write(DiscountExpireDate.ToOADate());
		bw.Write(PurchasedItems.Count);
		for (int l = 0; l < PurchasedItems.Count; l++)
		{
			bw.Write(PurchasedItems[l]);
			bw.Write(ItemUpgrades[l]);
			bw.Write(ItemsCount[l]);
		}
		bw.Write(LastIncomeDate.ToOADate());
		for (int m = 0; m < ChaptersCount; m++)
		{
			for (int n = 0; n < 10; n++)
			{
				bw.Write(MedalsEarned[m, n]);
			}
		}
		bw.Write(MoneyInPiggyBank);
		bw.Write(PiggyBankExpireDate.ToOADate());
		bw.Write(PlayerMoney);
		bw.Write(SurvivalChapterId);
		bw.Write(SurvivalLevel);
		bw.Write(UnlockedChallenges.Count);
		for (int num = 0; num < UnlockedChallenges.Count; num++)
		{
			bw.Write(UnlockedChallenges[num]);
		}
		for (int num2 = 0; num2 < ChaptersCount; num2++)
		{
			bw.Write(UnlockLevelChapter[num2]);
		}
		bw.Write(DoubleSkully);
		bw.Write(EarlyStart);
		bw.Write(DrowsyReaper);
		bw.Write(ExtraLife);
		for (int num3 = 0; num3 < AdventureChaptersCount; num3++)
		{
			for (int num4 = 0; num4 < 10; num4++)
			{
				bw.Write(TimePerLevel[num3, num4]);
			}
		}
		for (int num5 = 0; num5 < SurvivalChaptersCount; num5++)
		{
			for (int num6 = 0; num6 < 4; num6++)
			{
				bw.Write(MetersPerEndlessLevel[num5, num6]);
			}
		}
		bw.Write(TotalMetersPlayed);
		bw.Write(PendingMoney);
		bw.Write(LastTapjoyMoney);
	}

	protected override void OnRead(BinaryReader br)
	{
		ChaptersCount = br.ReadInt32();
		ActiveChests = new List<byte>[ChaptersCount, 10];
		for (int i = 0; i < ChaptersCount; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				ActiveChests[i, j] = new List<byte>();
				int num = br.ReadInt32();
				for (int k = 0; k < num; k++)
				{
					byte item = br.ReadByte();
					ActiveChests[i, j].Add(item);
				}
			}
		}
		ActiveGiftId = br.ReadSByte();
		AdventureChapterId = br.ReadInt16();
		AdventureLevel = br.ReadByte();
		CurrentAvatarId = br.ReadByte();
		CurrentChapterId = br.ReadInt16();
		CurrentLevel = br.ReadByte();
		DaysInARow = br.ReadByte();
		DiscountExpireDate = DateTime.FromOADate(br.ReadDouble());
		int num2 = br.ReadInt32();
		PurchasedItems.Clear();
		ItemUpgrades.Clear();
		ItemsCount.Clear();
		for (int l = 0; l < num2; l++)
		{
			PurchasedItems.Add(br.ReadInt16());
			ItemUpgrades.Add(br.ReadByte());
			ItemsCount.Add(br.ReadByte());
		}
		LastIncomeDate = DateTime.FromOADate(br.ReadDouble());
		MedalsEarned = new byte[ChaptersCount, 10];
		for (int m = 0; m < ChaptersCount; m++)
		{
			for (int n = 0; n < 10; n++)
			{
				MedalsEarned[m, n] = br.ReadByte();
			}
		}
		MoneyInPiggyBank = br.ReadInt32();
		PiggyBankExpireDate = DateTime.FromOADate(br.ReadDouble());
		PlayerMoney = br.ReadInt32();
		SurvivalChapterId = br.ReadInt16();
		SurvivalLevel = br.ReadByte();
		int num3 = br.ReadInt32();
		UnlockedChallenges.Clear();
		for (int num4 = 0; num4 < num3; num4++)
		{
			short item2 = br.ReadInt16();
			UnlockedChallenges.Add(item2);
		}
		UnlockLevelChapter = new byte[ChaptersCount];
		for (int num5 = 0; num5 < ChaptersCount; num5++)
		{
			UnlockLevelChapter[num5] = br.ReadByte();
		}
		DoubleSkully = br.ReadBoolean();
		EarlyStart = br.ReadBoolean();
		DrowsyReaper = br.ReadBoolean();
		ExtraLife = br.ReadBoolean();
		for (int num6 = 0; num6 < AdventureChaptersCount; num6++)
		{
			for (int num7 = 0; num7 < 10; num7++)
			{
				TimePerLevel[num6, num7] = br.ReadByte();
			}
		}
		for (int num8 = 0; num8 < SurvivalChaptersCount; num8++)
		{
			for (int num9 = 0; num9 < 4; num9++)
			{
				MetersPerEndlessLevel[num8, num9] = br.ReadInt16();
			}
		}
		TotalMetersPlayed = br.ReadInt32();
		PendingMoney = br.ReadInt32();
		LastTapjoyMoney = br.ReadInt32();
	}

	protected override byte[] Encrypt(byte[] data)
	{
		return XOREncryption.Encrypt(data, "slkdfjLKJLLA(U8as98yha9fh*Y(*Y");
	}

	protected override byte[] Decrypt(byte[] data)
	{
		return XOREncryption.Decrypt(data, "slkdfjLKJLLA(U8as98yha9fh*Y(*Y");
	}

	protected override RevisionResult OnCheckRevision(string base64)
	{
		GameData gameData = new GameData();
		if (gameData.DecodeFromBase64(base64))
		{
			if (gameData.PlayerMoney < PlayerMoney)
			{
				return RevisionResult.Lower;
			}
			if (gameData.PlayerMoney > PlayerMoney)
			{
				return RevisionResult.Higher;
			}
			return RevisionResult.Equal;
		}
		return RevisionResult.Error;
	}
}
