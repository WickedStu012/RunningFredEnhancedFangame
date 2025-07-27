public class OnPlayerBonus : GameEvent
{
	public const string NAME = "OnPlayerBonus";

	public static OnPlayerBonus Instance = new OnPlayerBonus();

	public float Multiplier = 0.5f;

	public OnPlayerBonus()
	{
		name = "OnPlayerBonus";
	}
}
