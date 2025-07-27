using System.Collections.Generic;
using UnityEngine;

public class BeLordGameCircle : BeLordBase
{
	private static BeLordGameCircle instance;

	private bool isWaitingResponse;

	private bool isInited;

	private bool isAuthenticated;

	private Dictionary<string, BeLordAchievementInfo> blAchievements;

	private event BeLordCommandResult onCmdResult;

	private event BeLordCommandResult onCmdAuthResult;

	private event BeLordCommandResult onCmdAuthLogOut;

	public static BeLordGameCircle GetInstance()
	{
		if (instance == null)
		{
			instance = new BeLordGameCircle();
			instance.isWaitingResponse = false;
			instance.Init();
		}
		return instance;
	}

	public void Init()
	{
		if (!isInited)
		{
			instance.registerEvents();
			isInited = true;
		}
	}

	private void registerEvents()
	{
	}

	private void unregisterEvents()
	{
	}

	public bool IsSocialNetworkServiceAvailable()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	public void AuthenticatePlayer(string[] info, BeLordCommandResult onCmdResult, BeLordCommandResult onCmdLogOut)
	{
	}

	public bool IsPlayerAuthenticated()
	{
		return isAuthenticated;
	}

	public string GetPlayerName()
	{
		return string.Empty;
	}

	public void LoadAchievements(BeLordCommandResult onCmdResult)
	{
	}

	public Dictionary<string, BeLordAchievementInfo> GetAchievements()
	{
		return blAchievements;
	}

	public void UnlockAchievement(string id, BeLordCommandResult onCmdResult)
	{
	}

	public bool HasAchievement(string id)
	{
		return blAchievements.ContainsKey(id);
	}

	public void ReportScore(int score, string categoryId, BeLordCommandResult onCmdResult)
	{
	}

	public void GetScore(BeLordTimeScope timeScope, string categoryId, BeLordCommandResult onCmdResult)
	{
		Debug.Log("GameCircle.GetScore. NOT SUPPORTED");
	}

	public void GetScore(bool onlyFriends, BeLordTimeScope timeScope, string categoryId, BeLordCommandResult onCmdResult)
	{
		Debug.Log("GameCircle.GetScore. NOT SUPPORTED");
	}

	public void GetScore(bool onlyFriends, BeLordTimeScope timeScope, string categoryId, int fromRange, int count, BeLordCommandResult onCmdResult)
	{
		Debug.Log("GameCircle.GetScore. NOT SUPPORTED");
	}

	public List<BeLordLeaderboardItem> GetScoreData()
	{
		Debug.Log("GameCircle.GetScoreData. NOT SUPPORTED");
		return null;
	}

	public bool IsWaitingResponse()
	{
		return isWaitingResponse;
	}

	public void ResetAchievements()
	{
		Debug.Log("GameCircle.ResetAchievements");
	}

	public void OpenDashboard(BeLordSimpleDelegate onClosed)
	{
		if (!isAuthenticated)
		{
		}
	}

	public void OpenLeaderboards()
	{
		if (!isAuthenticated)
		{
		}
	}

	public void OpenAchievements()
	{
		if (!isAuthenticated)
		{
		}
	}
}
