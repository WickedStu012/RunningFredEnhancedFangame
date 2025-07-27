public class EvnClimbJump : IEvent
{
	public EvnClimbJump()
	{
		code = EventCode.EVN_CLIMB_JUMP;
	}

	public override bool Check()
	{
		return InputManager.GetJumpDown();
	}
}
