using UnityEngine;

public class ToothPickTosser : MonoBehaviour
{
	public enum Mode
	{
		TRIGGER = 0,
		BURST = 1,
		CONTINUOUS = 2
	}

	public enum State
	{
		CLOSED = 0,
		OPENING = 1,
		IDLE = 2,
		TROW = 3,
		CLOSING = 4
	}

	private const float LAUNCH_DELTA_T_TRIGGER = 0.25f;

	private const float LAUNCH_DELTA_CONTINUOUS = 0.25f;

	public GameObject baseSpikes;

	public GameObject[] spikes;

	public State initialState;

	public Mode mode;

	public bool throwToPlayer = true;

	private int spikesLeft;

	private State state;

	private float accumTime;

	private bool collide;

	private float launchDeltaTime;

	private Quaternion closedQuat = new Quaternion(0.7f, 0f, 0f, -0.7f);

	private Quaternion openQuat = new Quaternion(0.9f, 0f, 0f, -0.5f);

	private Quaternion idleQuat;

	private Quaternion playerQuat;

	private Vector3[] spikesPos;

	private Quaternion[] spikesRot;

	private void Start()
	{
		spikesPos = new Vector3[spikes.Length];
		spikesRot = new Quaternion[spikes.Length];
		for (int i = 0; i < spikes.Length; i++)
		{
			spikesPos[i] = spikes[i].transform.localPosition;
			spikesRot[i] = spikes[i].transform.localRotation;
		}
		if (mode == Mode.TRIGGER)
		{
			state = State.CLOSED;
			baseSpikes.transform.localRotation = closedQuat;
			launchDeltaTime = 0.25f;
		}
		else if (mode == Mode.BURST)
		{
			state = State.OPENING;
			baseSpikes.transform.localRotation = closedQuat;
			launchDeltaTime = 0.25f;
		}
		else if (mode == Mode.CONTINUOUS)
		{
			state = State.TROW;
			baseSpikes.transform.localRotation = openQuat;
			launchDeltaTime = 0.25f;
		}
		idleQuat = base.transform.rotation;
		collide = false;
		if (spikes != null)
		{
			spikesLeft = spikes.Length;
		}
		else
		{
			Debug.LogWarning("Please, refence the spikes array in ToothPickTosser to its spikes.");
		}
	}

	private void Update()
	{
		switch (state)
		{
		case State.OPENING:
			accumTime += Time.deltaTime;
			baseSpikes.transform.localRotation = Quaternion.Slerp(closedQuat, openQuat, accumTime);
			if (throwToPlayer)
			{
				base.transform.rotation = Quaternion.Slerp(idleQuat, playerQuat, accumTime);
			}
			if (accumTime >= 1f)
			{
				accumTime = 0f;
				baseSpikes.transform.localRotation = openQuat;
				state = State.TROW;
			}
			break;
		case State.TROW:
			accumTime += Time.deltaTime;
			if (!(accumTime > launchDeltaTime))
			{
				break;
			}
			trowSpike();
			accumTime = 0f;
			if (spikesLeft == 0)
			{
				if (mode == Mode.TRIGGER || mode == Mode.BURST)
				{
					state = State.CLOSING;
				}
				else
				{
					resetSpikes();
				}
			}
			break;
		case State.CLOSING:
			accumTime += Time.deltaTime;
			baseSpikes.transform.localRotation = Quaternion.Slerp(openQuat, closedQuat, accumTime);
			if (accumTime >= 1f)
			{
				accumTime = 0f;
				baseSpikes.transform.localRotation = closedQuat;
				if (mode == Mode.BURST)
				{
					resetSpikes();
					state = State.OPENING;
				}
				else
				{
					state = State.CLOSED;
				}
			}
			break;
		case State.IDLE:
			break;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (mode != Mode.TRIGGER || collide)
		{
			return;
		}
		if (CharHelper.IsColliderFromPlayer(c))
		{
			if (state == State.IDLE)
			{
				state = State.TROW;
				trowSpike();
			}
			else
			{
				Transform playerTransform = CharHelper.GetPlayerTransform();
				base.transform.LookAt(playerTransform);
				playerQuat = base.transform.rotation;
				base.transform.rotation = idleQuat;
				state = State.OPENING;
			}
		}
		collide = true;
	}

	private void trowSpike()
	{
		if (spikesLeft > 0)
		{
			ToothPickSpike component = spikes[spikesLeft - 1].GetComponent<ToothPickSpike>();
			if (component != null)
			{
				component.Trigger();
			}
			spikesLeft--;
		}
	}

	private void resetSpikes()
	{
		if (!ToothPickSpike.IsCharHit())
		{
			for (int i = 0; i < spikes.Length; i++)
			{
				spikes[i].transform.localPosition = spikesPos[i];
				spikes[i].transform.localRotation = spikesRot[i];
				spikes[i].GetComponent<ToothPickSpike>().Reset();
			}
			spikesLeft = spikes.Length;
		}
	}
}
