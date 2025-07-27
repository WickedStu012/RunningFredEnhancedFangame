using UnityEngine;

public class MusicStarterChallenge : MonoBehaviour
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
			SoundManager.PlayMusic(1000);
			musicStarted = true;
		}
	}
}
