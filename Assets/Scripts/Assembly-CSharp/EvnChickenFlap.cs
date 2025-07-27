public class EvnChickenFlap : IEvent
{
	private CharProps props;

	public EvnChickenFlap()
	{
		code = EventCode.EVN_CHICKEN_FLAP;
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		if (props.HasWings)
		{
			return false;
		}
		return InputManager.GetJumpDown();
	}
}
