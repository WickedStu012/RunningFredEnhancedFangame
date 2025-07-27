using UnityEngine;

public class ActChickenFlap : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private float jumpTimeK;

	private float dt;

	private int consecutiveFlaps;

	public ActChickenFlap(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CHICKEN_FLAP;
		props = CharHelper.GetProps();
		jumpTimeK = props.MaxJumpTime;
		consecutiveFlaps = 0;
	}

	public override bool CanGetIn()
	{
		if (props.ChickenFlaps == 1)
		{
			return consecutiveFlaps < 1;
		}
		if (props.ChickenFlaps == 2)
		{
			return consecutiveFlaps < 3;
		}
		if (props.ChickenFlaps == 3)
		{
			return consecutiveFlaps < 4;
		}
		if (props.ChickenFlaps == 4)
		{
			return consecutiveFlaps < 6;
		}
		return consecutiveFlaps < 9;
	}

	public override void GetIn(params object[] list)
	{
		if (sm.GetPrevActionState() != ActionCode.CHICKEN_FLAP)
		{
			jumpTimeK = props.MaxJumpTime;
		}
		sm.MoveDirection = Vector3.zero;
		sm.ResetLastYPos();
		accumTime = 0f;
		accumTimeJump = 0f;
		CharAnimManager.ChickenFlap();
		SoundManager.PlaySound(0);
		sm.ConsecutiveJumpCounter++;
		sm.IsGoingUp = true;
		if (sm.GetPrevActionState() != ActionCode.CHICKEN_FLAP)
		{
			consecutiveFlaps = 0;
		}
		consecutiveFlaps++;
		if (props.ChickenFlaps == 1)
		{
			jumpTimeK *= 0.5f;
		}
		else if (props.ChickenFlaps == 2)
		{
			jumpTimeK *= 0.6f;
		}
		else if (props.ChickenFlaps == 3)
		{
			jumpTimeK *= 0.7f;
		}
		else if (props.ChickenFlaps == 4)
		{
			jumpTimeK *= 0.8f;
		}
		else
		{
			jumpTimeK *= 0.95f;
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
		if (accumTimeJump < jumpTimeK)
		{
			moveCharacterInJump(true);
		}
		else
		{
			sm.IsGoingUp = false;
			moveCharacterInJump(false);
		}
		steerCharacter();
	}

	private void steerCharacter()
	{
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
			num = 6.5f;
			sm.SteerDirection += sm.FloorZAngle * 0.01f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
