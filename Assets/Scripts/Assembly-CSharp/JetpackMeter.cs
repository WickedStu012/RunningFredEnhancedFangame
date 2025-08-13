using UnityEngine;

public class JetpackMeter : MonoBehaviour
{
	private enum States
	{
		IDLE = 0,
		HEATING_UP = 1,
		OVERHEATING = 2,
		HEATING_DOWN = 3,
		COOLING_DOWN = 4,
		EXPLODE = 5
	}

	private const float HEATING_UP_SPEED = 100f;

	private const float HEATING_DOWN_SPEED = 50f;

	private const float OVERHEATING_TIME_MAX = 3f;

	private const float ANGLE_OVERHEATING = 135f;

	private const float ANGLE_EXPLODE = 180f;

	private const float SPRINT_HEATING_UP_SPEED = 200f; // Double heating rate for sprint mode

	public static JetpackMeter Instance;

	public GUI3DSlideTransition transition;

	private float angle;

	private States state;

	private float overheatingTimeAccum;

	private int sndJetpackOverheatingId;

	private bool rechargeJetpack = true;

	private bool isSprintMode = false;

	private void Awake()
	{
		Instance = this;
		state = States.IDLE;
	}

	private void Update()
	{
		switch (state)
		{
		case States.IDLE:
			break;
		case States.HEATING_UP:
			float heatingSpeed = isSprintMode ? SPRINT_HEATING_UP_SPEED : HEATING_UP_SPEED;
			angle += Time.deltaTime * heatingSpeed;
			if (angle >= 135f)
			{
				angle = 135f;
			}
			base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
			if (angle >= 135f)
			{
				CharHelper.GetProps().JetpackOverheating = true;
				// Only play sound if not already playing
				if (sndJetpackOverheatingId == 0)
				{
					sndJetpackOverheatingId = SoundManager.PlaySound(74);
				}
				state = States.OVERHEATING;
			}
			break;
		case States.OVERHEATING:
			float overheatingSpeed = isSprintMode ? SPRINT_HEATING_UP_SPEED : HEATING_UP_SPEED;
			angle += Time.deltaTime * overheatingSpeed;
			if (angle >= 180f)
			{
				angle = 180f;
			}
			base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
			if (angle >= 180f)
			{
				state = States.EXPLODE;
				SoundManager.StopSound(sndJetpackOverheatingId);
				CharHelper.GetCharStateMachine().onJetpackExplode();
			}
			break;
		case States.HEATING_DOWN:
			if (rechargeJetpack)
			{
				angle -= Time.deltaTime * 50f;
				if (angle <= 135f)
				{
					angle = 135f;
				}
				base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
				if (angle <= 135f)
				{
					CharHelper.GetProps().JetpackOverheating = false;
					SoundManager.StopSound(sndJetpackOverheatingId);
					state = States.COOLING_DOWN;
				}
			}
			break;
		case States.COOLING_DOWN:
			if (rechargeJetpack)
			{
				angle -= Time.deltaTime * 50f;
				if (angle <= 0f)
				{
					angle = 0f;
				}
				base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
				if (angle == 0f)
				{
					state = States.IDLE;
				}
			}
			break;
		case States.EXPLODE:
			break;
		}
	}

	public void StartUse(bool sprintMode = false)
	{
		rechargeJetpack = false;
		isSprintMode = sprintMode;
		Debug.Log("JetpackMeter StartUse - SprintMode: " + sprintMode + ", HeatingSpeed: " + (sprintMode ? SPRINT_HEATING_UP_SPEED : HEATING_UP_SPEED) + ", CurrentState: " + state);
		
		// Only prevent usage if the jetpack is actually exploding
		// Allow usage even if overheating, as long as there's fuel
		if (state == States.EXPLODE)
		{
			Debug.Log("JetpackMeter: Cannot start use - jetpack is exploding");
			// Don't allow usage if exploding
			return;
		}
		
		// If currently cooling down, continue from current angle
		if (state == States.HEATING_DOWN || state == States.COOLING_DOWN)
		{
			Debug.Log("JetpackMeter: Continuing from cooling state, angle: " + angle);
			// Continue from current state, don't reset
			state = States.HEATING_UP;
		}
		else if (CharHelper.GetProps().JetpackOverheating)
		{
			Debug.Log("JetpackMeter: Starting in overheating state");
			state = States.OVERHEATING;
			// Ensure overheating sound is playing if not already
			if (sndJetpackOverheatingId == 0)
			{
				sndJetpackOverheatingId = SoundManager.PlaySound(74);
			}
		}
		else
		{
			Debug.Log("JetpackMeter: Starting in normal heating state");
			state = States.HEATING_UP;
		}
	}

	public void StopUse()
	{
		isSprintMode = false;
		Debug.Log("JetpackMeter StopUse - CurrentState: " + state);
		if (CharHelper.GetProps().JetpackOverheating)
		{
			state = States.HEATING_DOWN;
		}
		else
		{
			state = States.COOLING_DOWN;
			// Stop overheating sound when cooling down normally
			if (sndJetpackOverheatingId != 0)
			{
				SoundManager.StopSound(sndJetpackOverheatingId);
				sndJetpackOverheatingId = 0;
			}
		}
	}

	public void Recharge()
	{
		rechargeJetpack = true;
	}

	public void Reset()
	{
		Debug.Log("JetpackMeter Reset - Previous state: " + state + ", angle: " + angle);
		angle = 0f;
		base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
		state = States.IDLE;
		CharHelper.GetProps().JetpackOverheating = false;
		SoundManager.StopSound(sndJetpackOverheatingId);
		Debug.Log("JetpackMeter Reset complete - New state: " + state);
	}

	public void StopSound()
	{
		SoundManager.StopSound(sndJetpackOverheatingId);
	}
}
