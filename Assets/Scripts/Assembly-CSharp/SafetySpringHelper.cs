public class SafetySpringHelper
{
	public static bool WillShowSafetySpringPopup()
	{
		CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
		ItemInfo item = Store.Instance.GetItem(122);
		if ((charStateMachine.GetCurrentState() != ActionCode.SAFETY_SPRING || !charStateMachine.IsGoingUp) && item.Count > 0 && charStateMachine.SafetySpringUseCountInThisMatch == 0)
		{
			charStateMachine.SafetySpringUseCountInThisMatch++;
			charStateMachine.SwitchTo(ActionCode.SAFETY_SPRING);
			return true;
		}
		return false;
	}
}
