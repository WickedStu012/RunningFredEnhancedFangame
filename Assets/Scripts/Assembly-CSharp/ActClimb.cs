using UnityEngine;

public class ActClimb : IAction
{
	private enum State
	{
		GRABBING = 0,
		DONE = 1
	}

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private bool canMoveOnX = true;

	private float animStairsGrabLength;

	private float animAccumTime;

	private State state;

	private FredCamera fredCam;

	private float dt;

	public ActClimb(GameObject player)
		: base(player)
	{
		stateName = ActionCode.CLIMB;
		props = CharHelper.GetProps();
		animStairsGrabLength = CharAnimManager.GetStairsGrabLength();
	}

	private bool isClimbWall()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(sm.playerT.position + Vector3.up, Vector3.forward, out hitInfo, 2f, 73728) && Tag.IsStairs(hitInfo.transform.tag))
		{
			LevelFrontalWall component = hitInfo.transform.gameObject.GetComponent<LevelFrontalWall>();
			return component != null && component.climb;
		}
		return false;
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		animAccumTime = 0f;
		state = State.GRABBING;
		playerT.position = new Vector3(playerT.position.x, playerT.position.y + 0.6f, playerT.position.z);
		if (sm.inertia.x < -0.5f)
		{
			CharAnimManager.WallGrabRight();
		}
		else if (sm.inertia.x > 0.5f)
		{
			CharAnimManager.WallGrabLeft();
		}
		else
		{
			CharAnimManager.StairsGrab();
		}
		canMoveOnX = isClimbWall();
		sm.ConsecutiveJumpCounter = 0;
		sm.ConsecutiveWallJumpCounter = 0;
		if (!canMoveOnX)
		{
			sm.MoveDirection = Vector3.zero;
		}
		if (fredCam == null && Camera.main != null)
		{
			fredCam = Camera.main.GetComponent<FredCamera>();
		}
		if (fredCam != null)
		{
			fredCam.SwitchMode(FredCamera.Mode.CLIMB);
		}
		SoundManager.PlaySound(41);
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		sm.ResetLastYPos();
		this.dt = dt;
		switch (state)
		{
		case State.GRABBING:
			animAccumTime += dt;
			if (animAccumTime >= animStairsGrabLength)
			{
				CharAnimManager.Climb();
				state = State.DONE;
			}
			break;
		case State.DONE:
			if (canMoveOnX)
			{
				MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
			}
			moveCharacter();
			break;
		}
	}

	private void moveCharacter()
	{
		sm.MoveDirection = new Vector3((!canMoveOnX) ? 0f : sm.SteerDirection, 40f * dt, 0f);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
