using UnityEngine;

public class FXVolumeSlider : MonoBehaviour
{
	public SndIdMenu Click = SndIdMenu.SND_MENU_CLICK;

	private GUI3DSlider slider;

	private void Start()
	{
		slider.Progress = (int)(ConfigParams.fxVolume * 100f);
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
		SoundManager.FxVolume = evt.Value / 100f;
		SoundManager.PlaySound(Click);
	}
}
