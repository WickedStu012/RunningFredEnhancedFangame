using UnityEngine;

public class GUI3DOnPressEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnPress";

	public Vector3 Position;

	public GUI3DOnPressEvent()
		: base(null, "GUI3DOnPress")
	{
	}

	public GUI3DOnPressEvent(Object target, Vector3 position)
		: base(target, "GUI3DOnPress")
	{
		Position = position;
	}
}
