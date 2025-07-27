using UnityEngine;

public class ActBounceSpring : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private const float maxJumpTime = 0.4f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private float dt;

	public ActBounceSpring(GameObject player)
		: base(player)
	{
		stateName = ActionCode.BOUNCE_SPRING;
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
		CharAnimManager.Dive();
		SoundManager.PlaySound(66);
		sm.IsGoingUp = true;
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		if (Time.timeScale != 0f)
		{
			MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
			accumTimeJump += dt;
			if (accumTimeJump < 0.4f)
			{
				moveCharacterInJump(true);
			}
			else
			{
				CharAnimManager.Jump();
				sm.IsGoingUp = false;
				moveCharacterInJump(false);
			}
			steerCharacter();
		}
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
