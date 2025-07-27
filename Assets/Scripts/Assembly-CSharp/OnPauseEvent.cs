public class OnPauseEvent : GameEvent
{
	public const string NAME = "OnPauseEvent";

	public bool Paused = true;

	public OnPauseEvent()
	{
		name = "OnPauseEvent";
	}
}
