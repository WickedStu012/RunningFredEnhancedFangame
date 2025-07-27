using UnityEngine;

public class BallistaArrow : MonoBehaviour
{
	private static bool charHit;

	private bool activated;

	private int updateCounter;

	private Rigidbody rb;

	private bool isLockingPos;

	private Vector3 lockPos;

	private Quaternion lockRot;

	private bool collide;

	private void Start()
	{
		isLockingPos = false;
		collide = false;
	}

	private void Update()
	{
		if (!collide)
		{
			if (activated)
			{
				updateCounter++;
			}
		}
		else if (isLockingPos)
		{
			base.transform.position = lockPos;
			base.transform.rotation = lockRot;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || updateCounter <= 10)
		{
			return;
		}
		if (CharHelper.IsColliderFromPlayer(c))
		{
			if (charHit)
			{
				return;
			}
			collide = true;
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			charStateMachine.SwitchTo(ActionCode.RAGDOLL);
			string text = string.Empty;
			Transform transform = null;
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
			if (ConfigParams.useGore)
			{
				if (string.Compare(text, "head") == 0)
				{
					CharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD, 10);
				}
				else
				{
					CharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
				}
			}
			transform.GetComponent<Rigidbody>().AddForce(base.transform.forward * 250f);
			rb.isKinematic = true;
			charHit = true;
			GameEventDispatcher.Dispatch(null, new PlayerHittedByArrow());
		}
		else if (!c.name.StartsWith("arrow"))
		{
			collide = true;
			rb.isKinematic = true;
			isLockingPos = true;
			lockPos = base.transform.position;
			lockRot = base.transform.rotation;
		}
	}

	public void Trigger()
	{
		activated = true;
		rb = base.transform.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.AddForce(base.transform.forward * 4000f);
		rb.AddTorque(new Vector3(-20f, 0f, 0f));
	}

	public void Reset()
	{
		activated = false;
		updateCounter = 0;
		collide = false;
		rb.isKinematic = true;
		isLockingPos = false;
	}

	public static bool IsCharHit()
	{
		return charHit;
	}
}
