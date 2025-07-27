using UnityEngine;

public class TriggerLongAxe : MonoBehaviour
{
	public GameObject trapBlade;

	private bool collide;

	private Quaternion fromAngle;

	private Quaternion toAngle;

	private float accumTime;

	private void Start()
	{
		fromAngle = trapBlade.transform.localRotation;
		toAngle = Quaternion.Euler(-90f, 0f, 0f);
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			trapBlade.transform.localRotation = Quaternion.Slerp(fromAngle, toAngle, accumTime / 0.2f);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			collide = true;
		}
	}
}
