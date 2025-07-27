using System;
using UnityEngine;

public class ActCatapult : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private float accumTime;

	private Quaternion targetRotation;

	private float accumTimeJump;

	private CharProps props;

	private float bouncerForce = 1f;

	private float prevPosY;

	private float maxDiveLength;

	private float accumTimeDiveLength;

	private Vector3 bouncerNormal;

	private float characterAngle;

	private Vector3 velocity;

	private float dt;

	private float accumAngle;

	public ActCatapult(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CATAPULT;
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
			bouncerForce = Convert.ToSingle(list[0]);
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
		playerT = CharHelper.GetPlayerTransform();
		prevPosY = playerT.position.y;
		characterAngle = 0f;
		accumAngle = 0f;
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
		velocity = sm.GetVelocity();
		accumTimeJump += dt;
		if (accumTimeJump < props.MaxJumpTime)
		{
			accumTimeDiveLength += dt;
			if (accumTimeDiveLength > maxDiveLength)
			{
				CharAnimManager.DiveLoop();
			}
			moveCharacterInJump(true);
		}
		else
		{
			if (playerT.position.y < prevPosY)
			{
				sm.IsGoingUp = false;
			}
			if (Physics.Raycast(playerT.position, Vector3.down, 2f, 12800))
			{
				sm.SwitchTo(ActionCode.DRAGGING);
			}
			else if (velocity.y < -80f)
			{
				sm.SwitchTo(ActionCode.DIVE);
			}
			prevPosY = playerT.position.y;
			moveCharacterInJump(false);
		}
		rotateChar();
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
			num = 6.5f * bouncerForce;
			sm.SteerDirection += sm.FloorZAngle * 0.1f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection * 1.5f, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt + num * dt, vector.z) + new Vector3(bouncerNormal.x * bouncerForce, 0f, bouncerNormal.z * bouncerForce);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}

	private void rotateChar()
	{
		if (velocity.y > 0f)
		{
			characterAngle = Mathf.Atan(velocity.z / velocity.y) * 57.29578f;
		}
		if (sm.SteerDirection < 0f)
		{
			accumAngle += 0.25f;
			if (accumAngle > 2f)
			{
				accumAngle = 2f;
			}
		}
		else
		{
			accumAngle += -0.25f;
			if (accumAngle < -2f)
			{
				accumAngle = -2f;
			}
		}
		if (Application.platform == RuntimePlatform.Android || Application.isEditor)
		{
			characterAngle = Mathf.Clamp(characterAngle, 0f, 40f);
		}
		else
		{
			characterAngle = Mathf.Clamp(characterAngle, 0f, 80f);
		}
		playerT.rotation = Quaternion.Euler(characterAngle, 0f, accumAngle);
	}
}
