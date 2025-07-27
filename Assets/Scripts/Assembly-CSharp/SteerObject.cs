using UnityEngine;

public class SteerObject : MonoBehaviour
{
	public float Speed = 10f;

	public float MaxAngle = 20f;

	private float angle;

	private float factor = 1f;

	private void Update()
	{
		angle += Speed * factor * Time.deltaTime;
		if (angle >= MaxAngle || angle <= 0f - MaxAngle)
		{
			angle = MaxAngle * factor;
			factor *= -1f;
		}
		base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
