using UnityEngine;

public class GUI3DOnDeactivate : GUI3DEvent
{
	public const string EventName = "GUI3DOnDeactivate";

	public GUI3DOnDeactivate()
		: base(null, "GUI3DOnDeactivate")
	{
	}

	public GUI3DOnDeactivate(Object target)
		: base(target, "GUI3DOnDeactivate")
	{
	}
}
