using System.Collections.Generic;
using UnityEngine;

public class BeLordAchievementSync
{
	private const int MAX_ERROR_COUNT = 3;

	private static List<string> achsToSync;

	private static bool isSyncing;

	private static int syncingIdx;

	private static BeLordBase belord;

	private static bool waitingResponse;

	private static int errorCount;

	private static BeLordAchievementSyncFinish cbFinishSync;

	public static void SyncAchievements(GameAchievements gameAchievements, BeLordBase belord)
	{
		SyncAchievements(gameAchievements, belord);
	}

	public static void SyncAchievements(GameAchievements gameAchievements, BeLordBase belord, BeLordAchievementSyncFinish cbFinishSync)
	{
		Debug.Log("Start Sync");
		if (isSyncing)
		{
			Debug.Log("[WARNING] SyncAchievements was called but its still syncing");
			return;
		}
		BeLordAchievementSync.cbFinishSync = cbFinishSync;
		achsToSync = null;
		BeLordAchievementSync.belord = belord;
		Dictionary<string, BeLordAchievementInfo> achievements = belord.GetAchievements();
		int count = gameAchievements.GetCount();
		bool flag = true;
		for (int i = 0; i < count; i++)
		{
			BeLordAchievementInfo achievementByIdx = gameAchievements.GetAchievementByIdx(i);
			if (achievements.ContainsKey(achievementByIdx.identifier))
			{
				BeLordAchievementInfo beLordAchievementInfo = achievements[achievementByIdx.identifier];
				if (beLordAchievementInfo.completed && !achievementByIdx.completed)
				{
					achievementByIdx.completed = beLordAchievementInfo.completed;
					achievementByIdx.percentComplete = beLordAchievementInfo.percentComplete;
				}
				else if (achievementByIdx.completed && !beLordAchievementInfo.completed)
				{
					beLordAchievementInfo.completed = achievementByIdx.completed;
					beLordAchievementInfo.percentComplete = achievementByIdx.percentComplete;
					if (achsToSync == null)
					{
						achsToSync = new List<string>();
					}
					achsToSync.Add(achievementByIdx.identifier);
					flag = false;
					Debug.Log(string.Format("Should sync achievement id: {0}", achievementByIdx.identifier));
				}
			}
			else if (achievementByIdx.completed)
			{
				if (achsToSync == null)
				{
					achsToSync = new List<string>();
				}
				achsToSync.Add(achievementByIdx.identifier);
				flag = false;
				Debug.Log(string.Format("Should sync achievement id: {0}", achievementByIdx.identifier));
			}
		}
		if (flag)
		{
			Debug.Log("The cloud and the local achievements were totally sync.");
			if (BeLordAchievementSync.cbFinishSync != null)
			{
				BeLordAchievementSync.cbFinishSync();
			}
		}
		else
		{
			Debug.Log("Some achievements should be syncronized (achievements are on local but not on cloud)");
			isSyncing = true;
			waitingResponse = false;
			syncingIdx = 0;
			errorCount = 0;
		}
	}

	public static void UpdateSync()
	{
		if (isSyncing && !waitingResponse)
		{
			Debug.Log(string.Format("sending unlock ach to gamecenter. Id: {0}", achsToSync[syncingIdx]));
			belord.UnlockAchievement(achsToSync[syncingIdx], onReportAchievementFinish);
			waitingResponse = true;
		}
	}

	private static void onReportAchievementFinish(BeLordCommand cmd, bool finishOk, string error)
	{
		waitingResponse = false;
		if (!finishOk)
		{
			Debug.Log(string.Format("[ERROR @ BeLordAchievementSync] Cannot report achievement to backend. Error: {0}", error));
			errorCount++;
			if (errorCount == 3)
			{
				Debug.Log("[ERROR @ BeLordAchievementSync] Cannot synchronize with server.");
				isSyncing = false;
				if (cbFinishSync != null)
				{
					cbFinishSync();
				}
			}
			return;
		}
		errorCount = 0;
		syncingIdx++;
		if (syncingIdx >= achsToSync.Count)
		{
			Debug.Log("The cloud and the local achievements were synchronized.");
			isSyncing = false;
			if (cbFinishSync != null)
			{
				cbFinishSync();
			}
		}
	}
}
