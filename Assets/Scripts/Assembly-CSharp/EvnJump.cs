public class EvnJump : IEvent
{
	public EvnJump()
	{
		code = EventCode.EVN_JUMP;
	}

	public override bool Check()
	{
		return InputManager.GetJumpDown();
	}
}
