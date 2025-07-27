using UnityEngine;

public class ActExplode : IAction
{
	public ActExplode(GameObject player)
		: base(player)
	{
		stateName = ActionCode.EXPLODE;
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
		ExplosionManager.Fire(sm.playerT.position);
		BloodSplatManager.Instance.Create(Random.Range(25, 40));
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
