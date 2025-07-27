using UnityEngine;

public class EvnSurfSideOff : IEvent
{
	private CharProps props;

	public EvnSurfSideOff()
	{
		code = EventCode.EVN_SURF_SIDE_OFF;
	}

	public override bool Check()
	{
		return sm.IsGrounded && Mathf.Abs(sm.FloorZAngle) < 10f;
	}
}
