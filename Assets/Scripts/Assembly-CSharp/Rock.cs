using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
	public enum Mode
	{
		NORMAL = 0,
		TRIGGERED_MOVEMENT = 1
	}

	public enum Direction
	{
		Left = 0,
		Right = 1,
		Forward = 2,
		Back = 3
	}

	public Mode mode;

	public bool isRolling = true;

	public float rollingSpeed = 50f;

	public Direction rollingDirection = Direction.Back;

	public bool relocateOnPlayerRespawn;

	private bool collide;

	private Vector3 direction;

	private Vector3 originalPos;

	private Quaternion originalRot;

	private bool isKin;

	private bool startCalled;

	private bool posAndRotStored;

	private static List<Rock> instances;

	private bool collideWithFloor;

	private float accumTimeCollideWithFloor;

	private void Start()
	{
		if (instances == null)
		{
			instances = new List<Rock>();
		}
		else if (!instances.Contains(this))
		{
			instances.Add(this);
		}
		if (mode == Mode.TRIGGERED_MOVEMENT)
		{
			base.transform.GetComponent<Rigidbody>().isKinematic = true;
		}
		if (!posAndRotStored)
		{
			originalPos = base.transform.position;
			originalRot = base.transform.rotation;
			isKin = base.transform.GetComponent<Rigidbody>().isKinematic;
			posAndRotStored = true;
		}
		collide = false;
		startCalled = true;
	}

	private void OnEnable()
	{
		relocate(false);
		switch (rollingDirection)
		{
		case Direction.Left:
			direction = Vector3.forward;
			break;
		case Direction.Right:
			direction = Vector3.back;
			break;
		case Direction.Forward:
			direction = Vector3.right;
			break;
		case Direction.Back:
			direction = Vector3.left;
			break;
		}
	}

	private void OnDisable()
	{
		if (instances != null)
		{
			if (instances.Contains(this))
			{
				instances.Remove(this);
			}
			if (instances.Count == 0)
			{
				instances = null;
			}
		}
	}

	private void OnLevelWasLoaded(int num)
	{
		instances = null;
	}

	private void Update()
	{
		if (isRolling && !collide)
		{
			base.GetComponent<Rigidbody>().AddTorque(direction * rollingSpeed);
		}
		if (collideWithFloor)
		{
			accumTimeCollideWithFloor += Time.deltaTime;
			if (accumTimeCollideWithFloor > 1f)
			{
				accumTimeCollideWithFloor = 0f;
				collideWithFloor = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!base.transform.GetComponent<Rigidbody>().isKinematic && !collide && CharHelper.IsColliderFromPlayer(c) && base.GetComponent<Rigidbody>().velocity.magnitude > 0.5f)
		{
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				CharHelper.GetCharStateMachine().Hit(-1f * new Vector3(Random.Range(-10, 10), 0f, Random.Range(-40, -80)));
				SoundManager.PlaySound(SndId.SND_GORE_IMPACT_GENERIC);
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			collide = true;
		}
	}

	private void OnCollisionEnter(Collision ci)
	{
		if (((!collideWithFloor && ci.gameObject.layer == 9) || ci.gameObject.layer == 13) && ci.relativeVelocity.y > 8f)
		{
			SoundManager.PlaySound(52);
			ScreenShaker.Shake(0.5f, 8f);
			collideWithFloor = true;
		}
	}

	private void relocate(bool resetKinVar)
	{
		if (startCalled && relocateOnPlayerRespawn)
		{
			base.transform.position = originalPos;
			base.transform.rotation = originalRot;
			if (resetKinVar)
			{
				base.transform.GetComponent<Rigidbody>().isKinematic = isKin;
			}
			collide = false;
			if (!base.GetComponent<Rigidbody>().isKinematic)
			{
				base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}
	}

	public void EnableRigidBody()
	{
		base.transform.GetComponent<Rigidbody>().isKinematic = false;
	}

	public static void Relocate()
	{
		if (instances == null)
		{
			return;
		}
		for (int i = 0; i < instances.Count; i++)
		{
			if (instances[i] != null && instances[i].enabled)
			{
				instances[i].relocate(true);
			}
		}
	}
}
