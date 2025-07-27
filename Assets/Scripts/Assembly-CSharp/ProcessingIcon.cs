using UnityEngine;

public class ProcessingIcon : MonoBehaviour
{
	public float Angles = 1f;

	public float Timer = 0.3f;

	private float timer;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		timer -= GUI3DManager.Instance.DeltaTime;
		if (timer <= 0f)
		{
			base.transform.rotation *= Quaternion.AngleAxis(0f - Angles, Vector3.forward);
			timer = Timer;
		}
	}
}
