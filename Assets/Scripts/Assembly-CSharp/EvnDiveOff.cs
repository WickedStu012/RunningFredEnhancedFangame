public class EvnDiveOff : IEvent
{
	public EvnDiveOff()
	{
		code = EventCode.EVN_DIVE_OFF;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		return !InputManager.GetDuck();
	}
}
