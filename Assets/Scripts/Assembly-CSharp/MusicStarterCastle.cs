using UnityEngine;

public class MusicStarterCastle : MonoBehaviour
{
	private bool musicStarted;
	private bool cinematicActive;

	private void Start()
	{
		musicStarted = false;
		cinematicActive = SceneParamsManager.Instance.GetBool("LaunchCinematic", false);
		
		if (cinematicActive)
		{
			// Listen for cinematic end event
			GameEventDispatcher.AddListener("OnEndCinematic", OnCinematicEnd);
		}
		else
		{
			// No cinematic, start music immediately
			StartMusic();
		}
	}

	private void Update()
	{
		if (!musicStarted && !cinematicActive)
		{
			StartMusic();
		}
	}

	private void StartMusic()
	{
		if (!musicStarted)
		{
			SoundManager.PlayMusic(1000, 1001);
			musicStarted = true;
		}
	}

	private void OnCinematicEnd(object sender, GameEvent evt)
	{
		cinematicActive = false;
		GameEventDispatcher.RemoveListener("OnEndCinematic", OnCinematicEnd);
		StartMusic();
	}

	private void OnDisable()
	{
		GameEventDispatcher.RemoveListener("OnEndCinematic", OnCinematicEnd);
	}
}
