using UnityEngine;

public class GUI3DOnTransitionEndEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnTransitionEndEvent";

	public GUI3DOnTransitionEndEvent()
		: base(null, "GUI3DOnTransitionEndEvent")
	{
	}

	public GUI3DOnTransitionEndEvent(Object target)
		: base(target, "GUI3DOnTransitionEndEvent")
	{
	}
}
