using UnityEngine;

public class MovementHelper
{
	private const float MAX_ROT_ANGLE = 45f;

	private const float MAX_UPDOWN_ROT_ANGLE = 15f;

	private const float MAX_ROT_ANGLE_JETPACK = 60f;

	private const float MAX_ROT_X_ANGLE = 15f;

	private const float MIN_Z_SPEED = 0.7f;

	private const float MAX_Z_SPEED = 1.6f;

	private const float Acceleration = 30f;

	private const float MaxFallingSpeed = 130f;

	private const float Deacceleration = 15f;

	private const float MaxMovingSpeed = 25f;

	private static Vector3 velocity;

	private static Vector3 accelCorrection;

	private static Rigidbody Root;

	private static float rotAngleAccum;

	private static float prevMov;

	private static float rotAngleAccumUpDown;

	private static float prevMovUpDown;

	private static Camera mainCamera;

	public static void CheckMoveActions(CharStateMachine sm, ref float accumTime, ref Quaternion targetRotation, bool affectXAxis)
	{
		if (!InputManager.listenInput || GameManager.IsFredDead())
		{
			return;
		}
		if (Application.isEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (Input.GetKeyUp(ActionKey.MOVE_LEFT) || Input.GetKeyUp(ActionKey.MOVE_RIGHT) || Input.GetKeyUp(ActionKey.MOVE_LEFT_2) || Input.GetKeyUp(ActionKey.MOVE_RIGHT_2))
			{
				accumTime = 0f;
			}
			float axis = Input.GetAxis("Horizontal");
			axis += ((!Input.GetKey(ActionKey.MOVE_LEFT) && !Input.GetKey(ActionKey.MOVE_LEFT_2)) ? 0f : (-0.8f));
			axis = (sm.SteerDirection = axis + ((!Input.GetKey(ActionKey.MOVE_RIGHT) && !Input.GetKey(ActionKey.MOVE_RIGHT_2)) ? 0f : 0.8f));
			if (axis < 0f)
			{
				targetRotation = Quaternion.Euler(new Vector3(targetRotation.eulerAngles.x, -45f, (!affectXAxis) ? 0f : 22.5f));
			}
			else if (axis > 0f)
			{
				targetRotation = Quaternion.Euler(new Vector3(targetRotation.eulerAngles.x, 45f, (!affectXAxis) ? 0f : (-22.5f)));
			}
			else
			{
				targetRotation = Quaternion.Euler(new Vector3(targetRotation.eulerAngles.x, 0f, 0f));
			}
		}
		else
		{
			sm.SteerDirection = InputManager.GetDirection() * -1.8f;
			rotAngleAccum = sm.SteerDirection - prevMov * 10f;
			targetRotation = Quaternion.Euler(new Vector3(0f, (0f - rotAngleAccum) * 10f, (!affectXAxis) ? 0f : (rotAngleAccum / 2f)));
			if (prevMov * sm.SteerDirection < 0f)
			{
				rotAngleAccum = 0f;
			}
			prevMov = sm.SteerDirection;
		}
	}

	public static void CalibrateXValue()
	{
		prevMovUpDown = InputManager.GetDirectionUpDown();
	}

	public static void CheckMoveActionsUpDown(CharStateMachine sm, ref float accumTime, ref Quaternion targetRotation)
	{
		CheckMoveActionsUpDown(sm, ref accumTime, ref targetRotation, -15f, 15f);
	}

