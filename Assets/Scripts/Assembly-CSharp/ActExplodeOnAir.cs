using UnityEngine;

public class ActExplodeOnAir : IAction
{
	public ActExplodeOnAir(GameObject player)
		: base(player)
	{
		stateName = ActionCode.EXPLODE_ON_AIR;
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		if (!ConfigParams.useGore)
		{
			sm.SwitchTo(ActionCode.DIE_IMPCT);
			return;
		}
		ExplosionManager.FireOnAir(sm.playerT);
		SoundManager.PlaySound(5);
		sm.gameObject.SetActive(false);
		GameEventDispatcher.Dispatch(this, new PlayerExploted());
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
	}
}
