using UnityEngine;

public class ActGlide : IAction
{
	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.1f;

	private float incAccelK = 1f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private FredCamera fredCam;

	private float dt;

	public ActGlide(GameObject player)
		: base(player)
	{
		stateName = ActionCode.GLIDING;
		props = CharHelper.GetProps();
		if (Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
	}

	public override bool CanGetIn()
	{
		if (Mathf.Abs(sm.FloorYPos - sm.transform.position.y) < 1f)
		{
			return false;
		}
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 0f;
		CharAnimManager.GlideLoop();
		if (fredCam == null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		fredCam.SwitchMode(FredCamera.Mode.GLIDE);
	}

	public override void GetOut()
	{
		fredCam.SwitchMode(FredCamera.Mode.NORMAL);
	}

	public override void OnGUI()
	{
		if (sm.GlideTimeLeft > 0f)
		{
			float num = sm.GlideTimeLeft / props.GlideMaxTime;
			float num2 = (float)Screen.width * 0.2f * num;
			if (num2 > 10f)
			{
				GUI.Box(new Rect((float)Screen.width * 0.4f, (float)Screen.height * 0.9f, num2, 20f), string.Empty);
			}
		}
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		sm.GlideTimeLeft -= dt;
		if (sm.GlideTimeLeft > 0f)
		{
			MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation, true);
			steerCharacter();
			moveCharacter();
		}
		else
		{
			sm.SwitchTo(ActionCode.DRAMATIC_JUMP);
		}
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
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * incAccelK, 0.55f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.1f * dt * 0.5f, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
