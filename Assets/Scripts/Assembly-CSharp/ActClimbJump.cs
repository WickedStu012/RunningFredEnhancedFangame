using UnityEngine;

public class ActClimbJump : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private float dt;

	public ActClimbJump(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CLIMB_JUMP;
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
		CharAnimManager.StairJump();
		SoundManager.PlaySound(0);
		sm.ConsecutiveJumpCounter++;
		sm.IsGoingUp = true;
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		steerCharacter();
		accumTimeJump += dt;
		if (accumTimeJump < props.MaxJumpTime)
		{
			moveCharacterInJump(true);
			return;
		}
		sm.IsGoingUp = false;
		moveCharacterInJump(false);
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
			sm.SteerDirection += sm.FloorZAngle * 0.01f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
