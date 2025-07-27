using UnityEngine;

public class Ballista : MonoBehaviour
{
	private const float ARROW_RESPAWN_TIME = 2f;

	public GameObject arrow;

	public bool throwToPlayer = true;

	public bool respawnArrowAfterThrow;

	public float minDistanceToPlayer = 64f;

	private float accumTime;

	private bool collide;

	private Vector3 arrowPos;

	private Quaternion arrowRot;

	private bool arrowThrowed;

	private Transform playerT;

	private void Start()
	{
		arrowPos = arrow.transform.localPosition;
		arrowRot = arrow.transform.localRotation;
		collide = false;
		arrowThrowed = false;
		playerT = CharHelper.GetPlayerTransform();
	}

	private void Update()
	{
		if (respawnArrowAfterThrow && arrowThrowed)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 2f)
			{
				resetArrow();
				accumTime = 0f;
				arrowThrowed = false;
			}
		}
		if (arrowThrowed)
		{
			return;
		}
		if (playerT == null)
		{
			playerT = CharHelper.GetPlayerTransform();
		}
		if (playerT == null)
		{
			Debug.LogError("Cannot throw the arrow because the player transform returned by CharHelper is still null.");
			return;
		}
		base.transform.LookAt(playerT);
		if (base.transform.position.z - playerT.position.z <= minDistanceToPlayer)
		{
			throwArrow();
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				base.GetComponent<Rigidbody>().isKinematic = false;
				base.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), Random.Range(10, 40), -10f));
				base.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
				collide = true;
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
		}
	}

	private void throwArrow()
	{
		if (!arrowThrowed)
		{
			BallistaArrow component = arrow.GetComponent<BallistaArrow>();
			if (component != null)
			{
				component.Trigger();
			}
			arrowThrowed = true;
		}
	}

	private void resetArrow()
	{
		if (!BallistaArrow.IsCharHit())
		{
			arrow.transform.localPosition = arrowPos;
			arrow.transform.localRotation = arrowRot;
			arrow.GetComponent<BallistaArrow>().Reset();
		}
	}
}
