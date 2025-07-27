using UnityEngine;

public class GUI3DOnTabControlChangeEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnTabControlChangeEvent";

	public GUI3DTab ActiveTab;

	public GUI3DOnTabControlChangeEvent()
		: base(null, "GUI3DOnTabControlChangeEvent")
	{
	}

	public GUI3DOnTabControlChangeEvent(Object target)
		: base(target, "GUI3DOnTabControlChangeEvent")
	{
	}
}
