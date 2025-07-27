using System;
using UnityEngine;

public class Catapult : MonoBehaviour
{
	private enum State
	{
		IDLE = 0,
		TO_LAUNCH = 1,
		LAUNCHED = 2
	}

	public GameObject platform;

	public float jumpForce = 2f;

	public float angle = 45f;

	private float accumTime;

	private bool collide;

	private State state;

	private Vector3 platformPos;

	private Quaternion platformRot;

	private void Start()
	{
		state = State.IDLE;
		platformPos = platform.transform.localPosition;
		platformRot = platform.transform.localRotation;
	}

	private void OnEnable()
	{
		if (collide)
		{
			base.transform.parent.GetComponent<Animation>().Stop();
			platform.transform.localPosition = platformPos;
			platform.transform.localRotation = platformRot;
			accumTime = 0f;
			collide = false;
			state = State.IDLE;
		}
	}

	private void Update()
	{
		switch (state)
		{
		case State.TO_LAUNCH:
		{
			accumTime += Time.deltaTime;
			base.transform.parent.GetComponent<Animation>().Play("Catapult");
			SoundManager.PlaySound(34);
			float z = 1f / Mathf.Tan(angle * ((float)Math.PI / 180f));
			Vector3 vector = new Vector3(0f, 1f, z);
			vector.Normalize();
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.CATAPULT, jumpForce, vector);
			accumTime = 0f;
			state = State.LAUNCHED;
			break;
		}
		case State.LAUNCHED:
			accumTime += Time.deltaTime;
			if (accumTime >= 0.1f)
			{
				base.transform.parent.GetComponent<Animation>().Stop();
				platform.transform.localPosition = platformPos;
				platform.transform.localRotation = platformRot;
				accumTime = 0f;
				collide = false;
				state = State.IDLE;
			}
			break;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			state = State.TO_LAUNCH;
			collide = true;
		}
	}
}
