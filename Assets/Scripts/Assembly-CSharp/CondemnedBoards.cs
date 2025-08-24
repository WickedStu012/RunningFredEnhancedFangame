using UnityEngine;

public class CondemnedBoards : MonoBehaviour
{
	public GameObject[] brokenParts;

	public GameObject boardPS;

	public int skulliesToThrow;

	public bool useSilverSkullies = true;

	public float maxSpeedToBounce = 22f;

	private bool collide;

	private ParticleSystem[] ps;

	private bool exploded;

	private GameObject[] skullies;

	private void Start()
	{
		exploded = false;
		for (int i = 0; i < brokenParts.Length; i++)
		{
			brokenParts[i].SetActive(false);
		}
		if (skulliesToThrow > 0)
		{
			skullies = new GameObject[skulliesToThrow];
			GameObject original = Resources.Load((!useSilverSkullies) ? "Pickups/GoldSkully" : "Pickups/SilverSkully") as GameObject;
			for (int j = 0; j < skulliesToThrow; j++)
			{
				skullies[j] = Object.Instantiate(original) as GameObject;
				skullies[j].transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + (float)(j * 2));
				skullies[j].transform.rotation = Quaternion.identity;
				skullies[j].SetActive(false);
			}
		}
		if (boardPS != null)
		{
			ps = boardPS.transform.GetComponentsInChildren<ParticleSystem>();
			boardPS.SetActive(false);
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			SoundManager.PlaySound(SndId.SND_WOOD_CRACK);
			boardPS.SetActive(true);
			for (int i = 0; i < ps.Length; i++)
			{
				ps[i].Emit = true;
			}
			if (base.GetComponent<Renderer>() != null)
			{
				base.GetComponent<Renderer>().enabled = false;
			}
			Explode();
			if (skulliesToThrow > 0)
			{
				throwSkullies();
			}
			if (charStateMachine.Speed < maxSpeedToBounce)
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.STAGGER);
			}
			collide = true;
		}
	}

	public void Explode()
	{
		if (exploded)
		{
			return;
		}
		base.GetComponent<Collider>().enabled = false;
		for (int i = 0; i < brokenParts.Length; i++)
		{
			brokenParts[i].SetActive(true);
			brokenParts[i].GetComponent<Renderer>().enabled = true;
			if (brokenParts[i].GetComponent<Rigidbody>() != null)
			{
				brokenParts[i].GetComponent<Rigidbody>().isKinematic = true;
				brokenParts[i].transform.position = base.transform.position + Vector3.up * 2f;
				brokenParts[i].GetComponent<Rigidbody>().isKinematic = false;
				brokenParts[i].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
				brokenParts[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				brokenParts[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(200f, 400f), Random.Range(0f, 400f), Random.Range(400f, 800f)));
				brokenParts[i].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
			}
		}
		exploded = true;
	}

	private void throwSkullies()
	{
		for (int i = 0; i < skullies.Length; i++)
		{
			skullies[i].SetActive(true);
		}
	}
}
