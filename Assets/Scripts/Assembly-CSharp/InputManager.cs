using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private const int ACCEL_SAMPLE_COUNT = 8;

	private static InputManager instance;

	public Texture button;

	private static bool isJumping;

	private static bool isDucking;

	private static bool isSuperSprint;

	private Vector2 jumpPressPosition;

	private static bool jumpAlreadyPressed;

	public static bool listenInput = true;

	private static DateTime timeSuperSprint;

	private float[] prevAccelXValues = new float[8];

	private static int accelValueSampleCount;

	private float accumTimeCheckSuperSprint;

	private bool zeemoteButtonBPressed;

	private void Start()
	{
		listenInput = true;
		isJumping = (isDucking = (isSuperSprint = false));
		GameEventDispatcher.AddListener("PlayerReachGoal", onPlayerReachGoal);
		accelValueSampleCount = 0;
		timeSuperSprint = DateTime.Now;
	}

	private void OnEnable()
	{
		instance = this;
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void OnGUI()
	{
		if (Time.timeScale != 0f && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) && isJumping && !ConfigParams.zeemoteConnected && !ConfigParams.useiCADE && !MogaInput.Instance.IsConnected() && button != null)
		{
			GUI.DrawTexture(new Rect(jumpPressPosition.x - (float)(button.width >> 1), jumpPressPosition.y - (float)(button.height >> 1), button.width, button.height), button);
		}
	}

	private void Update()
	{
		isJumping = (isDucking = false);
		if (!listenInput || GameManager.IsFredDead())
		{
			return;
		}
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (Input.GetMouseButton(0) || Input.GetKeyDown("joystick button 16"))
			{
				checkJumping(Input.mousePosition);
				checkDucking(Input.mousePosition);
			}
			if (!isJumping)
			{
				isJumping = Input.GetKey(ActionKey.JUMP) || Input.GetKey(ActionKey.JUMP2) || Input.GetKey("joystick button 16");
			}
			if (!isDucking)
			{
				isDucking = Input.GetKey(ActionKey.DUCK) || Input.GetKey("joystick button 18");
			}
		}
		else
		{
			checkJumping();
			checkDucking();
		}
	}

	private void checkDucking(Vector3 pos)
	{
		isDucking = 0f < pos.x && pos.x < (float)Screen.width * 0.25f && 0f < pos.y && pos.y < (float)Screen.height * 0.75f;
		if (!isDucking)
		{
		}
	}

	private void checkDucking()
	{
		bool flag = false;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < Input.touchCount; i++)
		{
			zero = Input.touches[i].position;
			flag = 0f < zero.x && zero.x < (float)Screen.width * 0.25f && 0f < zero.y && zero.y < (float)Screen.height * 0.75f;
			if (flag)
			{
				break;
			}
		}
		isDucking = flag;
		if (!isDucking)
		{
		}
	}

	private void checkJumping(Vector3 pos)
	{
		isJumping = (float)Screen.width * 0.5f < pos.x && pos.x < (float)Screen.width && 0f < pos.y && pos.y < (float)Screen.height * 0.75f;
		if (isJumping)
		{
			jumpPressPosition = new Vector2(pos.x, (float)Screen.height - pos.y);
		}
	}

	private void checkJumping()
	{
		bool flag = false;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < Input.touchCount; i++)
		{
			vector = Input.touches[i].position;
			flag = (float)Screen.width - (float)Screen.width * 0.45f < vector.x && vector.x < (float)Screen.width && 0f < vector.y && vector.y < (float)Screen.height * 0.75f;
			if (flag)
			{
				break;
			}
		}
		isJumping = flag;
		if (isJumping)
		{
			jumpPressPosition = new Vector2(vector.x, (float)Screen.height - vector.y);
		}
		if (MogaInput.Instance.IsConnected())
		{
			isJumping |= MogaInput.Instance.GetButtonA();
		}
	}

	private static void CheckSuperSprint()
	{
		if (instance != null)
		{
			instance.checkSuperSprint();
		}
	}

	private void checkSuperSprint()
	{
		if (!((DateTime.Now - timeSuperSprint).TotalSeconds > 2.0))
		{
			return;
		}
		accumTimeCheckSuperSprint += Time.deltaTime;
		if (!(accumTimeCheckSuperSprint >= 0.03f))
		{
			return;
		}
		accumTimeCheckSuperSprint -= accumTimeCheckSuperSprint % 0.03f;
		float num = 0f - Input.acceleration.y;
		if (accelValueSampleCount < 8)
		{
			accelValueSampleCount++;
		}
		for (int i = 0; i < 7; i++)
		{
			prevAccelXValues[i] = prevAccelXValues[i + 1];
		}
		prevAccelXValues[7] = num;
		if (accelValueSampleCount != 8 || !(num - prevAccelXValues[0] > 0.4f))
		{
			return;
		}
		for (int j = 0; j < 7; j++)
		{
			if (prevAccelXValues[j] >= prevAccelXValues[j + 1])
			{
				isSuperSprint = true;
				accelValueSampleCount = 0;
				timeSuperSprint = DateTime.Now;
			}
		}
	}

	public static void ResetSuperSprintTimer()
	{
		accelValueSampleCount = 0;
		timeSuperSprint = DateTime.Now;
		isSuperSprint = false;
	}

	public static float GetDirection()
	{
		if (!listenInput)
		{
			return 0f;
		}
		if (Application.isEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			float num = Input.GetAxis("Horizontal") * -1f;
			num += ((!Input.GetKey(ActionKey.MOVE_LEFT) && !Input.GetKey(ActionKey.MOVE_LEFT_2)) ? 0f : 0.5f);
			return num + ((!Input.GetKey(ActionKey.MOVE_RIGHT) && !Input.GetKey(ActionKey.MOVE_RIGHT_2)) ? 0f : (-0.5f));
		}
		if (MogaInput.Instance.IsConnected())
		{
			if (MogaInput.Instance.GetDPadLeft())
			{
				return 0.5f;
			}
			if (MogaInput.Instance.GetDPadRight())
			{
				return -0.5f;
			}
			return (0f - MogaInput.Instance.GetAxisX()) * 0.5f;
		}
		return Input.acceleration.x * -1f;
	}

	public static float GetDirectionUpDown()
	{
		if (!listenInput)
		{
			return 0f;
		}
		if (Application.isEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			float axis = Input.GetAxis("Vertical");
			axis += ((!Input.GetKey(ActionKey.MOVE_UP) && !Input.GetKey(ActionKey.MOVE_UP_2)) ? 0f : 0.5f);
			return axis + ((!Input.GetKey(ActionKey.MOVE_DOWN) && !Input.GetKey(ActionKey.MOVE_DOWN_2)) ? 0f : (-0.5f));
		}
		if (MogaInput.Instance.IsConnected())
		{
			if (MogaInput.Instance.GetDPadUp())
			{
				return -0.5f;
			}
			if (MogaInput.Instance.GetDPadDown())
			{
				return 0.5f;
			}
			return MogaInput.Instance.GetAxisY();
		}
		return Input.acceleration.y;
	}

	public static bool GetJumpDown()
	{
		if (!isJumping)
		{
			jumpAlreadyPressed = false;
		}
		if (isJumping && !jumpAlreadyPressed)
		{
			jumpAlreadyPressed = true;
			return true;
		}
		return false;
	}

	public static bool GetJumpKeyDown()
	{
		bool flag = false;
		if (MogaInput.Instance.IsConnected() && MogaInput.Instance.GetButtonA())
		{
			flag = true;
			if (!flag)
			{
				jumpAlreadyPressed = false;
			}
			if (flag && !jumpAlreadyPressed)
			{
				jumpAlreadyPressed = true;
			}
			else
			{
				jumpAlreadyPressed = false;
				flag = false;
			}
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition = Input.mousePosition;
			flag = (float)Screen.width * 0.5f < mousePosition.x && mousePosition.x < (float)Screen.width && 0f < mousePosition.y && mousePosition.y < (float)Screen.height * 0.75f;
			if (!flag)
			{
				jumpAlreadyPressed = false;
			}
			if (flag && !jumpAlreadyPressed)
			{
				jumpAlreadyPressed = true;
			}
			else
			{
				jumpAlreadyPressed = false;
				flag = false;
			}
		}
		return flag || Input.GetKeyDown(ActionKey.JUMP) || Input.GetKeyDown(ActionKey.JUMP2) || Input.GetKeyDown("joystick button 16");
	}

	public static bool GetJump()
	{
		return isJumping;
	}

	public static bool GetDuck()
	{
		return isDucking;
	}

	public static bool GetSuperSprint()
	{
		CheckSuperSprint();
		bool flag = isSuperSprint;
		if (isSuperSprint)
		{
			accelValueSampleCount = 0;
			isSuperSprint = false;
		}
		return flag || Input.GetKeyDown(ActionKey.SUPER_SPRINT) || (MogaInput.Instance.IsConnected() && MogaInput.Instance.GetButtonR2());
	}

	private void onPlayerReachGoal(object sender, GameEvent evn)
	{
		listenInput = false;
	}
}
