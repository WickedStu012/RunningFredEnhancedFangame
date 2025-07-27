public class OnTutorialObjectiveFail : GameEvent
{
	public const string NAME = "OnTutorialObjectiveFail";

	public LevelTutorialObjectiveType objType;

	public OnTutorialObjectiveFail(LevelTutorialObjectiveType ot)
	{
		name = "OnTutorialObjectiveFail";
		objType = ot;
	}
}
