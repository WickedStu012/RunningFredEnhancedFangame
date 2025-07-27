using UnityEngine;

public class LevelCastleEnd : LevelEnd
{
	private const float DELTA_POS = 12f;

	public GameObject door;

	private float accumTime;

	private float iniPosY;

	private new void Start()
	{
		base.Start();
		if (door != null)
		{
			iniPosY = door.transform.position.y;
		}
	}

	private new void Update()
	{
		base.Update();
		if (goalReached && door != null)
		{
			accumTime += Time.deltaTime;
			float num = accumTime / 3f;
			door.transform.position = new Vector3(door.transform.position.x, Mathf.Lerp(iniPosY, iniPosY + 12f, num), door.transform.position.z);
			if (num >= 1f)
			{
				EnderFinished();
			}
		}
	}
}
