using UnityEngine;

public class GUI3DSlideBounceTransition : GUI3DTransition
{
	public Vector3 StartPos;

	public Vector3 EndPos;

	public float MaxSpeed = 5000f;

	public float MaxAcceleration = 20f;

	public float BounceFactor = 0.2f;

	public bool RoundPos = true;

	private Vector3 speed;

	private Vector3 destPosition;

	private Vector3 direction;

	private Vector3 position;

	private Vector3 posDiff;

	private Vector3 lastDiff;

	private Vector3 rounded;

	private bool adjusted;

	protected override void Awake()
	{
		base.Awake();
		AdjustPositions();
		base.transform.localPosition = StartPos;
	}

	private void AdjustPositions()
	{
		if (!adjusted && !(panel == null) && !(base.transform.parent.tag == "GUI"))
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
			speed = direction * MaxSpeed;
		}
		else
		{
			position = EndPos;
			speed = Vector3.zero;
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
	}

	protected override void OnStartTransition()
	{
		if (CurrentState == States.Hide || CurrentState == States.Outro)
		{
			CurrentState = States.Intro;
			destPosition = EndPos;
			position = StartPos;
			posDiff = destPosition - position;
			lastDiff = posDiff;
			direction = posDiff.normalized;
			speed = direction * MaxSpeed;
		}
		else if (CurrentState == States.Show || CurrentState == States.Intro)
		{
			CurrentState = States.Outro;
			destPosition = StartPos;
			position = EndPos;
			posDiff = destPosition - position;
			lastDiff = posDiff;
			direction = posDiff.normalized;
			speed = Vector3.zero;
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
	}

	protected override void OnUpdate()
	{
		switch (CurrentState)
		{
		case States.Intro:
			SlideIn();
			break;
		case States.Outro:
			SlideOut();
			break;
		}
	}

	private void SlideOut()
	{
		speed += direction * MaxAcceleration * deltaTime;
		if (speed.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			speed = speed.normalized * MaxSpeed;
		}
		position += speed * deltaTime;
		posDiff = destPosition - position;
		if (posDiff.sqrMagnitude >= lastDiff.sqrMagnitude)
		{
			StopTransition();
			position = destPosition;
		}
		lastDiff = posDiff;
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
	}

	private void SlideIn()
	{
		speed += direction * MaxAcceleration * deltaTime;
		if (speed.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			speed = speed.normalized * MaxSpeed;
		}
		position += speed * deltaTime;
		posDiff = destPosition - position;
		if (posDiff.sqrMagnitude >= lastDiff.sqrMagnitude && speed.normalized == direction)
		{
			position = destPosition;
			speed *= 0f - BounceFactor;
		}
		if (speed.sqrMagnitude < 2f && (destPosition - position).sqrMagnitude < 0.01f)
		{
			position = destPosition;
			SetPosition(position);
			StopTransition();
		}
		else
		{
			lastDiff = posDiff;
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
