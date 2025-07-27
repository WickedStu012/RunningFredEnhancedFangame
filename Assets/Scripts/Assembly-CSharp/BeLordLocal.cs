using System.Collections.Generic;
using UnityEngine;

public class BeLordLocal
{
	private const string DEF_PLAYER_NAME = "DEFAULT";

	private const string ACH_PREFIX = "{0}_ACH_{1}";

	private const string ACH_PREFIX_RES = "{0}_ACH_{1}_SENT";

	private const string SCR_PREFIX = "PEND_SCR_{0}_SCR_{1}";

	private static GameAchievements gameAchievements;

	private static GameLeaderboards gameLeaderboards;

	private static string playerName;

	public static string GetLastPlayerLogged()
	{
		return PlayerPrefs.GetString("LASTLOGGEDPLAYERID", "DEFAULT");
	}

	public static void SetLastPlayerLogged(string playerName, GameAchievements gameAchievements, GameLeaderboards gameLeaderboards)
	{
		BeLordLocal.gameAchievements = gameAchievements;
		BeLordLocal.gameLeaderboards = gameLeaderboards;
		if (GetLastPlayerLogged() == "DEFAULT")
		{
			MoveAchievementsToPlayer("DEFAULT", playerName);
			MoveLeaderboardToPlayer("DEFAULT", playerName);
		}
		BeLordLocal.playerName = playerName;
		PlayerPrefs.SetString("LASTLOGGEDPLAYERID", playerName);
	}

	private static void MoveAchievementsToPlayer(string oldPlayerName, string newPlayerName)
	{
		int count = gameAchievements.GetCount();
		for (int i = 0; i < count; i++)
		{
			BeLordAchievementInfo achievementByIdx = gameAchievements.GetAchievementByIdx(i);
			int value = PlayerPrefs.GetInt(string.Format("{0}_ACH_{1}", oldPlayerName, achievementByIdx.identifier), 0);
			PlayerPrefs.SetInt(string.Format("{0}_ACH_{1}", newPlayerName, achievementByIdx.identifier), value);
			PlayerPrefs.DeleteKey(string.Format("{0}_ACH_{1}", oldPlayerName, achievementByIdx.identifier));
		}
	}

	public static void LoadFromLocal(GameAchievements gameAchievements)
	{
		LoadFromLocal(GetLastPlayerLogged(), gameAchievements);
	}

	public static void LoadFromLocal(string playerName, GameAchievements gameAchievements)
	{
		BeLordLocal.playerName = playerName;
		BeLordLocal.gameAchievements = gameAchievements;
		if (gameAchievements == null)
		{
			Debug.Log("Warning: Game Achievements is null!");
			return;
		}
		int count = gameAchievements.GetCount();
		for (int i = 0; i < count; i++)
		{
			BeLordAchievementInfo achievementByIdx = gameAchievements.GetAchievementByIdx(i);
			if (achievementByIdx != null)
			{
				achievementByIdx.completed = PlayerPrefs.GetInt(string.Format("{0}_ACH_{1}", playerName, achievementByIdx.identifier), 0) != 0;
				achievementByIdx.percentComplete = ((!achievementByIdx.completed) ? 0f : 100f);
			}
		}
	}

	public static void SaveToLocal()
	{
		if (gameAchievements == null)
		{
			Debug.Log("[ERROR] gameAchievements is null");
			return;
		}
		int count = gameAchievements.GetCount();
		for (int i = 0; i < count; i++)
		{
			BeLordAchievementInfo achievementByIdx = gameAchievements.GetAchievementByIdx(i);
			PlayerPrefs.SetInt(string.Format("{0}_ACH_{1}", playerName, achievementByIdx.identifier), achievementByIdx.completed ? 1 : 0);
		}
	}

	public static void SaveAchievement(string id, bool completed)
	{
		PlayerPrefs.SetInt(string.Format("{0}_ACH_{1}", playerName, id), completed ? 1 : 0);
	}

	public static bool GetAchievementSentStatus(string id)
	{
		return PlayerPrefs.GetInt(string.Format("{0}_ACH_{1}_SENT", playerName, id), 0) == 1;
	}

