using UnityEngine;

public class FredCamera : MonoBehaviour
{
	public enum Mode
	{
		NORMAL = 0,
		DIVE = 1,
		GLIDE = 2,
		CLIMB = 3
	}

	public enum ModeSwitch
	{
		NONE = 0,
		TO_NORMAL = 1,
		TO_DIVE = 2,
		TO_GLIDE = 3,
		TO_CLIMB = 4
	}

	public float distance = 6f;

	public float height = 4f;

	public float heightDamping = 5f;

	public float rotationDamping = 3f;

	public Mode mode;

	public GameObject shadowPlane;

	private float distanceToShadowPlane = 100f;

	private Transform player; 

	private GameObject cameraTarget;

	private Transform cameraTargetT;

	private bool cameraCanMove = true;

	private float currentCameraZPos;

	private float cameraZPos;

	private Vector3 targetPointNormal = new Vector3(0f, 0f, 10f);

	private Vector3 targetPointDive = new Vector3(0f, -10f, 1f);

	private Vector3 targetPointGlide = new Vector3(0f, 0f, 10f);

	private Vector3 targetPointCurrent;

	private Vector3 targetPoint;

	private float accumTime;

	private ModeSwitch modeSwitch;

	private float currentDistance;

	private float toDistance;

	private float dt;

	private bool fixXToPlayer;

	private Transform fixedToTransformPos;

	private Vector3 targetVelocity = Vector3.zero;

	private float targetVerticalVelocitySmoothed;

	private float accumTimeReachGoal;

	private float goalMoveToYPos;

	private Vector3 reachGoalCameraPos;

	

	private void Start()
	{
		cameraCanMove = true;
		cameraTarget = new GameObject("CameraTarget");
		cameraTargetT = cameraTarget.transform;
		player = CharHelper.GetPlayerTransform();
		updateCameraTargetPos();
		targetPoint = targetPointNormal;
	}

	private void OnEnable()
	{
		GameEventDispatcher.AddListener("PlayerReachGoal", onPlayerReachGoal);
		GameEventDispatcher.AddListener("PlayerDieFalling", OnPlayerDieFalling);
		GameEventDispatcher.AddListener("OnPlayerRespawningNow", OnPlayerRespawn);
		GameEventDispatcher.AddListener("OnEndLessReset", EndLessReset);
	}

	private void OnDisable()
	{
		GameEventDispatcher.RemoveListener("PlayerReachGoal", onPlayerReachGoal);
		GameEventDispatcher.RemoveListener("PlayerDieFalling", OnPlayerDieFalling);
		GameEventDispatcher.RemoveListener("OnPlayerRespawningNow", OnPlayerRespawn);
		GameEventDispatcher.RemoveListener("OnEndLessReset", EndLessReset);
	}

	private void OnGUI()
	{
	}

	private void updateCameraTargetPos()
	{
		if (player != null)
		{
			Vector3 position = cameraTargetT.position;
			cameraTargetT.position = player.transform.position + targetPoint;
			targetVelocity = (cameraTargetT.position - position) / Time.deltaTime;
		}
	}

	private void UpdateDistanceVars()
	{
		if (modeSwitch == ModeSwitch.NONE)
		{
			if (mode == Mode.NORMAL)
			{
				cameraZPos = base.transform.position.z;
			}
			else if (mode == Mode.DIVE)
			{
				cameraZPos = player.transform.position.z;
			}
			else if (mode == Mode.GLIDE)
			{
				cameraZPos = base.transform.position.z;
			}
			else if (mode == Mode.CLIMB)
			{
				cameraZPos = base.transform.position.z;
			}
			return;
		}
		if (modeSwitch == ModeSwitch.TO_DIVE)
		{
			targetPoint = Vector3.Slerp(targetPointCurrent, targetPointDive, accumTime);
			cameraZPos = Mathf.Lerp(currentCameraZPos, player.transform.position.z, accumTime);
		}
		else if (modeSwitch == ModeSwitch.TO_NORMAL)
		{
			targetPoint = Vector3.Slerp(targetPointCurrent, targetPointNormal, accumTime * 2f);
			cameraZPos = Mathf.Lerp(currentCameraZPos, base.transform.position.z, accumTime * 2f);
			distance = Mathf.Lerp(currentDistance, toDistance, accumTime);
		}
		else if (modeSwitch == ModeSwitch.TO_GLIDE)
		{
			targetPoint = Vector3.Slerp(targetPointCurrent, targetPointGlide, accumTime * 2f);
			cameraZPos = Mathf.Lerp(currentCameraZPos, base.transform.position.z, accumTime * 2f);
		}
		else if (modeSwitch == ModeSwitch.TO_CLIMB)
		{
			distance = Mathf.Lerp(currentDistance, toDistance, accumTime);
		}
		if (accumTime >= 1f)
		{
			modeSwitch = ModeSwitch.NONE;
		}
	}

