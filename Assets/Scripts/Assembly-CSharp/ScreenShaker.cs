using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
	private const float TIME_BETWEEN_SHAKES = 0.05f;

	public Camera cameraToShake;

	public bool shakeXAxis = true;

	public bool shakeYAxis;

	public bool shakeZAxis = true;

	private bool isShaking;

	private float accumTime;

	private float shakeForSeconds;

	private float shakeForce;

	private float accumTime2;

	private Transform camTransf;

	private static ScreenShaker instance;

	private float shakeFromPosX;

	private float shakeToPosX;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		getCamera();
	}

	private void getCamera()
	{
		if (!(camTransf != null))
		{
			if (cameraToShake == null)
			{
				cameraToShake = Camera.main;
			}
			if (cameraToShake != null)
			{
				camTransf = cameraToShake.transform;
			}
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void LateUpdate()
	{
		if (cameraToShake == null)
		{
			getCamera();
			if (cameraToShake == null)
			{
				return;
			}
		}
		if (isShaking)
		{
			accumTime += Time.deltaTime;
			accumTime2 += Time.deltaTime;
			if (accumTime2 > 0.05f)
			{
				shakeFromPosX = camTransf.position.x;
				shakeToPosX = shakeFromPosX + Random.Range(0f - shakeForce, shakeForce);
				accumTime2 %= 0.05f;
			}
			camTransf.position = new Vector3(Mathf.Lerp(shakeFromPosX, shakeToPosX, accumTime2), camTransf.position.y, camTransf.position.z);
			if (accumTime >= shakeForSeconds && shakeForSeconds > 0f)
			{
				isShaking = false;
			}
		}
	}

	public static void Shake(float timeInSeconds, float shakeForce)
	{
		instance.StartShake(timeInSeconds, shakeForce);
	}

	public static void StopShake()
	{
		instance.Stop();
	}

	private void Stop()
	{
		if (shakeForSeconds < 0f)
		{
			isShaking = false;
		}
	}

	public void StartShake(float timeInSeconds, float shakeForce)
	{
		isShaking = true;
		accumTime = 0f;
		accumTime2 = 0f;
		shakeForSeconds = timeInSeconds;
		this.shakeForce = shakeForce;
		if (camTransf != null)
		{
			shakeFromPosX = camTransf.position.x;
			shakeToPosX = shakeFromPosX + Random.Range(0f - shakeForce, shakeForce);
		}
	}
}
