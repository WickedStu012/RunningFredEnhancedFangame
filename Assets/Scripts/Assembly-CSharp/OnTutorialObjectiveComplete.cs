public class OnTutorialObjectiveComplete : GameEvent
{
	public const string NAME = "OnTutorialObjectiveComplete";

	public LevelTutorialObjectiveType objType;

	public OnTutorialObjectiveComplete(LevelTutorialObjectiveType ot)
	{
		name = "OnTutorialObjectiveComplete";
		objType = ot;
	}
}
