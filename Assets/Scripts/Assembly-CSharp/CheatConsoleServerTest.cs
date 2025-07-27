using UnityEngine;

public class CheatConsoleServerTest : MonoBehaviour
{
	private const int textFieldWidth = 80;

	private const int textFieldHeight = 22;

	public string itemId = "0";

	public string itemCount = "1";

	public string itemUseCount = "1";

	public string version = "1";

	public static CheatConsoleServerTest Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Get Cheat"))
		{
			CheatConsoleServer.GetCheat("1245", getCheatRes);
		}
		if (GUILayout.Button("List Cheats"))
		{
			CheatConsoleServer.ListCheats(listCheatsRes);
		}
		if (GUILayout.Button("Write Cheat"))
		{
			CheatConsoleServer.WriteCheat("1245", 0, 1, 5, 1, writeCheatRes);
		}
		if (GUILayout.Button("Delete All"))
		{
			CheatConsoleServer.DeleteAllCheats(deleteAllCheatsRes);
		}
		itemId = GUI.TextField(new Rect(230f, 340f, 80f, 22f), itemId);
		itemCount = GUI.TextField(new Rect(370f, 340f, 80f, 22f), itemCount);
		itemUseCount = GUI.TextField(new Rect(510f, 340f, 80f, 22f), itemUseCount);
		version = GUI.TextField(new Rect(680f, 340f, 80f, 22f), version);
	}

	private void getCheatRes(bool res, string resStr, int itemId, int itemCount, long cheatId)
	{
		Debug.Log(string.Format("res: {0} resStr: {1} itemId: {2} itemCount: {3} cheatId: {4}", res, resStr, itemId, itemCount, cheatId));
	}

	private void writeCheatRes(bool res, string resStr)
	{
		Debug.Log(string.Format("res: {0} resStr: {1}", res, resStr));
	}

	private void listCheatsRes(bool res, string resStr, string[] lines)
	{
		if (res)
		{
			if (lines != null)
			{
				for (int i = 0; i < lines.Length; i++)
				{
					Debug.Log(string.Format("line: {0} val: {1}", i, lines[i]));
				}
			}
		}
		else
		{
			Debug.Log(string.Format("ListCheats returned the error: {0}", resStr));
		}
	}

	public void deleteAllCheatsRes(bool res, string resStr)
	{
		Debug.Log(string.Format("res: {0} resStr: {1}", res, resStr));
	}
}
