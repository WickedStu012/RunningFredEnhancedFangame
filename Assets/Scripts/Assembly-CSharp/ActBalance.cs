using UnityEngine;

public class ActBalance : IAction
{
	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private float dt;

	public ActBalance(GameObject player)
		: base(player)
	{
		stateName = ActionCode.BALANCE;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.Balance();
		accumTime = 1f;
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		balanceAnimCheck();
		steerCharacter();
		moveCharacter();
	}

	private void steerCharacter()
	{
		accumTime += dt;
		if (accumTime < 1f)
		{
			playerT.localRotation = Quaternion.Slerp(playerT.localRotation, targetRotation, accumTime);
		}
		else
		{
			playerT.localRotation = targetRotation;
		}
	}

	private void moveCharacter()
	{
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1.001f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}

	private void balanceAnimCheck()
	{
		float num = 0.2f;
		bool flag = true;
		bool flag2 = true;
		RaycastHit hitInfo;
		if (Physics.Raycast(new Vector3(sm.playerT.position.x + num, 1000f, sm.playerT.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 8192))
		{
			flag = sm.playerT.position.y - hitInfo.transform.position.y > 1f;
			if (flag)
			{
				CharAnimManager.BalanceLeft();
				return;
			}
		}
		if (Physics.Raycast(new Vector3(sm.playerT.position.x - num, 1000f, sm.playerT.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 8192))
		{
			flag2 = sm.playerT.position.y - hitInfo.transform.position.y > 1f;
			if (flag2)
			{
				CharAnimManager.BalanceRight();
				return;
			}
		}
		if (flag)
		{
			CharAnimManager.BalanceLeft();
		}
		else if (flag2)
		{
			CharAnimManager.BalanceRight();
		}
		else
		{
			CharAnimManager.Balance();
		}
	}
}
