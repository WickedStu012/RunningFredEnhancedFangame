using UnityEngine;

public class ActBounce : IAction
{
	private enum BounceState
	{
		FALLING_BACK = 0,
		ON_FLOOR = 1,
		STANDING = 2
	}

	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private BounceState state;

	private float bounceEndLen;

	private float bounceEndFastLen;

	private CharProps props;

	private float dt;

	private float getInPosY;

	public ActBounce(GameObject player)
		: base(player)
	{
		stateName = ActionCode.BOUNCE;
		props = CharHelper.GetProps();
		bounceEndLen = CharAnimManager.GetBounceEndLength();
		bounceEndFastLen = CharAnimManager.GetBounceEndFastLength();
	}

	public override bool CanGetIn()
	{
		if (GameManager.IsFredDead())
		{
			return false;
		}
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.Bounce();
		state = BounceState.FALLING_BACK;
		accumTime = 0f;
		sm.SteerDirection *= -1f;
		if (ConfigParams.useGore)
		{
			CharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD, 1);
		}
		sm.AccumAccel = 0.55f;
		sm.ResetLastYPos();
		sm.MoveDirection = Vector3.zero;
		getInPosY = playerT.position.y;
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
		case BounceState.FALLING_BACK:
		{
			if (accumTime < 0.2f)
			{
				moveCharacter(true);
				break;
			}
			if (accumTime < 0.6f)
			{
				moveCharacter(false);
				break;
			}
			moveCharacter(false);
			float deltaY = sm.GetDeltaY();
			if (sm.IsGrounded && !GameManager.IsFredDead() && sm.HittedAgainstHardSurface() && (deltaY < props.minHeightToDie || (props.RubberBones && deltaY < props.minHeightToExplode)))
			{
				SoundManager.PlaySound(36);
				if (props.FastRecovery)
				{
					CharAnimManager.BounceEndFast();
				}
				else
				{
					CharAnimManager.BounceEnd();
				}
				state = BounceState.STANDING;
				accumTime = 0f;
			}
			break;
		}
		case BounceState.STANDING:
			if ((props.FastRecovery && accumTime >= bounceEndFastLen) || (!props.FastRecovery && accumTime >= bounceEndLen))
			{
				CharHelper.GetCharSkin().Blink(2f);
				CharAnimManager.StopAll();
				sm.SwitchTo(ActionCode.RUNNING);
			}
			break;
		}
		float num = getInPosY - playerT.position.y;
		if (num > 150f && isNoFloorBottom() && !GameManager.IsFredDead())
		{
			SoundManager.PlaySound(27);
			GameEventDispatcher.Dispatch(this, new PlayerDieFalling());
		}
	}

	private void moveCharacter(bool jump)
	{
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1.001f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		float num = 0f;
		if (jump)
		{
			num = 4f * dt;
		}
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num, sm.AccumAccel * -0.5f);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}

	private bool isNoFloorBottom()
	{
		return !Physics.Raycast(playerT.position, Vector3.down, float.PositiveInfinity, 22032896);
	}
}
