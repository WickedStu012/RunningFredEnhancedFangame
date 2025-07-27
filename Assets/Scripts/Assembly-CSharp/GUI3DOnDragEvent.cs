using UnityEngine;

public class GUI3DOnDragEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnDrag";

	public Vector3 RelativePosition;

	public bool Cancelled;

	public GUI3DOnDragEvent()
		: base(null, "GUI3DOnDrag")
	{
		Cancelled = false;
	}

	public GUI3DOnDragEvent(Object target, Vector3 relativePosition)
		: base(target, "GUI3DOnDrag")
	{
		Cancelled = false;
		RelativePosition = relativePosition;
	}

	public GUI3DOnDragEvent(Object target, Vector3 relativePosition, bool cancelled)
		: base(target, "GUI3DOnDrag")
	{
		Cancelled = cancelled;
		RelativePosition = relativePosition;
	}
}
