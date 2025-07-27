using UnityEngine;

public class ActAfterBurnerJump : IAction
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

	public ActAfterBurnerJump(GameObject player)
		: base(player)
	{
		stateName = ActionCode.AFTER_BURNER_JUMP;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		sm.MoveDirection = Vector3.zero;
		accumTime = 0f;
		accumTimeJump = 0f;
		characterAngle = 0f;
		accumAngle = 0f;
		CharAnimManager.Dive();
		maxDiveLength = CharAnimManager.GetDiveLength();
		sm.ConsecutiveJumpCounter++;
		sm.IsGoingUp = true;
		playerT = CharHelper.GetPlayerTransform();
		prevPosY = playerT.position.y;
		SoundManager.PlaySound(57);
		CharHelper.GetEffects().EnableAfterBurnerParticles();
		InputManager.ResetSuperSprintTimer();
		FovAnimator.FovIn();
	}

	public override void GetOut()
	{
		sm.IsGoingUp = false;
		playerT.rotation = Quaternion.identity;
		FovAnimator.FovOut();
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
			sm.SteerDirection += sm.FloorZAngle * 0.01f;
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection * 0.5f, 0f, sm.AccumAccel);
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
		playerT.rotation = Quaternion.Euler(characterAngle, 0f, accumAngle);
	}
}
