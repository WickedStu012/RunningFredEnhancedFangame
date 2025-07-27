using UnityEngine;

public class MusicStarterRooftop : MonoBehaviour
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
			SoundManager.PlayMusic(1001);
			musicStarted = true;
		}
	}
}
