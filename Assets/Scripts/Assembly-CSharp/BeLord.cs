using UnityEngine;

public class BeLord
{
	public enum InitState
	{
		NONE = 0,
		AUTHING = 1,
		AUTH_END = 2,
		ACH_LOADING = 3,
		SYNC = 4,
		START_REPORTING_PENDING_SCORES = 5,
		REPORTING_PENDING_SCORES = 6,
		WAITING_REPORT_SCORE = 7,
		INITIAL_HANDSHAKE_DONE = 8
	}

	public enum Mode
	{
		OFFLINE = 0,
		ONLINE = 1
	}

	private static Mode mode;

	private static BeLordBase belord;

	private static BeLordCommandResult result;

	private static GameAchievements gameAchievements;

	private static GameLeaderboards gameLeaderboards;

	private static InitState state;

	private static dlgGetScoreResult onGetScoreResult;

	private static string reportingAchievement;

	private static string reportingLeaderboard;

	private static int reportingLeaderboardScore;

	private static string reportingLeaderboard2;

	private static int reportingLeaderboardScore2;

	public static bool Enable = true;

	private static float accumTime;

	private static int pendingScoreIdx = -1;

	public static bool IsSocialNetworkServiceAvailable()
	{
		if (belord == null)
		{
			return false;
		}
		return belord.IsSocialNetworkServiceAvailable();
	}

