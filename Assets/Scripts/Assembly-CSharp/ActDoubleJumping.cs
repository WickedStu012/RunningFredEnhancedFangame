using UnityEngine;

public class ActDoubleJumping : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.2f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private float accumTimeDJumpAnim;

	private float djumpLength;

	private CharProps props;

	private float dt;

	public ActDoubleJumping(GameObject player)
		: base(player)
	{
		stateName = ActionCode.DOUBLE_JUMP;
		props = CharHelper.GetProps();
		djumpLength = CharAnimManager.GetDoubleJumpLength();
	}

	public override bool CanGetIn()
	{
		return sm.ConsecutiveJumpCounter < props.SuccesiveJumpCount;
	}

	public override void GetIn(params object[] list)
	{
		sm.IsGoingUp = true;
		accumTime = 0f;
		accumTimeJump = 0f;
		accumTimeDJumpAnim = 0f;
		if (sm.ConsecutiveJumpCounter < props.SuccesiveJumpCount)
		{
			CharAnimManager.DoubleJump();
			sm.ConsecutiveJumpCounter++;
			SoundManager.PlaySound(0);
			sm.MoveDirection = new Vector3(sm.MoveDirection.x, 0f, sm.MoveDirection.z);
		}
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		accumTimeJump += dt;
		if (accumTimeJump < props.DoubleJumpTime)
		{
			moveCharacterInJump(true);
		}
		else
		{
			sm.IsGoingUp = false;
			moveCharacterInJump(false);
		}
		if (props.ChickenFlaps > 0)
		{
			accumTimeDJumpAnim += dt;
			if (accumTimeDJumpAnim > djumpLength && InputManager.GetJumpDown())
			{
				sm.SwitchTo(ActionCode.CHICKEN_FLAP);
			}
		}
		steerCharacter();
	}

	private void steerCharacter()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		accumTime += dt;
		if (cc.velocity.x != 0f || cc.velocity.z != 0f)
		{
			if (accumTime < 1f)
			{
				playerT.localRotation = Quaternion.Slerp(playerT.localRotation, Quaternion.LookRotation(new Vector3(cc.velocity.x, 0f, cc.velocity.z)), accumTime);
			}
			else
			{
				playerT.localRotation = Quaternion.LookRotation(new Vector3(cc.velocity.x, 0f, cc.velocity.z));
			}
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
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 0.2f * dt + num * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
