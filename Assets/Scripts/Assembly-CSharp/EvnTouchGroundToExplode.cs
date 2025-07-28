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
			// RubberBones reduces fall damage by increasing the height thresholds
			float effectiveMinHeightToExplode = props.RubberBones ? props.minHeightToExplode * 1.2f : props.minHeightToExplode;
			
			if (num > effectiveMinHeightToExplode && sm.HittedAgainstHardSurface())
			{
				ScreenShaker.Shake(0.5f, 8f);
				return true;
			}
		}
		return false;
	}
}
