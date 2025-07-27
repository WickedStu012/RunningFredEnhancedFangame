using UnityEngine;

public class WingsItem : MonoBehaviour
{
	public static WingsItem Instance;

	public GameObject Shine;

	public GameObject wingsPickup;

	private bool disappear;

	private float accumTime;

	private bool picked;

	private void Start()
	{
		if (!CharHelper.GetProps().HasWings)
		{
			picked = false;
			disappear = false;
			if (Shine != null)
			{
				Shine.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		Instance = this;
		if (picked)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void Update()
	{
		if (disappear)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.1f)
			{
				disappear = false;
				base.gameObject.SetActive(false);
			}
		}
		else if (!picked)
		{
			wingsPickup.transform.Rotate(Vector3.forward, Time.deltaTime * 50f);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!picked && CharHelper.IsColliderFromPlayer(c))
		{
			CharHelper.GetProps().HasWings = true;
			CharHelper.GetCharStateMachine().ShowWings();
			SoundManager.PlaySound(base.transform.position, 73);
			if (Shine != null)
			{
				Shine.SetActive(true);
			}
			disappear = true;
			picked = true;
		}
	}
}
