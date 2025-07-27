using UnityEngine;

public class ExpandButton : MonoBehaviour
{
	public LogoCollapseExpandTransition LogoTransition;

	public SlideCamera SlideCamera;

	public GUI3DPopFrontTransition TapForInstantAction;

	public GUI3DSlideTransition MoreGamesTransition;

	public Vector3 OriginalPos;

	public Vector3 ExpandedPos;

	private GUI3DButton button;

	private GUI3DMenuSlideTransition transition;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent += ReleaseEvent;
	}

	private void OnDisable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.ReleaseEvent -= ReleaseEvent;
	}

	private void ReleaseEvent(GUI3DOnReleaseEvent evt)
	{
		if (transition == null && button.GetPanel() != null)
		{
			transition = button.GetPanel().GetComponent<GUI3DMenuSlideTransition>();
		}
		if (transition != null)
		{
			if (transition.CurrentState == GUI3DTransition.States.Expanded || transition.CurrentState == GUI3DTransition.States.Expanding)
			{
				SoundManager.PlaySound(SndIdMenu.SND_MENU_COLLAPSE_MENU);
			}
			else
			{
				StatsManager.LogEvent(StatVar.MAIN_MENU_BUTTON, "EXPAND");
				SoundManager.PlaySound(SndIdMenu.SND_MENU_EXPAND_MENU);
			}
			transition.ToggleExpanded();
		}
		if (LogoTransition != null && ((transition.CurrentState == GUI3DTransition.States.Expanding && (LogoTransition.CurrentState == GUI3DTransition.States.Expanded || LogoTransition.CurrentState == GUI3DTransition.States.Expanding)) || (transition.CurrentState == GUI3DTransition.States.Collapsing && (LogoTransition.CurrentState == GUI3DTransition.States.Collapsed || LogoTransition.CurrentState == GUI3DTransition.States.Collapsing))))
		{
			LogoTransition.ToggleExpanded();
		}
		if (SlideCamera != null && ((transition.CurrentState == GUI3DTransition.States.Expanding && (SlideCamera.CurrentState == SlideCamera.States.Expanded || SlideCamera.CurrentState == SlideCamera.States.Expanding)) || (transition.CurrentState == GUI3DTransition.States.Collapsing && (SlideCamera.CurrentState == SlideCamera.States.Collapsed || SlideCamera.CurrentState == SlideCamera.States.Collapsing))))
		{
			SlideCamera.ToggleExpanded();
		}
		if (TapForInstantAction != null && ((transition.CurrentState == GUI3DTransition.States.Expanding && (TapForInstantAction.CurrentState == GUI3DTransition.States.Show || TapForInstantAction.CurrentState == GUI3DTransition.States.Intro)) || (transition.CurrentState == GUI3DTransition.States.Collapsing && (TapForInstantAction.CurrentState == GUI3DTransition.States.Hide || TapForInstantAction.CurrentState == GUI3DTransition.States.Outro))))
		{
			TapForInstantAction.StartTransition();
		}
		if (MoreGamesTransition != null && ((transition.CurrentState == GUI3DTransition.States.Expanding && (MoreGamesTransition.CurrentState == GUI3DTransition.States.Show || MoreGamesTransition.CurrentState == GUI3DTransition.States.Intro)) || (transition.CurrentState == GUI3DTransition.States.Collapsing && (MoreGamesTransition.CurrentState == GUI3DTransition.States.Hide || MoreGamesTransition.CurrentState == GUI3DTransition.States.Outro))))
		{
			MoreGamesTransition.StartTransition();
		}
	}
}
