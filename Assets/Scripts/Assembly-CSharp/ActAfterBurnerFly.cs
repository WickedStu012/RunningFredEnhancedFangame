using UnityEngine;

public class ActAfterBurnerFly : IAction
{
	private const float minAccelK = 1.4f;

	private const float maxAccelK = 3.2f;

	private const float maxAccelK2 = 3.6f;

	private const float gravityK = 0.1f;

	private const float minFlyAngle = -25f;

	private const float maxFlyAngle = 45f;

	private float incAccelK = 1f;

	private float incAccelImpulseK = 2.2f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private FredCamera fredCam;

	private Wings wings;

	private float flyAngleK;

	private float unfoldAnimLen;

	private float accumTimeImpulse;

	private Vector3 impulseForward;

	private float dt;

	private int sndFlyId;

	private float steerUpDownClamped;

	public ActAfterBurnerFly(GameObject player)
		: base(player)
	{
		stateName = ActionCode.AFTER_BURNER_FLY;
		props = CharHelper.GetProps();
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
		accumTime = 0f;
		if (fredCam == null && Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		fredCam.SwitchMode(FredCamera.Mode.GLIDE);
		if (wings == null)
		{
			GameObject gameObject = CharHelper.GetCharStateMachine().GetWings();
			wings = gameObject.GetComponent<Wings>();
		}
		sndFlyId = SoundManager.PlaySound(38);
		impulseForward = CharHelper.GetPlayer().transform.forward;
		accumTimeImpulse = 0f;
		CharAnimManager.FlyBoost();
		wings.Boost();
		CharHelper.GetEffects().EnableAfterBurnerParticles();
		FovAnimator.FovIn();
		SoundManager.PlaySound(57);
	}

	public override void GetOut()
	{
		SoundManager.StopSound(sndFlyId);
		fredCam.SwitchMode(FredCamera.Mode.NORMAL);
		FovAnimator.FovOut();
	}

	public override void Update(float dt)
	{
		if (sm.SteerDirectionUpDown != 0f)
		{
			steerUpDownClamped = getClampedValue(sm.SteerDirectionUpDown, -0.65f, 0.45f);
		}
		else
		{
			steerUpDownClamped = 0f;
		}
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation, true);
		sm.SteerDirection *= 2f;
		MovementHelper.CheckMoveActionsUpDown(sm, ref accumTime, ref targetRotation, 45f, -25f);
		float num = playerT.rotation.eulerAngles.x;
		if (num > 180f)
		{
			num -= 360f;
		}
		flyAngleK = num / 45f;
		moveCharacter();
		steerCharacter();
	}

	private void steerCharacter()
	{
		accumTime += dt;
		float num = sm.SteerDirection * 45f / 1.6f;
		float x = steerUpDownClamped * 45f / 0.8f;
		if (accumTime < 1f)
		{
			playerT.localRotation = Quaternion.Slerp(playerT.localRotation, Quaternion.Euler(x, 0f, 0f - num), accumTime);
		}
		else
		{
			playerT.localRotation = Quaternion.Euler(x, 0f, 0f - num);
		}
	}

	private void moveCharacter()
	{
		float num = 0f;
		Vector3 moveDirection = Vector3.zero;
		if (accumTimeImpulse < 1.5f)
		{
			accumTimeImpulse += dt;
			sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelImpulseK * (1f + flyAngleK), 1.4f, 3.6f);
			moveDirection = new Vector3(sm.SteerDirection + impulseForward.x, impulseForward.y, impulseForward.z * 2f);
			sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK * (1f + flyAngleK), 1.4f, 3.2f);
			num = sm.MoveDirection.y * 0.5f + Physics.gravity.y * 0.1f * dt * (20f + 100f * steerUpDownClamped);
			moveDirection = new Vector3(sm.SteerDirection, num, sm.AccumAccel);
		}
		else
		{
			CharAnimManager.GlideLoop();
			wings.Glide();
			FovAnimator.FovOut();
			sm.SwitchTo(ActionCode.FLY);
		}
		moveDirection.Normalize();
		moveDirection *= sm.AccumAccel;
		sm.MoveDirection = moveDirection;
		Vector3 moveDirection2 = sm.MoveDirection;
		cc.Move(moveDirection2 * dt * props.RunningAcceleration);
	}

	private float getClampedValue(float valin, float min, float max)
	{
		float num = Mathf.Clamp(valin, min, max) - min / (max - min);
		return 0.70000005f * num + -0.1f;
	}
}
