using UnityEngine;

public class EvnDragTimeout : IEvent
{
	private float accumTime;

	private float maxTime;

	public EvnDragTimeout()
	{
		code = EventCode.EVN_DRAG_TIMEOUT;
	}

	public override void StateChange()
	{
		accumTime = 0f;
	}

	public override bool Check()
	{
		if (maxTime == 0f)
		{
			maxTime = CharHelper.GetProps().DragMaxTime;
		}
		accumTime += Time.deltaTime;
		return accumTime > maxTime;
	}
}
