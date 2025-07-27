using UnityEngine;

public class MoveablePlatform : MonoBehaviour
{
	public enum SpinAxis
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	public enum EndBehaviour
	{
		STOP = 0,
		RETURN_TO_INITIAL_POS = 1,
		RESTART = 2
	}

	public enum State
	{
		STOPPED = 0,
		MOVING_TO_DST = 1,
		MOVING_TO_SRC = 2
	}

	public GameObject dstGO;

	public EndBehaviour endBehaviour;

	public float movSpeed = 1f;

	public bool transformPos = true;

	public bool transformRot;

	public bool spin;

	public SpinAxis spinAxis;

	public float spinSpeed = 1f;

	public float offsetMov;

	private Transform dstT;

	private State state;

	private Vector3 originalPos;

	private Quaternion originalRot;

	private Vector3 dstPos;

	private Quaternion dstRot;

	private bool oriPosAndRotStored;

	private float accumTime;

	private void Start()
	{
		if (dstGO != null)
		{
			dstT = dstGO.transform;
			dstGO.SetActive(false);
			state = State.MOVING_TO_DST;
		}
		oriPosAndRotStored = false;
		accumTime = offsetMov;
	}

	private void Update()
	{
		if (Application.isEditor && dstGO != null)
		{
			Debug.DrawLine(base.transform.position, dstGO.transform.position, Color.green);
		}
		getPosAndRot();
		switch (state)
		{
		case State.MOVING_TO_DST:
			accumTime += Time.deltaTime;
			if (transformPos)
			{
				base.transform.position = Vector3.Lerp(originalPos, dstPos, accumTime * movSpeed);
			}
			if (transformRot)
			{
				base.transform.rotation = Quaternion.Slerp(originalRot, dstRot, accumTime * movSpeed);
			}
			if (accumTime * movSpeed >= 1f)
			{
				if (endBehaviour == EndBehaviour.STOP)
				{
					state = State.STOPPED;
				}
				else if (endBehaviour == EndBehaviour.RESTART)
				{
					accumTime = 0f;
				}
				else if (endBehaviour == EndBehaviour.RETURN_TO_INITIAL_POS)
				{
					accumTime = 0f;
					state = State.MOVING_TO_SRC;
				}
			}
			break;
		case State.MOVING_TO_SRC:
			accumTime += Time.deltaTime;
			if (transformPos)
			{
				base.transform.position = Vector3.Lerp(dstPos, originalPos, accumTime * movSpeed);
			}
			if (transformRot)
			{
				base.transform.rotation = Quaternion.Slerp(dstRot, originalRot, accumTime * movSpeed);
			}
			if (accumTime * movSpeed >= 1f)
			{
				accumTime = 0f;
				state = State.MOVING_TO_DST;
			}
			break;
		}
		if (spin)
		{
			switch (spinAxis)
			{
			case SpinAxis.X:
				base.transform.RotateAround(Vector3.right, Time.deltaTime * spinSpeed);
				break;
			case SpinAxis.Y:
				base.transform.RotateAround(Vector3.up, Time.deltaTime * spinSpeed);
				break;
			case SpinAxis.Z:
				base.transform.RotateAround(Vector3.forward, Time.deltaTime * spinSpeed);
				break;
			}
		}
	}

	private void getPosAndRot()
	{
		if (!oriPosAndRotStored)
		{
			originalPos = base.transform.position;
			originalRot = base.transform.rotation;
			if (dstT != null)
			{
				dstPos = dstT.position;
				dstRot = dstT.rotation;
			}
			oriPosAndRotStored = true;
		}
	}
}
