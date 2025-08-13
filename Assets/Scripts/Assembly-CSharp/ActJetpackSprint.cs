using UnityEngine;

public class ActJetpackSprint : IAction
{
	private enum FovState
	{
		FOV_IN = 0,
		NORMAL = 1,
		FOV_OUT = 2
	}

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = -0.1f;

	private float incAccelK = 1.02f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private float accumTimeSprintLength;

	private Jetpack jetpack;

	private int sndId;

	private float dt;

	private float accumTimeJetPackRot;

	public ActJetpackSprint(GameObject player)
		: base(player)
	{
		stateName = ActionCode.JETPACK_SPRINT;
		props = CharHelper.GetProps();
	}

	public void ResetTimerToReturnToRunning()
	{
		accumTimeSprintLength = 0f;
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 1f;
		CharAnimManager.SuperSprint();
		sm.ResetLastYPos();
		accumTimeSprintLength = 0f;
		accumTimeJetPackRot = 0f;
		sm.ResetLastYPos();
		CharAnimManager.JetpackSprint();
		GameObject jetpackGO = sm.GetJetpack();
		if (jetpackGO != null)
		{
			jetpack = jetpackGO.GetComponent<Jetpack>();
			jetpack.EnableTurbo();
		}
		else
		{
			Debug.LogError("Cannot find the jetpack");
		}
		// Add jetpack meter integration
		if (JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.StartUse(true); // Enable sprint mode
		}
		sndId = SoundManager.PlaySound(SndId.SND_JETPACK);
		FovAnimator.FovIn();
	}

	public override void GetOut()
	{
		if (jetpack != null)
		{
			jetpack.DisableTurbo();
		}
		// Add jetpack meter integration
		if (JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.StopUse();
		}
		SoundManager.StopSound(sndId);
		FovAnimator.FovOut();
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		props.JetPackFuelLeft -= props.JetpackConsumptionSprint * dt;
		accumTimeSprintLength += dt;
		if (accumTimeSprintLength > props.JetpackSuperSprintTime - 0.5f)
		{
			FovAnimator.FovOut();
		}
		if (accumTimeSprintLength > props.SuperSprintTime)
		{
			CharAnimManager.SuperSprintToRun();
			sm.SwitchTo(ActionCode.RUNNING);
		}
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		// Add up/down movement handling for jetpack sprint
		accumTimeJetPackRot += dt;
		MovementHelper.CheckMoveActionsUpDownInverted(sm, ref accumTimeJetPackRot, ref targetRotation);
		clampSpeed();
		steerCharacter();
		moveCharacter();
		
		// Only stop when fuel is actually depleted, not when overheating
		// The jetpack meter handles overheating separately
		if (props.JetPackFuelLeft <= 0f)
		{
			if (jetpack != null)
			{
				jetpack.DisableTurbo();
				jetpack.EnableFuelOut();
			}
			// Reset jetpack meter when sprint ends due to fuel depletion
			if (JetpackMeter.Instance != null)
			{
				JetpackMeter.Instance.StopUse();
			}
			sm.SwitchTo(ActionCode.DRAMATIC_JUMP);
		}
	}

	private void clampSpeed()
	{
		if (props.MaxSprintSpeed <= sm.Speed && sm.Speed <= props.MaxSprintSpeed * 1.1f)
		{
			incAccelK = 1f;
		}
		else if (sm.Speed > props.MaxSprintSpeed * 1.1f)
		{
			incAccelK = 0.98f;
		}
		else
		{
			// Reduce up/down movement acceleration sensitivity
			incAccelK = props.SuperSprintAccelK * (sm.SteerDirectionUpDown * -0.5f + 1f);
		}
	}

	private void steerCharacter()
	{
		accumTime += dt;
		if (accumTime < 1f)
		{
			// Reduce rotation sensitivity for smoother movement
			Quaternion targetRot = targetRotation;
			
			// Limit the X rotation (up/down) to prevent excessive tilting
			Vector3 eulerAngles = targetRot.eulerAngles;
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, -30f, 30f);
			targetRot = Quaternion.Euler(eulerAngles);
			
			playerT.localRotation = Quaternion.Slerp(playerT.localRotation, targetRot, accumTime * 0.5f);
		}
		else
		{
			// Apply the same rotation limits
			Vector3 eulerAngles = targetRotation.eulerAngles;
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, -30f, 30f);
			playerT.localRotation = Quaternion.Euler(eulerAngles);
		}
	}

	private void moveCharacter()
	{
		if (playerT.position.y >= 45f)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		else
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		// Reduce up/down movement sensitivity to prevent excessive vertical movement
		float gravityK = -0.05f * sm.SteerDirectionUpDown + -0.1f;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * gravityK * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
