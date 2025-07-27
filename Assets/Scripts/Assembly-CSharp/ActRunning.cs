using UnityEngine;

public class ActRunning : IAction
{
	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.6f;

	private float incAccelK = 1f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private FredCamera fredCam;

	private float dt;

	public ActRunning(GameObject player)
		: base(player)
	{
		stateName = ActionCode.RUNNING;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		sm.SteerDirection = 0f;
		accumTime = 1f;
		sm.ConsecutiveJumpCounter = 0;
		sm.ConsecutiveWallJumpCounter = 0;
		sm.ResetLastYPos();
		if (fredCam == null && Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		if (fredCam != null)
		{
			fredCam.SwitchMode(FredCamera.Mode.NORMAL);
		}
		CharAnimManager.Run();
		if (JetpackMeter.Instance != null)
		{
			JetpackMeter.Instance.Recharge();
		}
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
		CharAnimManager.RunAndSprint((sm.Speed - props.MinRunSpeed) / (props.MaxRunSpeed - props.MinRunSpeed), sm.Speed / 18f);
		clampSpeed();
		moveCharacter();
		steerCharacter();
	}

	private void clampSpeed()
	{
		if (props.MaxRunSpeed <= sm.Speed && sm.Speed <= props.MaxRunSpeed * 1.1f)
		{
			incAccelK = 1f;
		}
		else if (sm.Speed > props.MaxRunSpeed * 1.1f)
		{
			incAccelK = 0.995f;
		}
		else
		{
			incAccelK = props.RunningAccelK;
		}
	}

	private void steerCharacter()
	{
		accumTime += dt;
		if (cc.velocity.x != 0f || cc.velocity.z != 0f)
		{
			if (accumTime < 1f)
			{
				playerT.localRotation = Quaternion.Slerp(playerT.localRotation, Quaternion.LookRotation(new Vector3(cc.velocity.x, 0f, cc.velocity.z)), accumTime * 2f);
			}
			else
			{
				playerT.localRotation = Quaternion.LookRotation(new Vector3(cc.velocity.x, 0f, cc.velocity.z));
			}
		}
	}

	private void moveCharacter()
	{
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, (!cc.isGrounded) ? (sm.MoveDirection.y + Physics.gravity.y * 0.6f * dt) : 0f, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
