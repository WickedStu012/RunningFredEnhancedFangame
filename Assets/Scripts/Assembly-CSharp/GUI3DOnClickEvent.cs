using UnityEngine;

public class GUI3DOnClickEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnClick";

	public Vector3 Position;

	public GUI3DOnClickEvent()
		: base(null, "GUI3DOnClick")
	{
	}

	public GUI3DOnClickEvent(Object target, Vector3 position)
		: base(target, "GUI3DOnClick")
	{
		Position = position;
	}
}
