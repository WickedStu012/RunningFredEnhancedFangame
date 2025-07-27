using UnityEngine;

public class ActBurnt : IAction
{
	private enum State
	{
		RUN_BURN = 0,
		DIE_BURN = 1,
		RAGDOLL = 2
	}

	private const float minAccelK = 0.55f;

	private const float maxAccelK = 1f;

	private const float gravityK = 0.55f;

	private float accumTime;

	private Quaternion targetRotation;

	private CharProps props;

	private State state;

	private float burnEndAnimLength;

	private int sndCrispy;

	private CharSkin skin;

	private float dt;

	public ActBurnt(GameObject player)
		: base(player)
	{
		stateName = ActionCode.BURNT;
		props = CharHelper.GetProps();
		skin = player.GetComponent<CharSkin>();
		burnEndAnimLength = CharAnimManager.GetBurnEndLength();
	}

	public override bool CanGetIn()
	{
		return true;
	}

	public override void GetIn(params object[] list)
	{
		if (ConfigParams.useGore)
		{
			skin.SetSkin(CharSkinType.CHARRED);
		}
		CharAnimManager.Burnt();
		accumTime = 0f;
		state = State.RUN_BURN;
		sndCrispy = SoundManager.PlaySound(SndId.SND_GORE_CRISPY);
		SoundManager.FadeOutSound(sndCrispy, 2f, null);
		CharHelper.GetEffects().EnableBurnParticles();
		if (ConfigParams.useGore)
		{
			SoundManager.PlaySound(SndId.SND_FRED_EXTREME_PAIN);
		}
		GameEventDispatcher.Dispatch(null, new PlayerWasBurnt());
	}

	public override void GetOut()
	{
	}

	public override void Update(float dt)
	{
		this.dt = dt;
		switch (state)
		{
		case State.RUN_BURN:
			accumTime += dt;
			if (accumTime > 0.5f && !sm.IsGrounded)
			{
				sm.SwitchTo(ActionCode.RAGDOLL);
				state = State.RAGDOLL;
			}
			if (accumTime > 1.5f)
			{
				accumTime = 0f;
				CharAnimManager.BurnEnd();
				state = State.DIE_BURN;
			}
			MovementHelper.CheckMoveActions(sm, ref accumTime, ref targetRotation);
			steerCharacter();
			moveCharacter();
			break;
		case State.DIE_BURN:
			if (!sm.IsGrounded)
			{
				sm.MoveDirection = Vector3.zero;
				sm.SwitchTo(ActionCode.RAGDOLL);
				state = State.RAGDOLL;
			}
			accumTime += dt;
			if (accumTime > burnEndAnimLength)
			{
				if (ConfigParams.useGore)
				{
					SoundManager.PlaySound(SndId.SND_FRED_FINAL_GASP);
				}
				sm.MoveDirection = Vector3.zero;
				sm.SwitchTo(ActionCode.RAGDOLL);
				state = State.RAGDOLL;
			}
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
		sm.AccumAccel = Mathf.Clamp(sm.AccumAccel + sm.FloorNormalZ * 0.01f, 0.55f, 1f);
		sm.MoveDirection = new Vector3(sm.SteerDirection, sm.MoveDirection.y + Physics.gravity.y * 0.55f * dt, sm.AccumAccel);
		cc.Move(sm.MoveDirection * dt * props.RunningAcceleration);
	}
}
