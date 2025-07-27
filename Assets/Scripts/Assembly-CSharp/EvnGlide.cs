public class EvnGlide : IEvent
{
	public EvnGlide()
	{
		code = EventCode.EVN_GLIDE;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		return false;
	}
}
