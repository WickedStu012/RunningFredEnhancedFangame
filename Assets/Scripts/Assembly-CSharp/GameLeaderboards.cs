using UnityEngine;

public class GameLeaderboards : MonoBehaviour
{
	private static GameLeaderboards instance;

	public string[] leaderboardsIds;

	public string[] leaderboardsNumericIds;

	private void Awake()
	{
		instance = this;
	}

	public static string GetLeaderboardNumericId(string strId)
	{
		for (int i = 0; i < instance.leaderboardsIds.Length; i++)
		{
			if (string.Compare(instance.leaderboardsIds[i], strId) == 0)
			{
				return instance.leaderboardsNumericIds[i];
			}
		}
		return null;
	}
}
