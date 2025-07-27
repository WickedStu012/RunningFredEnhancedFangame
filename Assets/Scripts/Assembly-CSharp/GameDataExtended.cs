using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataExtended : GameData
{
	public List<string> Achievements = new List<string>();

	public Dictionary<string, byte> AchievementsId = new Dictionary<string, byte>();

	public string[] AchievementsStrId = new string[20];

	public GameDataExtended()
	{
		AchievementsId["distancerunner"] = 0;
		AchievementsId["distancerunner2"] = 1;
		AchievementsId["distancerunner3"] = 2;
		AchievementsId["closecall1"] = 3;
		AchievementsId["closecall2"] = 4;
		AchievementsId["closecall3"] = 5;
		AchievementsId["adeptmanorsurvivalist"] = 6;
		AchievementsId["expertmanorsurvivalist"] = 7;
		AchievementsId["mastermanorsurvivalist"] = 8;
		AchievementsId["beginnermanorrunner"] = 9;
		AchievementsId["adeptmanorrunner"] = 10;
		AchievementsId["expertmanorrunner"] = 11;
		AchievementsId["mastermanorrunner"] = 12;
		AchievementsId["adeptcavesurvivalist"] = 13;
		AchievementsId["expertcavesurvivalist"] = 14;
		AchievementsId["mastercavesurvivalist"] = 15;
		AchievementsId["beginnercavesurvivalist"] = 16;
		AchievementsId["adeptcaverunner"] = 17;
		AchievementsId["expertcaverunner"] = 18;
		AchievementsId["mastercaverunner"] = 19;
		AchievementsStrId[0] = "distancerunner";
		AchievementsStrId[1] = "distancerunner2";
		AchievementsStrId[2] = "distancerunner3";
		AchievementsStrId[3] = "closecall1";
		AchievementsStrId[4] = "closecall1";
		AchievementsStrId[5] = "closecall1";
		AchievementsStrId[6] = "adeptmanorsurvivalist";
		AchievementsStrId[7] = "expertmanorsurvivalist";
		AchievementsStrId[8] = "mastermanorsurvivalist";
		AchievementsStrId[9] = "beginnermanorrunner";
		AchievementsStrId[10] = "adeptmanorrunner";
		AchievementsStrId[11] = "expertmanorrunner";
		AchievementsStrId[12] = "mastermanorrunner";
		AchievementsStrId[13] = "adeptcavesurvivalist";
		AchievementsStrId[14] = "expertcavesurvivalist";
		AchievementsStrId[15] = "mastercavesurvivalist";
		AchievementsStrId[16] = "beginnercavesurvivalist";
		AchievementsStrId[17] = "adeptcaverunner";
		AchievementsStrId[18] = "expertcaverunner";
		AchievementsStrId[19] = "mastercaverunner";
	}

	protected override void OnWrite(BinaryWriter bw)
	{
		base.OnWrite(bw);
		bw.Write((byte)Achievements.Count);
		for (int i = 0; i < Achievements.Count; i++)
		{
			string text = Achievements[i];
			try
			{
				byte value = AchievementsId[text];
				bw.Write(value);
			}
			catch
			{
				Debug.Log(string.Format("Cannot find key: {0}", text));
			}
		}
	}

	protected override void OnRead(BinaryReader br)
	{
		base.OnRead(br);
		int num = br.ReadByte();
		Achievements.Clear();
		for (int i = 0; i < num; i++)
		{
			int num2 = br.ReadByte();
			string item = AchievementsStrId[num2];
			Achievements.Add(item);
		}
	}

	protected override RevisionResult OnCheckRevision(string base64)
	{
		GameDataExtended gameDataExtended = new GameDataExtended();
		if (gameDataExtended.DecodeFromBase64(base64))
		{
			if (gameDataExtended.PlayerMoney < PlayerMoney)
			{
				return RevisionResult.Lower;
			}
			if (gameDataExtended.PlayerMoney > PlayerMoney)
			{
				return RevisionResult.Higher;
			}
			return RevisionResult.Equal;
		}
		return RevisionResult.Error;
	}
}
