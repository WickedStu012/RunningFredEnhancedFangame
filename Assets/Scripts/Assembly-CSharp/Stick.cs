using UnityEngine;

public class Stick : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		if (CharHelper.IsColliderFromPlayer(col) && CharHelper.GetCharStateMachine().GetCurrentState() != ActionCode.BALANCE)
		{
			CharHelper.GetCharStateMachine().SwitchTo(ActionCode.BALANCE);
		}
	}
}
