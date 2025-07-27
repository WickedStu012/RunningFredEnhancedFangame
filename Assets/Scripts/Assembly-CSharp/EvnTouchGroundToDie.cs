public class EvnTouchGroundToDie : IEvent
{
	private CharProps props;

	public EvnTouchGroundToDie()
	{
		code = EventCode.EVN_TOUCH_GROUND_TO_DIE;
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
			if (!props.RubberBones && props.minHeightToDie <= num && num < props.minHeightToExplode && sm.HittedAgainstHardSurface())
			{
				ScreenShaker.Shake(0.5f, 8f);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
		}
		return false;
	}
}
