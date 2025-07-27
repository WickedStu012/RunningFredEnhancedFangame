using UnityEngine;

public class FpsMeter
{
	private float accumDeltaTime;

	private int frames;

	private float fps;

	private float lastSample;

	private int gotIntervals;

	public float GetFPS()
	{
		return fps;
	}

	public bool HasFPS()
	{
		return gotIntervals > 2;
	}

	public void Update()
	{
		frames++;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		if (lastSample != 0f)
		{
			num = realtimeSinceStartup - lastSample;
			accumDeltaTime += num;
		}
		lastSample = realtimeSinceStartup;
		if (accumDeltaTime >= 1f)
		{
			fps = frames;
			accumDeltaTime -= 1f;
			frames = 0;
			gotIntervals++;
		}
	}
}
