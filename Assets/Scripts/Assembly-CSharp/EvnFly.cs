public class EvnFly : IEvent
{
	private CharProps props;

	public EvnFly()
	{
		code = EventCode.EVN_FLY;
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		if (props.HasWings && InputManager.GetDuck())
		{
			return true;
		}
		return false;
	}
}
