using UnityEngine;

public class LevelRandomObjectPlacer : MonoBehaviour
{
	public enum LocationType
	{
		DISCRETE = 0,
		NOT_DISCRETE = 1
	}

	public LocationType locType;

	public float chanceToAppear = 1f;

	public bool randomizeHallSaw;

	public bool randomizeWreckBall;

	public Vector3[] objsPos;

	public Quaternion[] objsRots;

	private void OnEnable()
	{
		if (chanceToAppear < 1f)
		{
			if (Random.Range(0f, 1f) <= chanceToAppear)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
			}
			else if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
		}
		if (randomizeHallSaw)
		{
			HallSawWheel componentInChildren = base.gameObject.GetComponentInChildren<HallSawWheel>();
			if (!(componentInChildren != null))
			{
				return;
			}
			componentInChildren.speed *= Random.Range(0.75f, 1.25f);
			componentInChildren.wheelJump = Random.Range(0, 2) == 0;
			if (componentInChildren.wheelJump)
			{
				componentInChildren.maxJumpHeight *= Random.Range(0.75f, 1.25f);
				componentInChildren.jumpInterval *= Random.Range(0.75f, 1.25f);
				if (componentInChildren.jumpInterval < 0f)
				{
					componentInChildren.jumpInterval = 0f;
				}
			}
		}
		else if (randomizeWreckBall)
		{
			WrackBall componentInChildren2 = base.gameObject.GetComponentInChildren<WrackBall>();
			if (componentInChildren2 != null)
			{
				componentInChildren2.clockWise = Random.Range(0, 2) == 0;
			}
		}
	}

	public void locateObj(float dy)
	{
		if (base.gameObject.activeSelf && objsPos != null && objsRots != null && objsPos.Length > 0 && objsPos.Length == objsRots.Length)
		{
			if (locType == LocationType.DISCRETE)
			{
				int num = Random.Range(0, objsPos.Length);
				base.transform.localPosition = objsPos[num] + new Vector3(0f, dy, 0f);
				base.transform.rotation = objsRots[num];
			}
			else if (objsPos.Length > 1 && objsRots.Length > 1)
			{
				Vector3 vector = Vector3.Lerp(objsPos[0], objsPos[1], Random.Range(0f, 1f));
				Quaternion rotation = Quaternion.Slerp(objsRots[0], objsRots[1], Random.Range(0f, 1f));
				base.transform.localPosition = vector + new Vector3(0f, dy, 0f);
				base.transform.rotation = rotation;
			}
		}
	}
}
