public class EvnJetpackOff : IEvent
{
	public EvnJetpackOff()
	{
		code = EventCode.EVN_JETPACK_OFF;
	}

	public override bool Check()
	{
		if (!InputManager.GetJump())
		{
			return true;
		}
		return false;
	}
}
