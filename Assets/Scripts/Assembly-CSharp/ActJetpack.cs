using UnityEngine;

public class ActJetpack : IAction
{
	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.2f;

	private const float iniGravityK = -0.05f;

	private float incAccelK = 1.01f;

	private float gravityK = -0.05f;

	private float accumTime;

	private float accumTimeJetPackRot;

	private Quaternion targetRotation;

	private CharProps props;

	private Jetpack jetpack;

	private int sndId;

	private float dt;

	public ActJetpack(GameObject player)
		: base(player)
	{
		stateName = ActionCode.JETPACK;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 1f;
		accumTimeJetPackRot = 0f;
		sm.MoveDirection = Vector3.zero;
		sm.ConsecutiveJumpCounter = 0;
		sm.ResetLastYPos();
		CharAnimManager.Jetpack();
		GameObject gameObject = sm.GetJetpack();
		if (gameObject != null)
		{
			jetpack = gameObject.GetComponent<Jetpack>();
			jetpack.EnableNormal();
		}
		else
		{
			Debug.LogError("Cannot find the jetpack");
		}
		if (JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.StartUse(false); // Normal mode
		}
		MovementHelper.CalibrateXValue();
		sndId = SoundManager.PlaySound(SndId.SND_JETPACK);
	}

	public override void GetOut()
	{
		jetpack.DisableNormal();
		if (JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.StopUse();
		}
		SoundManager.StopSound(sndId);
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		// props.JetPackFuelLeft -= props.JetpackConsumption * dt; // Disabled fuel consumption
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		accumTimeJetPackRot += dt;
		MovementHelper.CheckMoveActionsUpDownInverted(sm, ref accumTimeJetPackRot, ref targetRotation);
		steerCharacter();
		clampSpeed();
		moveCharacter();
	}

	private void clampSpeed()
	{
		if (props.MaxJetpackSpeed <= sm.Speed && sm.Speed <= props.MaxJetpackSpeed * 1.1f)
		{
			incAccelK = 1f;
			gravityK = 0f;
		}
		else if (sm.Speed > props.MaxJetpackSpeed * 1.1f)
		{
			incAccelK = 0.995f;
			gravityK = 0f;
		}
		else
		{
			incAccelK = props.JetpackAccelK * (sm.SteerDirectionUpDown * -1f + 1f);
			gravityK = -0.1f * sm.SteerDirectionUpDown + -0.05f;
		}
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
		if (playerT.position.y >= 45f)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		else
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK, 0.55f, 1.2f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * gravityK * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
