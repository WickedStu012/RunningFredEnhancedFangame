using UnityEngine;

public class EvnWallRunPlatform : IEvent
{
	private const float distance = 1f;

	private RaycastHit hit;

	private Vector3 originPos;

	public EvnWallRunPlatform()
	{
		code = EventCode.EVN_WALL_RUN_PLATFORM;
	}

	public override bool Check()
	{
		originPos = sm.playerT.position + new Vector3(0f, 0.5f, 0f);
		if (Physics.Raycast(originPos, Vector3.right, out hit, 1.5f, 1048576))
		{
			sm.playerT.position = new Vector3(hit.point.x - 1f, sm.playerT.position.y + 0.5f, sm.playerT.position.z);
			return true;
		}
		if (Physics.Raycast(originPos, Vector3.left, out hit, 1.5f, 1048576))
		{
			sm.playerT.position = new Vector3(hit.point.x + 1f, sm.playerT.position.y + 0.5f, sm.playerT.position.z);
			return true;
		}
		return false;
	}
}
