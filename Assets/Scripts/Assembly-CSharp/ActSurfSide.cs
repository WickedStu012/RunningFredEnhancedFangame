using UnityEngine;

public class ActSurfSide : IAction
{
	private const float incAccelK = 1.001f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 5f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private bool moveToLeft = true;

	private int sndDragId;

	private float dt;

	public ActSurfSide(GameObject player)
		: base(player)
	{
		stateName = ActionCode.SURF_SIDE;
		props = CharHelper.GetProps();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		accumTime = 1f;
		moveToLeft = sm.FloorZAngle < -10f;
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
		moveCharacter();
		playerT.rotation = Quaternion.LookRotation(new Vector3(sm.MoveDirection.x, 0f, sm.MoveDirection.z));
	}

	private void moveCharacter()
	{
		sm.SteerDirection = sm.SteerDirection * 0.2f + ((!moveToLeft) ? 0.2f : (-0.2f));
		sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		sm.AccumAccel = 0.55f;
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 5f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
