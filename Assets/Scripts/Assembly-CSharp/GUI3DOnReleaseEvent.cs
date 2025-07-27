using UnityEngine;

public class GUI3DOnReleaseEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnRelease";

	public GUI3DOnReleaseEvent()
		: base(null, "GUI3DOnRelease")
	{
	}

	public GUI3DOnReleaseEvent(Object target)
		: base(target, "GUI3DOnRelease")
	{
	}
}
