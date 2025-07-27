using UnityEngine;

public class ActivateTreasurePanel : MonoBehaviour
{
	public GUI3DText Text;

	public string Event = string.Empty;

	private GUI3DTransition transition;

	private void Awake()
	{
		transition = GetComponent<GUI3DTransition>();
		if (Event != string.Empty)
		{
			GameEventDispatcher.AddListener(Event, OnEvent);
		}
	}

	private void OnEvent(object sender, GameEvent evt)
	{
		TreasurePickup treasurePickup = (TreasurePickup)evt;
		Text.SetDynamicText("+" + treasurePickup.GoldCount);
		transition.StartTransition();
	}
}
