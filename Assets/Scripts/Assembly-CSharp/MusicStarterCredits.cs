using UnityEngine;

public class MusicStarterCredits : MonoBehaviour
{
	private bool musicStarted;

	private void Start()
	{
		musicStarted = false;
	}

	private void Update()
	{
		if (!musicStarted)
		{
			SoundManager.PlayMusic(1);
			musicStarted = true;
		}
	}
}
