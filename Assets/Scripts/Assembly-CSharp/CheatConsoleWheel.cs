using UnityEngine;

public class CheatConsoleWheel : MonoBehaviour
{
	private float accel;

	private float angle;

	private float newAngle;

	private float accumTime;

	private bool fixAngle;

	private void Start()
	{
		angle = 18f;
		newAngle = angle;
		base.transform.localRotation = Quaternion.AngleAxis(newAngle, Vector3.left);
	}

	private void Update()
	{
		accel *= 0.95f;
		if (accel > 100f || accel < -100f)
		{
			angle += Time.deltaTime * accel;
			base.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.left);
			accumTime = 0f;
			newAngle = Mathf.Floor(angle / 36f) * 36f + 18f;
		}
		else if (fixAngle)
		{
			accumTime += Time.deltaTime;
			if (accumTime < 0.8f)
			{
				base.transform.localRotation = Quaternion.AngleAxis(Mathf.Lerp(angle, newAngle, accumTime), Vector3.left);
				return;
			}
			base.transform.localRotation = Quaternion.AngleAxis(newAngle, Vector3.left);
			angle = newAngle;
			accumTime = 0f;
			fixAngle = false;
		}
	}

	public void ApplyImpulse(float acc)
	{
		accel = acc;
	}

	public void Move(float ang)
	{
		angle += ang;
		newAngle = angle;
		base.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.left);
	}

	public void FixAngle()
	{
		fixAngle = true;
		accumTime = 0f;
		newAngle = Mathf.Floor(angle / 36f) * 36f + 18f;
	}

	public int GetCode()
	{
		float num = angle % 360f;
		if (num < 0f)
		{
			num += 360f;
		}
		return (int)(num / 36f) % 10;
	}
}
