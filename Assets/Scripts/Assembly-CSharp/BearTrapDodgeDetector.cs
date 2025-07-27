using UnityEngine;

public class BearTrapDodgeDetector : MonoBehaviour
{
	public BearTrap bearTrap;

	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			bearTrap.Dodge();
			collide = true;
		}
	}
}
