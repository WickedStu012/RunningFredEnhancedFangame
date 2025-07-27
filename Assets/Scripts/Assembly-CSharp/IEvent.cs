public abstract class IEvent
{
	protected EventCode code;

	protected CharStateMachine sm;

	public IEvent()
	{
	}

	public void SetStateMachineRef(CharStateMachine sm)
	{
		this.sm = sm;
	}

	public EventCode GetCode()
	{
		return code;
	}

	public virtual void StateChange()
	{
	}

	public abstract bool Check();
}
