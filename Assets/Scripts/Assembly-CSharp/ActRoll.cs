using UnityEngine;

public class ActRoll : IAction
{
	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private float maxTime;

	private CharProps props;

	private float dt;

	private Quaternion targetRotation;

	public ActRoll(GameObject player)
		: base(player)
	{
		stateName = ActionCode.ROLL;
		maxTime = CharAnimManager.GetRollLength() * 0.6f;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.Roll();
		accumTime = 0f;
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		moveCharacter();
		accumTime += dt;
		if (accumTime > maxTime)
		{
			CharAnimManager.StopAll();
			sm.SwitchTo(ActionCode.RUNNING);
			accumTime = 0f;
		}
	}

	private void moveCharacter()
	{
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1.001f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
