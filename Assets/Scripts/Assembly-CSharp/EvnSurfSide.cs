public class EvnSurfSide : IEvent
{
	private CharProps props;

	public EvnSurfSide()
	{
		code = EventCode.EVN_SURF_SIDE;
	}

	public override bool Check()
	{
		return sm.IsGrounded && (sm.FloorZAngle < -10f || sm.FloorZAngle > 10f);
	}
}
