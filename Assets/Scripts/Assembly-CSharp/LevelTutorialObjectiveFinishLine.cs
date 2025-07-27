using UnityEngine;

public class LevelTutorialObjectiveFinishLine : MonoBehaviour
{
	public LevelTutorialObjectiveCounter objCounter;

	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			objCounter.ObjectiveChunkFinish();
			collide = true;
		}
	}
}