	public static void CheckMoveActionsUpDown(CharStateMachine sm, ref float accumTime, ref Quaternion targetRotation, float minAngle, float maxAngle)
	{
		if (!InputManager.listenInput || GameManager.IsFredDead())
		{
			return;
		}
		if (Application.isEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (Input.GetKeyUp(ActionKey.MOVE_UP) || Input.GetKeyUp(ActionKey.MOVE_DOWN))
			{
				accumTime = 0f;
			}
			float axis = Input.GetAxis("Vertical");
			axis += ((!Input.GetKey(ActionKey.MOVE_UP) && !Input.GetKey(ActionKey.MOVE_UP_2)) ? 0f : 0.8f);
			axis = (sm.SteerDirectionUpDown = axis + ((!Input.GetKey(ActionKey.MOVE_DOWN) && !Input.GetKey(ActionKey.MOVE_DOWN_2)) ? 0f : (-0.8f)));
			if (axis < 0f)
			{
				targetRotation = Quaternion.Euler(new Vector3(minAngle, targetRotation.eulerAngles.y, 0f));
			}
			else if (axis > 0f)
			{
				targetRotation = Quaternion.Euler(new Vector3(maxAngle, targetRotation.eulerAngles.y, 0f));
			}
			else
			{
				targetRotation = Quaternion.Euler(new Vector3(0f, targetRotation.eulerAngles.y, 0f));
			}
		}
		else
		{
			sm.SteerDirectionUpDown = InputManager.GetDirectionUpDown() + 0.3f;
			rotAngleAccumUpDown = Mathf.Clamp(rotAngleAccumUpDown + sm.SteerDirectionUpDown * -5f, minAngle, maxAngle);
			sm.SteerDirectionUpDown = Mathf.Clamp(sm.SteerDirectionUpDown, -0.8f, 0.8f);
			targetRotation = Quaternion.Euler(new Vector3(rotAngleAccumUpDown, targetRotation.eulerAngles.y, 0f));
		}
	}

	public static void CheckMoveActionsUpDownInverted(CharStateMachine sm, ref float accumTime, ref Quaternion targetRotation)
	{
		if (!InputManager.listenInput || GameManager.IsFredDead())
		{
			return;
		}
		if (Application.isEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (Input.GetKeyUp(ActionKey.MOVE_UP) || Input.GetKeyUp(ActionKey.MOVE_DOWN))
			{
				accumTime = 0f;
			}
			float axis = Input.GetAxis("Vertical");
			axis += ((!Input.GetKey(ActionKey.MOVE_UP) && !Input.GetKey(ActionKey.MOVE_UP_2)) ? 0f : (-0.6f));
			axis = (sm.SteerDirectionUpDown = axis + ((!Input.GetKey(ActionKey.MOVE_DOWN) && !Input.GetKey(ActionKey.MOVE_DOWN_2)) ? 0f : 0.2f));
			if (axis < 0f)
			{
				targetRotation = Quaternion.Euler(new Vector3(Mathf.Lerp(targetRotation.eulerAngles.x, 60f, accumTime), targetRotation.eulerAngles.y, targetRotation.eulerAngles.z));
			}
			else if (axis > 0f)
			{
				targetRotation = Quaternion.Euler(new Vector3(Mathf.Lerp(targetRotation.eulerAngles.x, 0f, accumTime), targetRotation.eulerAngles.y, targetRotation.eulerAngles.z));
			}
			else
			{
				targetRotation = Quaternion.Euler(new Vector3(Mathf.Lerp(targetRotation.eulerAngles.x, 0f, accumTime), targetRotation.eulerAngles.y, targetRotation.eulerAngles.z));
			}
		}
		else
		{
			sm.SteerDirectionUpDown = (InputManager.GetDirectionUpDown() - prevMovUpDown) * -1f;
			rotAngleAccumUpDown = Mathf.Clamp(sm.SteerDirectionUpDown * -120f, -15f, 60f);
			sm.SteerDirectionUpDown = Mathf.Clamp(sm.SteerDirectionUpDown, -0.5f, 0.5f);
			targetRotation = Quaternion.Euler(new Vector3(rotAngleAccumUpDown, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z));
		}
	}

	public static void CheckMoveActions(CharStateMachine sm, ref float accumTime, ref Quaternion targetRotation)
	{
		CheckMoveActions(sm, ref accumTime, ref targetRotation, false);
	}

	public static float GetRotationAngle()
	{
		return rotAngleAccum;
	}
}
