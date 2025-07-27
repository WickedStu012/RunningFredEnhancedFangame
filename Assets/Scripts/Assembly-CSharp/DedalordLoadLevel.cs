using UnityEngine;

public class DedalordLoadLevel : MonoBehaviour
{
	private string load = string.Empty;

	private string scene = string.Empty;

	private static DedalordLoadLevel instance;

	private bool loading;

	private int count;

	private AsyncOperation oper;

	private bool fading;

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		scene = Application.loadedLevelName;
	}

	private void OnEnable()
	{
		instance = this;
	}

	public static void LoadSync()
	{
		instance.load = Levels.MainMenu;
		instance.loading = true;
		Application.LoadLevel("Syncronizing");
		instance.scene = "Syncronizing";
		instance.oper = null;
		instance.enabled = true;
		instance.fading = false;
	}

	public static void LoadLevel(string level)
	{
		instance.load = level;
		instance.loading = true;
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			allCameras[i].enabled = false;
		}
		GUI3DGlobalParameters.Instance.ClearTextures();
		GameEventDispatcher.ClearSceneListeners();
		if (level == Levels.MainMenu || level == "Credits")
		{
			Application.LoadLevel("EmptyProxySceneForMenu");
			instance.scene = "EmptyProxySceneForMenu";
		}
		else
		{
			Application.LoadLevel("EmptyProxyScene");
			instance.scene = "EmptyProxyScene";
		}
		instance.oper = null;
		instance.enabled = true;
		instance.fading = false;
		instance.count = 0;
	}

	public static string GetCurrentScene()
	{
		if (instance != null)
		{
			return instance.scene;
		}
		return string.Empty;
	}

	public static string GetLevel()
	{
		if (instance != null)
		{
			return instance.load;
		}
		return string.Empty;
	}

	private void OnFadeDone()
	{
		instance.scene = load;
		if (string.Compare(Application.loadedLevelName, "Syncronizing", true) == 0)
		{
			oper = Application.LoadLevelAsync(load);
		}
		else
		{
			oper = Application.LoadLevelAdditiveAsync(load);
		}
	}

	private void FixedUpdate()
	{
		if (!loading)
		{
			return;
		}
		if (oper == null)
		{
			if (!fading)
			{
				if (count >= 1)
				{
					CameraFade.Instance.FadeIn(0.35f, instance.OnFadeDone);
					fading = true;
					count = 0;
				}
				count++;
			}
		}
		else if (oper.isDone)
		{
			if (load != "CharCreator" && load != "TutorialLoader")
			{
				CameraFade.Instance.FadeIn(0.35f);
			}
			loading = false;
			instance.enabled = false;
		}
	}
}
