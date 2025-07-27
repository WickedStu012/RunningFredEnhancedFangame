using UnityEngine;

public class ActClimbEnd : IAction
{
	private CharProps props;

	private float maxTime;

	private float accumTime;

	private float dt;

	public ActClimbEnd(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CLIMB_END;
		props = CharHelper.GetProps();
		maxTime = CharAnimManager.GetClimbEndLength();
	}

	public override bool CanGetIn()
	{
		return !GameManager.IsFredDead();
	}

	public override void GetIn(params object[] list)
	{
		sm.AccumAccel = 0f;
		CharAnimManager.ClimbEnd();
		accumTime = 0f;
		SoundManager.PlaySound(42);
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		accumTime += dt;
		if (accumTime > maxTime)
		{
			sm.SwitchTo(ActionCode.RUNNING);
		}
		moveCharacter();
	}

	private void moveCharacter()
	{
		sm.MoveDirection = new Vector3(0f, 10f * dt, 0f);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
