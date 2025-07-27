using UnityEngine;

public class BearTrap : MonoBehaviour
{
	private enum State
	{
		CLOSING = 0,
		CLOSED = 1
	}

	public GameObject part1;

	public GameObject part2;

	private bool collide;

	private bool closing;

	private State state;

	private float accumTime;

	private Quaternion part1Start;

	private Quaternion part2Start;

	private Quaternion part1End;

	private Quaternion part2End;

	private float closeSpeed = 10f;

	private bool dodge;

	private void Start()
	{
		collide = false;
		part1Start = part1.transform.rotation;
		part2Start = part2.transform.rotation;
		part1End = new Quaternion(0.2f, 0f, 0f, -1f);
		part2End = new Quaternion(1f, 0f, 0f, -0.1f);
	}

	private void Update()
	{
		if (!closing)
		{
			return;
		}
		switch (state)
		{
		case State.CLOSING:
			accumTime += Time.deltaTime;
			part1.transform.rotation = Quaternion.Slerp(part1Start, part1End, accumTime * closeSpeed);
			part2.transform.rotation = Quaternion.Slerp(part2Start, part2End, accumTime * closeSpeed);
			if (!(accumTime * closeSpeed >= 1f))
			{
				break;
			}
			if (!dodge)
			{
				if (ConfigParams.useGore)
				{
					CharHelper.GetCharSkin().DismemberRandom();
				}
				else
				{
					CharHelper.GetCharStateMachine().SwitchTo(ActionCode.DIE_IMPCT);
				}
			}
			state = State.CLOSED;
			break;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			SoundManager.PlaySound(base.transform.position, 14);
			closing = true;
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				dodge = false;
			}
			else
			{
				dodge = true;
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BOUNCE);
			}
			collide = true;
		}
	}

	public void Dodge()
	{
		if (!collide)
		{
			SoundManager.PlaySound(base.transform.position, 14);
			dodge = true;
			closing = true;
			collide = true;
		}
	}
}
