using UnityEngine;

public class LevelTutorialObjectiveCounter : MonoBehaviour
{
	public int objTotalNumber;

	public LevelTutorialObjectiveType objectiveType;

	private int objCount;

	private bool eventWasCalled;

	private void Start()
	{
		objCount = 0;
	}

	public void ObjectiveComplete()
	{
		objCount++;
		if (objCount == objTotalNumber)
		{
			Debug.Log("All the objectives were completed");
			GameEventDispatcher.Dispatch(this, new OnTutorialObjectiveComplete(objectiveType));
			eventWasCalled = true;
		}
	}

	public void ObjectiveChunkFinish()
	{
		if (!eventWasCalled)
		{
			Debug.Log("Objectives not completed");
			GameEventDispatcher.Dispatch(this, new OnTutorialObjectiveFail(objectiveType));
			eventWasCalled = true;
		}
	}
}
