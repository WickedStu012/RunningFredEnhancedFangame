using UnityEngine;
public class EvnJetpackSprint : IEvent
{
	private CharProps props;

	public EvnJetpackSprint()
	{
		code = EventCode.EVN_JETPACK_SPRINT;
		Debug.Log("EvnJetpackSprint event created");
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		
		// Check if player has jetpack and fuel
		if (!props.HasJetpack || props.JetPackFuelLeft <= 0f)
		{
			return false;
		}
		
		// Check if jetpack is exploding (only prevent usage if actually exploding)
		if (JetpackMeter.Instance != null)
		{
			// Allow usage even if overheating, as long as there's fuel
			// Only prevent if the meter is in EXPLODE state
			// This will be handled by the meter's StartUse method
		}
		
		// Check for super sprint input (device tilt forward or sprint key)
		if (InputManager.GetSuperSprint())
		{
			Debug.Log("JetpackSprint triggered by GetSuperSprint()");
			return true;
		}
		
		// Check for up key press directly (with reduced sensitivity)
		if (Input.GetKeyDown(ActionKey.MOVE_UP) || Input.GetKeyDown(ActionKey.MOVE_UP_2))
		{
			Debug.Log("JetpackSprint triggered by up key press");
			return true;
		}
		
		// Check for up movement input (device tilt forward)
		// Note: CheckMoveActionsUpDownInverted uses negative values for up movement
		CharStateMachine sm = CharHelper.GetCharStateMachine();
		if (sm != null && sm.SteerDirectionUpDown < -0.4f)
		{
			Debug.Log("JetpackSprint triggered by SteerDirectionUpDown: " + sm.SteerDirectionUpDown);
			return true;
		}
		
		return false;
	}
}
