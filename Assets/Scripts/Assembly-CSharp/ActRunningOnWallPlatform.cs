using System;
using UnityEngine;

public class ActRunningOnWallPlatform : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private const float targetYPos = 3f;

	private const double GET_IN_MIN_DELTA_TIME = 500.0;

	private CharProps props;

	private float accumTime;

	private DateTime lastGetInTime;

	private float dt;

	public ActRunningOnWallPlatform(GameObject player)
		: base(player)
	{
		props = CharHelper.GetProps();
		stateName = ActionCode.RUNNING_ON_WALL_PLATFORM;
	}

	public override bool CanGetIn()
	{
		return (DateTime.Now - lastGetInTime).TotalMilliseconds > 750.0;
	}

	public override void GetIn(params object[] list)
	{
		lastGetInTime = DateTime.Now;
		if (CharHelper.GetPlayerTransform().position.x < 0f)
		{
			CharAnimManager.RunningOnLeftWall();
		}
		else
		{
			CharAnimManager.RunningOnRightWall();
		}
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		moveCharacter();
	}

	private void moveCharacter()
	{
		sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(0f, sm.MoveDirection.y, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
