using UnityEngine;

public class EvnSurfOff : IEvent
{
	private CharProps props;

	public EvnSurfOff()
	{
		code = EventCode.EVN_SURF_OFF;
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
		return !sm.IsGrounded || sm.FloorXAngle <= props.MinAngleToSurf;
	}
}