	public static void Login(BeLordBackend be, GameInfo gameInfo, GameAchievements gameAchievements, GameLeaderboards gameLB)
	{
		if (!Enable)
		{
			return;
		}
		belord = BeLordFactory.GetInstance(be);
		BeLord.gameAchievements = gameAchievements;
		gameLeaderboards = gameLB;
		BeLordLocal.LoadFromLocal(BeLord.gameAchievements);
		state = InitState.NONE;
		if (belord.IsSocialNetworkServiceAvailable())
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				Debug.Log("BeLord. No Internet. Go OFFLINE.");
				mode = Mode.OFFLINE;
				state = InitState.INITIAL_HANDSHAKE_DONE;
				return;
			}
			if (belord.IsPlayerAuthenticated())
			{
				Debug.Log("--------- BeLord.IsPlayerAuthenticated() returned true.");
				state = InitState.INITIAL_HANDSHAKE_DONE;
				mode = Mode.ONLINE;
				return;
			}
			Debug.Log("--------- BeLord.IsPlayerAuthenticated() returned false.");
			state = InitState.AUTHING;
			if (gameInfo != null)
			{
				belord.AuthenticatePlayer(gameInfo.GetGameInfo(), onAuthPlayer, onAuthPlayerLogout);
			}
			else
			{
				belord.AuthenticatePlayer(null, onAuthPlayer, onAuthPlayerLogout);
			}
		}
		else
		{
			mode = Mode.OFFLINE;
			state = InitState.INITIAL_HANDSHAKE_DONE;
		}
	}

	public static bool IsLoggedIn()
	{
		if (belord == null)
		{
			return false;
		}
		return belord.IsPlayerAuthenticated();
	}

	public static InitState GetStatus()
	{
		return state;
	}

	public static Mode GetMode()
	{
		return mode;
	}

	public static string GetPlayerName()
	{
		return belord.GetPlayerName();
	}

	public static void Update()
	{
		InitState initState = state;
		if (initState == InitState.AUTH_END)
		{
			state = InitState.INITIAL_HANDSHAKE_DONE;
		}
	}

	public static void LoadAchievements(BeLordCommandResult result)
	{
		BeLord.result = result;
		belord.LoadAchievements(onAchLoadingFinish);
	}

	public static bool UnlockAchievement(string id, StringConsts msg)
	{
		if (!Enable)
		{
			return false;
		}
		reportingAchievement = id;
		if (!BeLordLocal.HasAchievement(id))
		{
			if (mode == Mode.ONLINE)
			{
				belord.UnlockAchievement(id, onAchievementFinish);
			}
			BeLordLocal.UnlockAchievement(id);
			return true;
		}
		if (!BeLordLocal.GetAchievementSentStatus(id) && mode == Mode.ONLINE)
		{
			belord.UnlockAchievement(id, onAchievementFinish);
		}
		return false;
	}

	public static bool UnlockAchievement(string id, string msg)
	{
		if (!Enable)
		{
			return false;
		}
		reportingAchievement = id;
		if (!BeLordLocal.HasAchievement(id))
		{
			if (mode == Mode.ONLINE)
			{
				belord.UnlockAchievement(id, onAchievementFinish);
			}
			BeLordLocal.UnlockAchievement(id);
			return true;
		}
		if (!BeLordLocal.GetAchievementSentStatus(id) && mode == Mode.ONLINE)
		{
			belord.UnlockAchievement(id, onAchievementFinish);
		}
		return false;
	}

	public static void ReportScore(string id, int pnts)
	{
		Debug.Log(string.Format("BeLord.ReportScore. id: {0} pnts: {1} BeLord.Enabled: {2} mode: {3}", id, pnts, Enable, mode));
		if (Enable && IsSocialNetworkServiceAvailable())
		{
			reportingLeaderboard2 = string.Empty;
			if (mode == Mode.ONLINE)
			{
				reportingLeaderboard = id;
				reportingLeaderboardScore = pnts;
				belord.ReportScore(pnts, id, onReportScoreFinish);
			}
			else
			{
				BeLordLocal.ReportScore(id, pnts);
			}
		}
	}

	public static void ReportTwoScores(string id, int pnts, string id2, int pnts2)
	{
		if (Enable && IsSocialNetworkServiceAvailable())
		{
			if (mode == Mode.ONLINE)
			{
				reportingLeaderboard = id;
				reportingLeaderboardScore = pnts;
				reportingLeaderboard2 = id2;
				reportingLeaderboardScore2 = pnts2;
				belord.ReportScore(pnts, id, onReportScoreFinish);
			}
			else
			{
				BeLordLocal.ReportScore(id, pnts);
				BeLordLocal.ReportScore(id2, pnts2);
			}
		}
	}

	public static void ReportPendingScores()
	{
		ReportPendingScoresStartingAt(0);
	}

	private static void ReportPendingScoresStartingAt(int lbIdx)
	{
		if (lbIdx >= gameLeaderboards.leaderboardsIds.Length)
		{
			state = InitState.INITIAL_HANDSHAKE_DONE;
			return;
		}
		for (int i = lbIdx; i < gameLeaderboards.leaderboardsIds.Length; i++)
		{
			int pendingToSendScore = BeLordLocal.GetPendingToSendScore(gameLeaderboards.leaderboardsIds[i]);
			if (pendingToSendScore > 0)
			{
				pendingScoreIdx = i;
				state = InitState.WAITING_REPORT_SCORE;
				belord.ReportScore(pendingToSendScore, gameLeaderboards.leaderboardsIds[i], onReportScorePendingFinish);
				return;
			}
		}
		lbIdx = gameLeaderboards.leaderboardsIds.Length;
		state = InitState.INITIAL_HANDSHAKE_DONE;
	}

	public static bool HasAchievement(string id)
	{
		return BeLordLocal.HasAchievement(id);
	}

	public static int GetNumAchievementsUnlocked()
	{
		return gameAchievements.GetUnlockedCount();
	}

	public static int GetNumAchievements()
	{
		return gameAchievements.GetCount();
	}

	public static void GetScore(bool friendsOnly, BeLordTimeScope timeScope, string catId, int fromRange, int count, dlgGetScoreResult onGetScoreResult)
	{
		if (mode == Mode.OFFLINE)
		{
			if (onGetScoreResult != null)
			{
				onGetScoreResult(null, false, Strings.Get(StringConsts.LB_TRYING_TO_GET_OFFLINE));
			}
		}
		else
		{
			BeLord.onGetScoreResult = onGetScoreResult;
			belord.GetScore(friendsOnly, timeScope, catId, fromRange, count, onGetScore);
		}
	}

	public static void ResetAchievements()
	{
		BeLordLocal.ResetAchievements();
		if (mode == Mode.ONLINE)
		{
			belord.ResetAchievements();
		}
	}

	public static bool IsOnline()
	{
		return mode == Mode.ONLINE;
	}

	public static void OpenDashboard(BeLordSimpleDelegate onClosed)
	{
		belord.OpenDashboard(onClosed);
	}

	private static void onAuthPlayer(BeLordCommand cmd, bool finishOk, string error)
	{
		Debug.Log(string.Format("PlayerAuthenticated. onPlayerAuthenticated. ok: {0} err: {1}", finishOk, error));
		if (!finishOk)
		{
			mode = Mode.OFFLINE;
			state = InitState.INITIAL_HANDSHAKE_DONE;
		}
		else
		{
			mode = Mode.ONLINE;
			state = InitState.AUTH_END;
		}
	}

	private static void onAuthPlayerLogout(BeLordCommand cmd, bool finishOk, string error)
	{
		Debug.Log("Logged out");
		mode = Mode.OFFLINE;
		state = InitState.INITIAL_HANDSHAKE_DONE;
	}

	private static void onAchLoadingFinish(BeLordCommand cmd, bool finishOk, string error)
	{
		if (!finishOk)
		{
			mode = Mode.OFFLINE;
			state = InitState.INITIAL_HANDSHAKE_DONE;
		}
		else
		{
			state = InitState.SYNC;
			BeLordAchievementSync.SyncAchievements(gameAchievements, belord, onSyncFinish);
		}
		if (result != null)
		{
			result(cmd, finishOk, error);
		}
	}

	private static void onAchievementFinish(BeLordCommand cmd, bool finishOk, string error)
	{
		if (finishOk)
		{
			BeLordLocal.SaveAchievementSentOK(reportingAchievement);
		}
	}

	private static void onSyncFinish()
	{
		state = InitState.START_REPORTING_PENDING_SCORES;
	}

	private static void onGetScore(BeLordCommand cmd, bool finishOk, string error)
	{
		if (onGetScoreResult != null)
		{
			if (finishOk)
			{
				onGetScoreResult(belord.GetScoreData(), finishOk, error);
			}
			else
			{
				onGetScoreResult(null, finishOk, error);
			}
		}
	}

	private static void onReportScoreFinish(BeLordCommand cmd, bool finishOk, string error)
	{
		if (finishOk)
		{
			BeLordLocal.ClearPendingToSendScore(reportingLeaderboard);
		}
		else
		{
			BeLordLocal.ReportScore(reportingLeaderboard, reportingLeaderboardScore);
		}
		if (reportingLeaderboard2 != string.Empty)
		{
			ReportScore(reportingLeaderboard2, reportingLeaderboardScore2);
		}
	}

	private static void onReportScorePendingFinish(BeLordCommand cmd, bool finishOk, string error)
	{
		if (finishOk)
		{
			if (pendingScoreIdx != -1 && pendingScoreIdx < gameLeaderboards.leaderboardsIds.Length)
			{
				BeLordLocal.ClearPendingToSendScore(gameLeaderboards.leaderboardsIds[pendingScoreIdx]);
			}
			if (pendingScoreIdx >= gameLeaderboards.leaderboardsIds.Length)
			{
				state = InitState.INITIAL_HANDSHAKE_DONE;
				return;
			}
			pendingScoreIdx++;
			state = InitState.REPORTING_PENDING_SCORES;
		}
		else
		{
			state = InitState.INITIAL_HANDSHAKE_DONE;
		}
	}

	public static void OpenLeaderboards()
	{
		belord.OpenLeaderboards();
	}

	public static void OpenAchievements()
	{
		belord.OpenAchievements();
	}
}
