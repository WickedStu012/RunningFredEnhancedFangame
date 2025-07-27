using UnityEngine;

public class EvnWaitForTimer : IEvent
{
	private const float DEF_TIME = 5f;

	private float targetTime;

	private float accumTime;

	public EvnWaitForTimer()
	{
		code = EventCode.EVN_WAIT_FOR_TIMER;
		targetTime = 5f;
	}

	public EvnWaitForTimer(float secondsToWait)
	{
		code = EventCode.EVN_WAIT_FOR_TIMER;
		targetTime = secondsToWait;
	}

	public override void StateChange()
	{
		accumTime = 0f;
	}

	public override bool Check()
	{
		accumTime += Time.deltaTime;
		return accumTime >= targetTime;
	}
}
