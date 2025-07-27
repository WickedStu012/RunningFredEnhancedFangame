public class EvnTouchGroundFromFly : IEvent
{
	private CharProps props;

	public EvnTouchGroundFromFly()
	{
		code = EventCode.EVN_TOUCH_GROUND_FROM_FLY;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		if (!sm.IsGoingUp && sm.IsGrounded)
		{
			float num = sm.lastYPosition - sm.playerT.position.y;
			if (2f < num && num < props.minHeightToRoll)
			{
				SoundManager.PlaySound(36);
				if (num > 1f)
				{
					CharHelper.GetEffects().EnableHardLanding();
				}
				return true;
			}
		}
		return false;
	}
}
