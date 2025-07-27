using UnityEngine;

public class Shine : MonoBehaviour
{
	private enum States
	{
		Showing = 0,
		Hidden = 1
	}

	public float MinWaitTime = 1f;

	public float MaxWaitTime = 3f;

	public float MinLifeTime = 0.1f;

	public float MaxLifeTime = 0.5f;

	public float MinSpeed = 50f;

	public float MaxSpeed = 100f;

	public Vector3 MinScale = new Vector3(0.2f, 0.2f, 1f);

	public Vector3 MaxScale = new Vector3(1f, 1f, 1f);

	private States state;

	private float currentTime;

	private Vector3 currentScale;

	private Vector3 targetScale;

	private Vector3 scaleFactor;

	private float speed;

	private void OnEnable()
	{
		currentTime = Random.Range(MinWaitTime, MaxWaitTime);
		state = States.Hidden;
		currentScale = Vector3.forward;
		base.transform.localScale = currentScale;
	}

	private void FixedUpdate()
	{
		switch (state)
		{
		case States.Showing:
			currentTime -= Time.deltaTime;
			if (currentTime <= 0f)
			{
				currentTime = Random.Range(MinWaitTime, MaxWaitTime);
				currentScale.x = (currentScale.y = 0f);
				base.transform.localScale = currentScale;
				state = States.Hidden;
			}
			currentScale.x += scaleFactor.x * Time.deltaTime;
			currentScale.y += scaleFactor.y * Time.deltaTime;
			if (currentScale.x > targetScale.x || currentScale.y > targetScale.y)
			{
				currentScale.x = targetScale.x;
				currentScale.y = targetScale.y;
				scaleFactor.x *= -1f;
				scaleFactor.y *= -1f;
			}
			else if (currentScale.x < 0f || currentScale.y < 0f)
			{
				currentScale.x = 0f;
				currentScale.y = 0f;
			}
			base.transform.localScale = currentScale;
			base.transform.localRotation *= Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward);
			break;
		case States.Hidden:
			currentTime -= GUI3DManager.Instance.DeltaTime;
			if (currentTime <= 0f)
			{
				currentTime = Random.Range(MinLifeTime, MaxLifeTime);
				targetScale.x = Random.Range(MinScale.x, MaxScale.x);
				targetScale.y = Random.Range(MinScale.y, MaxScale.y);
				scaleFactor.x = targetScale.x / currentTime * 2f;
				scaleFactor.y = targetScale.y / currentTime * 2f;
				speed = Random.Range(MinSpeed, MaxSpeed);
				state = States.Showing;
			}
			break;
		}
	}
}
