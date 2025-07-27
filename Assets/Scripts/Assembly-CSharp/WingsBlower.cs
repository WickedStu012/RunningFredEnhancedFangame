using UnityEngine;

public class WingsBlower : MonoBehaviour
{
	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			ActionCode currentState = charStateMachine.GetCurrentState();
			if (currentState == ActionCode.FLY)
			{
				(charStateMachine.GetCurrentAction() as ActFly).OverABlower();
			}
			collide = true;
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (collide && CharHelper.IsColliderFromPlayer(c))
		{
			collide = false;
		}
	}
}
