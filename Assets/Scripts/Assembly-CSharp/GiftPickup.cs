internal class GiftPickup : GameEvent
{
	public const string NAME = "GiftPickup";

	public static GiftPickup Instance = new GiftPickup();

	public GiftPickup()
	{
		name = "GiftPickup";
	}
}
