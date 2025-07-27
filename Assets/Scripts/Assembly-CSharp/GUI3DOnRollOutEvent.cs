using UnityEngine;

public class GUI3DOnRollOutEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnRollOut";

	public GUI3DOnRollOutEvent()
		: base(null, "GUI3DOnRollOut")
	{
	}

	public GUI3DOnRollOutEvent(Object target)
		: base(target, "GUI3DOnRollOut")
	{
	}
}
