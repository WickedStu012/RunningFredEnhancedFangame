using System;
using UnityEngine;

public class ServerDateBackEnd : MonoBehaviour
{
	public static ServerDateBackEnd Instance;

	private ServerDateBackEndRes cb;

	private bool waitingServerDate;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		Instance = this;
	}

	private void Update()
	{
		if (waitingServerDate)
		{
			CmdGetServerDate.Update();
		}
	}

	public void GetServerDate(ServerDateBackEndRes onGetServerDate)
	{
		cb = onGetServerDate;
		waitingServerDate = true;
		CmdGetServerDate.GetDate(onGetServerDateRes);
	}

	private void onGetServerDateRes(bool res, string str)
	{
		try
		{
			if (res)
			{
				int year = 0;
				int month = 0;
				int day = 0;
				if (str.Length == 8)
				{
					year = int.Parse(str.Substring(0, 4));
					month = int.Parse(str.Substring(4, 2));
					day = int.Parse(str.Substring(6, 2));
				}
				DateTime date = new DateTime(year, month, day);
				if (cb != null)
				{
					cb(date);
				}
			}
			else if (cb != null)
			{
				cb(DateTime.MinValue);
			}
		}
		catch
		{
			if (cb != null)
			{
				cb(DateTime.MinValue);
			}
		}
		waitingServerDate = false;
	}
}
