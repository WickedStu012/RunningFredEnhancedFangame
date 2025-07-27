public class EvnDuck : IEvent
{
	public EvnDuck()
	{
		code = EventCode.EVN_DUCK;
	}

	public override bool Check()
	{
		return sm.IsGrounded && InputManager.GetDuck();
	}
}
