using UnityEngine;

public class LowFrequencyRandom
{
	private float lastRandom;

	private Vector3 lastInsideSphere;

	private Quaternion rot;

	private Vector3 eulerRot;

	private float timeCounter;

	private float frequency = 1f;

	public LowFrequencyRandom(float frequency = 1f)
	{
		this.frequency = frequency;
	}

	public void Update()
	{
		bool flag = false;
		if (timeCounter > frequency)
		{
			lastRandom = Random.value;
			lastInsideSphere = Random.insideUnitSphere;
			rot = Random.rotation;
			eulerRot = (new Vector3(Random.value, Random.value, Random.value) - Vector3.one * 0.5f) * 2f;
			flag = true;
		}
		timeCounter += Time.deltaTime;
		if (flag)
		{
			timeCounter = 0f;
		}
	}

	public Vector3 GetInsideSphere()
	{
		return lastInsideSphere;
	}

	public Quaternion GetRotation()
	{
		return rot;
	}

	public Vector3 GetEulerRot()
	{
		return eulerRot;
	}

	public float GetValue()
	{
		return lastRandom;
	}
}
