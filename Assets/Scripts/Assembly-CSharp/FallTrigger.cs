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
		
		// Debug logging to help identify issues
		Debug.Log("FallTrigger OnTriggerEnter - checkUpdateCount: " + checkUpdateCount + ", collide: " + collide + ", isPlayer: " + CharHelper.IsColliderFromPlayer(c));
		
		if (checkUpdateCount == 0)
		{
			CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
			if (collide || !CharHelper.IsColliderFromPlayer(c) || GameManager.IsFredDead())
			{
				Debug.Log("FallTrigger: Skipping due to collide=" + collide + ", isPlayer=" + CharHelper.IsColliderFromPlayer(c) + ", isDead=" + GameManager.IsFredDead());
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
					Debug.Log("FallTrigger: Using safety spring");
					charStateMachine.SafetySpringUseCountInThisMatch++;
					charStateMachine.SwitchTo(ActionCode.SAFETY_SPRING);
					checkUpdateCount = 2;
				}
				else
				{
					Debug.Log("FallTrigger: Triggering fall death");
					dieFalling();
				}
			}
			else
			{
				Debug.Log("FallTrigger: Skipping due to state: " + charStateMachine.GetCurrentState() + ", isGoingUp: " + charStateMachine.IsGoingUp);
			}
		}
		else
		{
			Debug.Log("FallTrigger: Decrementing checkUpdateCount from " + checkUpdateCount + " to " + (checkUpdateCount - 1));
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
		Debug.Log("FallTrigger: Dispatching PlayerDieFalling event");
		GameEventDispatcher.Dispatch(this, new PlayerDieFalling());
	}

	public static void Reset()
	{
		checkUpdateCount = 2;
		Debug.Log("FallTrigger: Reset called, checkUpdateCount set to 2");
	}
}
