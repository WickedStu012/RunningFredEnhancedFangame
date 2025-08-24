using UnityEngine;

public class ActDive : IAction
{
	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.4f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.15f;

	private const float maxTimeForFallingFred = 1f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private float maxTimeDive;

	private float accumTimeDiveLoop;

	private FredCamera fredCam;

	private int sndDiveId;

	private float dt;

	private float getInPosY;

	private Vector3[] rbsPos;

	private Quaternion[] rbsRot;

	public ActDive(GameObject player)
		: base(player)
	{
		stateName = ActionCode.DIVE;
		props = CharHelper.GetProps();
		maxTimeDive = CharAnimManager.GetDramaticFallingStartLength();
		if (Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.DramaticFallingStart();
		
		// Use CharHead's HandleDiveAction method instead of CharHeadAnimManager
		GameObject headGO = CharHeadHelper.GetHeadGameObject();
		if (headGO != null)
		{
			CharHead charHead = headGO.GetComponent<CharHead>();
			if (charHead != null)
			{
				charHead.HandleDiveAction(true);
			}
		}
		
		accumTime = 1f;
		accumTimeDiveLoop = 0f;
		sm.MoveDirection = Vector3.zero;
		sm.ConsecutiveJumpCounter = 0;
		sm.ConsecutiveWallJumpCounter = 0;
		if (fredCam == null && Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		if (fredCam != null)
		{
			fredCam.SwitchMode(FredCamera.Mode.DIVE);
		}
		sm.DisableBlob();
		if (Random.Range(0, 1) == 0)
		{
			sndDiveId = SoundManager.PlaySound(39);
		}
		else
		{
			sndDiveId = SoundManager.PlaySound(40);
		}
		getInPosY = playerT.position.y;
		ScreenShaker.Shake(-1f, 1f);
		GameEventDispatcher.AddListener("PlayerDieFalling", OnPlayerIsDead);
	}

	private void OnPlayerIsDead(object sender, GameEvent evt)
	{
		GameEventDispatcher.RemoveListener("PlayerDieFalling", OnPlayerIsDead);
		ScreenShaker.StopShake();
	}

	public override void GetOut()
	{
		// Use CharHead's HandleDiveAction method instead of CharHeadAnimManager
		GameObject headGO = CharHeadHelper.GetHeadGameObject();
		if (headGO != null)
		{
			CharHead charHead = headGO.GetComponent<CharHead>();
			if (charHead != null)
			{
				charHead.HandleDiveAction(false);
			}
		}
		
		if (fredCam != null)
		{
			fredCam.SwitchMode(FredCamera.Mode.NORMAL);
		}
		sm.EnableBlob();
		SoundManager.StopSound(sndDiveId);
		ScreenShaker.StopShake();
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		accumTimeDiveLoop += dt;
		if (accumTimeDiveLoop >= maxTimeDive)
		{
			CharAnimManager.DramaticFalling();
		}
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		MovementHelper.CheckMoveActionsUpDown(sm, ref accumTime, ref targetRotation);
		steerCharacter();
		moveCharacter();
		float num = getInPosY - playerT.position.y;
		if (num > 400f && isNoFloorBottom() && !GameManager.IsFredDead())
		{
			ScreenShaker.StopShake();
			SoundManager.PlaySound(27);
			GameEventDispatcher.Dispatch(this, new PlayerDieFalling());
		}
	}

	private void steerCharacter()
	{
		accumTime += dt;
		if (accumTime < 1f)
		{
			playerT.localRotation = Quaternion.Slerp(playerT.localRotation, targetRotation, accumTime);
		}
		else
		{
			playerT.localRotation = targetRotation;
		}
	}

	private void moveCharacter()
	{
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y, sm.MoveDirection.z);
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1.001f + sm.FloorNormalZ * 0.01f, 0.4f, 1.6f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.15f * dt, 0.4f + sm.SteerDirectionUpDown);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}

	private bool isNoFloorBottom()
	{
		return !Physics.Raycast(playerT.position, Vector3.down, float.PositiveInfinity, 22032896);
	}
}
