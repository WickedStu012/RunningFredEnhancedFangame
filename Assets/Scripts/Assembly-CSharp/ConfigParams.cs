public class ConfigParams
{
	public static bool DeactivateGore = false;

	public static bool useGameMusic = true;

	public static float masterVolume = 1f;

	public static float backgroundVolume = 1f;

	public static float musicVolume = 0f;

	public static float fxVolume = 1f;

	public static bool useGore = true;

	public static bool useLeaderboardAndAchievements = true;

	public static bool useICloud = true;

	public static string decSep = ",";

	public static bool fullScreen = false;

	public static bool IsDemoVersion = false;

	public static bool showGameDataWrongVersionDialog = false;

	public static int IronFredGrimmyGoal = 20;

	public static bool useZeemote = true;

	public static bool useiCADE = false;

	public static SkiingFredInfo skiingFredInfo = new SkiingFredInfo();

	public static bool skiingFredIsAvailable = true;

	public static bool zeemoteConnected = false;

	public static bool isKindle = false;

	public static void SaveSettings()
	{
		PlayerPrefsWrapper.SetMusicVolume(musicVolume);
		PlayerPrefsWrapper.SetFXVolume(fxVolume);
		PlayerPrefsWrapper.SetGore(useGore);
		PlayerPrefsWrapper.SetGameCenter(useLeaderboardAndAchievements);
		PlayerPrefsWrapper.SetICloud(useICloud);
		PlayerPrefsWrapper.SetSkiingFredAvailable(skiingFredIsAvailable);
	}

	public static void LoadSettings()
	{
		musicVolume = PlayerPrefsWrapper.GetMusicVolume();
		fxVolume = PlayerPrefsWrapper.GetFXVolume();
		useGore = PlayerPrefsWrapper.GetGore(useGore);
		useLeaderboardAndAchievements = PlayerPrefsWrapper.GetGameCenter(useLeaderboardAndAchievements);
		useICloud = PlayerPrefsWrapper.GetICloud(useICloud);
		skiingFredIsAvailable = PlayerPrefsWrapper.GetSkiingFredAvailable();
		if (DeactivateGore)
		{
			useGore = false;
		}
	}

	public static bool IsKongregate()
	{
		return false;
	}
}
