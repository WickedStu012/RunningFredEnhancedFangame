using UnityEngine;

public class ActExplodeOnWall : IAction
{
	public ActExplodeOnWall(GameObject player)
		: base(player)
	{
		stateName = ActionCode.EXPLODE_ON_WALL;
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
		sm.DisableBlob();
		ExplosionManager.FireOnWall(sm.playerT.position);
		SoundManager.PlaySound(5);
		BloodSplatManager.Instance.Create(Random.Range(25, 40));
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
