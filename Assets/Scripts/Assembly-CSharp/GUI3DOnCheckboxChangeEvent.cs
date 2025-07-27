using UnityEngine;

public class GUI3DOnCheckboxChangeEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnCheckboxChangeEvent";

	public bool Checked;

	public GUI3DOnCheckboxChangeEvent()
		: base(null, "GUI3DOnCheckboxChangeEvent")
	{
	}

	public GUI3DOnCheckboxChangeEvent(Object target)
		: base(target, "GUI3DOnCheckboxChangeEvent")
	{
	}
}
