public class CharChangeState : GameEvent
{
	public const string NAME = "CharChangeState";

	public IAction CurrentState;

	public CharChangeState()
	{
		name = "CharChangeState";
	}
}
