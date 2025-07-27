using UnityEngine;

public class ActRespawn : IAction
{
	private const float incAccelK = 1f;

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1.6f;

	private const float gravityK = 0.25f;

	private CharProps props;

	private float dt;

	private float accumTime;

	private float animLen;

	public ActRespawn(GameObject player)
		: base(player)
	{
		stateName = ActionCode.RESPAWN;
		props = CharHelper.GetProps();
		animLen = CharAnimManager.GetRespawnLength();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		cc.SimpleMove(Vector3.zero);
		sm.SteerDirection = 0f;
		playerT.localRotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f));
		CharAnimManager.Respawn();
		CharBuilderHelper.RebuildChar2();
		accumTime = 0f;
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		accumTime += Time.deltaTime;
		if (accumTime >= animLen)
		{
			CharAnimManager.StopAll();
			sm.SwitchTo(ActionCode.RUNNING);
		}
		moveCharacter();
	}

	private void moveCharacter()
	{
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel * 1f + sm.FloorNormalZ * 0.01f, 0.55f, 1.6f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.25f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
