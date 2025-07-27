using UnityEngine;

public class Cinematic1 : MonoBehaviour
{
	private Transform lookThisT;

	private bool turn;

	private Quaternion q1;

	private Quaternion q2;

	private float accumTime;

	private bool stopLookAt;

	private void Start()
	{
		lookThisT = CharHelper.GetPlayerTransform();
		turn = false;
	}

	private void Update()
	{
		if (!turn)
		{
			if (!stopLookAt)
			{
				base.transform.LookAt(lookThisT);
			}
			return;
		}
		accumTime += Time.deltaTime;
		base.transform.rotation = Quaternion.Slerp(q1, q2, accumTime);
		if (accumTime > 1f)
		{
			turn = false;
		}
	}

	public void SetLookThis(Transform t)
	{
		lookThisT = t;
	}

	public void StopLookAt()
	{
		stopLookAt = true;
	}

	public void TurnToTransform(Transform t)
	{
		Transform target = lookThisT;
		lookThisT = t;
		q1 = base.transform.rotation;
		base.transform.LookAt(lookThisT);
		q2 = base.transform.rotation;
		base.transform.LookAt(target);
		turn = true;
		stopLookAt = false;
	}
}
