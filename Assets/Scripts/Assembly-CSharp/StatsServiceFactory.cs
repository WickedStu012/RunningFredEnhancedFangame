public class StatsServiceFactory
{
	public static iStatsService Create(StatsServiceType bet)
	{
		if (bet == StatsServiceType.FLURRY)
		{
			return new StatsFlurry();
		}
		return null;
	}
}
