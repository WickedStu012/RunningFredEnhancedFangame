using UnityEngine;

public class IceFlameWall : MonoBehaviour
{
	private CharStateMachine sm;

	private bool collide;

	private float accumTime;

	private void Start()
	{
		GameObject player = CharHelper.GetPlayer();
		if (player != null)
		{
			sm = player.GetComponent<CharStateMachine>();
		}
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= 3f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			if (sm == null)
			{
				GameObject player = CharHelper.GetPlayer();
				if (player != null)
				{
					sm = player.GetComponent<CharStateMachine>();
				}
			}
			sm.SwitchTo(ActionCode.FROZEN);
		}
		else
		{
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
		}
		collide = true;
	}
}
