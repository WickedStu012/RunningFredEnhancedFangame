using UnityEngine;

public class EvnJetpackImmediateWallJump : IEvent
{
	private CharProps props;

	private RaycastHit hit;

	public EvnJetpackImmediateWallJump()
	{
		code = EventCode.EVN_JETPACK_IMMEDIATE_WALL_JUMP;
		hit.point = Vector3.zero;
	}

	public override bool Check()
	{
		bool flag = false;
		if (Physics.Raycast(sm.playerT.position, Vector3.right, out hit, 1f, 10240) && InputManager.GetDirection() < 0f)
		{
			flag = true;
		}
		else if (Physics.Raycast(sm.playerT.position, Vector3.left, out hit, 1f, 9216) && InputManager.GetDirection() > 0f)
		{
			flag = true;
		}
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		if (!flag && props.HasJetpack && InputManager.GetJump())
		{
			return true;
		}
		return false;
	}
}
