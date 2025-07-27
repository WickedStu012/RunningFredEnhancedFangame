using System.Collections;
using UnityEngine;

public class KonBackEnd : MonoBehaviour
{
	private enum Cmd
	{
		NONE = 0,
		AUTHENTICATE = 1,
		GET_ITEMS = 2,
		GET_USER_ITEMS = 3,
		USE_ITEM = 4
	}

	public const string API_KEY = "81a54a52-142b-47c2-9106-f0ccd5d1fd00";

	public const int TIMEOUT = 10;

	private Cmd curCmd;

	private OnKonCmdRes _cb;

	private void Start()
	{
		curCmd = Cmd.NONE;
	}

	private void Update()
	{
		switch (curCmd)
		{
		case Cmd.AUTHENTICATE:
			KonAuthenticate.Update();
			break;
		case Cmd.GET_ITEMS:
			KonGetItems.Update();
			break;
		case Cmd.GET_USER_ITEMS:
			KonGetUserItems.Update();
			break;
		case Cmd.USE_ITEM:
			KonUseItem.Update();
			break;
		}
	}

	public void Authenticate(int userId, string authToken, OnKonCmdRes cb)
	{
		Debug.Log(string.Format("Call Authenticate. userId: {0} authToken: {1}", userId, authToken));
		curCmd = Cmd.AUTHENTICATE;
		_cb = cb;
		KonAuthenticate.Authenticate(userId, authToken, "81a54a52-142b-47c2-9106-f0ccd5d1fd00", onAuthenticateRes);
	}

	private void onAuthenticateRes(bool res, Hashtable ht)
	{
		Debug.Log(string.Format("onAuthenticateRes. res: {0}", res));
		curCmd = Cmd.NONE;
		if (_cb != null)
		{
			_cb(res, ht);
		}
	}

	public void GetItems(OnKonCmdRes cb)
	{
		curCmd = Cmd.GET_ITEMS;
		_cb = cb;
		KonGetItems.GetItems("81a54a52-142b-47c2-9106-f0ccd5d1fd00", onGetItemsRes);
	}

	private void onGetItemsRes(bool res, Hashtable ht)
	{
		curCmd = Cmd.NONE;
		if (_cb != null)
		{
			_cb(res, ht);
		}
	}

	public void GetUserItems(int userId, OnKonCmdRes cb)
	{
		curCmd = Cmd.GET_USER_ITEMS;
		_cb = cb;
		KonGetUserItems.GetUserItems("81a54a52-142b-47c2-9106-f0ccd5d1fd00", userId, onGetUserItemsRes);
	}

	private void onGetUserItemsRes(bool res, Hashtable ht)
	{
		curCmd = Cmd.NONE;
		if (_cb != null)
		{
			_cb(res, ht);
		}
	}

	public void UseItem(int userId, string authToken, int itemId, OnKonCmdRes cb)
	{
		curCmd = Cmd.USE_ITEM;
		_cb = cb;
		KonUseItem.UseItem("81a54a52-142b-47c2-9106-f0ccd5d1fd00", authToken, userId, itemId, onUseItemRes);
	}

	private void onUseItemRes(bool res, Hashtable ht)
	{
		curCmd = Cmd.NONE;
		if (_cb != null)
		{
			_cb(res, ht);
		}
	}
}
