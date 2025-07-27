using UnityEngine;

public class ChromeUserName : MonoBehaviour
{
	private static string username = string.Empty;

	private void Start()
	{
		if (Application.platform == RuntimePlatform.NaCl)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(this);
		}
	}

	public void SetUserName(string user)
	{
		username = user;
	}

	public static string GetUserName()
	{
		Debug.Log(string.Format("ChromeUserName. GetUserName: {0}", username));
		return username;
	}
}
