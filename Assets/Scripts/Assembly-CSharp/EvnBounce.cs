using UnityEngine;

public class EvnBounce : IEvent
{
	private Vector3 offset = new Vector3(0f, 0f, -0.5f);

	public EvnBounce()
	{
		code = EventCode.EVN_BOUNCE;
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
		RaycastHit hitInfo;
		if (Physics.Raycast(sm.playerT.position + Vector3.up + offset, Vector3.forward, out hitInfo, 1.9f, 65536))
		{
			LevelFrontalWall component = hitInfo.transform.gameObject.GetComponent<LevelFrontalWall>();
			if (component == null || !component.climb)
			{
				return true;
			}
		}
		return false;
	}
}
