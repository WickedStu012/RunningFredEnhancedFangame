using System;
using UnityEngine;

public class TapjoyPlacementsManager
{
	private static float OriginalFxVolume;

	private static float OriginalMusicVolume;

	private static bool IsFirstTime = true;

	private static bool restoreSoundVolume;

	public static event Action callbackOnDeactiveProcessing;

	public static void init()
	{
		if (IsFirstTime)
		{
			BeLordTapJoy.callbackOnConnectSuccess += OnConnected;
		}
	}

	public static void PlacementLoadAndShow(string placementName, bool popupProcessingEnable = true)
	{
		AcivateProcessing(popupProcessingEnable);
		BeLordTapJoy.Instance.PlacementLoadAndShow(placementName);
	}

	public static void ShowOffers()
	{
		AcivateProcessing();
		BeLordTapJoy.Instance.ShowOffers();
	}

	private static void OnResume()
	{
		if (BeLordTapJoy.DebugMode)
		{
			Debug.Log("TapjoyPlacementsManager.OnResume() is Processing: " + (GUI3DPopupManager.Instance.CurrentPopupName() == "Processing"));
		}
		DeactivateProcessing();
	}

	private static void AcivateProcessing(bool popupProcessingEnable = true)
	{
		if (popupProcessingEnable && GUI3DPopupManager.Instance != null)
		{
			GUI3DPopupManager.Instance.ShowPopup("Processing");
		}
		MuteSound();
		BeLordTapJoy.callbackResume += OnResume;
	}

	private static void OnSoundInit()
	{
		SoundManager.callbackOnInitialized -= OnSoundInit;
		MuteSound();
	}

	private static void MuteSound()
	{
		if (!restoreSoundVolume && SoundManager.initialized)
		{
			OriginalFxVolume = SoundManager.FxVolume;
			OriginalMusicVolume = SoundManager.MusicVolume;
			SoundManager.FxVolume = 0f;
			SoundManager.MusicVolume = 0f;
			restoreSoundVolume = true;
		}
		else if (!SoundManager.initialized)
		{
			SoundManager.callbackOnInitialized += OnSoundInit;
		}
	}

	private static void UnmuteSound()
	{
		if (restoreSoundVolume && SoundManager.initialized)
		{
			SoundManager.FxVolume = OriginalFxVolume;
			SoundManager.MusicVolume = OriginalMusicVolume;
			restoreSoundVolume = false;
		}
		SoundManager.callbackOnInitialized -= OnSoundInit;
	}

	private static void DeactivateProcessing()
	{
		BeLordTapJoy.callbackResume -= OnResume;
		if (GUI3DPopupManager.Instance.CurrentPopupName() == "Processing")
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.No);
		}
		if (TapjoyPlacementsManager.callbackOnDeactiveProcessing != null)
		{
			TapjoyPlacementsManager.callbackOnDeactiveProcessing();
		}
		UnmuteSound();
	}

	private static void OnConnected()
	{
		BeLordTapJoy.callbackOnConnectSuccess -= OnConnected;
		if (IsFirstTime)
		{
			IsFirstTime = false;
			if (PlayerPrefs.GetInt("TapJoyFirstTime", 0) == 0)
			{
				PlayerPrefs.SetInt("TapJoyFirstTime", 1);
			}
			else
			{
				PlacementLoadAndShow("app_launch_2", false);
			}
		}
	}
}
