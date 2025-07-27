public class EvnBalanceEnd : IEvent
{
	public EvnBalanceEnd()
	{
		code = EventCode.EVN_BALANCE_END;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		return sm.floorType != FloorType.BALANCE_BEAM;
	}
}
