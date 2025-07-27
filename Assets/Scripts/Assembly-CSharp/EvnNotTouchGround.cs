public class EvnNotTouchGround : IEvent
{
	public EvnNotTouchGround()
	{
		code = EventCode.EVN_NOT_TOUCH_GROUND;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		return !sm.IsGrounded;
	}
}
