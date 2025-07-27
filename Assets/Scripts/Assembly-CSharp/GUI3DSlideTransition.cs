using UnityEngine;

public class GUI3DSlideTransition : GUI3DTransition
{
	public enum Type
	{
		AtEnd = 0,
		AtBegining = 1
	}

	public Vector3 StartPos;

	public Vector3 EndPos;

	public float MaxSpeed = 5000f;

	public float MaxAcceleration = 20f;

	public bool RoundPos = true;

	public Type EaseType;

	private Vector3 speed = Vector3.zero;

	private Vector3 destPosition;

	private Vector3 lastDifference;

	private Vector3 position;

	private Vector3 rounded;

	private bool adjusted;

	protected override void Awake()
	{
		base.Awake();
		AdjustPositions();
		Vector3 startPos = StartPos;
		base.transform.localPosition = startPos;
		position = startPos;
	}

	private void AdjustPositions()
	{
		if (!adjusted && !(panel == null) && !(base.transform.parent.tag != "GUI"))
		{
			adjusted = true;
			if (panel.AutoAdjustPosition)
			{
				panel.AutoAdjustPosition = false;
				StartPos.x = StartPos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				StartPos.y = StartPos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				EndPos.x = EndPos.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				EndPos.y = EndPos.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				MaxSpeed = MaxSpeed / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				MaxAcceleration = MaxAcceleration / (float)panel.ReferenceScreenWidth * (float)Screen.width;
			}
		}
	}

	protected override void OnResetTransition()
	{
		AdjustPositions();
		if (CurrentState != States.Show)
		{
			position = StartPos;
			CurrentState = States.Hide;
		}
		else
		{
			position = EndPos;
		}
		if (RoundPos)
		{
			rounded.x = Mathf.Round(position.x);
			rounded.y = Mathf.Round(position.y);
			rounded.z = Mathf.Round(position.z);
		}
		else
		{
			rounded = position;
		}
		base.transform.localPosition = rounded;
		speed = Vector3.zero;
	}

	protected override void OnStartTransition()
	{
		if (CurrentState == States.Hide || CurrentState == States.Outro)
		{
			CurrentState = States.Intro;
			destPosition = EndPos;
			position = StartPos;
			lastDifference = destPosition - StartPos;
		}
		else if (CurrentState == States.Show || CurrentState == States.Intro)
		{
			CurrentState = States.Outro;
			destPosition = StartPos;
			position = EndPos;
			lastDifference = destPosition - EndPos;
		}
		if (RoundPos)
		{
			rounded.x = Mathf.Round(position.x);
			rounded.y = Mathf.Round(position.y);
			rounded.z = Mathf.Round(position.z);
		}
		else
		{
			rounded = position;
		}
		base.transform.localPosition = rounded;
		speed = Vector3.zero;
	}

	protected override void OnStopTransition()
	{
		if (CurrentState == States.Intro)
		{
			position = EndPos;
			CurrentState = States.Show;
		}
		else if (CurrentState == States.Outro)
		{
			position = StartPos;
			CurrentState = States.Hide;
		}
		if (RoundPos)
		{
			rounded.x = Mathf.Round(position.x);
			rounded.y = Mathf.Round(position.y);
			rounded.z = Mathf.Round(position.z);
		}
		else
		{
			rounded = position;
		}
		base.transform.localPosition = rounded;
		speed = Vector3.zero;
	}

	protected override void OnUpdate()
	{
		States currentState = CurrentState;
		if (currentState == States.Intro || currentState == States.Outro)
		{
			Slide();
		}
	}

	private void Slide()
	{
		if (!(position != destPosition))
		{
			return;
		}
		Vector3 vector = destPosition - position;
		if (EaseType == Type.AtEnd)
		{
			if (CurrentState == States.Intro)
			{
				speed = vector * MaxAcceleration;
			}
			else
			{
				Vector3 vector2 = position - EndPos;
				if (vector2.sqrMagnitude < 1f)
				{
					vector2 = vector.normalized;
				}
				speed = vector2 * MaxAcceleration;
			}
		}
		else if (EaseType == Type.AtBegining)
		{
			speed += vector.normalized * MaxAcceleration * deltaTime;
		}
		if (speed.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			speed = speed.normalized * MaxSpeed;
		}
		position += speed * deltaTime;
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
		if (RoundPos)
		{
			rounded.x = Mathf.Round(position.x);
			rounded.y = Mathf.Round(position.y);
			rounded.z = Mathf.Round(position.z);
			base.transform.localPosition = rounded;
		}
		else
		{
			base.transform.localPosition = position;
		}
	}
}
