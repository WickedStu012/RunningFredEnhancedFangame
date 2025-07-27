using UnityEngine;

public class LookAtPlayerForTrailer : MonoBehaviour
{
	private enum State
	{
		LOOKING_PLAYER_IN = 0,
		ZOOMING = 1,
		PAUSE = 2,
		LOOKING_PLAYER_OUT = 3
	}

	private GameObject goPlayer;

	private State state;

	private float accumTime;

	private float lastTime;

	private void Start()
	{
		state = State.LOOKING_PLAYER_IN;
		goPlayer = GameObject.FindWithTag("Player");
	}

	private void Update()
	{
		switch (state)
		{
		case State.LOOKING_PLAYER_IN:
			base.GetComponent<Camera>().transform.LookAt(goPlayer.transform);
			break;
		case State.ZOOMING:
			accumTime += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			base.GetComponent<Camera>().fieldOfView = 60f - Mathf.Lerp(0f, 40f, accumTime * 2f);
			Time.timeScale = Mathf.Lerp(1f, 0f, accumTime * 3f);
			if (base.GetComponent<Camera>().fieldOfView <= 20f)
			{
				accumTime = 0f;
				state = State.PAUSE;
			}
			base.GetComponent<Camera>().transform.LookAt(goPlayer.transform);
			break;
		case State.PAUSE:
			accumTime += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			if (accumTime > 2f)
			{
				accumTime = 0f;
				Time.timeScale = 0f;
				state = State.LOOKING_PLAYER_OUT;
			}
			break;
		case State.LOOKING_PLAYER_OUT:
			accumTime += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			Time.timeScale = Mathf.Lerp(0f, 1f, accumTime * 3f);
			base.GetComponent<Camera>().transform.LookAt(goPlayer.transform);
			break;
		}
	}

	public void StartZoom()
	{
		state = State.ZOOMING;
		lastTime = Time.realtimeSinceStartup;
	}
}
