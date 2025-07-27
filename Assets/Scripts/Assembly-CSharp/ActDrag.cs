using UnityEngine;

public class ActDrag : IAction
{
	private const float incAccelK = 0.999f;

	private const float minAccelK = 0.4f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 5f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private int sndDragId;

	private float dt;

	public ActDrag(GameObject player)
		: base(player)
	{
		stateName = ActionCode.DRAGGING;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.Drag();
		cc.height = 0.7f;
		cc.center = new Vector3(0f, 0.4f, 0f);
		sm.transform.localRotation = Quaternion.identity;
		accumTime = 1f;
		sndDragId = SoundManager.PlaySound(35);
	}

	public override void GetOut()
	{
		cc.height = 2.1f;
		cc.center = new Vector3(0f, 1.1f, 0f);
		SoundManager.StopSound(sndDragId);
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
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
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 0.999f + sm.FloorNormalZ * 0.01f, 0.4f, 1.6f);
		Vector3 vector = new Vector3(sm.SteerDirection, 0f, sm.AccumAccel);
		vector.Normalize();
		vector *= sm.AccumAccel;
		sm.MoveDirection = new Vector3(vector.x, sm.MoveDirection.y + Physics.gravity.y * 5f * dt, vector.z);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
