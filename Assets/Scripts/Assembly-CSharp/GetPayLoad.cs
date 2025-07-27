using System.Text;
using UnityEngine;

public class GetPayLoad
{
	private const float TIMEOUT = 10f;

	private const string RF_BACKEND = "http://www.dedalord.com/runningfred/be/rfbe.php";

	private const string RES_BEGIN = "<res>";

	private const string RES_END = "</res>";

	private const string RESERR_BEGIN = "<err>";

	private const string RESERR_END = "</err>";

	private static WWW www;

	private static bool waiting;

	private static float accumTime;

	private static PayloadRes payloadRes;

	public static void GetRes(string marketId, PayloadRes cbfn)
	{
		payloadRes = cbfn;
		www = new WWW(string.Format("{0}?op={1}", "http://www.dedalord.com/runningfred/be/rfbe.php", marketId));
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
			string text = Encoding.ASCII.GetString(www.bytes);
			int num = text.IndexOf("<res>");
			if (num != -1)
			{
				string str = text.Substring(num + "<res>".Length, text.IndexOf("</res>") - num - "</res>".Length + 1);
				payloadRes(true, str);
				return;
			}
			num = text.IndexOf("<err>");
			if (num != -1)
			{
				string str2 = text.Substring(num + "<err>".Length, text.IndexOf("</err>") - num - "</err>".Length + 1);
				payloadRes(false, str2);
			}
			else
			{
				payloadRes(false, string.Format("Error parsing the response from server. Response: {0} Error: {1}", text, www.error));
			}
		}
		else
		{
			accumTime += Time.deltaTime;
			if (accumTime > 10f)
			{
				waiting = false;
				payloadRes(false, www.error);
			}
		}
	}
}
