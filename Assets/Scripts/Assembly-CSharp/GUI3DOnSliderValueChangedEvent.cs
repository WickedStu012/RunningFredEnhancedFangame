using UnityEngine;

public class GUI3DOnSliderValueChangedEvent : GUI3DEvent
{
	public const string EventName = "GUI3DOnSliderValueChanged";

	public float Value;

	public GUI3DOnSliderValueChangedEvent()
		: base(null, "GUI3DOnSliderValueChanged")
	{
	}

	public GUI3DOnSliderValueChangedEvent(Object target, float sliderValue)
		: base(target, "GUI3DOnSliderValueChanged")
	{
		Value = sliderValue;
	}
}
