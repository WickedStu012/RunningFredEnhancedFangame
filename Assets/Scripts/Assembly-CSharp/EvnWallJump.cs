using UnityEngine;

public class EvnWallJump : IEvent
{
	private RaycastHit hit;

	public EvnWallJump()
	{
		code = EventCode.EVN_WALL_JUMP;
	}

	public override bool Check()
	{
		if (InputManager.GetJump() && !sm.IsGrounded)
		{
			if (Physics.Raycast(sm.playerT.position, Vector3.right, out hit, 1f, 10240) && InputManager.GetDirection() < 0f)
			{
				float num = ((!Tag.IsPlatform(hit.transform.gameObject.tag)) ? 0.5f : 0.75f);
				sm.playerT.position = new Vector3(hit.point.x - num, sm.playerT.position.y, sm.playerT.position.z);
				return true;
			}
			if (Physics.Raycast(sm.playerT.position, Vector3.left, out hit, 1f, 9216) && InputManager.GetDirection() > 0f)
			{
				float num2 = ((!Tag.IsPlatform(hit.transform.gameObject.tag)) ? 0.5f : 0.75f);
				sm.playerT.position = new Vector3(hit.point.x + num2, sm.playerT.position.y, sm.playerT.position.z);
				return true;
			}
		}
		return false;
	}
}
