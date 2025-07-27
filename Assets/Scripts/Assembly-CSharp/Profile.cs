public class Profile
{
	private static PerformanceScore profilePerf = PerformanceScore.AVERAGE;

	private static bool performanceChecked;

	public static PerformanceScore Score
	{
		get
		{
			return profilePerf;
		}
	}

	public static void CheckPerformance()
	{
		if (!performanceChecked)
		{
			profilePerf = PerformanceScore.AVERAGE;
			performanceChecked = true;
		}
	}

	public static bool GreaterOrEqualTo(PerformanceScore score)
	{
		return profilePerf >= score;
	}

	public static bool GreaterThan(PerformanceScore score)
	{
		return profilePerf > score;
	}

	public static bool LessOrEqualTo(PerformanceScore score)
	{
		return profilePerf <= score;
	}

	public static bool LessThan(PerformanceScore score)
	{
		return profilePerf < score;
	}
}
