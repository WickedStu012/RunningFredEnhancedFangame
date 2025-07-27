public class OnSurvivalLevelUp : GameEvent
{
	public const string NAME = "OnSurvivalLevelUp";

	public static OnSurvivalLevelUp Instance = new OnSurvivalLevelUp();

	public string LevelName = string.Empty;

	public OnSurvivalLevelUp()
	{
		name = "OnSurvivalLevelUp";
	}
}
