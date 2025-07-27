using UnityEngine;

public class iPodMusicCheckbox : MonoBehaviour
{
	private GUI3DCheckbox checkbox;

	private void Awake()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = !ConfigParams.useGameMusic;
	}

	private void OnEnable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.StartCheckStatus = !ConfigParams.useGameMusic;
		checkbox.CheckboxChangeEvent += OnChange;
	}

	private void OnDisable()
	{
		if (checkbox == null)
		{
			checkbox = GetComponent<GUI3DCheckbox>();
		}
		checkbox.CheckboxChangeEvent -= OnChange;
	}

	private void OnChange(GUI3DOnCheckboxChangeEvent evt)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer && ConfigParams.useGameMusic != !evt.Checked)
		{
			ConfigParams.useGameMusic = !evt.Checked;
			if (ConfigParams.useGameMusic)
			{
				AddMusic.PlayOrPause();
				SoundManager.PlayMusic(0, 1);
			}
			else
			{
				SoundManager.StopMusic();
				AddMusic.ShowMusicSelector();
				AddMusic.PlayOrPause();
			}
		}
	}
}
