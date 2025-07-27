using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
	public enum DamageType
	{
		FIRE = 0,
		HIT = 1
	}

	private enum State
	{
		IDLE = 0,
		MOVE_UP = 1
	}

	public DamageType dmgType;

	public float maxHeight;

	public float interval = 1f;

	public float startTimeForBurst;

	public float speed = 80f;

	private State state;

	private float accumTime;

	private float iniPosY;

	private bool collide;

	private float accumTimeCollide;

	private void Start()
	{
		state = State.IDLE;
		iniPosY = base.transform.position.y;
		accumTime = startTimeForBurst;
	}

	private void Update()
	{
		float num = 0f;
		switch (state)
		{
		case State.IDLE:
			accumTime += Time.deltaTime;
			if (accumTime > interval)
			{
				accumTime = 0f;
				state = State.MOVE_UP;
			}
			break;
		case State.MOVE_UP:
			accumTime += Time.deltaTime;
			num = maxHeight * Mathf.Sin(accumTime * speed * ((float)Math.PI / 180f));
			base.transform.position = new Vector3(base.transform.position.x, iniPosY + num, base.transform.position.z);
			if (accumTime * speed >= 180f)
			{
				accumTime = 0f;
				state = State.IDLE;
			}
			break;
		}
		if (collide)
		{
			accumTimeCollide += Time.deltaTime;
			if (accumTimeCollide >= 1f)
			{
				accumTimeCollide = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (dmgType == DamageType.FIRE)
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BURNT);
			}
			else
			{
				SoundManager.PlaySound(6);
				CharHelper.GetCharStateMachine().Hit(-1f * new Vector3(UnityEngine.Random.Range(-10, 10), 0f, UnityEngine.Random.Range(-40, -80)));
			}
			ScreenShaker.Shake(0.5f, 8f);
			collide = true;
		}
	}
}
