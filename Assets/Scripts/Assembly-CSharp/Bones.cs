using UnityEngine;

public class Bones : MonoBehaviour
{
	public Transform[] bones;

	private bool collide;

	private void Start()
	{
		collide = false;
		if (bones == null)
		{
			base.enabled = false;
		}
		if (Profile.LessThan(PerformanceScore.GOOD))
		{
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].GetComponent<Collider>().enabled = false;
				bones[i].GetComponent<Rigidbody>().isKinematic = true;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		collide = true;
		CharHelper.GetCharStateMachine().SwitchTo(ActionCode.TRIP);
		if (Profile.GreaterThan(PerformanceScore.GOOD))
		{
			for (int i = 0; i < bones.Length; i++)
			{
				bones[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-200f, 200f), Random.Range(100f, 200f), Random.Range(100f, 200f)));
				bones[i].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-200f, 200f), Random.Range(-200f, 200f), Random.Range(-200f, 200f)));
			}
		}
	}
}
