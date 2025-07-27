using UnityEngine;

public class Holer : MonoBehaviour
{
	private enum State
	{
		IDLE = 0,
		FALLING = 1,
		WAITING = 2,
		UP = 3
	}

	public HolerSpikes spikes;

	public float spikesSpeed = 10f;

	private bool collide;

	private float accumTime;

	private State state;

	private bool killPlayer;

	private void Start()
	{
		collide = false;
		state = State.IDLE;
	}

	private void Update()
	{
		switch (state)
		{
		case State.FALLING:
			accumTime += Time.deltaTime;
			if (accumTime > 0.2f)
			{
				if (killPlayer && ConfigParams.useGore)
				{
					CharHelper.GetCharStateMachine().SwitchTo(ActionCode.EXPLODE);
				}
				accumTime = 0f;
				state = State.WAITING;
			}
			break;
		case State.WAITING:
			accumTime += Time.deltaTime;
			if (accumTime > 3f)
			{
				spikes.MoveUp();
				state = State.UP;
				collide = false;
			}
			break;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c) && !GameManager.IsFredDead())
		{
			spikes.Trigger(spikesSpeed);
			state = State.FALLING;
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
				killPlayer = true;
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
				killPlayer = false;
			}
			collide = true;
		}
	}

	public void TriggerFake()
	{
		spikes.Trigger(spikesSpeed);
		state = State.FALLING;
		killPlayer = false;
	}
}
