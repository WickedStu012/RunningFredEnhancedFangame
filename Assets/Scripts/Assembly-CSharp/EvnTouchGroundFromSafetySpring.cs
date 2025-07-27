public class EvnTouchGroundFromSafetySpring : IEvent
{
	private CharProps props;

	public EvnTouchGroundFromSafetySpring()
	{
		code = EventCode.EVN_TOUCH_GROUND;
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
			SoundManager.PlaySound(36);
			if (num > 1f)
			{
				CharHelper.GetEffects().EnableHardLanding();
			}
			return true;
		}
		return false;
	}
}
