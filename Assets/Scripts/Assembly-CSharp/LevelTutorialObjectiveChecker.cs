using UnityEngine;

public class LevelTutorialObjectiveChecker : MonoBehaviour
{
	public LevelTutorialObjectiveCounter objCounter;

	private bool collide;

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (!CharHelper.GetCharSkin().IsBlinking())
			{
				objCounter.ObjectiveComplete();
			}
			collide = true;
		}
	}
}
