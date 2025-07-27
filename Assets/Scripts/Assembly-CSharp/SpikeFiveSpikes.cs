using UnityEngine;

public class SpikeFiveSpikes : MonoBehaviour
{
	private enum State
	{
		IDLE = 0,
		RISING = 1
	}

	public bool impalePlayer = true;

	private float accumTime;

	private State state;

	private float initialYPos = -4f;

	public float finalYPos;

	private float moveSpeed;

	private Vector3[] points;

	private float accumTimeReset;

	private bool playerImpaled;

	private void Start()
	{
		state = State.IDLE;
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>(true);
		points = new Vector3[componentsInChildren.Length - 1];
		int num = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "point")
			{
				points[num++] = new Vector3(componentsInChildren[i].position.x, componentsInChildren[i].position.y, componentsInChildren[i].position.z);
				Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
	}

	private void OnDisable()
	{
		playerImpaled = false;
		FixedJoint component = base.transform.parent.gameObject.GetComponent<FixedJoint>();
		if (component != null)
		{
			Object.Destroy(component);
		}
	}

	private void Update()
	{
		if (state == State.RISING)
		{
			accumTime += Time.deltaTime;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(initialYPos, finalYPos, accumTime * moveSpeed), base.transform.localPosition.z);
			if (!(accumTime * moveSpeed >= 1f))
			{
			}
		}
		if (!playerImpaled)
		{
			return;
		}
		accumTimeReset += Time.deltaTime;
		if (accumTimeReset >= 5f)
		{
			FixedJoint component = base.transform.parent.gameObject.GetComponent<FixedJoint>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			accumTimeReset = 0f;
			playerImpaled = false;
		}
	}

	public void Trigger(float spd, bool tryKillPlayer, float distanceToPlayerK)
	{
		if (state == State.RISING)
		{
			return;
		}
		moveSpeed = spd;
		state = State.RISING;
		SoundManager.PlaySound(11);
		Vector3 nearPosition = getNearPosition(CharHelper.GetPlayerTransform().position);
		Vector3 localPosition = CharHelper.GetTransformByName("Armature").transform.localPosition;
		float num = Vector3.Distance(nearPosition, CharHelper.GetPlayerTransform().position + localPosition);
		if (!tryKillPlayer || !(num < distanceToPlayerK))
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			if (ConfigParams.useGore && impalePlayer)
			{
				CharHelper.GetPlayer().transform.position = new Vector3(nearPosition.x, nearPosition.y, nearPosition.z) - localPosition;
				CharHelper.GetCharSkin().Pierced(CharHelper.GetTransformByName("torso1").transform.position);
				FixedJoint fixedJoint = base.transform.parent.gameObject.AddComponent<FixedJoint>();
				fixedJoint.connectedBody = CharHelper.GetTransformByName("torso1").GetComponent<Rigidbody>();
				playerImpaled = true;
				if (Camera.main != null)
				{
					Camera.main.GetComponent<FredCamera>().SetXPosToPlayer();
				}
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
