using UnityEngine;

public class PigeonTrigger : MonoBehaviour
{
	private bool collide;

	private Pigeon pigeon;

	private void Start()
	{
		collide = false;
		pigeon = GetComponentInChildren<Pigeon>();
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			pigeon.Fly();
			collide = true;
		}
	}
}
