using UnityEngine;

public class GUI3DOnActivate : GUI3DEvent
{
	public const string EventName = "GUI3DOnActivate";

	public GUI3DOnActivate()
		: base(null, "GUI3DOnActivate")
	{
	}

	public GUI3DOnActivate(Object target)
		: base(target, "GUI3DOnActivate")
	{
	}
}
