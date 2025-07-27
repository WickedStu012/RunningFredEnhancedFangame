using UnityEngine;

public class Skeleton : MonoBehaviour
{
	public enum Mode
	{
		RAGDOLL_ENABLED = 0,
		RAGDOLL_ENABLED_BY_TRIGGER = 1,
		RADGOLL_DISABLED = 2
	}

	public Mode mode;

	private bool collide;

	private Rigidbody[] rbs;

	private BoxCollider[] bcs;

	private CapsuleCollider[] ccs;

	private SphereCollider[] scs;

	private CharacterJoint[] cjs;

	private void Start()
	{
		rbs = base.transform.GetComponentsInChildren<Rigidbody>();
		bcs = base.transform.GetComponentsInChildren<BoxCollider>();
		ccs = base.transform.GetComponentsInChildren<CapsuleCollider>();
		scs = base.transform.GetComponentsInChildren<SphereCollider>();
		if (mode == Mode.RAGDOLL_ENABLED_BY_TRIGGER || mode == Mode.RADGOLL_DISABLED)
		{
			enableRagdoll(false);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (mode == Mode.RAGDOLL_ENABLED_BY_TRIGGER && !collide && CharHelper.IsColliderFromPlayer(c))
		{
			enableRagdoll(true);
			collide = true;
		}
	}

	private void enableRagdoll(bool en)
	{
		for (int i = 0; i < rbs.Length; i++)
		{
			rbs[i].isKinematic = !en;
		}
		for (int j = 0; j < bcs.Length; j++)
		{
			if (bcs[j] != base.GetComponent<Collider>())
			{
				bcs[j].isTrigger = !en;
			}
		}
		for (int k = 0; k < ccs.Length; k++)
		{
			ccs[k].isTrigger = !en;
		}
		for (int l = 0; l < scs.Length; l++)
		{
			scs[l].isTrigger = !en;
		}
	}
}
