using UnityEngine;

public class ActStopped : IAction
{
	public ActStopped(GameObject player)
		: base(player)
	{
		stateName = ActionCode.STOPPED;
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.StopAll();
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
	}
}
