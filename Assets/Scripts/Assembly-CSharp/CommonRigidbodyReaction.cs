using UnityEngine;

public class CommonRigidbodyReaction : MonoBehaviour
{
	private bool collide;

	private float accumTime;

	private void Start()
	{
		collide = false;
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 3f)
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
			base.GetComponent<Rigidbody>().isKinematic = false;
			base.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.3f, 0.3f), 0.2f, 0.7f) * 2000f);
			base.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
			collide = true;
		}
	}
}
