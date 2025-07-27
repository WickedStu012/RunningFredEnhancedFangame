using System;
using UnityEngine;

public class DailyOfferBackEnd : MonoBehaviour
{
	private enum CurCmd
	{
		NONE = 0,
		GET_TOKEN = 1,
		GET_OFFER = 2
	}

	private enum NextCmd
	{
		GET_OFFER = 0
	}

	public static DailyOfferBackEnd Instance;

	private CurCmd curCmd;

	private NextCmd nextCmd;

	private string token;

	private DailyOfferBackEndRes cb;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		Instance = this;
	}

	private void Start()
	{
		curCmd = CurCmd.NONE;
	}

	private void Update()
	{
		switch (curCmd)
		{
		case CurCmd.GET_TOKEN:
			CmdGetTokenDO.Update();
			break;
		case CurCmd.GET_OFFER:
			CmdGetOfferDO.Update();
			break;
		}
	}

	private void getToken()
	{
		curCmd = CurCmd.GET_TOKEN;
		CmdGetTokenDO.GetToken(getTokenRes);
	}

	public void GetOffer(DailyOfferBackEndRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.GET_OFFER;
		getToken();
	}

	private void getTokenRes(bool res, string str)
	{
		curCmd = CurCmd.NONE;
		if (res)
		{
			token = str;
			if (nextCmd == NextCmd.GET_OFFER)
			{
				curCmd = CurCmd.GET_OFFER;
				CmdGetOfferDO.GetOffer(token, onGetOfferRes);
			}
		}
		else if (cb != null)
		{
			cb(false, DateTime.MinValue, 0, -2);
		}
	}

	private void onGetOfferRes(bool res, string str)
	{
		try
		{
			if (res)
			{
				string[] array = str.Split(';');
				if (array != null && array.Length == 3)
				{
					int year = 0;
					int month = 0;
					int day = 0;
					if (array[0].Length == 8)
					{
						year = int.Parse(array[0].Substring(0, 4));
						month = int.Parse(array[0].Substring(4, 2));
						day = int.Parse(array[0].Substring(6, 2));
					}
					DateTime date = new DateTime(year, month, day);
					int discount = int.Parse(array[1]);
					int itemId = int.Parse(array[2]);
					if (cb != null)
					{
						cb(true, date, discount, itemId);
					}
				}
				else if (cb != null)
				{
					cb(true, DateTime.MinValue, 0, -2);
				}
			}
			else if (cb != null)
			{
				cb(false, DateTime.MinValue, 0, -2);
			}
		}
		catch
		{
			if (cb != null)
			{
				cb(false, DateTime.MinValue, 0, -2);
			}
		}
	}
}
