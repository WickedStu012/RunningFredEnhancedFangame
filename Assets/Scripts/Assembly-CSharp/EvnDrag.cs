using UnityEngine;

public class EvnDrag : IEvent
{
	private enum DragActioState
	{
		STILL_PRESSED = 0,
		FIRST_RELEASE = 1,
		SECOND_PRESS = 2,
		SECOND_RELEASE = 3
	}

	private DragActioState dragState;

	private float accumTimePress;

	public EvnDrag()
	{
		code = EventCode.EVN_DRAG;
		dragState = DragActioState.STILL_PRESSED;
	}

	public override void StateChange()
	{
		dragState = DragActioState.STILL_PRESSED;
		accumTimePress = 0f;
	}

	public override bool Check()
	{
		switch (dragState)
		{
		case DragActioState.STILL_PRESSED:
			if (!InputManager.GetDuck())
			{
				dragState = DragActioState.FIRST_RELEASE;
			}
			break;
		case DragActioState.FIRST_RELEASE:
			accumTimePress += Time.deltaTime;
			if (accumTimePress < 0.2f)
			{
				if (InputManager.GetDuck() && sm.CanSwitchTo(ActionCode.DRAGGING))
				{
					sm.SwitchTo(ActionCode.DRAGGING);
				}
			}
			else
			{
				CharAnimManager.ForceRun();
				sm.SwitchTo(ActionCode.RUNNING);
			}
			break;
		}
		return false;
	}
}