	public static void SaveAchievementSentOK(string id)
	{
		PlayerPrefs.SetInt(string.Format("{0}_ACH_{1}_SENT", playerName, id), 1);
	}

	public static void ResetAchievements()
	{
		if (gameAchievements == null)
		{
			Debug.Log("[ERROR] gameAchievements is null");
			return;
		}
		int count = gameAchievements.GetCount();
		for (int i = 0; i < count; i++)
		{
			BeLordAchievementInfo achievementByIdx = gameAchievements.GetAchievementByIdx(i);
			PlayerPrefs.SetInt(string.Format("{0}_ACH_{1}", playerName, achievementByIdx.identifier), 0);
			achievementByIdx.completed = false;
			achievementByIdx.percentComplete = 0f;
			Debug.Log(string.Format("{0}_ACH_{1}", playerName, achievementByIdx.identifier));
		}
	}

	public static void CopyFromCloud(Dictionary<string, BeLordAchievementInfo> cloudAchs)
	{
		if (gameAchievements == null)
		{
			Debug.Log("[ERROR] gameAchievements is null");
			return;
		}
		int count = gameAchievements.GetCount();
		for (int i = 0; i < count; i++)
		{
			BeLordAchievementInfo achievementByIdx = gameAchievements.GetAchievementByIdx(i);
			if (cloudAchs.ContainsKey(achievementByIdx.identifier))
			{
				BeLordAchievementInfo beLordAchievementInfo = cloudAchs[achievementByIdx.identifier];
				achievementByIdx.completed = beLordAchievementInfo.completed;
				achievementByIdx.percentComplete = beLordAchievementInfo.percentComplete;
			}
		}
	}

	public static bool HasAchievement(string id)
	{
		if (gameAchievements == null)
		{
			Debug.Log("[ERROR] gameAchievements is null");
			return false;
		}
		BeLordAchievementInfo achievementById = gameAchievements.GetAchievementById(id);
		if (achievementById != null)
		{
			return achievementById.completed;
		}
		return false;
	}

	public static void UnlockAchievement(string id)
	{
		if (gameAchievements == null)
		{
			Debug.Log("[ERROR] gameAchievements is null");
			return;
		}
		BeLordAchievementInfo achievementById = gameAchievements.GetAchievementById(id);
		if (achievementById != null)
		{
			achievementById.completed = true;
			achievementById.percentComplete = 100f;
		}
		else
		{
			Debug.Log(string.Format("Warning. Cannot find achievement: {0}", id));
		}
		SaveAchievement(id, true);
	}

	public static void ReportScore(string id, int pnts)
	{
		PlayerPrefs.SetInt(string.Format("PEND_SCR_{0}_SCR_{1}", playerName, id), pnts);
	}

	private static void MoveLeaderboardToPlayer(string oldPlayerName, string newPlayerName)
	{
		for (int i = 0; i < gameLeaderboards.leaderboardsIds.Length; i++)
		{
			int num = PlayerPrefs.GetInt(string.Format("PEND_SCR_{0}_SCR_{1}", oldPlayerName, gameLeaderboards.leaderboardsIds[i]), 0);
			if (num > 0)
			{
				PlayerPrefs.DeleteKey(string.Format("PEND_SCR_{0}_SCR_{1}", oldPlayerName, gameLeaderboards.leaderboardsIds[i]));
				PlayerPrefs.SetInt(string.Format("PEND_SCR_{0}_SCR_{1}", newPlayerName, gameLeaderboards.leaderboardsIds[i]), num);
			}
		}
	}

	private static void ShowAllPendingsToSendScore()
	{
		for (int i = 0; i < gameLeaderboards.leaderboardsIds.Length; i++)
		{
		}
	}

	public static int GetPendingToSendScore(string id)
	{
		return PlayerPrefs.GetInt(string.Format("PEND_SCR_{0}_SCR_{1}", playerName, id), 0);
	}

	public static void ClearPendingToSendScore(string id)
	{
		PlayerPrefs.DeleteKey(string.Format("PEND_SCR_{0}_SCR_{1}", playerName, id));
	}
}
