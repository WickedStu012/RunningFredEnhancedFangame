using UnityEngine;

public class ActDiveRagdoll : IAction
{
	private const float TIME_TO_SLEEP = 5f;

	private Rigidbody[] rbs;

	private BoxCollider[] bcs;

	private CapsuleCollider[] ccs;

	private SphereCollider[] scs;

	private CharacterJoint[] cjs;

	private float accumTime;

	private bool isSleep;

	private Vector3[] rbsPos;

	private Quaternion[] rbsRot;

	public ActDiveRagdoll(GameObject player)
		: base(player)
	{
		stateName = ActionCode.DIVE;
		rbs = player.GetComponentsInChildren<Rigidbody>();
		rbsPos = new Vector3[rbs.Length];
		rbsRot = new Quaternion[rbs.Length];
		for (int i = 0; i < rbs.Length; i++)
		{
			rbsPos[i] = rbs[i].transform.localPosition;
			rbsRot[i] = rbs[i].transform.localRotation;
		}
		bcs = player.GetComponentsInChildren<BoxCollider>();
		ccs = player.GetComponentsInChildren<CapsuleCollider>();
		scs = player.GetComponentsInChildren<SphereCollider>();
		enableRagdoll(false);
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.StopAll();
		cc.enabled = false;
		enableRagdoll(true);
		isSleep = false;
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		if (!isSleep)
		{
			accumTime += dt;
			if (accumTime > 5f)
			{
				sleep();
			}
		}
	}

	private void sleep()
	{
		isSleep = true;
		for (int i = 0; i < rbs.Length; i++)
		{
			rbs[i].Sleep();
		}
	}

	private void enableRagdoll(bool en)
	{
		if (!en)
		{
			cc.height = 2.1f;
		}
		else
		{
			cc.height = 0.6f;
		}
		for (int i = 0; i < rbs.Length; i++)
		{
			rbs[i].isKinematic = !en;
		}
		for (int j = 0; j < bcs.Length; j++)
		{
			bcs[j].isTrigger = !en;
		}
		for (int k = 0; k < ccs.Length; k++)
		{
			ccs[k].isTrigger = !en;
		}
		for (int l = 0; l < scs.Length; l++)
		{
			scs[l].isTrigger = !en;
		}
		if (!en)
		{
			for (int m = 0; m < rbs.Length; m++)
			{
				rbs[m].transform.localPosition = rbsPos[m];
				rbs[m].transform.localRotation = rbsRot[m];
			}
		}
	}
}
