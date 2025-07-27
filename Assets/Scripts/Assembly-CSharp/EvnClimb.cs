using UnityEngine;

public class EvnClimb : IEvent
{
	private static int stairId;

	private float accumTime;

	private Vector3 offset = new Vector3(0f, 0f, -0.5f);

	private RaycastHit hit;

	public EvnClimb()
	{
		code = EventCode.EVN_CLIMB;
		stairId = 0;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		if (GameManager.IsFredDead())
		{
			return false;
		}
		if (stairId != 0)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.4f)
			{
				stairId = 0;
			}
		}
		if (Physics.Raycast(sm.playerT.position + Vector3.up + offset, Vector3.forward, out hit, 2f, 73728) && Tag.IsStairs(hit.transform.tag))
		{
			LevelFrontalWall component = hit.transform.gameObject.GetComponent<LevelFrontalWall>();
			if (component == null)
			{
				if (stairId != hit.collider.GetInstanceID())
				{
					accumTime = 0f;
					stairId = hit.collider.GetInstanceID();
					sm.playerT.position = new Vector3(hit.transform.position.x, sm.playerT.position.y, hit.point.z - 0.2f);
					sm.playerT.transform.LookAt(new Vector3(hit.transform.position.x, sm.playerT.position.y, hit.transform.position.z), Vector3.up);
					return true;
				}
			}
			else if (component.climb)
			{
				stairId = 0;
				sm.playerT.position = new Vector3(sm.playerT.position.x, sm.playerT.position.y, hit.point.z - 0.6f);
				sm.playerT.transform.LookAt(new Vector3(sm.playerT.position.x, sm.playerT.position.y, hit.transform.position.z), Vector3.up);
				return true;
			}
		}
		return false;
	}
}
