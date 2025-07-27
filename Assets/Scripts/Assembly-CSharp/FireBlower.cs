using UnityEngine;

public class FireBlower : MonoBehaviour
{
	public enum Mode
	{
		DISABLED = 0,
		CONSTANT = 1,
		BURSTS = 2
	}

	public Mode mode = Mode.CONSTANT;

	public float startTimeForBurst;

	public float activeTime;

	public float notActiveTime;

	public ParticleSystem[] flameParticleSystem;

	public bool disableOnAverageOrLower;

	private bool collide;

	private bool isActive = true;

	private float accumTime;

	private int sndFireBlowerId;

	private int updateCountToSndPlay = 2;

	private void Start()
	{
		if (disableOnAverageOrLower)
		{
			mode = Mode.DISABLED;
			base.gameObject.SetActive(false);
			return;
		}
		accumTime = startTimeForBurst;
		if (Profile.LessOrEqualTo(PerformanceScore.AVERAGE))
		{
			Light componentInChildren = base.transform.GetComponentInChildren<Light>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren.gameObject);
			}
		}
	}

	private void Update()
	{
		if (mode == Mode.DISABLED)
		{
			return;
		}
		if (updateCountToSndPlay > 0)
		{
			updateCountToSndPlay--;
			if (updateCountToSndPlay == 0 && base.GetComponent<AudioSource>() != null)
			{
				base.GetComponent<AudioSource>().Play();
			}
		}
		if (mode != Mode.BURSTS)
		{
			return;
		}
		accumTime += Time.deltaTime;
		if (isActive)
		{
			if (accumTime > activeTime)
			{
				setActive(false);
				accumTime = 0f;
			}
		}
		else if (accumTime > notActiveTime)
		{
			setActive(true);
			accumTime = 0f;
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && isActive && CharHelper.IsColliderFromPlayer(c) && mode != Mode.DISABLED)
		{
			collide = true;
			if (!ProtectiveVestHelper.UseProtectiveVestIfAvailable())
			{
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BURNT);
			}
			else
			{
				SoundManager.PlaySound(SndId.SND_FRED_OUCH);
			}
		}
	}

	private void setActive(bool val)
	{
		isActive = val;
		for (int i = 0; i < flameParticleSystem.Length; i++)
		{
			flameParticleSystem[i].Emit = val;
		}
	}
}
