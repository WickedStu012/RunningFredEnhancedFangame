using UnityEngine;

public class SpikeFive : MonoBehaviour
{
	public SpikeFiveSpikes spikes;

	public float spikesSpeed = 10f;

	public float distanceToPlayerMax = 2f;

	private bool collide;

	private void Start()
	{
		collide = false;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c) && !GameManager.IsFredDead())
		{
			spikes.Trigger(spikesSpeed, true, distanceToPlayerMax);
			collide = true;
		}
	}
}
