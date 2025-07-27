using System;
using System.Collections;
using UnityEngine;

public class BlackLordGetInfo : MonoBehaviour
{
	public static BlackLordGetInfo Instance;

	private bool waitingResponse;

	private SkiingFredInfoRes cb;

	private DateTime lastQuerySkiingFredInfoTimeStamp = DateTime.MinValue;

	private SkiingFredInfo lastQuerySkiingFredInfoResponse;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		Instance = this;
	}

	private void OnDisable()
	{
		Instance = null;
	}

	private void Update()
	{
		if (waitingResponse)
		{
			CmdGetInfo.Update();
		}
	}

	public void GetSkiingFredInfo(SkiingFredInfoRes ber)
	{
		if ((DateTime.Now - lastQuerySkiingFredInfoTimeStamp).TotalHours >= 24.0 || lastQuerySkiingFredInfoResponse == null)
		{
			waitingResponse = true;
			cb = ber;
			CmdGetInfo.GetInfo(onGetInfoRes);
		}
		else
		{
			ber(true, lastQuerySkiingFredInfoResponse);
		}
	}

	private void onGetInfoRes(bool res, string str)
	{
		waitingResponse = false;
		if (res)
		{
			Hashtable hashtable = MiniJSON.jsonDecode(str) as Hashtable;
			Debug.Log(string.Format("onGetInfoRes: available: {0}", hashtable["Available"]));
			lastQuerySkiingFredInfoTimeStamp = DateTime.Now;
			lastQuerySkiingFredInfoResponse = new SkiingFredInfo(hashtable);
			if (cb != null)
			{
				cb(true, lastQuerySkiingFredInfoResponse);
			}
		}
		else
		{
			lastQuerySkiingFredInfoResponse = null;
			if (cb != null)
			{
				cb(false, null);
			}
		}
	}
}
