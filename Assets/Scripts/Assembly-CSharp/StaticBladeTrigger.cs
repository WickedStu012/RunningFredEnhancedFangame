using UnityEngine;

public class StaticBladeTrigger : MonoBehaviour
{
	private static bool collide;

	private float accumTime;

	private void Start()
	{
		GameObject player = CharHelper.GetPlayer();
		if (player == null)
		{
			Debug.LogError(string.Format("@StaticBladeTrigger Cannot find the player in scene."));
		}
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 3f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c) || GameManager.IsFredDead() || CharHelper.GetCharStateMachine().GetCurrentState() == ActionCode.DRAGGING)
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
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
