public class EvnDramaticJump : IEvent
{
	private const float DIF_HEIGHT_TO_ROLL = 6f;

	private CharProps props;

	public EvnDramaticJump()
	{
		code = EventCode.EVN_DRAMATIC_JUMP;
	}

	public override void StateChange()
	{
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
		float num = sm.lastYPosition - sm.playerT.position.y;
		return num > 4f;
	}
}
