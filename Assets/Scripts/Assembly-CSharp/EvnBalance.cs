public class EvnBalance : IEvent
{
	public EvnBalance()
	{
		code = EventCode.EVN_BALANCE;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		return sm.floorType == FloorType.BALANCE_BEAM;
	}
}
