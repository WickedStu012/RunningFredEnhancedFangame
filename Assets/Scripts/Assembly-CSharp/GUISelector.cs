using UnityEngine;

public class GUISelector : MonoBehaviour
{
	public GUI3DTransition SprintBar;

	public GUI3DTransition JetpackBar;

	private ActionCode lastState;

	private void Awake()
	{
		GameEventDispatcher.AddListener("CharChangeState", OnStateChange);
	}

	private void OnStateChange(object sender, GameEvent e)
	{
		CharChangeState charChangeState = (CharChangeState)e;
		if (charChangeState.CurrentState.GetState() == ActionCode.JETPACK || charChangeState.CurrentState.GetState() == ActionCode.JETPACK_SPRINT)
		{
			if (SprintBar != null && lastState != ActionCode.JETPACK)
			{
				SprintBar.StartTransition();
				SprintBar.TransitionEndEvent += OnSprintTransitionEnd;
			}
		}
		else if (charChangeState.CurrentState.GetState() != ActionCode.JETPACK && charChangeState.CurrentState.GetState() == ActionCode.JETPACK_SPRINT)
		{
		}
		lastState = charChangeState.CurrentState.GetState();
	}

	private void OnSprintTransitionEnd(GUI3DOnTransitionEndEvent e)
	{
		if (SprintBar != null)
		{
			SprintBar.TransitionEndEvent -= OnSprintTransitionEnd;
		}
	}

	private void OnJetpackTransitionEnd(GUI3DOnTransitionEndEvent e)
	{
	}
}
