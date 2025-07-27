using UnityEngine;

public class ActSuperSprint : IAction
{
	private enum FovState
	{
		FOV_IN = 0,
		NORMAL = 1,
		FOV_OUT = 2
	}

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float incAccelK = 1.003f;

	private float accumTime;

	private Quaternion targetRotation;

	private GameObject smokeTrail;

	private CharProps props;

	private float accumTimeSprintLength;

	private float dt;

	public ActSuperSprint(GameObject player)
		: base(player)
	{
		stateName = ActionCode.SUPER_SPRINT;
		smokeTrail = GameObject.FindWithTag("PlayerTrail");
		props = CharHelper.GetProps();
	}

	public void ResetTimerToReturnToRunning()
	{
		accumTimeSprintLength = 0f;
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 1f;
		CharAnimManager.SuperSprint();
		sm.ResetLastYPos();
		accumTimeSprintLength = 0f;
		if (smokeTrail != null)
		{

			smokeTrail.SetActive(true);
		}
		SoundManager.PlaySound(57);
		CharHelper.GetEffects().EnableAfterBurnerParticles();
		FovAnimator.FovIn();
	}

	public override void GetOut()
	{
		if (smokeTrail != null)
		{
			smokeTrail.SetActive(false);
		}
		FovAnimator.FovOut();
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		accumTimeSprintLength += dt;
		if (accumTimeSprintLength > props.SuperSprintTime - 0.5f)
		{
			FovAnimator.FovOut();
		}
		if (accumTimeSprintLength > props.SuperSprintTime)
		{
			CharAnimManager.SuperSprintToRun();
			sm.SwitchTo(ActionCode.RUNNING);
		}
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		clampSpeed();
		steerCharacter();
		moveCharacter();
	}

	private void clampSpeed()
	{
		if (props.MaxSprintSpeed <= sm.Speed && sm.Speed <= props.MaxSprintSpeed * 1.1f)
		{
			incAccelK = 1f;
		}
		else if (sm.Speed > props.MaxSprintSpeed * 1.1f)
		{
			incAccelK = 0.98f;
		}
		else
		{
			incAccelK = props.SuperSprintAccelK;
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
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, (!cc.isGrounded) ? (sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt) : 0f, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
