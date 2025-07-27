using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BeLordGC : BeLordBase
{
	private static BeLordGC instance;

	private Dictionary<string, BeLordAchievementInfo> blAchievements;

	private List<BeLordLeaderboardItem> blLeaderboardData;

	private bool isWaitingResponse;

	private event BeLordCommandResult onCmdResult;

	public static BeLordGC GetInstance()
	{
		if (instance == null)
		{
			instance = new BeLordGC();
			instance.isWaitingResponse = false;
		}
		return instance;
	}

	public bool IsSocialNetworkServiceAvailable()
	{
		return true;
	}

	public void AuthenticatePlayer(string[] info, BeLordCommandResult onCmdResult, BeLordCommandResult onCmdLogOut)
	{
		isWaitingResponse = true;
		Debug.Log("GameCenter.AuthenticatePlayer (Init)");
		Social.localUser.Authenticate(delegate(bool success)
		{
			if (success)
			{
				Debug.Log("Authentication successful");
				string message = "Username: " + Social.localUser.userName + "\nUser ID: " + Social.localUser.id + "\nIsUnderage: " + Social.localUser.underage;
				Debug.Log(message);
			}
			else
			{
				Debug.Log("Authentication failed");
			}
			if (onCmdResult != null)
			{
				onCmdResult(BeLordCommand.LOGIN, success, null);
			}
		});
	}

	public void Logout()
	{
		Debug.Log("Social API does not support automated logouts");
	}

	public bool IsPlayerAuthenticated()
	{
		return Social.localUser.authenticated;
	}

	public string GetPlayerName()
	{
		return Social.localUser.userName;
	}

	public bool IsWaitingResponse()
	{
		return isWaitingResponse;
	}

	public void ResetAchievements()
	{
		Debug.Log("Social API cannot reset achievements");
	}

	public void OpenDashboard(BeLordSimpleDelegate onClosed)
	{
	}

	public void OpenAchievements()
	{
		Social.ShowAchievementsUI();
	}

	public void LoadAchievements(BeLordCommandResult onCmdResult)
	{
		this.onCmdResult = onCmdResult;
		isWaitingResponse = true;
		Social.LoadAchievements(onAchievementsLoaded);
	}

	private void onAchievementsLoaded(IAchievement[] achievements)
	{
		isWaitingResponse = false;
		blAchievements = new Dictionary<string, BeLordAchievementInfo>();
		foreach (IAchievement achievement in achievements)
		{
			blAchievements.Add(achievement.id, new BeLordAchievementInfo(achievement.id, achievement.hidden, achievement.completed, achievement.lastReportedDate, (float)achievement.percentCompleted));
		}
		if (this.onCmdResult != null)
		{
			this.onCmdResult(BeLordCommand.LOAD_ACHIEVEMENTS, true, null);
		}
	}

	public Dictionary<string, BeLordAchievementInfo> GetAchievements()
	{
		return blAchievements;
	}

	public void UnlockAchievement(string id, BeLordCommandResult onCmdResult)
	{
		this.onCmdResult = onCmdResult;
		isWaitingResponse = true;
		if (blAchievements == null)
		{
			blAchievements = new Dictionary<string, BeLordAchievementInfo>();
		}
		if (blAchievements.ContainsKey(id))
		{
			blAchievements[id].completed = true;
			blAchievements[id].percentComplete = 100f;
		}
		else
		{
			BeLordAchievementInfo value = new BeLordAchievementInfo(id, false, true, DateTime.Now, 100f);
			blAchievements.Add(id, value);
		}
		Social.ReportProgress(id, 100.0, onAchievementReported);
	}

	private void onAchievementReported(bool res)
	{
		isWaitingResponse = false;
		if (this.onCmdResult != null)
		{
			this.onCmdResult(BeLordCommand.REPORT_ACHIEVEMENT, res, null);
		}
	}

	public bool HasAchievement(string id)
	{
		return blAchievements.ContainsKey(id);
	}

	public void OpenLeaderboards()
	{
		Social.ShowLeaderboardUI();
	}

	public void ReportScore(int score, string categoryId, BeLordCommandResult onCmdResult)
	{
		this.onCmdResult = onCmdResult;
		isWaitingResponse = true;
		Social.ReportScore(score, categoryId, onScoreReported);
	}

	private void onScoreReported(bool res)
	{
		isWaitingResponse = false;
		if (this.onCmdResult != null)
		{
			this.onCmdResult(BeLordCommand.REPORT_SCORE, res, null);
		}
	}

	public void GetScore(BeLordTimeScope timeScope, string categoryId, BeLordCommandResult onCmdResult)
	{
		Debug.Log("BeLordGC warning. Current implementation does not support timeScopes");
		GetScore(false, timeScope, categoryId, onCmdResult);
	}

	public void GetScore(bool onlyFriends, BeLordTimeScope timeScope, string categoryId, BeLordCommandResult onCmdResult)
	{
		Debug.Log("BeLordGC warning. Current implementation does not support timeScopes and only friends functionallity");
		GetScore(onlyFriends, timeScope, categoryId, 1, 10, onCmdResult);
	}

	public void GetScore(bool onlyFriends, BeLordTimeScope timeScope, string categoryId, int fromRange, int count, BeLordCommandResult onCmdResult)
	{
		Debug.Log("BeLordGC warning. Current implementation does not support timeScopes, only friends functionallity and ranges");
		this.onCmdResult = onCmdResult;
		Social.LoadScores(categoryId, onGetScores);
		isWaitingResponse = true;
	}

	private void onGetScores(IScore[] scores)
	{
		isWaitingResponse = false;
		blLeaderboardData = new List<BeLordLeaderboardItem>();
		if (scores.Length > 0)
		{
			foreach (IScore score in scores)
			{
				blLeaderboardData.Add(new BeLordLeaderboardItem(score.rank, score.userID, score.userID, false, score.value, score.leaderboardID, score.date));
			}
		}
		if (this.onCmdResult != null)
		{
			this.onCmdResult(BeLordCommand.LOAD_SCORES, true, null);
		}
	}

	public List<BeLordLeaderboardItem> GetScoreData()
	{
		return blLeaderboardData;
	}
}
