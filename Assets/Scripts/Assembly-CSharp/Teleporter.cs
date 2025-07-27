using UnityEngine;

[ExecuteInEditMode]
public class Teleporter : MonoBehaviour
{
	public int reusableTimes;

	public GameObject destinationPortal;

	private float accumTime;

	private int usesCount;

	private bool colliding;

	private void Start()
	{
		usesCount = 0;
	}

	private void OnEnable()
	{
		colliding = false;
		if (reusableTimes > 0 && usesCount >= reusableTimes)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			Debug.DrawLine(base.transform.position, destinationPortal.transform.position, Color.green);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!CharHelper.IsColliderFromPlayer(c) || colliding)
		{
			return;
		}
		colliding = true;
		Transform playerTransform = CharHelper.GetPlayerTransform();
		playerTransform.position = destinationPortal.transform.position;
		SoundManager.PlaySound(32);
		CharHelper.GetCharStateMachine().ResetLastYPos();
		ChunkRelocator.ModifyChunkVisibility();
		if (reusableTimes != 0)
		{
			usesCount++;
			if (usesCount == reusableTimes)
			{
				disableTeleporters();
			}
		}
	}

	private void OnTriggerExit(Collider c)
	{
		colliding = false;
	}

	private void disableTeleporters()
	{
		destinationPortal.SetActive(false);
		base.gameObject.SetActive(false);
	}
}
