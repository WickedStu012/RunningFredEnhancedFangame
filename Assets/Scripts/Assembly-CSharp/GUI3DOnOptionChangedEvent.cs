using UnityEngine;

public class GUI3DOnOptionChangedEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnOptionChanged";

	public GUI3DCheckbox ActiveCheckbox;

	public GUI3DOnOptionChangedEvent()
		: base(null, "GUI3DOnOptionChanged")
	{
	}

	public GUI3DOnOptionChangedEvent(Object target, GUI3DCheckbox activeCheckbox)
		: base(target, "GUI3DOnOptionChanged")
	{
		ActiveCheckbox = activeCheckbox;
	}
}
