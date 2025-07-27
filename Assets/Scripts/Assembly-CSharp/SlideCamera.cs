using UnityEngine;

public class SlideCamera : MonoBehaviour
{
	public enum States
	{
		Collapsing = 0,
		Expanding = 1,
		Collapsed = 2,
		Expanded = 3
	}

	public GUI3DMenuSlideTransition MenuSlideTransition;

	public Vector3 StartPos;

	public Vector3 ExpandPos;

	public Vector3 CollapsePos;

	public float MaxSpeed = 4000f;

	public float MaxAcceleration = 10f;

	public States CurrentState = States.Expanded;

	public bool StartExpanded = true;

	private Vector3 speed;

	private Vector3 destPosition;

	private Vector3 lastDifference;

	private void Awake()
	{
		if (MenuSlideTransition != null)
		{
			MenuSlideTransition.TransitionStartEvent += OnTransitionStart;
		}
		base.transform.localPosition = StartPos;
		CurrentState = States.Expanded;
	}

	public void ToggleExpanded()
	{
		if (CurrentState == States.Collapsed || CurrentState == States.Collapsing)
		{
			CurrentState = States.Expanding;
			destPosition = ExpandPos;
			lastDifference = destPosition - base.transform.localPosition;
		}
		else if (CurrentState == States.Expanded || CurrentState == States.Expanding)
		{
			CurrentState = States.Collapsing;
			destPosition = CollapsePos;
			lastDifference = destPosition - base.transform.localPosition;
		}
	}

	private void StopTransition()
	{
		if (CurrentState == States.Expanding)
		{
			base.transform.localPosition = ExpandPos;
			CurrentState = States.Expanded;
		}
		else if (CurrentState == States.Collapsing)
		{
			base.transform.localPosition = CollapsePos;
			CurrentState = States.Collapsed;
		}
	}

	private void FixedUpdate()
	{
		States currentState = CurrentState;
		if (currentState == States.Collapsing || currentState == States.Expanding)
		{
			Slide();
		}
	}

	private void Slide()
	{
		if (!(base.transform.localPosition != destPosition))
		{
			return;
		}
		Vector3 vector = destPosition - base.transform.localPosition;
		Vector3 vector2;
		if (CurrentState == States.Expanding)
		{
			vector2 = vector * MaxAcceleration;
		}
		else
		{
			Vector3 vector3 = destPosition - base.transform.localPosition;
			if (vector3.sqrMagnitude < 1f)
			{
				vector3 = vector.normalized;
			}
			vector2 = vector3 * MaxAcceleration;
		}
		if (vector2.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			vector2 = vector2.normalized * MaxSpeed;
		}
		base.transform.localPosition += vector2 * Time.fixedDeltaTime;
		vector = destPosition - base.transform.localPosition;
		if (vector.sqrMagnitude > lastDifference.sqrMagnitude || (destPosition - base.transform.localPosition).sqrMagnitude < 0.01f)
		{
			StopTransition();
			base.transform.localPosition = destPosition;
		}
		lastDifference = vector;
	}

	private void OnTransitionStart(GUI3DOnTransitionStartEvent evt)
	{
		if (MenuSlideTransition.CurrentState == GUI3DTransition.States.Outro && CurrentState != States.Expanded && CurrentState != States.Expanding)
		{
			ToggleExpanded();
		}
	}
}
