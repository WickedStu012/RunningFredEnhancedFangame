using UnityEngine;

public class GUI3DOnTabChangeEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnTabChangeEvent";

	public bool Active;

	public GUI3DOnTabChangeEvent()
		: base(null, "GUI3DOnTabChangeEvent")
	{
	}

	public GUI3DOnTabChangeEvent(Object target)
		: base(target, "GUI3DOnTabChangeEvent")
	{
	}
}
