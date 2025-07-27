using UnityEngine;

public class ActDoubleJumpSide : IAction
{
	private enum State
	{
		NOT_CTRL = 0,
		CTRL = 1
	}

	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.2f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private float accumTimeSpinJump;

	private float spinJumpLength;

	private State state;

	private float dt;

	public ActDoubleJumpSide(GameObject player)
		: base(player)
	{
		stateName = ActionCode.DOUBLE_JUMP_SIDE;
		props = CharHelper.GetProps();
		spinJumpLength = CharAnimManager.GetSpinLeftLength();
	}

	public override bool CanGetIn()
	{
		return sm.ConsecutiveWallJumpCounter <= props.WallBounce;
	}

	public override void GetIn(params object[] list)
	{
		sm.IsGoingUp = true;
		accumTime = 0f;
		accumTimeJump = 0f;
		accumTimeSpinJump = 0f;
		if (sm.inertia.x > 0f)
		{
			sm.SteerDirection = 0.5f;
			CharAnimManager.SpinLeft();
		}
		else
		{
			sm.SteerDirection = -0.5f;
			CharAnimManager.SpinRight();
		}
		SoundManager.PlaySound(0);
		sm.MoveDirection = new Vector3(sm.MoveDirection.x, 0f, sm.MoveDirection.z);
		state = State.NOT_CTRL;
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		accumTimeSpinJump += dt;
		if (accumTimeSpinJump > spinJumpLength)
		{
			CharAnimManager.DramaticJump();
		}
		accumTimeJump += dt;
		switch (state)
		{
		case State.NOT_CTRL:
			if (accumTimeJump > 0.15f)
			{
				state = State.CTRL;
			}
			break;
		case State.CTRL:
			MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
			steerCharacter();
			break;
		}
		if (accumTimeJump < props.DoubleJumpTime)
		{
			moveCharacterInJump(true);
		}
		else
		{
			sm.IsGoingUp = false;
			moveCharacterInJump(false);
		}
		if (props.ChickenFlaps > 0 && InputManager.GetJumpDown())
		{
			sm.SwitchTo(ActionCode.CHICKEN_FLAP);
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

	private void moveCharacterInJump(bool stayInJump)
	{
		float num = 0f;
		if (stayInJump)
		{
			num = 10f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.2f * dt + num * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
