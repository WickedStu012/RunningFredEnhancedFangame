using UnityEngine;

public class AerealAccelerator : MonoBehaviour
{
	private bool collide;

	private float accumTime;

	private void Start()
	{
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 1f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
		base.transform.RotateAroundLocal(base.transform.up, 0f - Time.deltaTime);
	}

	private void OnTriggerStay(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			ActFly actFly = CharHelper.GetCharStateMachine().GetCurrentAction() as ActFly;
			if (actFly != null)
			{
				actFly.AddImpulse(base.transform.up);
				collide = true;
			}
		}
	}
}
