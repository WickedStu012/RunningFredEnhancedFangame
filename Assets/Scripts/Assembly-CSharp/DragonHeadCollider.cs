using UnityEngine;

public class DragonHeadCollider : MonoBehaviour
{
	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BURNT);
			collide = true;
		}
	}
}
