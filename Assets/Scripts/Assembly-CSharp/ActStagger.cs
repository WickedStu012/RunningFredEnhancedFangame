using UnityEngine;

public class ActStagger : IAction
{
	private enum State
	{
		STATE1 = 0,
		STATE2 = 1
	}

	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float AUTO_SWITCH_TIMER = 0.75f;

	private float accumTime;

	private State state;

	private CharProps props;

	private float dt;

	public ActStagger(GameObject player)
		: base(player)
	{
		stateName = ActionCode.STAGGER;
		AUTO_SWITCH_TIMER = CharAnimManager.GetHitAndContinueLength();
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return !GameManager.IsFredDead();
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 0f;
		CharAnimManager.HitAndContinue();
		sm.Speed -= 5f;
		state = State.STATE1;
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		moveCharacter();
		accumTime += dt;
		switch (state)
		{
		case State.STATE1:
			if (accumTime > AUTO_SWITCH_TIMER)
			{
				CharAnimManager.Run();
				accumTime = 0f;
				state = State.STATE2;
			}
			break;
		case State.STATE2:
			if (accumTime > AUTO_SWITCH_TIMER)
			{
				sm.SwitchTo(ActionCode.RUNNING);
			}
			break;
		}
	}

	private void moveCharacter()
	{
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1.001f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
