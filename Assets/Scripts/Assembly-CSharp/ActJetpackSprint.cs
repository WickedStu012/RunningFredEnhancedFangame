using UnityEngine;

public class ActJetpackSprint : IAction
{
	private enum FovState
	{
		FOV_IN = 0,
		NORMAL = 1,
		FOV_OUT = 2
	}

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = -0.1f;

	private float incAccelK = 1.02f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private float accumTimeSprintLength;

	private Jetpack jetpack;

	private int sndId;

	private float dt;

	public ActJetpackSprint(GameObject player)
		: base(player)
	{
		stateName = ActionCode.JETPACK_SPRINT;
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
		sm.ResetLastYPos();
		CharAnimManager.JetpackSprint();
		jetpack = sm.GetJetpack().GetComponent<Jetpack>();
		jetpack.EnableTurbo();
		sndId = SoundManager.PlaySound(SndId.SND_JETPACK);
		FovAnimator.FovIn();
	}

	public override void GetOut()
	{
		jetpack.DisableTurbo();
		SoundManager.StopSound(sndId);
		FovAnimator.FovOut();
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		props.JetPackFuelLeft -= props.JetpackConsumptionSprint * dt;
		accumTimeSprintLength += dt;
		if (accumTimeSprintLength > props.JetpackSuperSprintTime - 0.5f)
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
		if (props.JetPackFuelLeft <= 0f)
		{
			jetpack.DisableTurbo();
			jetpack.EnableFuelOut();
			sm.SwitchTo(ActionCode.DRAMATIC_JUMP);
		}
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
		sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * -0.1f * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
