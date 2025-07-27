using UnityEngine;

public class DebugLevelReloader : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 180, 80f, 180f, 80f), "Reset"))
		{
			SoundManager.StopAll();
			DedalordLoadLevel.LoadLevel(Application.loadedLevelName);
		}
	}
}
