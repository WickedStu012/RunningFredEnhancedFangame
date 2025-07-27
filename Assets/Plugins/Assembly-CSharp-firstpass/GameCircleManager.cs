using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCircleManager : MonoBehaviour
{
	public static event Action serviceReadyEvent;

	public static event Action<string> serviceNotReadyEvent;

	public static event Action<string> playerAliasReceivedEvent;

	public static event Action<string> playerAliasFailedEvent;

	public static event Action<string> submitScoreFailedEvent;

	public static event Action submitScoreSucceededEvent;

	public static event Action<string> requestLeaderboardsFailedEvent;

	public static event Action<List<GameCircleLeaderboard>> requestLeaderboardsSucceededEvent;

	public static event Action<string> requestLocalPlayerScoreFailedEvent;

	public static event Action<string, string> requestLocalPlayerScoreSucceededEvent;

	public static event Action<string> requestScoresFailedEvent;

	public static event Action<GameCircleLeaderboard> requestScoresSucceededEvent;

	public static event Action<string> updateAchievementFailedEvent;

	public static event Action updateAchievementSucceededEvent;

	public static event Action<string> loadIconFailedEvent;

	public static event Action<string> loadIconSucceededEvent;

	public static event Action onAlreadySynchronizedEvent;

	public static event Action onConflictDeferralEvent;

	public static event Action onGameUploadSuccessEvent;

	public static event Action<string> onSynchronizeFailureEvent;

	public static event Action onNewGameDataEvent;

	public static event Action onPlayerCancelledEvent;

	public static event Action<string> onRevertFailureEvent;

	public static event Action onRevertedGameDataEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void serviceReady(string empty)
	{
		GameCircle.requestLocalPlayerProfile();
		if (GameCircleManager.serviceReadyEvent != null)
		{
			GameCircleManager.serviceReadyEvent();
		}
	}

	public void serviceNotReady(string param)
	{
		if (GameCircleManager.serviceNotReadyEvent != null)
		{
			GameCircleManager.serviceNotReadyEvent(param);
		}
	}

	public void playerAliasReceived(string playerAlias)
	{
		if (GameCircleManager.playerAliasReceivedEvent != null)
		{
			GameCircleManager.playerAliasReceivedEvent(playerAlias);
		}
	}

	public void playerAliasFailed(string error)
	{
		if (GameCircleManager.playerAliasFailedEvent != null)
		{
			GameCircleManager.playerAliasFailedEvent(error);
		}
	}

	public void submitScoreFailed(string param)
	{
		if (GameCircleManager.submitScoreFailedEvent != null)
		{
			GameCircleManager.submitScoreFailedEvent(param);
		}
	}

	public void submitScoreSucceeded(string empty)
	{
		if (GameCircleManager.submitScoreSucceededEvent != null)
		{
			GameCircleManager.submitScoreSucceededEvent();
		}
	}

	public void requestLeaderboardsFailed(string param)
	{
		if (GameCircleManager.requestLeaderboardsFailedEvent != null)
		{
			GameCircleManager.requestLeaderboardsFailedEvent(param);
		}
	}

	public void requestLeaderboardsSucceeded(string json)
	{
		if (GameCircleManager.requestLeaderboardsSucceededEvent == null)
		{
			return;
		}
		List<GameCircleLeaderboard> list = new List<GameCircleLeaderboard>();
		ArrayList arrayList = json.arrayListFromJson();
		foreach (Hashtable item in arrayList)
		{
			list.Add(GameCircleLeaderboard.fromHashtable(item));
		}
		GameCircleManager.requestLeaderboardsSucceededEvent(list);
	}

	public void requestLocalPlayerScoreFailed(string param)
	{
		if (GameCircleManager.requestLocalPlayerScoreFailedEvent != null)
		{
			GameCircleManager.requestLocalPlayerScoreFailedEvent(param);
		}
	}

	public void requestLocalPlayerScoreSucceeded(string scoreInfo)
	{
		if (GameCircleManager.requestLocalPlayerScoreSucceededEvent != null)
		{
			string[] array = scoreInfo.Split(new string[1] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				GameCircleManager.requestLocalPlayerScoreSucceededEvent(array[0], array[1]);
			}
		}
	}

	public void requestScoresFailed(string error)
	{
		if (GameCircleManager.requestScoresFailedEvent != null)
		{
			GameCircleManager.requestScoresFailedEvent(error);
		}
	}

	public void requestScoresSucceeded(string json)
	{
		if (GameCircleManager.requestScoresSucceededEvent != null)
		{
			GameCircleLeaderboard obj = GameCircleLeaderboard.fromHashtable(json.hashtableFromJson());
			GameCircleManager.requestScoresSucceededEvent(obj);
		}
	}

	public void updateAchievementFailed(string param)
	{
		if (GameCircleManager.updateAchievementFailedEvent != null)
		{
			GameCircleManager.updateAchievementFailedEvent(param);
		}
	}

	public void updateAchievementSucceeded(string empty)
	{
		if (GameCircleManager.updateAchievementSucceededEvent != null)
		{
			GameCircleManager.updateAchievementSucceededEvent();
		}
	}

	public void loadIconFailed(string param)
	{
		if (GameCircleManager.loadIconFailedEvent != null)
		{
			GameCircleManager.loadIconFailedEvent(param);
		}
	}

	public void loadIconSucceeded(string file)
	{
		if (GameCircleManager.loadIconSucceededEvent != null)
		{
			GameCircleManager.loadIconSucceededEvent(file);
		}
	}

	public void onAlreadySynchronized(string empty)
	{
		if (GameCircleManager.onAlreadySynchronizedEvent != null)
		{
			GameCircleManager.onAlreadySynchronizedEvent();
		}
	}

	public void onConflictDeferral(string empty)
	{
		if (GameCircleManager.onConflictDeferralEvent != null)
		{
			GameCircleManager.onConflictDeferralEvent();
		}
	}

	public void onGameUploadSuccess(string empty)
	{
		if (GameCircleManager.onGameUploadSuccessEvent != null)
		{
			GameCircleManager.onGameUploadSuccessEvent();
		}
	}

	public void onSynchronizeFailure(string error)
	{
		if (GameCircleManager.onSynchronizeFailureEvent != null)
		{
			GameCircleManager.onSynchronizeFailureEvent(error);
		}
	}

	public void onNewGameData(string empty)
	{
		if (GameCircleManager.onNewGameDataEvent != null)
		{
			GameCircleManager.onNewGameDataEvent();
		}
	}

	public void onPlayerCancelled(string empty)
	{
		if (GameCircleManager.onPlayerCancelledEvent != null)
		{
			GameCircleManager.onPlayerCancelledEvent();
		}
	}

	public void onRevertFailure(string error)
	{
		if (GameCircleManager.onRevertFailureEvent != null)
		{
			GameCircleManager.onRevertFailureEvent(error);
		}
	}

	public void onRevertedGameData(string empty)
	{
		if (GameCircleManager.onRevertedGameDataEvent != null)
		{
			GameCircleManager.onRevertedGameDataEvent();
		}
	}
}
