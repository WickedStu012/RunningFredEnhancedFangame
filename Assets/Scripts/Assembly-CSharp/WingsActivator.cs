using UnityEngine;

public class WingsActivator : MonoBehaviour
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
		base.transform.RotateAroundLocal(base.transform.up, Time.deltaTime);
	}

	private void OnTriggerStay(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (CharHelper.GetProps().HasWings)
		{
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			charStateMachine.SwitchTo(ActionCode.FLY);
			ActFly actFly = CharHelper.GetCharStateMachine().GetCurrentAction() as ActFly;
			if (actFly != null)
			{
				actFly.AddImpulse(base.transform.up);
			}
		}
		collide = true;
	}
}
