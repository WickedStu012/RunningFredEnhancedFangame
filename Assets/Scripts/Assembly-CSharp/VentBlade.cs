using System.Collections.Generic;
using UnityEngine;

public class VentBlade : MonoBehaviour
{
	public Vent vent;

	private bool collide;

	private float accumTime;

	private List<Transform> points;

	private ObstacleFallBack objFB;

	private void Start()
	{
		if (vent == null && base.transform.parent != null)
		{
			vent = base.transform.parent.GetComponent<Vent>();
		}
		getSpikePoints();
		objFB = GetComponent<ObstacleFallBack>();
		if (objFB != null)
		{
			objFB.enabled = false;
		}
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 2f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
		{
			if (points != null)
			{
				Vector3 position = CharHelper.GetPlayerTransform().position;
				Transform nearPosition = getNearPosition(position);
				float distanceToPlayer = Vector3.Distance(nearPosition.position, position);
				vent.TouchPlayer(nearPosition.position, distanceToPlayer);
			}
			else
			{
				vent.TouchPlayer(Vector3.zero, 0f);
			}
		}
		else
		{
			SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
		}
		collide = true;
	}

	private void getSpikePoints()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>(true);
		points = null;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(componentsInChildren[i].name, "point", true) == 0)
			{
				if (points == null)
				{
					points = new List<Transform>();
				}
				points.Add(componentsInChildren[i]);
			}
		}
	}

	private Transform getNearPosition(Vector3 playerPos)
	{
		int index = 0;
		float num = Vector3.Distance(points[0].transform.position, playerPos);
		for (int i = 1; i < points.Count; i++)
		{
			float num2 = Vector3.Distance(points[i].transform.position, playerPos);
			if (num2 < num)
			{
				num = num2;
				index = i;
			}
		}
		return points[index].transform;
	}

	public void SetActiveFB(bool val)
	{
		if (objFB != null)
		{
			objFB.enabled = val;
		}
	}
}
