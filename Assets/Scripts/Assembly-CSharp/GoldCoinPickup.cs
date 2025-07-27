internal class GoldCoinPickup : GameEvent
{
	public const string NAME = "GoldCoinPickup";

	public static GoldCoinPickup Instance = new GoldCoinPickup();

	public GoldCoinPickup()
	{
		name = "GoldCoinPickup";
	}
}
