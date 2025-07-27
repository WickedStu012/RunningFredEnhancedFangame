using UnityEngine;

public class IronGate : MonoBehaviour
{
	public bool openAtPlayer = true;

	public GameObject door;

	private Vector3 posOpenDoor;

	private Vector3 posClosedDoor;

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
		posOpenDoor = door.transform.localPosition;
		posClosedDoor = new Vector3(door.transform.localPosition.x, 14f, door.transform.localPosition.z);
	}

	private void Update()
	{
		if (openDoor)
		{
			accumTime += Time.deltaTime;
			door.transform.localPosition = Vector3.Lerp(posOpenDoor, posClosedDoor, accumTime);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			SoundManager.PlaySound(28);
			openDoor = true;
			collide = true;
		}
	}
}
