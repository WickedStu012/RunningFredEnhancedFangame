internal class PlayerWasCrushed : GameEvent
{
	public const string NAME = "PlayerWasCrushed";

	public static PlayerWasCrushed Instance = new PlayerWasCrushed();

	public PlayerWasCrushed()
	{
		name = "PlayerWasCrushed";
	}
}
