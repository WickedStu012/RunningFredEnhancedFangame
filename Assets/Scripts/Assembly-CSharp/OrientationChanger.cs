using UnityEngine;

public class OrientationChanger : MonoBehaviour
{
	private const int UPDATE_COUNT_AUTOROTATION = 3;

	private static bool freeze;

	private int updateCount;

	private DeviceOrientation lastDeviceOrientation;

	public static ScreenOrientation curOrientation;

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void setKeyboardAutorotate()
	{
	}

	public static void SetFreeze(bool val)
	{
		freeze = val;
		if (freeze)
		{
			Screen.autorotateToLandscapeRight = Screen.orientation == ScreenOrientation.LandscapeRight;
			Screen.autorotateToLandscapeLeft = Screen.orientation == ScreenOrientation.LandscapeLeft;
		}
		else
		{
			Screen.autorotateToLandscapeRight = true;
			Screen.autorotateToLandscapeLeft = true;
		}
	}
}
