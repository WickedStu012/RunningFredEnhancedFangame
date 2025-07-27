using UnityEngine;

public class DragonHead : MonoBehaviour
{
	public bool rotateHorizontally;

	public float horizontalSpeed = 3f;

	public bool rotateVertically;

	public float verticalSpeed = 3f;

	private float xAngle1 = 230f;

	private float xAngle2 = 320f;

	private float yAngle1 = 280f;

	private float yAngle2 = 320f;

	private float accumTimeH;

	private float accumTimeV;

	private bool dirPosH = true;

	private bool dirPosV = true;

	private void Start()
	{
		accumTimeH = 0.5f;
		accumTimeV = 0.5f;
	}

	private void Update()
	{
		if (rotateHorizontally || rotateVertically)
		{
			accumTimeH += Time.deltaTime;
			accumTimeV += Time.deltaTime;
			float num = accumTimeH * horizontalSpeed;
			float num2 = accumTimeV * verticalSpeed;
			base.transform.localRotation = Quaternion.Euler(new Vector3((!rotateVertically) ? base.transform.localRotation.eulerAngles.x : Mathf.Lerp(yAngle1, yAngle2, (!dirPosV) ? (1f - num2) : num2), base.transform.localRotation.eulerAngles.y, (!rotateHorizontally) ? base.transform.localRotation.eulerAngles.z : Mathf.Lerp(xAngle1, xAngle2, (!dirPosH) ? (1f - num) : num)));
			if (num >= 1f)
			{
				accumTimeH = 0f;
				dirPosH = !dirPosH;
			}
			if (num2 >= 1f)
			{
				accumTimeV = 0f;
				dirPosV = !dirPosV;
			}
		}
	}
}
