public class CheatServerBackEnd
{
	private enum CurCmd
	{
		NONE = 0,
		GET_TOKEN = 1,
		GET_CHEAT = 2,
		WRITE_CHEAT = 3,
		LIST_CHEATS = 4,
		DELETE_ALL_CHEATS = 5
	}

	private enum NextCmd
	{
		GET_CHEAT = 0,
		WRITE_CHEAT = 1,
		LIST_CHEATS = 2,
		DELETE_ALL_CHEATS = 3
	}

	private CurCmd curCmd;

	private NextCmd nextCmd;

	private string token;

	private BackendRes_CS cb;

	private BackendGetCheatRes_CS gccb;

	private BackendWriteCheatRes_CS wccb;

	private BackendListCheatsRes_CS lccb;

	private BackendDeleteAllCheatsRes_CS dccb;

	private int itemId;

	private int itemCount;

	private int itemUseCount;

	private int version;

	private string cheatCode;

	public void Update()
	{
		switch (curCmd)
		{
		case CurCmd.GET_TOKEN:
			CmdGetToken_CS.Update();
			break;
		case CurCmd.GET_CHEAT:
			CmdGetCheat_CS.Update();
			break;
		case CurCmd.WRITE_CHEAT:
			CmdWriteCheat_CS.Update();
			break;
		case CurCmd.LIST_CHEATS:
			CmdListCheats_CS.Update();
			break;
		case CurCmd.DELETE_ALL_CHEATS:
			CmdDeleteAllCheats_CS.Update();
			break;
		}
	}

	private void getToken()
	{
		curCmd = CurCmd.GET_TOKEN;
		CmdGetToken_CS.GetToken(getTokenRes);
	}

	private void getTokenRes(bool res, string str)
	{
		curCmd = CurCmd.NONE;
		if (res)
		{
			token = str;
			if (nextCmd == NextCmd.GET_CHEAT)
			{
				curCmd = CurCmd.GET_CHEAT;
				CmdGetCheat_CS.GetCheat(token, cheatCode, getCheatRes);
			}
			else if (nextCmd == NextCmd.WRITE_CHEAT)
			{
				curCmd = CurCmd.WRITE_CHEAT;
				CmdWriteCheat_CS.WriteCheat(token, cheatCode, itemId, itemCount, itemUseCount, version, writeCheatRes);
			}
			else if (nextCmd == NextCmd.LIST_CHEATS)
			{
				curCmd = CurCmd.LIST_CHEATS;
				CmdListCheats_CS.ListCheats(token, listCheatsRes);
			}
			else if (nextCmd == NextCmd.DELETE_ALL_CHEATS)
			{
				curCmd = CurCmd.DELETE_ALL_CHEATS;
				CmdDeleteAllCheats_CS.DeleteAllCheats(token, deleteAllCheatsRes);
			}
		}
		else if (nextCmd == NextCmd.GET_CHEAT)
		{
			curCmd = CurCmd.NONE;
			gccb(res, str, 0, 0, 0L);
		}
		else if (nextCmd == NextCmd.WRITE_CHEAT)
		{
			curCmd = CurCmd.NONE;
			wccb(res, str);
		}
		else if (nextCmd == NextCmd.LIST_CHEATS)
		{
			curCmd = CurCmd.NONE;
			lccb(res, str, null);
		}
		else if (nextCmd == NextCmd.DELETE_ALL_CHEATS)
		{
			curCmd = CurCmd.NONE;
			dccb(res, str);
		}
	}

	private void getCheatRes(bool res, string resStr, int itemId, int itemCount, long cheatItemId)
	{
		if (gccb != null)
		{
			gccb(res, resStr, itemId, itemCount, cheatItemId);
		}
	}

	public void GetCheat(string code, BackendGetCheatRes_CS ber)
	{
		cheatCode = code;
		gccb = ber;
		nextCmd = NextCmd.GET_CHEAT;
		getToken();
	}

	public void WriteCheat(string code, int itemId, int itemCount, int itemUseCount, int version, BackendWriteCheatRes_CS ber)
	{
		cheatCode = code;
		this.itemId = itemId;
		this.itemCount = itemCount;
		this.itemUseCount = itemUseCount;
		this.version = version;
		wccb = ber;
		nextCmd = NextCmd.WRITE_CHEAT;
		getToken();
	}

	private void writeCheatRes(bool res, string resStr)
	{
		if (wccb != null)
		{
			wccb(res, resStr);
		}
	}

	public void ListCheats(BackendListCheatsRes_CS ber)
	{
		lccb = ber;
		nextCmd = NextCmd.LIST_CHEATS;
		getToken();
	}

	private void listCheatsRes(bool res, string resStr, string[] lines)
	{
		if (lccb != null)
		{
			lccb(res, resStr, lines);
		}
	}

	public void DeleteAllCheats(BackendDeleteAllCheatsRes_CS ber)
	{
		dccb = ber;
		nextCmd = NextCmd.DELETE_ALL_CHEATS;
		getToken();
	}

	private void deleteAllCheatsRes(bool res, string resStr)
	{
		if (dccb != null)
		{
			dccb(res, resStr);
		}
	}
}
