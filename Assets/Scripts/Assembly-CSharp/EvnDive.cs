public class EvnDive : IEvent
{
	public EvnDive()
	{
		code = EventCode.EVN_DIVE;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		float num = sm.lastYPosition - sm.playerT.position.y;
		return num > 20f;
	}
}
