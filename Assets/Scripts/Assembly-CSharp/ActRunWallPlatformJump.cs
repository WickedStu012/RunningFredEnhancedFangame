using UnityEngine;

public class ActRunWallPlatformJump : IAction
{
	private enum State
	{
		NOT_CTRL = 0,
		CTRL = 1
	}

	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private State state;

	private bool isLeftWall;

	private float dt;

	public ActRunWallPlatformJump(GameObject player)
		: base(player)
	{
		stateName = ActionCode.RUN_WALL_PLATFORM_JUMP;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		sm.MoveDirection = Vector3.zero;
		accumTime = 0f;
		accumTimeJump = 0f;
		SoundManager.PlaySound(0);
		sm.IsGoingUp = true;
		state = State.NOT_CTRL;
		isLeftWall = playerT.position.x < 0f;
		if (isLeftWall)
		{
			CharAnimManager.WallBounceLeft();
		}
		else
		{
			CharAnimManager.WallBounceRight();
		}
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		switch (state)
		{
		case State.NOT_CTRL:
			steerCharacter();
			accumTimeJump += dt;
			if (accumTimeJump < props.MaxJumpTime)
			{
				moveCharacterInJump(true);
				break;
			}
			CharAnimManager.DramaticJump();
			state = State.CTRL;
			break;
		case State.CTRL:
			MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
			steerCharacter();
			sm.IsGoingUp = false;
			moveCharacterInJump(false);
			break;
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
			num = 6.5f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3((!isLeftWall) ? (-0.5f) : 0.5f, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}

	private void moveCharacter()
	{
		sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(0f, sm.MoveDirection.y, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
