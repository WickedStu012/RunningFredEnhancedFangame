using UnityEngine;

public class LogoCollapseExpandTransition : GUI3DTransition
{
	public Vector3 StartPos;

	public Vector3 EndPos;

	public Vector3 CollapsePos;

	public Vector3 CollapseScale;

	public Vector3 ExpandScale;

	public float MaxSpeed = 4000f;

	public float MaxAcceleration = 10f;

	public float MaxScaleSpeed = 1f;

	private Vector3 speed;

	private Vector3 destPosition;

	private Vector3 lastDifference;

	private Vector3 scaleSpeed;

	private Vector3 destScale;

	private Vector3 lastScaleDifference;

	private bool adjusted;

	protected override void Awake()
	{
		base.Awake();
		OnResetTransition();
	}

	private void AdjustPosition()
	{
		if (!adjusted)
		{
			adjusted = true;
			if (panel.AutoAdjustPosition)
			{
				panel.AutoAdjustPosition = false;
				StartPos.x = StartPos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				StartPos.y = StartPos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				EndPos.x = EndPos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				EndPos.y = EndPos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				CollapsePos.x = CollapsePos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				CollapsePos.y = CollapsePos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				CollapseScale.x = CollapseScale.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				CollapseScale.y = CollapseScale.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				ExpandScale.x = ExpandScale.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				ExpandScale.y = ExpandScale.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
			}
		}
	}

	public void ToggleExpanded()
	{
		if (CurrentState == States.Collapsed || CurrentState == States.Collapsing)
		{
			CurrentState = States.Expanding;
			destScale = ExpandScale;
			base.transform.localScale = CollapseScale;
			lastScaleDifference = destScale - CollapseScale;
			destPosition = EndPos;
			lastDifference = destPosition - base.transform.localPosition;
		}
		else if (CurrentState == States.Expanded || CurrentState == States.Expanding)
		{
			CurrentState = States.Collapsing;
			destScale = CollapseScale;
			base.transform.localScale = ExpandScale;
			lastScaleDifference = destScale - ExpandScale;
			destPosition = CollapsePos;
			lastDifference = destPosition - base.transform.localPosition;
		}
		speed = lastScaleDifference.normalized * MaxScaleSpeed;
	}

	protected override void OnResetTransition()
	{
		AdjustPosition();
		base.transform.localPosition = StartPos;
		CurrentState = States.Hide;
	}

	protected override void OnStartTransition()
	{
		if (CurrentState == States.Hide || CurrentState == States.Outro)
		{
			CurrentState = States.Intro;
			destPosition = EndPos;
			base.transform.localScale = ExpandScale;
			base.transform.localPosition = StartPos;
			lastDifference = destPosition - StartPos;
		}
		else if (CurrentState == States.Collapsed || CurrentState == States.Expanded || CurrentState == States.Expanding || CurrentState == States.Collapsing || CurrentState == States.Intro)
		{
			CurrentState = States.Outro;
			destPosition = StartPos;
			lastDifference = destPosition - base.transform.localPosition;
		}
	}

	protected override void OnStopTransition()
	{
		if (CurrentState == States.Intro)
		{
			base.transform.localPosition = EndPos;
			CurrentState = States.Expanded;
		}
		else if (CurrentState == States.Outro)
		{
			base.transform.localPosition = StartPos;
			CurrentState = States.Hide;
		}
	}

	protected override void OnUpdate()
	{
		switch (CurrentState)
		{
		case States.Intro:
		case States.Outro:
			Slide();
			break;
		case States.Collapsing:
		case States.Expanding:
			Scale();
			Slide();
			break;
		}
	}

	private void Scale()
	{
		if (base.transform.localScale != destScale)
		{
			base.transform.localScale += speed * deltaTime;
			Vector3 vector = destScale - base.transform.localScale;
			if (vector.sqrMagnitude > lastScaleDifference.sqrMagnitude || vector.sqrMagnitude < 0.001f)
			{
				StopTransition();
				base.transform.localScale = destScale;
			}
			lastScaleDifference = vector;
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
		if (CurrentState == States.Intro || CurrentState == States.Expanding)
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
		base.transform.localPosition += vector2 * deltaTime;
		vector = destPosition - base.transform.localPosition;
		if (vector.sqrMagnitude > lastDifference.sqrMagnitude || vector.sqrMagnitude < 0.01f)
		{
			StopTransition();
			base.transform.localPosition = destPosition;
		}
		lastDifference = vector;
	}
}
