using UnityEngine;

public class EvnWallRunPlatformOff : IEvent
{
	private Vector3 originPos;

	public EvnWallRunPlatformOff()
	{
		code = EventCode.EVN_WALL_RUN_PLATFORM_OFF;
	}

	public override bool Check()
	{
		originPos = sm.playerT.position + new Vector3(0f, 0.5f, 0f);
		if (!Physics.Raycast(originPos, Vector3.right, 1f, 1048576) && !Physics.Raycast(originPos, Vector3.left, 1f, 1048576))
		{
			return true;
		}
		return false;
	}
}
