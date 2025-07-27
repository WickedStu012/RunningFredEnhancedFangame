using UnityEngine;

public class AbyssSpikes : MonoBehaviour
{
	public bool bounceBack = true;

	private static bool collide;

	private static bool jointConnected;

	private static bool usingShield;

	private Vector3[] points;

	private Vector3 spikePos;

	private float accumTimeCollide;

	private float accumTimeShield;

	private void Start()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>(true);
		points = new Vector3[componentsInChildren.Length - 1];
		int num = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "point")
			{
				points[num++] = componentsInChildren[i].position;
				Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
	}

	private void OnEnable()
	{
		collide = false;
		usingShield = false;
	}

	private void OnDisable()
	{
		FixedJoint component = base.gameObject.GetComponent<FixedJoint>();
		if (component != null)
		{
			Object.Destroy(component);
		}
		collide = false;
		usingShield = false;
	}

	private void Update()
	{
		if (ConfigParams.useGore && collide && !jointConnected)
		{
			jointConnected = true;
			Transform transformByName = CharHelper.GetTransformByName("torso1");
			if (transformByName != null)
			{
				FixedJoint fixedJoint = base.gameObject.GetComponent<FixedJoint>();
				if (fixedJoint == null)
				{
					fixedJoint = base.gameObject.AddComponent<FixedJoint>();
				}
				if (fixedJoint != null)
				{
					fixedJoint.connectedBody = transformByName.GetComponent<Rigidbody>();
				}
			}
		}
		if (collide)
		{
			accumTimeCollide += Time.deltaTime;
			if (!GameManager.IsFredDead() && accumTimeCollide > 3f)
			{
				FixedJoint component = base.gameObject.GetComponent<FixedJoint>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				collide = false;
				accumTimeCollide = 0f;
				jointConnected = false;
			}
		}
		if (usingShield)
		{
			accumTimeShield += Time.deltaTime;
			if (accumTimeShield > 1f)
			{
				usingShield = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || usingShield || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			accumTimeCollide = 0f;
			collide = true;
			spikePos = getNearPosition(CharHelper.GetPlayerTransform().position);
			CharHelper.GetPlayer().transform.position = new Vector3(spikePos.x, CharHelper.GetPlayerTransform().position.y, spikePos.z);
			Transform transformByName = CharHelper.GetTransformByName("torso1");
			if (transformByName != null)
			{
				CharHelper.GetCharSkin().Pierced(transformByName.transform.position);
			}
			if (Camera.main != null)
			{
				Camera.main.GetComponent<FredCamera>().SetXPosToPlayer();
			}
		}
		else
		{
			accumTimeShield = 0f;
			usingShield = true;
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			if (bounceBack)
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			else
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE_SPRING);
			}
		}
	}

	private Vector3 getNearPosition(Vector3 playerPos)
	{
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
