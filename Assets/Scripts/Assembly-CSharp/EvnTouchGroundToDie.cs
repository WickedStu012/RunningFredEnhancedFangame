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
			// RubberBones reduces fall damage by increasing the height thresholds
			float effectiveMinHeightToDie = props.RubberBones ? props.minHeightToDie * 1.2f : props.minHeightToDie;
			float effectiveMinHeightToExplode = props.RubberBones ? props.minHeightToExplode * 1.2f : props.minHeightToExplode;
			
			if (effectiveMinHeightToDie <= num && num < effectiveMinHeightToExplode && sm.HittedAgainstHardSurface())
			{
				ScreenShaker.Shake(0.5f, 8f);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
		}
		return false;
	}
}
