using UnityEngine;

public class ActFrozen : IAction
{
	private enum State
	{
		START_FREEZE = 0,
		START_ICE_COLOR_ANIM = 1,
		DEATH = 2
	}

	private const float minAccelK = 0.55f;

	private const float gravityK = 0.55f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private State state;

	private float freezeAnimLength;

	private IceBlock iceBlock;

	private Material characterMat;

	private float dt;

	public ActFrozen(GameObject player)
		: base(player)
	{
		stateName = ActionCode.FROZEN;
		props = CharHelper.GetProps();
		freezeAnimLength = CharAnimManager.GetFreezeLength();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		CharAnimManager.Freeze();
		accumTime = 0f;
		state = State.START_FREEZE;
		SoundManager.PlaySound(68);
		GameEventDispatcher.Dispatch(null, new PlayerFrozen());
	}

	public override void GetOut()
	{
		if (iceBlock != null)
		{
			iceBlock.gameObject.SetActive(false);
		}
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		switch (state)
		{
		case State.START_FREEZE:
			accumTime += dt;
			if (accumTime > freezeAnimLength)
			{
				accumTime = 0f;
				if (iceBlock == null)
				{
					iceBlock = IceManager.GetIceBlock();
					iceBlock.gameObject.transform.parent = sm.playerT;
					iceBlock.gameObject.transform.localPosition = new Vector3(0f, -0.2f, -0.2f);
					iceBlock.gameObject.name = "IceBlock";
					iceBlock.gameObject.SetActive(true);
					iceBlock.ResetBlock();
				}
				else
				{
					iceBlock.gameObject.SetActive(true);
					iceBlock.ResetBlock();
				}
				state = State.START_ICE_COLOR_ANIM;
			}
			MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
			steerCharacter();
			moveCharacter();
			break;
		case State.START_ICE_COLOR_ANIM:
			iceBlock.StartMaterialAnim();
			state = State.DEATH;
			break;
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
		if (cc.isGrounded)
		{
			sm.MoveDirection = new Vector3(sm.SteerDirection, 0f, 0f);
		}
		sm.AccumAccel *= 0.95f;
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.55f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
