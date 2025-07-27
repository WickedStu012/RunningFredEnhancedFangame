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
			if (!props.RubberBones)
			{
				if (props.minHeightToRoll <= num && num < props.minHeightToTrip)
				{
					SoundManager.PlaySound(36);
					CharHelper.GetEffects().EnableImpactGround();
					return true;
				}
			}
			else if (props.minHeightToRoll <= num && num < props.minHeightToExplode)
			{
				SoundManager.PlaySound(36);
				CharHelper.GetEffects().EnableImpactGround();
				return true;
			}
		}
		return false;
	}
}
