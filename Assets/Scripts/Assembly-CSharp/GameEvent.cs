public class GameEvent
{
	protected string name;

	public string Name
	{
		get
		{
			return name;
		}
	}

	public GameEvent()
	{
	}

	public GameEvent(string name)
	{
		this.name = name;
	}
}
