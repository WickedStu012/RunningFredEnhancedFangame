using UnityEngine;

public class GUI3DOnRollOverEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnRollOver";

	public GUI3DOnRollOverEvent()
		: base(null, "GUI3DOnRollOver")
	{
	}

	public GUI3DOnRollOverEvent(Object target)
		: base(target, "GUI3DOnRollOver")
	{
	}
}
