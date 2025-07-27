using UnityEngine;

public class LoadMainMenu : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUILayout.Button("Load Scene", GUILayout.MaxWidth(320f), GUILayout.MaxHeight(200f)))
		{
			Application.LoadLevel(Levels.MainMenu);
		}
	}
}
