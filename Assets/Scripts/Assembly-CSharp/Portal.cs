using UnityEngine;

public class Portal : MonoBehaviour
{
	public bool openAtPlayer = true;

	public GameObject leftSheet;

	public GameObject rightSheet;

	public bool openTowardPlayer = true;

	private Quaternion leftOpenDoor;

	private Quaternion leftClosedDoor;

	private Quaternion rightOpenDoor;

	private Quaternion rightClosedDoor;

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
		leftOpenDoor = leftSheet.transform.rotation;
		rightOpenDoor = rightSheet.transform.rotation;
		if (openTowardPlayer)
		{
			leftClosedDoor = new Quaternion(-0.4f, 0.6f, 0.6f, 0.4f);
			rightClosedDoor = new Quaternion(-0.4f, -0.6f, -0.6f, 0.4f);
		}
		else
		{
			leftClosedDoor = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
			rightClosedDoor = new Quaternion(-0.5f, 0.5f, 0.5f, 0.5f);
		}
	}

	private void Update()
	{
		if (openDoor)
		{
			accumTime += Time.deltaTime;
			leftSheet.transform.rotation = Quaternion.Slerp(leftOpenDoor, leftClosedDoor, accumTime);
			rightSheet.transform.rotation = Quaternion.Slerp(rightOpenDoor, rightClosedDoor, accumTime);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			SoundManager.PlaySound(29);
			openDoor = true;
			collide = true;
		}
	}
}
