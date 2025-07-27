using UnityEngine;

public class BlackLord
{
	private enum CurCmd
	{
		NONE = 0,
		ITUNES_RECEIPT_VALIDATION = 1,
		GET_TOKEN = 2,
		READ_DATA = 3,
		WRITE_DATA = 4,
		REMOVE_DATA = 5
	}

	private enum NextCmd
	{
		READ_DATA = 0,
		WRITE_DATA = 1,
		REMOVE_DATA = 2
	}

	private CurCmd curCmd;

	private NextCmd nextCmd;

	private string token;

	private string username;

	private string dataToSend;

	private BlackLordRes cb;

	private string editorTestUserName;

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void Update()
	{
		switch (curCmd)
		{
		case CurCmd.ITUNES_RECEIPT_VALIDATION:
			blValidateReceipt.Update();
			break;
		case CurCmd.GET_TOKEN:
			blGetToken.Update();
			break;
		case CurCmd.READ_DATA:
			break;
		case CurCmd.WRITE_DATA:
			break;
		case CurCmd.REMOVE_DATA:
			break;
		}
	}

	private void getToken()
	{
		curCmd = CurCmd.GET_TOKEN;
		blGetToken.GetToken(getTokenRes);
	}

	private void getTokenRes(bool res, string str)
	{
		curCmd = CurCmd.NONE;
		if (res)
		{
			token = str;
			if (nextCmd == NextCmd.READ_DATA)
			{
				curCmd = CurCmd.READ_DATA;
				CmdReadData.ReadData(token, username, readDataRes);
			}
			else if (nextCmd == NextCmd.WRITE_DATA)
			{
				curCmd = CurCmd.WRITE_DATA;
				CmdWriteData.WriteData(token, username, dataToSend, writeDataRes);
			}
			else if (nextCmd == NextCmd.REMOVE_DATA)
			{
				curCmd = CurCmd.REMOVE_DATA;
				CmdRemoveData.RemoveData(token, username, removeDataRes);
			}
		}
		else if (cb != null)
		{
			cb(false, str);
		}
	}

	public void ReadData(string user, BlackLordRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.READ_DATA;
		getToken();
		username = user;
	}

	public void ReadData(BlackLordRes ber)
	{
		if (username == null)
		{
			username = getUsername();
		}
		if (username != null)
		{
			cb = ber;
			nextCmd = NextCmd.READ_DATA;
			getToken();
		}
		else if (ber != null)
		{
			ber(false, "Username is null");
		}
	}

	private void readDataRes(bool res, string str)
	{
		if (!res)
		{
			Debug.Log(string.Format("ReadData Error"));
		}
		if (cb != null)
		{
			cb(res, str);
		}
	}

	public void WriteData(string user, string data, BlackLordRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.WRITE_DATA;
		getToken();
		username = user;
		dataToSend = data;
	}

	public void WriteData(string data, BlackLordRes ber)
	{
		if (username == null)
		{
			username = getUsername();
		}
		if (username != null)
		{
			cb = ber;
			nextCmd = NextCmd.WRITE_DATA;
			getToken();
			dataToSend = data;
		}
		else if (ber != null)
		{
			ber(false, "Username is null");
		}
	}

	private void writeDataRes(bool res, string str)
	{
		if (!res)
		{
			Debug.Log(string.Format("WriteData Error"));
		}
		if (cb != null)
		{
			cb(res, str);
		}
	}

	public void RemoveData(string user, BlackLordRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.REMOVE_DATA;
		getToken();
		username = user;
	}

	public void RemoveData(BlackLordRes ber)
	{
		if (username == null)
		{
			username = getUsername();
		}
		if (username != null)
		{
			cb = ber;
			nextCmd = NextCmd.REMOVE_DATA;
			getToken();
		}
		else if (ber != null)
		{
			ber(false, "Username is null");
		}
	}

	private void removeDataRes(bool res, string str)
	{
		if (res)
		{
			Debug.Log(string.Format("RemoveData ok"));
		}
		else
		{
			Debug.Log(string.Format("RemoveData Error"));
		}
		if (cb != null)
		{
			cb(res, str);
		}
	}

	public bool IsAvailable()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	public bool IsUserAuthenticated()
	{
		username = getUsername();
		return username != null;
	}

	private string getUsername()
	{
		if (Application.isEditor)
		{
			return editorTestUserName;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return string.Empty;
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return null;
	}

	public void SetTestUserName(string un)
	{
		Debug.Log(string.Format("SetTestUserName name: {0}", un));
		editorTestUserName = un;
	}
}
