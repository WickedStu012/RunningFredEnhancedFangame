using UnityEngine;

public abstract class IAction
{
	protected ActionCode stateName;

	protected Transform playerT;

	protected CharacterController cc;

	protected CharStateMachine sm;

	public IAction(GameObject player)
	{
		playerT = player.transform;
		cc = player.GetComponent<CharacterController>();
		sm = player.GetComponent<CharStateMachine>();
	}

	public ActionCode GetState()
	{
		return stateName;
	}

	public abstract bool CanGetIn();

	public abstract void GetIn(params object[] list);

	public abstract void GetOut();

	public abstract void Update(float dt);

	public virtual void OnGUI()
	{
	}
}
