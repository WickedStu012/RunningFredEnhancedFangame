using UnityEngine;

public class SpikeRoll : MonoBehaviour
{
	private bool collide;

	private Vector3[] points;

	private float accumTimeReset;

	private void Start()
	{
		collide = false;
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		points = new Vector3[componentsInChildren.Length - 1];
		int num = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "point")
			{
				points[num++] = componentsInChildren[i].position;
				componentsInChildren[i].gameObject.SetActive(false);
			}
		}
	}

	private void OnDisable()
	{
		FixedJoint component = base.gameObject.GetComponent<FixedJoint>();
		if (component != null)
		{
			Object.Destroy(component);
		}
	}

	private void Update()
	{
		if (!collide)
		{
			return;
		}
		accumTimeReset += Time.deltaTime;
		if (accumTimeReset >= 5f)
		{
			FixedJoint component = base.gameObject.GetComponent<FixedJoint>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			accumTimeReset = 0f;
			collide = false;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.transform.gameObject.layer == 9 || c.transform.gameObject.layer == 13 || collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			if (ConfigParams.useGore)
			{
				Vector3 nearPosition = getNearPosition(CharHelper.GetPlayerTransform().position);
				FixedJoint fixedJoint = base.gameObject.GetComponent<FixedJoint>();
				if (fixedJoint == null)
				{
					fixedJoint = base.gameObject.AddComponent<FixedJoint>();
				}
				Transform transformByName = CharHelper.GetCharStateMachine().GetTransformByName("head");
				if (transformByName != null)
				{
					fixedJoint.connectedBody = transformByName.GetComponent<Rigidbody>();
				}
				CharHelper.GetCharSkin().Pierced(nearPosition);
			}
			else
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DIE_IMPCT);
			}
		}
		else
		{
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
		collide = true;
	}

	private Vector3 getNearPosition(Vector3 playerPos)
	{
		if (points == null || points.Length == 0)
		{
			return base.transform.position;
		}
		int num = 0;
		float num2 = Vector3.Distance(points[0], playerPos);
		for (int i = 1; i < points.Length; i++)
		{
			float num3 = Vector3.Distance(points[i], playerPos);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return points[num];
	}
}
