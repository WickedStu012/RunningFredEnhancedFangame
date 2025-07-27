using UnityEngine;

public class WebDataStoreTest : MonoBehaviour
{
	private string txt = string.Empty;

	private int num;

	private bool isAvailable;

	private void OnGUI()
	{
		if (GUILayout.Button("Read Data", GUILayout.Width(300f), GUILayout.Height(100f)))
		{
			WebDataStore.ReadData(onReadData);
		}
		if (GUILayout.Button("Write Data", GUILayout.Width(300f), GUILayout.Height(100f)))
		{
			WebDataStore.WriteData("Verdura", true);
		}
		if (GUILayout.Button("Remove Data", GUILayout.Width(300f), GUILayout.Height(100f)))
		{
			WebDataStore.RemoveData(null);
		}
		if (GUILayout.Button("Is Available?", GUILayout.Width(300f), GUILayout.Height(100f)))
		{
			isAvailable = WebDataStore.IsAvailable();
		}
		GUILayout.Label(string.Format("txt: {0}", txt));
		GUILayout.Label(string.Format("isAvailable: {0}", isAvailable));
		GUILayout.Label(string.Format("{0}", num));
		num++;
		if (num == 99999)
		{
			num = 0;
		}
	}

	private void onReadData(bool res, string str)
	{
		txt = str;
	}
}
