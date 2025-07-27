using UnityEngine;

public class FullScreenChecker : MonoBehaviour
{
	private Resolution[] resolutions;

	private int fsWidth;

	private int fsHeight;

	private int currentWidth;

	private int currentHeight;

	private void Awake()
	{
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
	}

	private void changeFullScreen()
	{
	}

	public static void ChangeToFullScreen(bool val)
	{
	}
}
