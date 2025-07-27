using UnityEngine;

public class Armor : MonoBehaviour
{
	private bool collide;

	private Rigidbody[] rbs;

	private void Start()
	{
		collide = false;
		rbs = base.gameObject.GetComponentsInChildren<Rigidbody>();
		BoxCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoxCollider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (base.gameObject.GetComponent<Collider>() != componentsInChildren[i])
			{
				Physics.IgnoreCollision(base.gameObject.GetComponent<Collider>(), componentsInChildren[i]);
			}
		}
		SphereCollider[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<SphereCollider>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (base.gameObject.GetComponent<Collider>() != componentsInChildren2[j])
			{
				Physics.IgnoreCollision(base.gameObject.GetComponent<Collider>(), componentsInChildren2[j]);
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			for (int i = 0; i < rbs.Length; i++)
			{
				rbs[i].isKinematic = false;
				rbs[i].AddForce(Vector3.forward * 200f);
			}
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			collide = true;
		}
	}
}
