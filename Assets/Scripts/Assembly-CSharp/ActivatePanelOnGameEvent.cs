using UnityEngine;

public class ActivatePanelOnGameEvent : MonoBehaviour
{
	public string Event = string.Empty;

	public bool PlaySound;

	public SndId Sound;

	private GUI3DTransition transition;

	private void Awake()
	{
		transition = GetComponent<GUI3DTransition>();
		if (Event != string.Empty)
		{
			GameEventDispatcher.AddListener(Event, OnEvent);
		}
	}

	protected virtual void OnEvent(object sender, GameEvent evt)
	{
		if (PlaySound)
		{
			SoundManager.PlaySound((int)Sound);
		}
		transition.StartTransition();
	}
}
