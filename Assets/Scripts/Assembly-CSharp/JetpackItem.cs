using UnityEngine;

public class JetpackItem : MonoBehaviour
{
	public GameObject Shine;

	private bool disappear;

	private float accumTime;

	private bool picked;

	private void Start()
	{
		picked = false;
		disappear = false;
		if (Shine != null)
		{
			Shine.SetActive(false);
		}
	}

	private void Update()
	{
		if (disappear)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.5f)
			{
				disappear = false;
				base.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!picked && CharHelper.IsColliderFromPlayer(c))
		{
			CharHelper.GetProps().HasJetpack = true;
			CharHelper.GetCharStateMachine().ShowJetpack(true);
			SoundManager.PlaySound(base.transform.position, 18);
			if (Shine != null)
			{
				Shine.SetActive(true);
			}
			base.GetComponent<Renderer>().enabled = false;
			disappear = true;
		}
	}
}
