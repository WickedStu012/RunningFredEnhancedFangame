using UnityEngine;

public class Accelerator : MonoBehaviour
{
	private bool collide;

	private float accumTime;

	private void Start()
	{
		collide = false;
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 2f)
			{
				collide = false;
				accumTime = 0f;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			switch (CharHelper.GetCharStateMachine().GetCurrentState())
			{
			case ActionCode.RUNNING:
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.SUPER_SPRINT);
				collide = true;
				break;
			case ActionCode.SUPER_SPRINT:
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.MEGA_SPRINT);
				collide = true;
				break;
			case ActionCode.MEGA_SPRINT:
			{
				IAction currentAction = CharHelper.GetCharStateMachine().GetCurrentAction();
				ActMegaSprint actMegaSprint = currentAction as ActMegaSprint;
				actMegaSprint.ResetTimerToReturnToRunning(true);
				collide = true;
				break;
			}
			}
		}
	}

	private void OnTriggerExit(Collider c)
	{
		if (collide && CharHelper.IsColliderFromPlayer(c))
		{
			ActionCode currentState = CharHelper.GetCharStateMachine().GetCurrentState();
			IAction currentAction = CharHelper.GetCharStateMachine().GetCurrentAction();
			switch (currentState)
			{
			case ActionCode.SUPER_SPRINT:
			{
				ActSuperSprint actSuperSprint = currentAction as ActSuperSprint;
				actSuperSprint.ResetTimerToReturnToRunning();
				break;
			}
			case ActionCode.MEGA_SPRINT:
			{
				ActMegaSprint actMegaSprint = currentAction as ActMegaSprint;
				actMegaSprint.ResetTimerToReturnToRunning(false);
				break;
			}
			}
		}
	}
}
