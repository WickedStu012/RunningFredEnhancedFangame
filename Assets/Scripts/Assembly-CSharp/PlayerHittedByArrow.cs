internal class PlayerHittedByArrow : GameEvent
{
	public const string NAME = "PlayerHittedByArrow";

	public static PlayerHittedByArrow Instance = new PlayerHittedByArrow();

	public PlayerHittedByArrow()
	{
		name = "PlayerHittedByArrow";
	}
}
