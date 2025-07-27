using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
	public bool openAtPlayer = true;

	public bool pauseBeforeOpen;

	public GameObject leftSheet;

	public GameObject rightSheet;

	private Vector3 leftOpenDoor;

	private Vector3 leftClosedDoor;

	private Vector3 rightOpenDoor;

	private Vector3 rightClosedDoor;

	private bool collide;

	private bool openDoor;

	private float accumTime;

	private void Start()
	{
		collide = false;
		if (!openAtPlayer)
		{
			base.enabled = false;
		}
		openDoor = false;
		leftClosedDoor = leftSheet.transform.localPosition;
		rightClosedDoor = rightSheet.transform.localPosition;
		leftOpenDoor = new Vector3(-12f, leftClosedDoor.y, leftClosedDoor.z);
		rightOpenDoor = new Vector3(12f, rightClosedDoor.y, rightClosedDoor.z);
	}

	private void Update()
	{
		if (openDoor)
		{
			accumTime += Time.deltaTime;
			leftSheet.transform.localPosition = Vector3.Lerp(leftClosedDoor, leftOpenDoor, accumTime);
			rightSheet.transform.localPosition = Vector3.Lerp(rightClosedDoor, rightOpenDoor, accumTime);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (pauseBeforeOpen)
			{
				GameEventDispatcher.Dispatch(this, new OnTutorialPause(this));
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.STOPPED);
			}
			else
			{
				OpenDoor();
			}
			collide = true;
		}
	}

	public void OpenDoor()
	{
		if (pauseBeforeOpen)
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.RUNNING);
		}
		SoundManager.PlaySound(58);
		openDoor = true;
	}
}
