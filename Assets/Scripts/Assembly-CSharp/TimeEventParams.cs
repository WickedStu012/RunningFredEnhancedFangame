using System;

internal class TimeEventParams
{
	public DateTime time;

	public string param1;

	public string param2;

	public TimeEventParams(DateTime dt, string p1, string p2)
	{
		time = dt;
		param1 = p1;
		param2 = p2;
	}
}
