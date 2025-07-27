using UnityEngine;

public class HideHudOnEvent : MonoBehaviour
{
	public GUI3DSlideTransition[] Transitions;

	public string[] HideEvents;

	public string[] ShowEvents;

	public GUI3D Gui;

	private void Awake()
	{
		if (HideEvents != null && HideEvents.Length > 0)
		{
			string[] hideEvents = HideEvents;
			foreach (string evt in hideEvents)
			{
				GameEventDispatcher.AddListener(evt, OnHideEvent);
			}
		}
		if (ShowEvents != null && ShowEvents.Length > 0)
		{
			string[] showEvents = ShowEvents;
			foreach (string evt2 in showEvents)
			{
				GameEventDispatcher.AddListener(evt2, OnShowEvent);
			}
		}
	}

	private void OnHideEvent(object sender, GameEvent evt)
	{
		if (Gui != null)
		{
			Gui.CheckEvents = false;
		}
		if (Transitions == null || Transitions.Length <= 0)
		{
			return;
		}
		GUI3DSlideTransition[] transitions = Transitions;
		foreach (GUI3DSlideTransition gUI3DSlideTransition in transitions)
		{
			if (gUI3DSlideTransition != null && (gUI3DSlideTransition.CurrentState == GUI3DTransition.States.Show || gUI3DSlideTransition.CurrentState == GUI3DTransition.States.Intro))
			{
				gUI3DSlideTransition.StartTransition();
			}
		}
	}

	private void OnShowEvent(object sender, GameEvent evt)
	{
		if (Gui != null)
		{
			Gui.CheckEvents = true;
		}
		if (Transitions == null || Transitions.Length <= 0)
		{
			return;
		}
		GUI3DSlideTransition[] transitions = Transitions;
		foreach (GUI3DSlideTransition gUI3DSlideTransition in transitions)
		{
			if (gUI3DSlideTransition != null && (gUI3DSlideTransition.CurrentState == GUI3DTransition.States.Hide || gUI3DSlideTransition.CurrentState == GUI3DTransition.States.Outro))
			{
				gUI3DSlideTransition.StartTransition();
			}
		}
	}
}
