internal class PlayerWasBurnt : GameEvent
{
	public const string NAME = "PlayerWasBurnt";

	public static PlayerWasBurnt Instance = new PlayerWasBurnt();

	public PlayerWasBurnt()
	{
		name = "PlayerWasBurnt";
	}
}
