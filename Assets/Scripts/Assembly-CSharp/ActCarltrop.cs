using UnityEngine;

public class ActCarltrop : IAction
{
	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.55f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private float dt;

	public ActCarltrop(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CARLTROP;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 1f;
		sm.ConsecutiveJumpCounter = 0;
		sm.ResetLastYPos();
		if (Random.Range(0, 2) == 0)
		{
			CharAnimManager.HurtFootLeft();
		}
		else
		{
			CharAnimManager.HurtFootRight();
		}
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		steerCharacter();
		moveCharacter();
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
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1.001f + sm.FloorNormalZ * 0.01f, 0.55f, 0.55f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.55f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
