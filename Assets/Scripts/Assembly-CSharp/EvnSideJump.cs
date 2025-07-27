public class EvnSideJump : IEvent
{
	public EvnSideJump()
	{
		code = EventCode.EVN_SIDE_JUMP;
	}

	public override bool Check()
	{
		return InputManager.GetJumpDown();
	}
}
