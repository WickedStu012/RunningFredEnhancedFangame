using UnityEngine;

public class Reaper : MonoBehaviour
{
	private enum States
	{
		None = 0,
		Following = 1,
		Slicing = 2,
		FaceCamera = 3,
		GoAway = 4
	}

	public float GoAwayAccel = 3f;

	public float Speed = 22f;

	public float SitheDistance = 2f;

	public float SliceTimer = 0.5f;

	public float RotationSpeed = 80f;

	private float currentAngle;

	private GameObject player;

	private States currentState = States.Following;

	private CharStateMachine stateMachine;

	private Rigidbody playerRigidBody;

	private float sliceTimer;

	private float goAwaySpeed;

	private ItemInfo panicPower;

	private bool panicUsed;

	private float backgroundVolume;

	private float musicVolume;

	private void Awake()
	{
		backgroundVolume = SoundManager.BackgroundVolume;
		musicVolume = SoundManager.MusicVolume;
		GameEventDispatcher.AddListener("ReaperIsCloseToPlayer", OnReaperClose);
		GameEventDispatcher.AddListener("ReaperIsFarFromPlayer", OnReaperFar);
		GameEventDispatcher.AddListener("OnHideGUIEvent", OnEnd);
		GameEventDispatcher.AddListener("PlayerReachGoal", OnEnd);
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnPlayerRespawn);
		GameEventDispatcher.AddListener("OnEndLessReset", OnPlayerRespawn);
		currentState = States.None;
		base.gameObject.SetActive(false);
		if (Store.Instance != null)
		{
			panicPower = Store.Instance.GetItem(104);
		}
		panicUsed = false;
	}

	private void OnPlayerRespawn(object sender, GameEvent evt)
	{
		GameEventDispatcher.AddListener("ReaperIsCloseToPlayer", OnReaperClose);
		GameEventDispatcher.AddListener("ReaperIsFarFromPlayer", OnReaperFar);
		GameEventDispatcher.AddListener("OnHideGUIEvent", OnEnd);
		GameEventDispatcher.AddListener("PlayerReachGoal", OnEnd);
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnPlayerRespawn);
		GameEventDispatcher.AddListener("OnEndLessReset", OnPlayerRespawn);
		currentState = States.None;
		base.gameObject.SetActive(false);
		panicUsed = false;
		currentAngle = 0f;
		goAwaySpeed = 0f;
		sliceTimer = 0f;
	}

	private void OnEnd(object sender, GameEvent evt)
	{
		GameEventDispatcher.RemoveListener("ReaperIsCloseToPlayer", OnReaperClose);
		GameEventDispatcher.RemoveListener("ReaperIsFarFromPlayer", OnReaperFar);
		GameEventDispatcher.RemoveListener("OnHideGUIEvent", OnEnd);
		GameEventDispatcher.RemoveListener("PlayerReachGoal", OnEnd);
		SoundManager.StopBackground();
		currentState = States.FaceCamera;
		goAwaySpeed = 0f;
	}

	private void OnDisable()
	{
		ConfigParams.musicVolume = musicVolume;
		ConfigParams.backgroundVolume = backgroundVolume;
	}

	private void OnReaperClose(object sender, GameEvent evt)
	{
		player = CharHelper.GetPlayer();
		if (player != null)
		{
			if (stateMachine == null)
			{
				stateMachine = CharHelper.GetCharStateMachine();
			}
			if (playerRigidBody == null)
			{
				Transform transformByName = CharHelper.GetTransformByName("torso1");
				if (transformByName != null)
				{
					playerRigidBody = transformByName.GetComponent<Rigidbody>();
				}
			}
			if (ReaperManager.Instance != null && player != null)
			{
				base.transform.position = player.transform.position - Vector3.forward * ReaperManager.Instance.VisibleDistance;
			}
			base.gameObject.SetActive(true);
			currentState = States.Following;
			Animation anim = base.GetComponent<Animation>();
			if (anim != null && anim["Move"] != null)
			{
				anim["Move"].wrapMode = WrapMode.Loop;
				anim.Play("Move");
			}
			else
			{
				Debug.LogWarning("Reaper: Animation component or Move clip not found");
			}
		}
		else
		{
			// Player is null, don't activate the reaper
			Debug.LogWarning("Reaper: Player is null, cannot activate reaper");
		}
	}

	private void OnReaperFar(object sender, GameEvent evt)
	{
		SoundManager.StopBackground();
		SoundManager.MusicVolume = musicVolume;
		SoundManager.BackgroundVolume = backgroundVolume;
		base.gameObject.SetActive(false);
		panicUsed = false;
		currentState = States.None;
	}

	private void FixedUpdate()
	{
		// Re-check player reference in case it became null
		if (player == null)
		{
			player = CharHelper.GetPlayer();
		}
		
		if (player != null)
		{
			switch (currentState)
			{
			case States.Following:
				Following(Time.fixedDeltaTime);
				break;
			case States.Slicing:
				Slicing(Time.fixedDeltaTime);
				break;
			case States.FaceCamera:
				FaceCamera(Time.fixedDeltaTime);
				break;
			case States.GoAway:
				GoAway(Time.fixedDeltaTime);
				break;
			}
		}
	}

	private void Following(float deltaTime)
	{
		if (player == null) return;
		
		base.transform.LookAt(player.transform);
		base.transform.position += base.transform.forward * Speed * deltaTime;
		Vector3 vector = player.transform.position - base.transform.position;
		float num = 0f;
		if (ReaperManager.Instance != null)
		{
			num = ReaperManager.Instance.VisibleDistance - SitheDistance;
		}
		num *= num;
		if (num > 0f)
		{
			SoundManager.BackgroundVolume = backgroundVolume - (vector.sqrMagnitude - SitheDistance * SitheDistance) / num;
			SoundManager.MusicVolume = musicVolume * ((vector.sqrMagnitude - SitheDistance * SitheDistance) / num);
		}
		SoundManager.BackgroundVolume = Mathf.Clamp(SoundManager.BackgroundVolume, 0f, backgroundVolume);
		SoundManager.MusicVolume = Mathf.Clamp(SoundManager.MusicVolume, 0f, musicVolume);
		if (vector.sqrMagnitude <= SitheDistance * SitheDistance && stateMachine != null && stateMachine.GetCurrentState() != ActionCode.SUPER_SPRINT && stateMachine.GetCurrentState() != ActionCode.AFTER_BURNER_JUMP)
		{
			base.transform.position = player.transform.position - base.transform.forward * SitheDistance;
			currentState = States.Slicing;
			Animation anim = base.GetComponent<Animation>();
			if (anim != null && anim["Slice1"] != null)
			{
				anim["Slice1"].wrapMode = WrapMode.Once;
				anim.Play("Slice1");
			}
			else
			{
				Debug.LogWarning("Reaper: Animation component or Slice1 clip not found");
			}
			sliceTimer = Time.time;
		}
	}

	private void Slicing(float deltaTime)
	{
		if (player == null) return;
		
		base.transform.LookAt(player.transform);
		base.transform.position += base.transform.forward * Speed * deltaTime;
		if ((player.transform.position - base.transform.position).sqrMagnitude > SitheDistance * SitheDistance)
		{
			currentState = States.Following;
			Animation anim = base.GetComponent<Animation>();
			if (anim != null && anim["Move"] != null)
			{
				anim["Move"].wrapMode = WrapMode.Loop;
				anim.Play("Move");
			}
			else
			{
				Debug.LogWarning("Reaper: Animation component or Move clip not found in Slicing");
			}
		}
		else if (panicPower != null && panicPower.Count > 0 && !panicUsed)
		{
			SoundManager.PlaySound(43);
			if (Store.Instance != null)
			{
				Store.Instance.ConsumeItem(panicPower.Id);
			}
			if (stateMachine != null)
			{
				stateMachine.SwitchTo(ActionCode.SUPER_SPRINT);
			}
			currentState = States.Following;
			panicUsed = true;
		}
		else if (Time.time - sliceTimer >= SliceTimer)
		{
			SoundManager.PlaySound(43);
			if (stateMachine != null)
			{
				stateMachine.SwitchTo(ActionCode.RAGDOLL);
			}
			base.transform.LookAt(base.transform.position + Vector3.forward);
			currentState = States.FaceCamera;
			if (playerRigidBody != null)
			{
				playerRigidBody.linearVelocity = Vector3.forward * 100f;
			}
		}
		else
		{
			base.transform.position = player.transform.position - base.transform.forward * SitheDistance;
		}
	}

	private void FaceCamera(float deltaTime)
	{
		Animation anim = base.GetComponent<Animation>();
		if (anim != null && !anim.isPlaying)
		{
			if (anim["Move"] != null)
			{
				anim["Move"].wrapMode = WrapMode.Loop;
				anim.Play("Move");
			}
			else
			{
				Debug.LogWarning("Reaper: Move animation clip not found in FaceCamera");
			}
		}
		if (currentAngle < 180f)
		{
			currentAngle += RotationSpeed * deltaTime;
			if (currentAngle >= 180f)
			{
				currentAngle = 180f;
				currentState = States.GoAway;
			}
		}
		base.transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
	}

	private void GoAway(float deltaTime)
	{
		Animation anim = base.GetComponent<Animation>();
		if (anim != null && !anim.isPlaying)
		{
			if (anim["Move"] != null)
			{
				anim["Move"].wrapMode = WrapMode.Loop;
				anim.Play("Move");
			}
			else
			{
				Debug.LogWarning("Reaper: Move animation clip not found in GoAway");
			}
		}
		goAwaySpeed += GoAwayAccel * deltaTime;
		base.transform.position += base.transform.forward * goAwaySpeed * deltaTime;
	}
}
