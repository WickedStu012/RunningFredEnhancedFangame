using UnityEngine;

public class GAEBackend : iBackEnd
{
	private enum CurCmd
	{
		NONE = 0,
		GET_TOKEN = 1,
		READ_DATA = 2,
		WRITE_DATA = 3,
		REMOVE_DATA = 4
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

	private BackendRes cb;

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
		case CurCmd.GET_TOKEN:
			CmdGetToken.Update();
			break;
		case CurCmd.READ_DATA:
			CmdReadData.Update();
			break;
		case CurCmd.WRITE_DATA:
			CmdWriteData.Update();
			break;
		case CurCmd.REMOVE_DATA:
			CmdRemoveData.Update();
			break;
		}
	}

	private void getToken()
	{
		curCmd = CurCmd.GET_TOKEN;
		CmdGetToken.GetToken(getTokenRes);
	}

	public void ReadData(string user, BackendRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.READ_DATA;
		getToken();
		username = user;
	}

	public void ReadData(BackendRes ber)
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

	public void WriteData(string user, string data, BackendRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.WRITE_DATA;
		getToken();
		username = user;
		dataToSend = data;
	}

	public void WriteData(string data, BackendRes ber)
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

	public bool IsAvailable()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	public void RemoveData(string user, BackendRes ber)
	{
		cb = ber;
		nextCmd = NextCmd.REMOVE_DATA;
		getToken();
		username = user;
	}

	public void RemoveData(BackendRes ber)
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

	private void readDataRes(bool res, string str)
	{
		if (!res)
		{
			Debug.Log(string.Format("ReadData Error. str: {0}", str));
		}
		if (cb != null)
		{
			cb(res, str);
		}
	}

	private void writeDataRes(bool res, string str)
	{
		if (res)
		{
		}
		if (cb != null)
		{
			cb(res, str);
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
		if (Application.platform == RuntimePlatform.Android)
		{
			return AndroidMarketAccount.GetUserName();
		}
		if (Application.platform == RuntimePlatform.NaCl)
		{
			return ChromeUserName.GetUserName();
		}
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			return KongregateAPI.GetUserName();
		}
		return null;
	}

	public void SetTestUserName(string un)
	{
		editorTestUserName = un;
	}
}
