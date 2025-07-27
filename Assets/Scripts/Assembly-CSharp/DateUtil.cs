using System;

public class DateUtil
{
	private const long SECS_TO_TICKS = 10000000L;

	private static DateTime refDate = new DateTime(1970, 1, 1);

	public static int ConvertToInt32(DateTime date)
	{
		return (int)(date - refDate).TotalSeconds;
	}

	public static int GetPHPTime()
	{
		return (int)(DateTime.UtcNow - refDate).TotalSeconds;
	}

	public static int ConvertToInt32(string date)
	{
		return ConvertToInt32(ConvertToDateTime(date));
	}

	public static DateTime ConvertToDateTime(int secs)
	{
		long num = secs;
		long ticks = refDate.Ticks + num * 10000000;
		return new DateTime(ticks);
	}

	public static DateTime ConvertToDateTime(string date)
	{
		if (date.Length == 10)
		{
			int day = Convert.ToInt32(date.Substring(0, 2));
			int month = Convert.ToInt32(date.Substring(3, 2));
			int year = Convert.ToInt32(date.Substring(6, 4));
			return new DateTime(year, month, day);
		}
		if (date.Length == 19)
		{
			int day2 = Convert.ToInt32(date.Substring(0, 2));
			int month2 = Convert.ToInt32(date.Substring(3, 2));
			int year2 = Convert.ToInt32(date.Substring(6, 4));
			int hour = Convert.ToInt32(date.Substring(11, 2));
			int minute = Convert.ToInt32(date.Substring(14, 2));
			int second = Convert.ToInt32(date.Substring(17, 2));
			return new DateTime(year2, month2, day2, hour, minute, second);
		}
		return DateTime.MinValue;
	}

	public static string ConvertToString(DateTime date)
	{
		return string.Format("{0:00}/{1:00}/{2:00}", date.Day, date.Month, date.Year);
	}

	public static string ConvertToString(int date)
	{
		return ConvertToString(ConvertToDateTime(date));
	}

	public static string ConvertToStringWithSeconds(DateTime date)
	{
		return string.Format("{0:00}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", date.Day, date.Month, date.Year, date.Hour, date.Minute, date.Second);
	}

	public static string ConvertToStringWithSeconds(int date)
	{
		return ConvertToStringWithSeconds(ConvertToDateTime(date));
	}
}
