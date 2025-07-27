using UnityEngine;

public class SpikeFiveFakeDetector : MonoBehaviour
{
	public SpikeFiveSpikes spikes;

	public float spikesSpeed = 10f;

	private bool collide;

	private void Start()
	{
		collide = false;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			spikes.Trigger(spikesSpeed, false, 0f);
			collide = true;
		}
	}
}
