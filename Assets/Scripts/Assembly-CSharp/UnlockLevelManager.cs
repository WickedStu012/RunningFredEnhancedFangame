using UnityEngine;

public class UnlockLevelManager : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Challenge)
		{
			GameEventDispatcher.AddListener("OnLevelComplete", OnFinishLevel);
		}
	}

	private void OnFinishLevel(object sender, GameEvent evt)
	{
		PlayerAccount.Instance.UnlockNextLevel();
		GameEventDispatcher.RemoveListener("OnLevelComplete", OnFinishLevel);
	}

	private void OnDisable()
	{
		GameEventDispatcher.RemoveListener("OnLevelComplete", OnFinishLevel);
	}
}
