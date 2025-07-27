using UnityEngine;

public class Bouncer : MonoBehaviour
{
	public int jumpForce = 2;

	private bool collide;

	private float accumTime;

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= 1f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c) && !GameManager.IsFredDead())
		{
			SoundManager.PlaySound(22);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCER_JUMP, jumpForce, base.transform.TransformDirection(base.transform.up) * -1f);
			collide = true;
		}
	}
}
