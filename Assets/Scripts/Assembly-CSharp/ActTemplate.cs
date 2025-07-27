using UnityEngine;

public class ActTemplate : IAction
{
	public ActTemplate(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CLIMB;
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
	}
}
