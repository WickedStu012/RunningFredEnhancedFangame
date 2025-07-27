using UnityEngine;

public class CheatConsoleServer : MonoBehaviour
{
	private static CheatServerBackEnd be;

	private void Start()
	{
		be = new CheatServerBackEnd();
		Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (be != null)
		{
			be.Update();
		}
	}

	public static void GetCheat(string code, BackendGetCheatRes_CS ber)
	{
		if (be == null)
		{
			ber(false, "Backend object is null", 0, 0, 0L);
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			be.GetCheat(code, ber);
		}
		else
		{
			ber(false, "No network connection", 0, 0, 0L);
		}
	}

	public static void WriteCheat(string code, int itemId, int itemCount, int itemUseCount, int version, BackendWriteCheatRes_CS ber)
	{
		if (be == null)
		{
			ber(false, "Backend object is null");
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			be.WriteCheat(code, itemId, itemCount, itemUseCount, version, ber);
		}
		else
		{
			ber(false, "No network connection");
		}
	}

	public static void ListCheats(BackendListCheatsRes_CS ber)
	{
		if (be == null)
		{
			ber(false, "Backend object is null", null);
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			be.ListCheats(ber);
		}
		else
		{
			ber(false, "No network connection", null);
		}
	}

	public static void DeleteAllCheats(BackendDeleteAllCheatsRes_CS ber)
	{
		if (be == null)
		{
			ber(false, "Backend object is null");
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			be.DeleteAllCheats(ber);
		}
		else
		{
			ber(false, "No network connection");
		}
	}
}
