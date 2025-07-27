using UnityEngine;

public class MusicVolumeSlider : MonoBehaviour
{
	private GUI3DSlider slider;

	private void Start()
	{
		slider.Progress = (int)(ConfigParams.musicVolume * 100f);
	}

	private void OnEnable()
	{
		if (slider == null)
		{
			slider = GetComponent<GUI3DSlider>();
		}
		slider.SliderValueChangedEvent += OnSliderChange;
	}

	private void OnDisable()
	{
		if (slider == null)
		{
			slider = GetComponent<GUI3DSlider>();
		}
		slider.SliderValueChangedEvent -= OnSliderChange;
	}

	private void OnSliderChange(GUI3DOnSliderValueChangedEvent evt)
	{
		SoundManager.MusicVolume = evt.Value / 100f;
	}
}
