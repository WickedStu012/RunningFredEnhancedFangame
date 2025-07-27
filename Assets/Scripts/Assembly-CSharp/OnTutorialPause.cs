public class OnTutorialPause : GameEvent
{
	public const string NAME = "OnTutorialPause";

	public TutorialDoor door;

	public OnTutorialPause(TutorialDoor door)
	{
		name = "OnTutorialPause";
		this.door = door;
	}
}
