using System.Collections;
using System.Text;
using UnityEngine;

public class KonGetUserItems
{
	private const string CMD_URL = "http://www.kongregate.com/api/user_items.json";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static OnKonCmdRes beRes;

	public static void GetUserItems(string apiKey, int userId, OnKonCmdRes cbfn)
	{
		beRes = cbfn;
		string text = string.Format("{0}?api_key={1}&user_id={2}", "http://www.kongregate.com/api/user_items.json", apiKey, userId);
		Debug.Log(text);
		www = new WWW(text);
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
				Debug.Log(string.Format("txt: {0}", text));
				if (text != null)
				{
					Hashtable ht = text.hashtableFromJson();
					beRes(true, ht);
				}
				else
				{
					Debug.Log("txt is null");
					beRes(false, null);
				}
			}
			else
			{
				Debug.Log(string.Format("www.error is {0}", www.error));
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
