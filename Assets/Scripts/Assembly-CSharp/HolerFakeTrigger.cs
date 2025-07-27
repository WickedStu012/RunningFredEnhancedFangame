using UnityEngine;

public class HolerFakeTrigger : MonoBehaviour
{
	private Holer holer;

	private bool collide;

	private float accumTime;

	private void Start()
	{
		holer = base.transform.parent.transform.GetComponent<Holer>();
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 1f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			collide = true;
			if (holer != null)
			{
				holer.TriggerFake();
			}
		}
	}
}
