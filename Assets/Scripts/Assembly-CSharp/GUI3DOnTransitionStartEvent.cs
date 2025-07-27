using UnityEngine;

public class GUI3DOnTransitionStartEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnTransitionStartEvent";

	public GUI3DOnTransitionStartEvent()
		: base(null, "GUI3DOnTransitionStartEvent")
	{
	}

	public GUI3DOnTransitionStartEvent(Object target)
		: base(target, "GUI3DOnTransitionStartEvent")
	{
	}
}
