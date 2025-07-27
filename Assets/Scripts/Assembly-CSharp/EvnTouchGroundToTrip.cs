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
			if (!props.RubberBones && props.minHeightToTrip <= num && num < props.minHeightToDie)
			{
				SoundManager.PlaySound(36);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
		}
		return false;
	}
}
