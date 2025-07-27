using UnityEngine;

public class EvnJetpack : IEvent
{
	private CharProps props;

	private float timeAccum;

	public EvnJetpack()
	{
		code = EventCode.EVN_JETPACK;
	}

	public override void StateChange()
	{
		timeAccum = 0f;
	}

	public override bool Check()
	{
		if (GameManager.IsFredDead())
		{
			return false;
		}
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		if (props.HasJetpack && InputManager.GetJump())
		{
			timeAccum += Time.deltaTime;
			if (timeAccum > 0.4f)
			{
				return true;
			}
		}
		return false;
	}
}
