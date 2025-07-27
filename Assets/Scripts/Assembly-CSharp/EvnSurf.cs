using UnityEngine;

public class EvnSurf : IEvent
{
	private CharProps props;

	public EvnSurf()
	{
		code = EventCode.EVN_SURF;
	}

	public override bool Check()
	{
		if (props == null)
		{
			GameObject player = CharHelper.GetPlayer();
			if (player == null)
			{
				return false;
			}
			props = CharHelper.GetProps();
			if (props == null)
			{
				return false;
			}
		}
		return sm.IsGrounded && sm.FloorXAngle > props.MinAngleToSurf;
	}
}
