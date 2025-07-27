using UnityEngine;

public class Pendulum : MonoBehaviour
{
	private bool collide;

	private float accumTime;

	private void Start()
	{
		base.transform.GetComponent<Animation>()["Pendulum"].speed = 0.75f;
		base.transform.GetComponent<Animation>().Play("Pendulum");
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
