using System;
using UnityEngine;

public class ActBouncerJump : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private int bouncerForce = 1;

	private float prevPosY;

	private float maxDiveLength;

	private float accumTimeDiveLength;

	private Vector3 bouncerNormal;

	private float dt;

	public ActBouncerJump(GameObject player)
		: base(player)
	{
		stateName = ActionCode.BOUNCER_JUMP;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		if (list != null)
		{
			bouncerForce = Convert.ToInt32(list[0]);
			bouncerNormal = (Vector3)list[1];
		}
		sm.MoveDirection = Vector3.zero;
		accumTime = 0f;
		accumTimeJump = 0f;
		CharAnimManager.Dive();
		maxDiveLength = CharAnimManager.GetDiveLength();
		SoundManager.PlaySound(0);
		sm.ConsecutiveJumpCounter++;
		sm.IsGoingUp = true;
		sm.ResetLastYPos();
		playerT = CharHelper.GetPlayerTransform();
		prevPosY = playerT.position.y;
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		steerCharacter();
		accumTimeJump += dt;
		if (accumTimeJump < props.MaxJumpTime)
		{
			accumTimeDiveLength += dt;
			if (accumTimeDiveLength > maxDiveLength)
			{
				CharAnimManager.DiveLoop();
			}
			moveCharacterInJump(true);
			return;
		}
		if (playerT.position.y < prevPosY)
		{
			if (sm.IsGoingUp)
			{
				CharAnimManager.BlendToDramaticJump();
			}
			sm.IsGoingUp = false;
			sm.SwitchTo(ActionCode.DRAMATIC_JUMP);
		}
		prevPosY = playerT.position.y;
		moveCharacterInJump(false);
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

	private void moveCharacterInJump(bool stayInJump)
	{
		float num = 0f;
		if (stayInJump)
		{
			num = 6.5f * (float)bouncerForce;
			sm.SteerDirection += sm.FloorZAngle * 0.01f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num * dt, vector.z) + new Vector3(bouncerNormal.x, 0f, bouncerNormal.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
