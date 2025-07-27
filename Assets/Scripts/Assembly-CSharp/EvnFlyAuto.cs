public class EvnFlyAuto : IEvent
{
	private CharProps props;

	public EvnFlyAuto()
	{
		code = EventCode.EVN_FLY_AUTO;
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		float num = sm.lastYPosition - sm.playerT.position.y;
		return num > 4f && props.HasWings;
	}
}
