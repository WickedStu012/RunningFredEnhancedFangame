public class EvnStagger : IEvent
{
	public EvnStagger()
	{
		code = EventCode.EVN_STAGGER;
	}

	public override bool Check()
	{
		return false;
	}
}
