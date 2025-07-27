using UnityEngine;

public class Treasure : MonoBehaviour
{
	public GameObject Shine;

	public bool rotate = true;

	public int GoldCount = 10;

	public int Id;

	private bool exploding;

	private float accumTime;

	private bool picked;

	private Transform transf;

	private Renderer[] thisRenderers;

	private TreasurePickup treasurePickup = new TreasurePickup();

	private void Awake()
	{
		picked = false;
		transf = base.transform;
		thisRenderers = GetComponentsInChildren<Renderer>();
		if (PlayerAccount.Instance != null && !PlayerAccount.Instance.IsTreasureActive(Id))
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (exploding)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.5f)
			{
				exploding = false;
				base.gameObject.SetActive(false);
			}
		}
		if (rotate)
		{
			transf.localRotation = RotatorManager.angle * new Quaternion(0f, -0.7f, 0f, 0.7f);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (picked)
		{
			return;
		}
		if (CharHelper.IsColliderFromPlayer(c))
		{
			SoundManager.PlaySound(base.transform.position, 3);
			treasurePickup.GoldCount = GoldCount;
			treasurePickup.Id = Id;
			GameEventDispatcher.Dispatch(this, treasurePickup);
			if (Shine != null)
			{
				GameObject gameObject = Object.Instantiate(Shine, base.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
				gameObject.transform.parent = base.transform;
			}
			Renderer[] array = thisRenderers;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = false;
			}
			accumTime = 0f;
			exploding = true;
			picked = true;
		}
		else
		{
			Debug.Log(string.Format("Collider: {0} Name: {1}", c.name, c.gameObject.name));
		}
	}
}
