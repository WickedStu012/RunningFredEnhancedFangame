using UnityEngine;

public class ActFallToTrip : IAction
{
	private enum State
	{
		START = 0,
		CHECK = 1,
		END = 2
	}

	private const float minAccelK = 0.55f;

	private const float tripAccelK = 0.25f;

	private const float gravityK = 0.25f;

	private CharProps props;

	private float maxTime2;

	private float accumTime;

	private State state;

	private float dt;

	public ActFallToTrip(GameObject player)
		: base(player)
	{
		stateName = ActionCode.FALL_TO_TRIP;
		props = CharHelper.GetProps();
		maxTime2 = CharAnimManager.GetTripEndLength();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.TripEnd();
		accumTime = 0f;
		state = State.START;
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		accumTime += dt;
		switch (state)
		{
		case State.START:
			moveCharacter(true, true);
			if (accumTime > 0.2f)
			{
				accumTime = 0f;
				state = State.CHECK;
			}
			break;
		case State.CHECK:
			moveCharacter(false, true);
			if (sm.IsGrounded)
			{
				accumTime = 0f;
				CharAnimManager.TripEnd();
				if (ConfigParams.useGore)
				{
					CharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD, 1);
					CharHelper.GetCharBloodSplat().SplatOn((Random.Range(0, 2) != 0) ? BodyDamage.RIGHT_LEG : BodyDamage.LEFT_LEG, 1);
				}
				state = State.END;
			}
			break;
		case State.END:
			moveCharacter(false, false);
			if (accumTime >= maxTime2)
			{
				CharHelper.GetCharSkin().Blink(2f);
				sm.AccumAccel = 0.55f;
				sm.SwitchTo(ActionCode.RUNNING);
			}
			break;
		}
	}

	private void moveCharacter(bool applyMov, bool applyAccel)
	{
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		float num = 0f;
		if (applyMov)
		{
			num = 4f * dt;
		}
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num, (!applyAccel) ? 0f : 0.55f);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
