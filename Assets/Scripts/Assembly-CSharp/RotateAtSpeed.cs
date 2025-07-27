using UnityEngine;

public class RotateAtSpeed : MonoBehaviour
{
	public Vector3 Speed = new Vector3(0f, 0f, 100f);

	private float lastTime;

	private float deltaTime;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		lastTime = 0f;
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (lastTime != 0f)
		{
			deltaTime = Time.realtimeSinceStartup - lastTime;
		}
		lastTime = Time.realtimeSinceStartup;
		base.transform.Rotate(Speed * deltaTime);
	}
}
