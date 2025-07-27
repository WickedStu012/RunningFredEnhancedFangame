using UnityEngine;

public class GUI3DMenuSlideTransition : GUI3DTransition
{
	public Vector3 StartPos;

	public Vector3 ExpandPos;

	public Vector3 CollapsePos;

	public float MaxSpeed = 4000f;

	public float MaxAcceleration = 10f;

	public bool StartExpanded = true;

	private Vector3 speed;

	private Vector3 destPosition;

	private Vector3 lastDifference;

	private Vector3 position;

	private Vector3 rounded;

	private bool adjusted;

	protected override void Awake()
	{
		base.Awake();
	}

	public void ToggleExpanded()
	{
		position = base.transform.localPosition;
		if (CurrentState == States.Hide || CurrentState == States.Outro || CurrentState == States.Collapsed || CurrentState == States.Collapsing)
		{
			CurrentState = States.Expanding;
			destPosition = ExpandPos;
			lastDifference = destPosition - position;
		}
		else if (CurrentState == States.Intro || CurrentState == States.Expanded || CurrentState == States.Expanding)
		{
			CurrentState = States.Collapsing;
			destPosition = CollapsePos;
			lastDifference = destPosition - position;
		}
	}

	private void AdjustPositions()
	{
		if (!adjusted)
		{
			adjusted = true;
			if (panel.AutoAdjustPosition)
			{
				panel.AutoAdjustPosition = false;
				StartPos.x = StartPos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				StartPos.y = StartPos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				ExpandPos.x = ExpandPos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				ExpandPos.y = ExpandPos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				CollapsePos.x = CollapsePos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				CollapsePos.y = CollapsePos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
			}
		}
	}

	protected override void OnResetTransition()
	{
		AdjustPositions();
		destPosition = (position = StartPos);
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		base.transform.localPosition = rounded;
	}

	protected override void OnStartTransition()
	{
		if (CurrentState == States.Hide || CurrentState == States.Outro)
		{
			CurrentState = States.Intro;
			if (StartExpanded)
			{
				destPosition = ExpandPos;
			}
			else
			{
				destPosition = CollapsePos;
			}
			position = StartPos;
			lastDifference = destPosition - StartPos;
		}
		else if (CurrentState == States.Expanding || CurrentState == States.Collapsing || CurrentState == States.Collapsed || CurrentState == States.Expanded || CurrentState == States.Intro)
		{
			CurrentState = States.Outro;
			destPosition = StartPos;
			position = base.transform.localPosition;
			lastDifference = destPosition - position;
		}
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		base.transform.localPosition = rounded;
	}

	protected override void OnStopTransition()
	{
		if (CurrentState == States.Expanding)
		{
			position = ExpandPos;
			CurrentState = States.Expanded;
		}
		else if (CurrentState == States.Collapsing)
		{
			position = CollapsePos;
			CurrentState = States.Collapsed;
		}
		else if (CurrentState == States.Intro)
		{
			if (StartExpanded)
			{
				position = ExpandPos;
				CurrentState = States.Expanded;
			}
			else
			{
				position = CollapsePos;
				CurrentState = States.Collapsed;
			}
		}
		else if (CurrentState == States.Outro)
		{
			position = StartPos;
			CurrentState = States.Hide;
		}
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		base.transform.localPosition = rounded;
	}

	protected override void OnUpdate()
	{
		switch (CurrentState)
		{
		case States.Intro:
		case States.Outro:
		case States.Collapsing:
		case States.Expanding:
			Slide();
			break;
		}
	}

	private void Slide()
	{
		if (!(position != destPosition))
		{
			return;
		}
		Vector3 vector = destPosition - position;
		Vector3 vector2;
		if (CurrentState == States.Intro || CurrentState == States.Expanding)
		{
			vector2 = vector * MaxAcceleration;
		}
		else
		{
			Vector3 vector3 = destPosition - position;
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
		position += vector2 * deltaTime;
		vector = destPosition - position;
		if (vector.sqrMagnitude > lastDifference.sqrMagnitude || (destPosition - position).sqrMagnitude < 0.01f)
		{
			position = destPosition;
			SetPosition(position);
			StopTransition();
		}
		else
		{
			lastDifference = vector;
			SetPosition(position);
		}
	}

	private void SetPosition(Vector3 position)
	{
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		base.transform.localPosition = rounded;
	}
}
