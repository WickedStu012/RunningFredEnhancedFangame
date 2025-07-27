using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public enum Mode
	{
		NORMAL = 0,
		TIMED = 1,
		TRIGGERED_MOVEMENT = 2
	}

	public Mode mode;

	public bool isRolling;

	public float rollingSpeed = 10f;

	public GameObject explosion;

	private bool collide;

	private GameObject explosionInstance;

	private float accumTime;

	private bool exploded;

	private float accumTimeForExplosion;

	private bool shouldExplode;

	public float explodeInTime;

	public bool relocateOnPlayerRespawn;

	private Vector3 originalPos;

	private Quaternion originalRot;

	private bool isKin;

	private bool startCalled;

	private bool posAndRotStored;

	private static List<Barrel> instances;

	private void Start()
	{
		if (instances == null)
		{
			instances = new List<Barrel>();
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
			if (base.transform.GetComponent<Rigidbody>() != null)
			{
				isKin = base.transform.GetComponent<Rigidbody>().isKinematic;
			}
			posAndRotStored = true;
		}
		collide = false;
		BarrelManager.CreateIfNecessary();
		startCalled = true;
	}

	private void OnEnable()
	{
		relocate(false);
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
		if (explosionInstance != null)
		{
			Object.Destroy(explosionInstance);
		}
	}

	private void Update()
	{
		if (isRolling && !collide && base.GetComponent<Rigidbody>() != null)
		{
			base.GetComponent<Rigidbody>().AddTorque(new Vector3(0f - rollingSpeed, 0f, 0f));
		}
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 2f && explosionInstance != null)
			{
				Object.Destroy(explosionInstance);
			}
		}
		if (shouldExplode || mode == Mode.TIMED)
		{
			accumTimeForExplosion += Time.deltaTime;
			if (accumTimeForExplosion > explodeInTime)
			{
				Explode();
				shouldExplode = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!exploded && !collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				destroyBarrel(CharHelper.IsColliderFromPlayer(c));
			}
			else
			{
				destroyBarrel(false);
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			collide = true;
		}
	}

	private void OnLevelWasLoaded(int num)
	{
		instances = null;
	}

	private void relocate(bool resetKinVar)
	{
		if (startCalled && relocateOnPlayerRespawn && !exploded)
		{
			base.transform.position = originalPos;
			base.transform.rotation = originalRot;
			if (resetKinVar && base.transform.GetComponent<Rigidbody>() != null)
			{
				base.transform.GetComponent<Rigidbody>().isKinematic = isKin;
			}
			collide = false;
			if (base.GetComponent<Rigidbody>() != null && !base.GetComponent<Rigidbody>().isKinematic)
			{
				base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}
	}

	private void destroyBarrel(bool destroyPlayer)
	{
		if (destroyPlayer)
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.EXPLODE);
		}
		SoundManager.PlaySound(SndId.SND_GORE_IMPACT_GENERIC);
		Explode();
	}

	public void ExplodeInTime(float time)
	{
		shouldExplode = true;
		explodeInTime = time;
	}

	public void Explode()
	{
		if (!exploded)
		{
			base.GetComponent<Collider>().enabled = false;
			base.GetComponent<Renderer>().enabled = false;
			GameObject[] parts = BarrelManager.GetParts1();
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i].SetActive(true);
				parts[i].GetComponent<Rigidbody>().isKinematic = true;
				parts[i].transform.position = base.transform.position;
				parts[i].GetComponent<Rigidbody>().isKinematic = false;
				parts[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
				parts[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				parts[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(200f, 400f), Random.Range(0f, 400f), Random.Range(200f, 400f)));
				parts[i].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
			}
			GameObject[] parts2 = BarrelManager.GetParts2();
			for (int j = 0; j < parts2.Length; j++)
			{
				parts2[j].SetActive(true);
				parts2[j].GetComponent<Rigidbody>().isKinematic = true;
				parts2[j].transform.position = base.transform.position;
				parts2[j].GetComponent<Rigidbody>().isKinematic = false;
				parts2[j].GetComponent<Rigidbody>().velocity = Vector3.zero;
				parts2[j].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				parts2[j].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(200f, 400f), Random.Range(200f, 400f), Random.Range(200f, 400f)));
				parts2[j].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
			}
			explosionInstance = Object.Instantiate(explosion, base.transform.position, Quaternion.identity) as GameObject;
			explosionInstance.name = "Barrel Explosion";
			exploded = true;
			if (base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().Play();
			}
			exploteNearBarrels();
		}
	}

	private void exploteNearBarrels()
	{
		Vector3 position = base.transform.position;
		Barrel[] array = Object.FindObjectsOfType(typeof(Barrel)) as Barrel[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != this && MathUtil.InsideDistance(position, array[i].transform.position, 10f))
			{
				array[i].ExplodeInTime(0.1f);
			}
		}
	}

	public void EnableRigidBody()
	{
		if (base.transform.GetComponent<Rigidbody>() != null)
		{
			base.transform.GetComponent<Rigidbody>().isKinematic = false;
			base.transform.GetComponent<Rigidbody>().WakeUp();
		}
	}

	public static void Relocate()
	{
		if (instances == null)
		{
			return;
		}
		for (int i = 0; i < instances.Count; i++)
		{
			if (instances[i].enabled)
			{
				instances[i].relocate(true);
			}
		}
	}
}
