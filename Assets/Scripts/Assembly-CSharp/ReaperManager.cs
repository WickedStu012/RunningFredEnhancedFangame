using UnityEngine;

public class ReaperManager : MonoBehaviour
{
	public float SpeedWhilePlayerStill = 12f;

	public float Speed = 1f;

	public float SpeedWhilePlayerSprint = -2f;

	public float SpeedWhilePlayerMegaSprint = -4f;

	public float SpeedWhilePlayerClimb = 5f;

	public float PlayerCruiseSpeed = 20f;

	public float PlayerSprintSpeed = 25f;

	public float PlayerMegaSprintSpeed = 30f;

	public float MaxDistance = 100f;

	public float VisibleDistance = 20f;

	public float CurrentDistance;

	private GameObject player;

	private Vector3 lastPosition;

	private Vector3 playerSpeed;

	private bool reaperIsVisible;

	private ReaperIsCloseToPlayer reaperCloseEvent = new ReaperIsCloseToPlayer();

	private ReaperIsFarFromPlayer reaperFarEvent = new ReaperIsFarFromPlayer();

	private static ReaperManager instance;

	public static ReaperManager Instance
	{
		get
		{
			return instance;
		}
	}

	public float Distance
	{
		get
		{
			return CurrentDistance / MaxDistance * 100f;
		}
	}

	public bool IsReaperVisible
	{
		get
		{
			return reaperIsVisible;
		}
	}

	private void Awake()
	{
		instance = this;
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnPlayerRespawn);
		GameEventDispatcher.AddListener("OnEndLessReset", EndLessReset);
		GameEventDispatcher.AddListener("OnEndLessResurrect", EndLessResurrect);
		CurrentDistance = MaxDistance;
		GameObject gameObject = Resources.Load(string.Format("Reaper/Prefabs/CuteReaper"), typeof(GameObject)) as GameObject;
		GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
		gameObject2.name = gameObject.name;
	}

	private void OnPlayerRespawn(object sender, GameEvent evt)
	{
		CurrentDistance = MaxDistance;
	}

	private void Start()
	{
		player = CharHelper.GetPlayer();
		lastPosition = player.transform.position;
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void FixedUpdate()
	{
		if (GameManager.IsFredDead())
		{
			return;
		}
		playerSpeed = (player.transform.position - lastPosition) / Time.fixedDeltaTime;
		if (playerSpeed.z >= PlayerMegaSprintSpeed)
		{
			CurrentDistance -= SpeedWhilePlayerMegaSprint * Time.fixedDeltaTime;
		}
		else if (playerSpeed.z >= PlayerSprintSpeed)
		{
			CurrentDistance -= SpeedWhilePlayerSprint * Time.fixedDeltaTime;
		}
		else if (playerSpeed.sqrMagnitude != 0f && playerSpeed.z > 0f)
		{
			CurrentDistance -= Speed * Time.fixedDeltaTime;
		}
		else if (playerSpeed.z != 0f || !(playerSpeed.y > 0f))
		{
			CurrentDistance -= SpeedWhilePlayerStill * Time.fixedDeltaTime;
		}
		if (CurrentDistance > MaxDistance)
		{
			CurrentDistance = MaxDistance;
		}
		else if (CurrentDistance < 0f)
		{
			CurrentDistance = 0f;
		}
		lastPosition = player.transform.position;
		if (reaperIsVisible)
		{
			if (CurrentDistance > VisibleDistance)
			{
				reaperIsVisible = false;
				GameEventDispatcher.Dispatch(this, reaperFarEvent);
			}
		}
		else if (CurrentDistance <= VisibleDistance)
		{
			SoundManager.BackgroundVolume = 0f;
			SoundManager.PlayBackground(53);
			reaperIsVisible = true;
			GameEventDispatcher.Dispatch(this, reaperCloseEvent);
		}
	}

	private void EndLessReset(object sender, GameEvent evt)
	{
		CurrentDistance = MaxDistance;
	}

	private void EndLessResurrect(object sender, GameEvent evt)
	{
		CurrentDistance = MaxDistance / 2;
	}
}
