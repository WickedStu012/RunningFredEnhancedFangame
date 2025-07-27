using System.Collections.Generic;

public interface BeLordBase
{
	bool IsSocialNetworkServiceAvailable();

	void AuthenticatePlayer(string[] info, BeLordCommandResult onCmdResult, BeLordCommandResult onCmdLogOut);

	bool IsPlayerAuthenticated();

	string GetPlayerName();

	void LoadAchievements(BeLordCommandResult onCmdResult);

	Dictionary<string, BeLordAchievementInfo> GetAchievements();

	void UnlockAchievement(string id, BeLordCommandResult onCmdResult);

	bool HasAchievement(string id);

	void ReportScore(int score, string categoryId, BeLordCommandResult onCmdResult);

	void GetScore(BeLordTimeScope timeScope, string categoryId, BeLordCommandResult onCmdResult);

	void GetScore(bool onlyFriends, BeLordTimeScope timeScope, string categoryId, BeLordCommandResult onCmdResult);

	void GetScore(bool onlyFriends, BeLordTimeScope timeScope, string categoryId, int fromRange, int count, BeLordCommandResult onCmdResult);

	List<BeLordLeaderboardItem> GetScoreData();

	bool IsWaitingResponse();

	void ResetAchievements();

	void OpenDashboard(BeLordSimpleDelegate onClosed);

	void OpenLeaderboards();

	void OpenAchievements();
}
