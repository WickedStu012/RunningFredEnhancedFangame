using UnityEngine;

public class FovAnimator : MonoBehaviour
{
	private enum FovState
	{
		FOV_IN = 0,
		NORMAL = 1,
		FOV_OUT = 2,
		NORMAL_FOV_IN = 3
	}

	private const float NORMAL_FOV = 60f;

	private const float MAX_FOV = 90f;

	private const float FOV_IN_SPEED = 1f;

	private const float FOV_OUT_SPEED = 2f;

	private float accumTimeFOV;

	private FovState fovState;

	private Camera cam;

	private static FovAnimator instance;

	private float iniFOV;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		cam = Camera.main;
		fovState = FovState.NORMAL;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
	{
		if (cam == null)
		{
			cam = Camera.main;
		}
		if (fovState != FovState.NORMAL)
		{
			animate();
		}
	}

	private void animate()
	{
		switch (fovState)
		{
		case FovState.FOV_IN:
		{
			accumTimeFOV += Time.deltaTime;
			float num = accumTimeFOV * 1f;
			if (num < 1f)
			{
				cam.fieldOfView = Mathf.Lerp(iniFOV, 90f, num);
				break;
			}
			cam.fieldOfView = 90f;
			accumTimeFOV = 0f;
			fovState = FovState.NORMAL_FOV_IN;
			break;
		}
		case FovState.FOV_OUT:
		{
			accumTimeFOV += Time.deltaTime;
			float num = accumTimeFOV * 2f;
			if (num < 1f)
			{
				cam.fieldOfView = Mathf.Lerp(iniFOV, 60f, num);
				break;
			}
			cam.fieldOfView = 60f;
			accumTimeFOV = 0f;
			fovState = FovState.NORMAL;
			break;
		}
		case FovState.NORMAL:
			break;
		}
	}

	public static void FovIn()
	{
		if (!(instance == null))
		{
			instance.fovIn();
		}
	}

	private void fovIn()
	{
		if (fovState != FovState.FOV_IN)
		{
			if (fovState == FovState.NORMAL)
			{
				accumTimeFOV = 0f;
				iniFOV = 60f;
				fovState = FovState.FOV_IN;
			}
			else if (fovState == FovState.FOV_OUT)
			{
				accumTimeFOV = 0f;
				iniFOV = cam.fieldOfView;
				fovState = FovState.FOV_IN;
			}
		}
	}

	public static void FovOut()
	{
		if (!(instance == null))
		{
			instance.fovOut();
		}
	}

	private void fovOut()
	{
		if (fovState != FovState.FOV_OUT)
		{
			if (fovState == FovState.NORMAL_FOV_IN)
			{
				accumTimeFOV = 0f;
				iniFOV = 90f;
				fovState = FovState.FOV_OUT;
			}
			else if (fovState == FovState.FOV_IN)
			{
				accumTimeFOV = 0f;
				iniFOV = cam.fieldOfView;
				fovState = FovState.FOV_OUT;
			}
		}
	}
}
