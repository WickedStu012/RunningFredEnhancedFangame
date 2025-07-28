public class EvnTouchGroundToRoll : IEvent
{
	private CharProps props;

	public EvnTouchGroundToRoll()
	{
		code = EventCode.EVN_ROLL;
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
			float effectiveMinHeightToRoll = props.RubberBones ? props.minHeightToRoll * 1.1f : props.minHeightToRoll;
			float effectiveMinHeightToTrip = props.RubberBones ? props.minHeightToTrip * 1.1f : props.minHeightToTrip;
			float effectiveMinHeightToExplode = props.RubberBones ? props.minHeightToExplode * 1.2f : props.minHeightToExplode;
			
			if (effectiveMinHeightToRoll <= num && num < effectiveMinHeightToTrip)
			{
				SoundManager.PlaySound(36);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
			else if (props.RubberBones && effectiveMinHeightToTrip <= num && num < effectiveMinHeightToExplode)
			{
				// With RubberBones, higher falls still cause rolling instead of tripping
				SoundManager.PlaySound(36);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
		}
		return false;
	}
}
