using UnityEngine;

public class ActSurf : IAction
{
	private const float incAccelK = 1.001f;

	private const float minAccelK = 1f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 5f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private int sndDragId;

	private float dt;

	public ActSurf(GameObject player)
		: base(player)
	{
		stateName = ActionCode.SURF;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 1f;
		CharAnimManager.Surf();
		sndDragId = SoundManager.PlaySound(35);
		CharHelper.GetEffects().EnableDragParticles();
	}

	public override void GetOut()
	{
		SoundManager.StopSound(sndDragId);
		CharHelper.GetEffects().DisableDragParticles();
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
		sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		sm.AccumAccel = 1f;
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 5f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
