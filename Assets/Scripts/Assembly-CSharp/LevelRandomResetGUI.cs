using UnityEngine;

public class LevelRandomResetGUI : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Reset", GUILayout.Width(200f)))
		{
			GameManager.ResetGameForEndless(true);
		}
	}
}
