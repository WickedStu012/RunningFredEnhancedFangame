using UnityEngine;

public class StatsTester : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Track Event 1"))
		{
			StatsTracker.Track(StatsVars.START_GAME);
		}
	}
}
