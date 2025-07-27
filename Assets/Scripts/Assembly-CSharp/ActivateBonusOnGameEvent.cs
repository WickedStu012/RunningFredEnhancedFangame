using UnityEngine;

public class ActivateBonusOnGameEvent : MonoBehaviour
{
	public string Event = string.Empty;

	private GUI3DTransition transition;

	private GUI3DText text;

	private void Awake()
	{
		transition = GetComponent<GUI3DTransition>();
		if (Event != string.Empty)
		{
			GameEventDispatcher.AddListener(Event, OnEvent);
		}
		text = GetComponentInChildren<GUI3DText>();
	}

	protected virtual void OnEvent(object sender, GameEvent evt)
	{
		transition.StartTransition();
		if (text != null)
		{
			text.SetDynamicText(((OnSurvivalLevelUp)evt).LevelName);
		}
	}
}
