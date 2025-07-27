using UnityEngine;

public class EvnClimbOff : IEvent
{
	public EvnClimbOff()
	{
		code = EventCode.EVN_CLIMB_OFF;
	}

	public override void StateChange()
	{
	}

	public override bool Check()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(sm.playerT.position + Vector3.up * 2.5f, Vector3.forward, out hitInfo, 2f, 73728) && Tag.IsStairs(hitInfo.transform.tag))
		{
			LevelFrontalWall component = hitInfo.transform.gameObject.GetComponent<LevelFrontalWall>();
			if (component == null || component.climb)
			{
				return false;
			}
		}
		return true;
	}
}
