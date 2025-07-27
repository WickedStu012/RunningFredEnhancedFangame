public class ProtectiveVestHelper
{
	public static bool UseProtectiveVestIfAvailable()
	{
		CharStateMachine charStateMachine = CharHelper.GetCharStateMachine();
		ItemInfo item = Store.Instance.GetItem(107);
		if (item.Count > 0 && charStateMachine.ProtectiveVestUseCountInThisMatch == 0)
		{
			charStateMachine.ProtectiveVestUseCountInThisMatch++;
			SoundManager.PlaySound(67);
			GUI3DPopupManager.Instance.ShowPopup("ConsumingItemPopupProtectiveVest", null, null, null, null, false, null);
			CharHelper.GetEffects().EnableProtectiveVestParticles();
			return true;
		}
		return false;
	}
}
