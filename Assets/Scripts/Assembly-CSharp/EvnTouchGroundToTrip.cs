public class EvnTouchGroundToTrip : IEvent
{
	private CharProps props;

	public EvnTouchGroundToTrip()
	{
		code = EventCode.EVN_FALL_TO_TRIP;
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
			float effectiveMinHeightToTrip = props.RubberBones ? props.minHeightToTrip * 1.1f : props.minHeightToTrip;
			float effectiveMinHeightToDie = props.RubberBones ? props.minHeightToDie * 1.2f : props.minHeightToDie;
			
			if (effectiveMinHeightToTrip <= num && num < effectiveMinHeightToDie)
			{
				SoundManager.PlaySound(36);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
		}
		return false;
	}
}
