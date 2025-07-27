using UnityEngine;

public class LevelEndCloseGate : MonoBehaviour
{
	public LevelRooftopEnd roofTopEnd;

	private bool collide;

	private void Start()
	{
		collide = false;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide)
		{
			if (CharHelper.IsColliderFromPlayer(c))
			{
			}
			collide = true;
		}
	}
}
