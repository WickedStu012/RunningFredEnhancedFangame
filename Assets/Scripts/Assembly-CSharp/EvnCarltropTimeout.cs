using UnityEngine;

public class EvnCarltropTimeout : IEvent
{
	private float accumTime;

	private CharProps props;

	public EvnCarltropTimeout()
	{
		code = EventCode.EVN_CARLTROP_TIMEOUT;
	}

	public override void StateChange()
	{
		accumTime = 0f;
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		accumTime += Time.deltaTime;
		return accumTime >= props.CarltropPenaltyTime;
	}
}
