using System.Collections;
using System.Text;
using UnityEngine;

public class KonGetItems
{
	private const string CMD_URL = "http://www.kongregate.com/api/items.json";

	private const string RES_BEGIN = "[do]";

	private const string RES_END = "[/do]";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static OnKonCmdRes beRes;

	public static void GetItems(string apiKey, OnKonCmdRes cbfn)
	{
		beRes = cbfn;
		www = new WWW(string.Format("{0}?api_key={1}", "http://www.kongregate.com/api/items.json", apiKey));
		waiting = true;
		accumTime = 0f;
	}

	public static void Update()
	{
		if (!waiting)
		{
			return;
		}
		if (www.isDone)
		{
			waiting = false;
			if (www.error == null)
			{
				string text = Encoding.ASCII.GetString(www.bytes);
				if (text != null)
				{
					Debug.Log(string.Format("txt: {0}", text));
					Hashtable ht = text.hashtableFromJson();
					beRes(true, ht);
				}
				else
				{
					beRes(false, null);
				}
			}
			else
			{
				beRes(false, null);
			}
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				Debug.Log("Timeout");
				waiting = false;
				beRes(false, null);
			}
		}
	}
}
