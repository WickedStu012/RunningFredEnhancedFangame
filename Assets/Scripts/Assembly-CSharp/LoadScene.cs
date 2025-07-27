using UnityEngine;

public class LoadScene : MonoBehaviour
{
	private const float LOAD_SCENE_TIME = 3f;

	public string loadSceneName = string.Empty;

	private float accumTime;

	private void Start()
	{
		accumTime = 0f;
	}

	private void OnGUI()
	{
		GUILayout.Label(string.Format("Will load scene in: {0}", 3f - accumTime));
		if (GUILayout.Button(string.Format("Load Scene: {0}", loadSceneName), GUILayout.MaxWidth(320f), GUILayout.MaxHeight(200f)))
		{
			Application.LoadLevel(loadSceneName);
		}
		if (GUILayout.Button(string.Format("Load MainMenu"), GUILayout.MaxWidth(320f), GUILayout.MaxHeight(200f)))
		{
			Application.LoadLevel(Levels.MainMenu);
		}
	}

	private void FixedUpdate()
	{
		accumTime += Time.deltaTime;
		if (accumTime >= 3f)
		{
			Application.LoadLevel(loadSceneName);
		}
	}
}
