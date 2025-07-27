using UnityEngine;

public class DiveTrigger : MonoBehaviour
{
	private bool collide;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DIVE);
			collide = true;
		}
	}
}
