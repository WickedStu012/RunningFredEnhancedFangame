using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
	private enum Cmd
	{
		NONE = 0,
		GET_SCHEDULE = 1,
		AD_WAS_SHOWED = 2,
		AD_WAS_TAPPED = 3
	}

	private const int adScheduleLifeTimeInSeconds = 86400;

	public static AdManager Instance;

	public Material AdMaterial;

	private AdInfo CurrentAd;

	private List<AdInfo> adSchedule;

	private bool waitingResponse;

	private DateTime lastQueryGetAdScheduleTimeStamp = DateTime.MinValue;

	private AdManagerRes cb;

	private int lastAdIdxReturned;

	private Cmd curCmd;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		curCmd = Cmd.NONE;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (waitingResponse)
		{
			switch (curCmd)
			{
			case Cmd.GET_SCHEDULE:
				CmdGetAd.Update();
				break;
			case Cmd.AD_WAS_SHOWED:
				CmdAdShowed.Update();
				break;
			case Cmd.AD_WAS_TAPPED:
				CmdAdTapped.Update();
				break;
			}
		}
	}

	public void GetAdScheduleFromWeb(AdManagerRes ber)
	{
		waitingResponse = true;
		curCmd = Cmd.GET_SCHEDULE;
		cb = ber;
		CmdGetAd.GetAd(onGetAdScheduleRes);
	}

	public void AdWasShowed()
	{
		AdInfo currentAd = GetCurrentAd();
		if (currentAd != null)
		{
			int id = currentAd.Id;
			waitingResponse = true;
			curCmd = Cmd.AD_WAS_SHOWED;
			CmdAdShowed.AdWasShowed(id, onAdWasShowedOrTappedRes);
		}
	}

	public void AdWasTapped()
	{
		AdInfo currentAd = GetCurrentAd();
		if (currentAd != null)
		{
			int id = currentAd.Id;
			waitingResponse = true;
			curCmd = Cmd.AD_WAS_TAPPED;
			CmdAdTapped.AdWasTapped(id, onAdWasShowedOrTappedRes);
		}
	}

	public bool IsScheduleCached()
	{
		if (LoadFromLocal())
		{
			return (DateTime.Now - lastQueryGetAdScheduleTimeStamp).TotalSeconds < 86400.0 && adSchedule != null;
		}
		return false;
	}

	public List<AdInfo> GetCachedSchedule()
	{
		return adSchedule;
	}

	private void LoadFromString(string jsonRes)
	{
		LoadFromString(jsonRes, true);
	}

	private void LoadFromString(string jsonRes, bool saveOnLocal)
	{
		if (jsonRes != null && jsonRes.Length != 0)
		{
			adSchedule = new List<AdInfo>();
			ArrayList arrayList = MiniJSON.jsonDecode(jsonRes) as ArrayList;
			for (int i = 0; i < arrayList.Count; i++)
			{
				adSchedule.Add(new AdInfo(arrayList[i] as Hashtable));
			}
			setProbabilities();
			if (saveOnLocal)
			{
				SaveOnLocal(jsonRes);
			}
		}
	}

	private void setProbabilities()
	{
		if (adSchedule.Count > 0)
		{
			double num = adSchedule[0].Probability;
			adSchedule[0].ProbabilityAccum = num;
			for (int i = 1; i < adSchedule.Count; i++)
			{
				num += adSchedule[i].Probability;
				adSchedule[i].ProbabilityAccum = num;
			}
		}
	}

	public AdInfo GetAdFromSchedule()
	{
		if (adSchedule == null)
		{
			return null;
		}
		if (adSchedule.Count > 0)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			for (int i = 0; i < adSchedule.Count; i++)
			{
				if ((double)num < adSchedule[i].ProbabilityAccum)
				{
					SaveReturnedIndex(i);
					CurrentAd = adSchedule[i];
					return CurrentAd;
				}
			}
			int idx = -1;
			SaveReturnedIndex(idx);
			return null;
		}
		return null;
	}

	private void SaveOnLocal(string jsonRes)
	{
		string value = DateUtil.ConvertToStringWithSeconds(DateTime.Now);
		PlayerPrefs.SetString("AdCacheTimeStamp", value);
		PlayerPrefs.SetString("AdCache", jsonRes);
	}

	private void SaveReturnedIndex(int idx)
	{
		lastAdIdxReturned = idx;
		PlayerPrefs.SetInt("AdCacheReturnedIndex", idx);
	}

	private bool LoadFromLocal()
	{
		string text = PlayerPrefs.GetString("AdCacheTimeStamp", null);
		lastAdIdxReturned = PlayerPrefs.GetInt("AdCacheReturnedIndex", 0);
		if (text != null)
		{
			lastQueryGetAdScheduleTimeStamp = DateUtil.ConvertToDateTime(text);
			string text2 = PlayerPrefs.GetString("AdCache", null);
			if (text2 != null && text2.Length > 0)
			{
				LoadFromString(text2, false);
				return true;
			}
			return false;
		}
		return false;
	}

	public AdInfo GetCurrentAd()
	{
		if (adSchedule != null && lastAdIdxReturned != -1 && lastAdIdxReturned < adSchedule.Count)
		{
			return adSchedule[lastAdIdxReturned];
		}
		return null;
	}

	private void onGetAdScheduleRes(bool res, string str)
	{
		curCmd = Cmd.NONE;
		waitingResponse = false;
		if (res)
		{
			LoadFromString(str);
			lastQueryGetAdScheduleTimeStamp = DateTime.Now;
			if (cb != null)
			{
				cb(true, null, adSchedule);
			}
		}
		else if (cb != null)
		{
			cb(false, "Get schedule from server returns with errors.", null);
		}
	}

	private void onAdWasShowedOrTappedRes(bool res, string str)
	{
		curCmd = Cmd.NONE;
		waitingResponse = false;
	}

	public void SetLoadingAdImageFlag(bool enable)
	{
		PlayerPrefs.SetInt("LoadingAdImage", enable ? 1 : 0);
	}

	public bool GetLoadingAdImageFlag()
	{
		return PlayerPrefs.GetInt("LoadingAdImage", 0) == 1;
	}

	public Material GetMaterial()
	{
		if (AdMaterial == null)
		{
			AdMaterial = new Material(Shader.Find("GUI/AlphaSelfIllum"));
		}
		return AdMaterial;
	}

	public void RemoveTextureFromMaterial()
	{
		if (AdMaterial != null)
		{
			AdMaterial.mainTexture = null;
		}
	}
}
