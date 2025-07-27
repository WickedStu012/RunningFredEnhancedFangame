public class EvnTouchGroundToExplode : IEvent
{
	private CharProps props;

	public EvnTouchGroundToExplode()
	{
		code = EventCode.EVN_TOUCH_GROUND_TO_EXPLODE;
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
			if (num > props.minHeightToExplode && sm.HittedAgainstHardSurface())
			{
				ScreenShaker.Shake(0.5f, 8f);
				return true;
			}
		}
		return false;
	}
}
