using UnityEngine;

public class FlameWall : MonoBehaviour
{
	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BURNT);
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			collide = true;
		}
	}
}
