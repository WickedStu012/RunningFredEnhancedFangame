using UnityEngine;

public class MusicStarterMoreGames : MonoBehaviour
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
			SoundManager.PlayMusic(0, 1);
			musicStarted = true;
		}
	}
}
