using UnityEngine;

public class OnLevelStart : MonoBehaviour
{
	private const int UPDATE_COUNT_NUM = 2;

	private int updateCount;

	private void Start()
	{
		updateCount = 0;
	}

	private void Update()
	{
		if (updateCount < 2)
		{
			updateCount++;
			if (updateCount == 2)
			{
				GameManager.OnLevelStart();
			}
		}
		GameManager.OnGameUpdate();
	}

	private void OnDestroy()
	{
		GameManager.OnLevelUnLoad();
	}
}
