using UnityEngine;

public class ToothPickSpike : MonoBehaviour
{
	private static bool charHit;

	private bool activated;

	private int updateCounter;

	private Rigidbody rb;

	private bool collide;

	private void Start()
	{
		collide = false;
	}

	private void Update()
	{
		if (!collide && activated)
		{
			updateCounter++;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || updateCounter <= 10 || !CharHelper.IsColliderFromPlayer(c) || charHit)
		{
			return;
		}
		collide = true;
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			charStateMachine.SwitchTo(ActionCode.RAGDOLL);
			string text = string.Empty;
			Transform transform = null;
			if (ConfigParams.useGore)
			{
				RaycastHit hitInfo;
				if (Physics.Raycast(base.transform.position, base.transform.forward, out hitInfo, 20f, 256))
				{
					Transform transformByName = CharHelper.GetTransformByName(hitInfo.transform.name);
					if (transformByName != null)
					{
						text = hitInfo.transform.name;
						transform = transformByName;
					}
				}
				if (transform == null)
				{
					text = ((Random.Range(0, 2) != 0) ? "torso1" : "head");
					transform = CharHelper.GetTransformByName(text);
				}
				base.transform.parent = transform.transform;
				base.transform.localPosition = new Vector3(-0.283f, -0.0165f, 0.178f);
				base.transform.localRotation = Quaternion.Euler(new Vector3(0f, -180f, 90f));
				if (string.Compare(text, "head") == 0)
				{
					CharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD, 10);
				}
				else
				{
					CharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
				}
				transform.GetComponent<Rigidbody>().AddForce(base.transform.forward * 250f);
			}
			rb.isKinematic = true;
			charHit = true;
			GameEventDispatcher.Dispatch(null, new PlayerHittedByArrow());
		}
		else
		{
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
	}

	public void Trigger()
	{
		activated = true;
		rb = base.transform.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.AddForce(base.transform.forward * 2000f);
		rb.AddTorque(new Vector3(-100f, 0f, 0f));
	}

	public void Reset()
	{
		activated = false;
		updateCounter = 0;
		collide = false;
		rb.isKinematic = true;
	}

	public static bool IsCharHit()
	{
		return charHit;
	}
}
