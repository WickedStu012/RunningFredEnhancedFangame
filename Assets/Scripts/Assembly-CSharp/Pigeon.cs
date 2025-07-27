using UnityEngine;

public class Pigeon : MonoBehaviour
{
	private enum State
	{
		IDLE = 0,
		EAT = 1,
		FLY = 2,
		DEAD = 3,
		REST = 4,
		SCARED = 5
	}

	public WayPath wp;

	private float accumtime;

	private float nextActionTime;

	private bool flyAway;

	private Vector3 pointStart;

	private Vector3 pointToFly;

	public float speed = 1f;

	public bool think;

	private void Start()
	{
		nextActionTime = Random.Range(0.5f, 2f);
		playIdle();
		wp.Play(base.gameObject, true, 1f);
		wp.Pause();
		flyAway = false;
		think = true;
	}

	private void Update()
	{
		if (!think)
		{
			return;
		}
		accumtime += Time.deltaTime;
		if (!flyAway)
		{
			if (accumtime > nextActionTime)
			{
				switch (Random.Range(0, 3))
				{
				case 0:
					wp.Pause();
					playIdle();
					break;
				case 1:
					wp.Resume();
					playWalk();
					break;
				case 2:
					wp.Pause();
					playEat();
					break;
				}
				nextActionTime = Random.Range(2f, 5f);
				accumtime = 0f;
			}
		}
		else
		{
			float num = accumtime * speed / 10f;
			base.transform.position = Vector3.Lerp(pointStart, pointToFly, num);
			if (num >= 1f)
			{
				Object.Destroy(base.transform.parent.gameObject);
			}
		}
	}

	private void playIdle()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		if (Random.Range(0, 2) == 0)
		{
			base.GetComponent<Animation>().Play("Idle1");
		}
		else
		{
			base.GetComponent<Animation>().Play("Idle1");
		}
	}

	private void playWalk()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Walk");
	}

	private void playEat()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Eat");
	}

	private void playFly()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Fly");
	}

	public void Fly()
	{
		flyAway = true;
		pointStart = base.transform.position;
		pointToFly = new Vector3(base.transform.position.x + Random.Range(-5f, 5f), base.transform.position.y + Random.Range(50f, 100f), base.transform.position.z + Random.Range(-5f, 5f));
		playFly();
		wp.Stop();
		accumtime = 0f;
	}

	public void Think(bool val)
	{
		think = val;
		if (!val)
		{
			wp.Pause();
		}
		else
		{
			wp.Resume();
		}
	}
}
