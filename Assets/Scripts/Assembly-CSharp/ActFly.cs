using UnityEngine;

public class ActFly : IAction
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

	private bool animGlideTriggered;

	private float accumTimeGlide;

	private float accumTimeOverABlower;

	private float accumTimeImpulse;

	private bool addingImpulse;

	private Vector3 impulseForward;

	private OnWingsFold onWingsFoldEvent = new OnWingsFold();

	private OnWingsUnfold onWingsUnfoldEvent = new OnWingsUnfold();

	private float dt;

	private int sndFlyId;

	private float steerUpDownClamped;

	public ActFly(GameObject player)
		: base(player)
	{
		stateName = ActionCode.FLY;
		props = CharHelper.GetProps();
		if (Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		addingImpulse = false;
	}

	public override bool CanGetIn()
	{
		if (Mathf.Abs(sm.FloorYPos - sm.transform.position.y) < 1f)
		{
			return false;
		}
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 0f;
		CharAnimManager.GlideLoop();
		if (fredCam == null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		fredCam.SwitchMode(FredCamera.Mode.GLIDE);
		if (wings == null)
		{
			GameObject gameObject = CharHelper.GetCharStateMachine().GetWings();
			wings = gameObject.GetComponent<Wings>();
		}
		wings.Unfold();
		unfoldAnimLen = wings.GetUnfoldLength();
		accumTimeGlide = 0f;
		animGlideTriggered = false;
		sndFlyId = SoundManager.PlaySound(38);
		GameEventDispatcher.Dispatch(this, onWingsUnfoldEvent);
		accumTimeOverABlower = props.TimeOverABlower;
	}

	public override void GetOut()
	{
		SoundManager.StopSound(sndFlyId);
		GameEventDispatcher.Dispatch(this, onWingsFoldEvent);
		FovAnimator.FovOut();
		fredCam.SwitchMode(FredCamera.Mode.NORMAL);
		wings.Fold();
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
		if (!animGlideTriggered)
		{
			accumTimeGlide += dt;
			if (accumTimeGlide > unfoldAnimLen)
			{
				wings.Glide();
				animGlideTriggered = true;
			}
		}
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
		sm.SteerDirection = Mathf.Clamp(sm.SteerDirection, -10000f, 10000f);
		float num = sm.SteerDirection * 45f / 1.6f;
		float num2 = steerUpDownClamped * 45f / 0.8f;
		if (num != 0f || num2 != 0f)
		{
			if (accumTime < 1f)
			{
				playerT.localRotation = Quaternion.Slerp(playerT.localRotation, Quaternion.Euler(num2, 0f, 0f - num), accumTime);
			}
			else
			{
				playerT.localRotation = Quaternion.Euler(num2, 0f, 0f - num);
			}
		}
	}

	private void moveCharacter()
	{
		float num = 0f;
		Vector3 moveDirection = Vector3.zero;
		if (addingImpulse)
		{
			if (accumTimeImpulse < 1.5f)
			{
				accumTimeImpulse += dt;
				sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelImpulseK * (1f + flyAngleK), 1.4f, 3.6f);
				moveDirection = new Vector3(sm.SteerDirection + impulseForward.x, impulseForward.y, impulseForward.z * 2f);
			}
			else
			{
				CharAnimManager.GlideLoop();
				wings.Glide();
				FovAnimator.FovOut();
				addingImpulse = false;
			}
		}
		if (!addingImpulse)
		{
			sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK * (1f + flyAngleK), 1.4f, 3.2f);
			num = sm.MoveDirection.y * 0.5f + Physics.gravity.y * 0.1f * dt * (20f + 100f * steerUpDownClamped);
			moveDirection = new Vector3(sm.SteerDirection, num, sm.AccumAccel);
		}
		moveDirection.Normalize();
		moveDirection *= sm.AccumAccel;
		if (accumTimeOverABlower < props.TimeOverABlower)
		{
			accumTimeOverABlower += dt;
			moveDirection = new Vector3(moveDirection.x, moveDirection.y + dt * 10f * (props.TimeOverABlower - accumTimeOverABlower), moveDirection.z);
		}
		sm.MoveDirection = moveDirection;
		Vector3 moveDirection2 = sm.MoveDirection;
		cc.Move(moveDirection2 * dt * props.RunningAcceleration);
	}

	public void OverABlower()
	{
		accumTimeOverABlower = 0f;
	}

	public void AddImpulse(Vector3 vecForward)
	{
		impulseForward = vecForward;
		addingImpulse = true;
		accumTimeImpulse = 0f;
		CharAnimManager.FlyBoost();
		wings.Boost();
		CharHelper.GetEffects().EnableAfterBurnerParticles();
		SoundManager.PlaySound(57);
		FovAnimator.FovIn();
	}

	private float getClampedValue(float valin, float min, float max)
	{
		float num = Mathf.Clamp(valin, min, max) - min / (max - min);
		return 0.70000005f * num + -0.1f;
	}
}
