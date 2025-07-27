using UnityEngine;

public class TriggerLongAxeBlade : MonoBehaviour
{
	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		float x = base.transform.localRotation.eulerAngles.x;
		if (collide || !CharHelper.IsColliderFromPlayer(c) || CharHelper.GetCharSkin().IsBlinking())
		{
			return;
		}
		if (x == -90f || x == 270f)
		{
			SoundManager.PlaySound(30);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
		else if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			if (ConfigParams.useGore)
			{
				CharHelper.GetCharSkin().DismemberRandom();
			}
			else
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DIE_IMPCT);
			}
		}
		else
		{
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
		collide = true;
	}
}
