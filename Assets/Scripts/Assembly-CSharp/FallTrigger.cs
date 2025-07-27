using UnityEngine;

public class FallTrigger : MonoBehaviour
{
	public bool ignoreSprings;

	private static bool collide;

	private bool check;

	private static int checkUpdateCount = 2;

	private static float accumTime;

	private static float lastTime;

	private ItemInfo itemSafetySpring;

	private void Awake()
	{
		GameEventDispatcher.AddListener("OnLevelLoaded", onLevelLoaded);
		lastTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			if (accumTime > 3f)
			{
				collide = false;
				accumTime = 0f;
			}
		}
	}

	private void onLevelLoaded(object sender, GameEvent evn)
	{
		check = true;
		checkUpdateCount = 2;
	}

	public void EnableTriggerCheck()
	{
		check = true;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!check)
		{
			return;
		}
		if (checkUpdateCount == 0)
		{
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			if (collide || !CharHelper.IsColliderFromPlayer(c) || GameManager.IsFredDead())
			{
				return;
			}
			if (itemSafetySpring == null)
			{
				itemSafetySpring = Store.Instance.GetItem(122);
			}
			if (charStateMachine.GetCurrentState() != ActionCode.RUNNING_TO_GOAL && (charStateMachine.GetCurrentState() != ActionCode.SAFETY_SPRING || !charStateMachine.IsGoingUp))
			{
				if (!ignoreSprings && itemSafetySpring.Count > 0 && charStateMachine.SafetySpringUseCountInThisMatch == 0)
				{
					charStateMachine.SafetySpringUseCountInThisMatch++;
					charStateMachine.SwitchTo(ActionCode.SAFETY_SPRING);
					checkUpdateCount = 2;
				}
				else
				{
					dieFalling();
				}
			}
		}
		else
		{
			checkUpdateCount--;
		}
	}

	private void dieFalling()
	{
		accumTime = 0f;
		lastTime = Time.realtimeSinceStartup;
		collide = true;
		SoundManager.PlaySound(27);
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival && CharHelper.GetProps().HasWings)
		{
			CharHelper.GetCharStateMachine().RemoveWings(true, true);
		}
		GameEventDispatcher.Dispatch(this, new PlayerDieFalling());
	}

	public static void Reset()
	{
		checkUpdateCount = 2;
	}
}
