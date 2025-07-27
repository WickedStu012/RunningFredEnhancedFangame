using UnityEngine;

public class TimerManager : MonoBehaviour
{
	private float timer;

	private bool counting;

	private static TimerManager instance;

	private bool cinematic;

	public static TimerManager Instance
	{
		get
		{
			return instance;
		}
	}

	public int Minutes
	{
		get
		{
			return (int)(timer / 60f);
		}
	}

	public int Seconds
	{
		get
		{
			return (int)(timer % 60f);
		}
	}

	public int TotalSeconds
	{
		get
		{
			return (int)timer;
		}
	}

	private void Awake()
	{
		cinematic = SceneParamsManager.Instance.GetBool("LaunchCinematic", false);
		instance = this;
		timer = 0f;
		if (cinematic)
		{
			GameEventDispatcher.AddListener("OnEndCinematic", OnStart);
		}
		else
		{
			GameEventDispatcher.AddListener("OnLevelLoaded", OnStart);
		}
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnStart);
		GameEventDispatcher.AddListener("OnLevelComplete", OnEnd);
		GameEventDispatcher.AddListener("OnPlayerDead", OnEnd);
		GameEventDispatcher.AddListener("OnPlayerRespawn", OnEnd);
	}

	private void OnStart(object sender, GameEvent evt)
	{
		counting = true;
	}

	private void OnEnd(object sender, GameEvent evt)
	{
		counting = false;
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void Update()
	{
		if (counting)
		{
			timer += Time.deltaTime;
		}
	}
}
