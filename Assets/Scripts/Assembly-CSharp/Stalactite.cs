using UnityEngine;

[ExecuteInEditMode]
public class Stalactite : MonoBehaviour
{
	public ParticleSystem[] ps;

	public float FallingSpeed = 50f;

	public GameObject warningPoint;

	public GameObject brokenFloor;

	public bool rayCastForPlaceWarningPoint = true;

	private bool collide;

	private bool collideFloor;

	private bool fallingDown;

	private void Start()
	{
		GetComponent<ObstacleFallBack>().enabled = false;
		for (int i = 0; i < ps.Length; i++)
		{
			ps[i].gameObject.SetActive(false);
		}
		if (brokenFloor != null)
		{
			brokenFloor.SetActive(false);
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, float.PositiveInfinity, 8704))
		{
			if (rayCastForPlaceWarningPoint && warningPoint != null)
			{
				warningPoint.transform.position = new Vector3(warningPoint.transform.position.x, hitInfo.transform.position.y + 0.1f, warningPoint.transform.position.z);
			}
			if (brokenFloor != null)
			{
				brokenFloor.transform.position = new Vector3(brokenFloor.transform.position.x, hitInfo.transform.position.y, brokenFloor.transform.position.z);
			}
		}
	}

	private void Update()
	{
		if (fallingDown)
		{
			base.transform.Translate(Vector3.down * Time.deltaTime * FallingSpeed, Space.World);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collideFloor && (c.gameObject.layer == 9 || c.gameObject.layer == 13))
		{
			SoundManager.PlaySound(52);
			ScreenShaker.Shake(0.6f, 8f);
			for (int i = 0; i < ps.Length; i++)
			{
				ps[i].gameObject.SetActive(true);
				ps[i].Emit = true;
			}
			collideFloor = true;
			fallingDown = false;
			GetComponent<ObstacleFallBack>().enabled = true;
			if (warningPoint != null)
			{
				warningPoint.SetActive(false);
			}
			if (brokenFloor != null)
			{
				brokenFloor.SetActive(true);
				brokenFloor.GetComponent<Renderer>().enabled = true;
			}
			if (base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().Play();
			}
		}
		if (!collide && !collideFloor && CharHelper.IsColliderFromPlayer(c))
		{
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			collide = true;
		}
	}

	public void Trigger()
	{
		fallingDown = true;
	}
}
