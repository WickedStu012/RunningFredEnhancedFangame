internal class TreasurePickup : GameEvent
{
	public const string NAME = "TreasurePickup";

	public int GoldCount;

	public int Id;

	public TreasurePickup()
	{
		name = "TreasurePickup";
	}
}