	private void Update()
	{
		dt = Time.deltaTime;
		accumTime += dt;
		if (GameManager.IsFredDead())
		{
			if (fixXToPlayer)
			{
				base.transform.position = new Vector3(fixedToTransformPos.position.x, base.transform.position.y, base.transform.position.z);
			}
		}
		else
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (player == null)
			{
				player = CharHelper.GetPlayerTransform();
				if (player == null)
				{
					return;
				}
			}
			updateCameraTargetPos();
			float b = 0f;
			float num = player.transform.position.y + height + ((mode != Mode.DIVE) ? 0f : 4f);
			bool flag = CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.RUNNING || CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.SURF || CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.SUPER_SPRINT || CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.MEGA_SPRINT;
			targetVerticalVelocitySmoothed = Mathf.MoveTowards(targetVerticalVelocitySmoothed, (!flag) ? 0f : targetVelocity.normalized.y, 0.002f);
			num += Mathf.Clamp01(0f - targetVerticalVelocitySmoothed) * 10f;
			num += Mathf.Clamp01(targetVerticalVelocitySmoothed) * -5f;
			float y = base.transform.eulerAngles.y;
			float y2 = base.transform.position.y;
			y = Mathf.LerpAngle(y, b, rotationDamping * dt);
			y2 = ((!(accumTime < 1f)) ? num : Mathf.Lerp(y2, num, heightDamping * accumTime));
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			base.transform.position = cameraTargetT.position + new Vector3(0f, 0f, -10f);
			base.transform.position -= quaternion * Vector3.forward * distance;
			UpdateDistanceVars();
			if (mode == Mode.CLIMB)
			{
				base.transform.position = new Vector3(base.transform.position.x, y2, base.transform.position.z);
			}
			else
			{
				base.transform.position = new Vector3(base.transform.position.x, y2, cameraZPos);
			}
			if (!cameraCanMove)
			{
				accumTimeReachGoal += dt;
				if (accumTimeReachGoal < 1f)
				{
					base.transform.position = Vector3.Lerp(reachGoalCameraPos, new Vector3(reachGoalCameraPos.x, goalMoveToYPos, reachGoalCameraPos.z), accumTimeReachGoal);
				}
				else
				{
					base.transform.position = new Vector3(reachGoalCameraPos.x, goalMoveToYPos, reachGoalCameraPos.z);
				}
			}
			Vector3 position = cameraTargetT.position;
			position += Vector3.up * Mathf.Clamp01(targetVerticalVelocitySmoothed) * 10f;
			base.transform.LookAt(position);
			if (!ConfigParams.zeemoteConnected && !ConfigParams.useiCADE && !MogaInput.Instance.IsConnected())
			{
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, (mode != Mode.DIVE && mode != Mode.CLIMB) ? MovementHelper.GetRotationAngle() : 0f);
			}
			if (shadowPlane != null)
			{
				shadowPlane.transform.position = new Vector3(0f, 0f, base.transform.position.z + distanceToShadowPlane);
			}
		}
	}

	private void onPlayerReachGoal(object sender, GameEvent evn)
	{
		cameraCanMove = false;
		accumTimeReachGoal = 0f;
		reachGoalCameraPos = base.transform.position;
		goalMoveToYPos = base.transform.position.y + 10f;
	}

	public void SwitchMode(Mode m)
	{
		if (m != mode)
		{
			mode = m;
			accumTime = 0f;
			targetPointCurrent = targetPoint;
			currentCameraZPos = cameraZPos;
			switch (m)
			{
			case Mode.NORMAL:
				modeSwitch = ModeSwitch.TO_NORMAL;
				currentDistance = distance;
				toDistance = 6f;
				break;
			case Mode.DIVE:
				modeSwitch = ModeSwitch.TO_DIVE;
				break;
			case Mode.GLIDE:
				modeSwitch = ModeSwitch.TO_GLIDE;
				break;
			case Mode.CLIMB:
				modeSwitch = ModeSwitch.TO_CLIMB;
				currentDistance = distance;
				toDistance = 12f;
				break;
			}
		}
	}

	private void OnPlayerDieFalling(object sender, GameEvent e)
	{
	}

	private void OnPlayerRespawn(object sender, GameEvent e)
	{
		cameraZPos = base.transform.position.z;
		fixXToPlayer = false;
	}

	public void SetDistanceToShadowPlane(float val)
	{
		distanceToShadowPlane = val;
	}

	public void ChangeCameraPosNow()
	{
		if (player == null)
		{
			player = CharHelper.GetPlayerTransform();
			if (player == null)
			{
				return;
			}
		}
		updateCameraTargetPos();
		float y = player.transform.position.y + height;
		Quaternion quaternion = Quaternion.Euler(0f, 0f, 0f);
		base.transform.position = cameraTargetT.position + new Vector3(0f, 0f, -10f);
		base.transform.position -= quaternion * Vector3.forward * distance;
		cameraZPos = (currentCameraZPos = base.transform.position.z);
		if (mode == Mode.CLIMB)
		{
			base.transform.position = new Vector3(base.transform.position.x, y, base.transform.position.z);
		}
		else
		{
			base.transform.position = new Vector3(base.transform.position.x, y, cameraZPos);
		}
		base.transform.LookAt(cameraTargetT);
		if (!ConfigParams.zeemoteConnected && !ConfigParams.useiCADE)
		{
			base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, ((mode != Mode.DIVE) ? MovementHelper.GetRotationAngle() : 0f) / 2);
		}
	}

	private void EndLessReset(object sender, GameEvent e)
	{
		SwitchMode(Mode.NORMAL);
		fixXToPlayer = false;
	}

	public void SetXPosToPlayer()
	{
		fixXToPlayer = true;
		Transform transformByName = CharHelper.GetTransformByName("torso1");
		if (transformByName != null)
		{
			fixedToTransformPos = transformByName;
		}
		else
		{
			fixedToTransformPos = player.transform;
		}
	}
}
