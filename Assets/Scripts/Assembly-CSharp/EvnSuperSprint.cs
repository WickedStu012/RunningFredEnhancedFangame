using UnityEngine;

public class EvnSuperSprint : IEvent
{
	private ItemInfo ii;

	public EvnSuperSprint()
	{
		code = EventCode.EVN_SUPER_SPRINT;
	}

	public override bool Check()
	{
		if (ii == null)
		{
			ii = Store.Instance.GetItem(121);
		}
		if (InputManager.GetSuperSprint())
		{
			if (ii.Count > 0)
			{
				Debug.Log("ConsumingItemPopupAfterBurner1");
				GUI3DPopupManager.Instance.ShowPopup("ConsumingItemPopupAfterBurner", null, null, null, null, false, null);
				return true;
			}
			if (CharHelper.GetCharStateMachine().AfterBurnerDisplayCount == 0)
			{
				CharHelper.GetCharStateMachine().AfterBurnerDisplayCount++;
				GUI3DPopupManager.Instance.ShowPopup("ConsumingItemPopupAfterBurner", null, null, null, null, false, null);
			}
		}
		return false;
	}
}
