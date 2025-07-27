using UnityEngine;

public class LevelTutorialEnd : LevelEnd
{
	public GameObject leftSheet;

	public GameObject rightSheet;

	private Vector3 leftOpenDoor;

	private Vector3 leftClosedDoor;

	private Vector3 rightOpenDoor;

	private Vector3 rightClosedDoor;

	private float accumTime;

	private new void Start()
	{
		base.Start();
		leftClosedDoor = leftSheet.transform.localPosition;
		rightClosedDoor = rightSheet.transform.localPosition;
		leftOpenDoor = new Vector3(-12f, leftClosedDoor.y, leftClosedDoor.z);
		rightOpenDoor = new Vector3(12f, rightClosedDoor.y, rightClosedDoor.z);
	}

	private new void Update()
	{
		base.Update();
		if (goalReached)
		{
			accumTime += Time.deltaTime;
			leftSheet.transform.localPosition = Vector3.Lerp(leftClosedDoor, leftOpenDoor, accumTime);
			rightSheet.transform.localPosition = Vector3.Lerp(rightClosedDoor, rightOpenDoor, accumTime);
			if (accumTime >= 1f)
			{
				EnderFinished();
			}
		}
	}
}
