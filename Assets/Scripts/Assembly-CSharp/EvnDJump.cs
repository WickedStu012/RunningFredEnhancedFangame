public class EvnDJump : IEvent
{
	private enum JumpActioState
	{
		STILL_PRESSED = 0,
		FIRST_RELEASE = 1,
		SECOND_PRESS = 2,
		SECOND_RELEASE = 3
	}

	private JumpActioState jumpState;

	public EvnDJump()
	{
		code = EventCode.EVN_DJUMP;
		jumpState = JumpActioState.STILL_PRESSED;
	}

	public override void StateChange()
	{
		jumpState = JumpActioState.STILL_PRESSED;
	}

	public override bool Check()
	{
		switch (jumpState)
		{
		case JumpActioState.STILL_PRESSED:
			if (!InputManager.GetJump())
			{
				jumpState = JumpActioState.FIRST_RELEASE;
			}
			break;
		case JumpActioState.FIRST_RELEASE:
			return InputManager.GetJumpDown();
		}
		return false;
	}
}
