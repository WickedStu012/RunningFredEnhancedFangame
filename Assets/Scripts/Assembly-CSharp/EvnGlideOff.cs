public class EvnGlideOff : IEvent
{
	public EvnGlideOff()
	{
		code = EventCode.EVN_GLIDE_OFF;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		return !InputManager.GetJump();
	}
}
