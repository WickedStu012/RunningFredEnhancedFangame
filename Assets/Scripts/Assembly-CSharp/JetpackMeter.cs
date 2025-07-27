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

	public static JetpackMeter Instance;

	public GUI3DSlideTransition transition;

	private float angle;

	private States state;

	private float overheatingTimeAccum;

	private int sndJetpackOverheatingId;

	private bool rechargeJetpack = true;

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
			angle += Time.deltaTime * 100f;
			if (angle >= 135f)
			{
				angle = 135f;
			}
			base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
			if (angle >= 135f)
			{
				CharHelper.GetProps().JetpackOverheating = true;
				sndJetpackOverheatingId = SoundManager.PlaySound(74);
				state = States.OVERHEATING;
			}
			break;
		case States.OVERHEATING:
			angle += Time.deltaTime * 100f;
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

	public void StartUse()
	{
		rechargeJetpack = false;
		if (CharHelper.GetProps().JetpackOverheating)
		{
			state = States.OVERHEATING;
		}
		else
		{
			state = States.HEATING_UP;
		}
	}

	public void StopUse()
	{
		if (CharHelper.GetProps().JetpackOverheating)
		{
			state = States.HEATING_DOWN;
		}
		else
		{
			state = States.COOLING_DOWN;
		}
	}

	public void Recharge()
	{
		rechargeJetpack = true;
	}

	public void Reset()
	{
		angle = 0f;
		base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - angle);
		state = States.IDLE;
		CharHelper.GetProps().JetpackOverheating = false;
		SoundManager.StopSound(sndJetpackOverheatingId);
	}

	public void StopSound()
	{
		SoundManager.StopSound(sndJetpackOverheatingId);
	}
}
