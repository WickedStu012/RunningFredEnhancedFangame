using UnityEngine;

public class ActRunningToGoal : IAction
{
	private enum State
	{
		WAITING_TARGET = 0,
		FOLLOW_TARGET = 1,
		TARGET_REACHED = 2
	}

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.6f;

	private float incAccelK = 1f;

	private Quaternion targetRotation;

	private CharProps props;

	private Vector3 targetPos;

	private Vector3 fromPos;

	private State state;

	private float accumTimeFollowTarget;

	private OnLevelComplete onLevelComplete = new OnLevelComplete();

	private float dt;

	public ActRunningToGoal(GameObject player)
		: base(player)
	{
		stateName = ActionCode.RUNNING_TO_GOAL;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		sm.ConsecutiveJumpCounter = 0;
		sm.ConsecutiveWallJumpCounter = 0;
		sm.ResetLastYPos();
		state = State.WAITING_TARGET;
		sm.SteerDirection = 0f;
		if (props.HasWings)
		{
			sm.RemoveWings(true, false);
		}
		if (props.HasJetpack && JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.Reset();
		}
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		CharAnimManager.RunAndSprint((sm.Speed - props.MinRunSpeed) / (props.MaxRunSpeed - props.MinRunSpeed), sm.Speed / 18f);
		clampSpeed();
		moveCharacter();
		State state = this.state;
		if (state == State.FOLLOW_TARGET)
		{
			accumTimeFollowTarget += dt;
			playerT.position = Vector3.Lerp(new Vector3(fromPos.x, playerT.position.y, fromPos.z), new Vector3(targetPos.x, playerT.position.y, targetPos.z), accumTimeFollowTarget);
			if (accumTimeFollowTarget >= 1f)
			{
				this.state = State.TARGET_REACHED;
				GameEventDispatcher.Dispatch(this, onLevelComplete);
			}
		}
	}

	private void clampSpeed()
	{
		if (props.MaxRunSpeed <= sm.Speed && sm.Speed <= props.MaxRunSpeed * 1.1f)
		{
			incAccelK = 1f;
		}
		else if (sm.Speed > props.MaxRunSpeed * 1.1f)
		{
			incAccelK = 0.995f;
		}
		else
		{
			incAccelK = props.RunningAccelK;
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
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 0.6f * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}

	public void SetTargetPoint(Vector3 targetPos)
	{
		playerT = CharHelper.GetPlayerTransform();
		fromPos = playerT.position;
		this.targetPos = targetPos;
		accumTimeFollowTarget = 0f;
		state = State.FOLLOW_TARGET;
	}
}
