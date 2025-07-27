using UnityEngine;

public class EvnDragOut : IEvent
{
	private float accumTime;

	private bool draggingEnd;

	private float animLength;

	private float accumTimeMinDragTime;

	private float dragMinTime;

	public EvnDragOut()
	{
		code = EventCode.EVN_DRAG_OUT;
		draggingEnd = false;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		if (animLength == 0f)
		{
			animLength = 0f;
		}
		if (dragMinTime == 0f)
		{
			dragMinTime = CharHelper.GetProps().DragMinTime;
		}
		if (!draggingEnd)
		{
			accumTimeMinDragTime += Time.deltaTime;
			if (!InputManager.GetDuck() && accumTimeMinDragTime >= dragMinTime)
			{
				draggingEnd = true;
				return false;
			}
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > animLength)
			{
				reset();
				return true;
			}
		}
		return false;
	}

	private void reset()
	{
		accumTimeMinDragTime = 0f;
		accumTime = 0f;
		draggingEnd = false;
	}
}
