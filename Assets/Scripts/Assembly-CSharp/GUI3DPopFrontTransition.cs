using UnityEngine;

public class GUI3DPopFrontTransition : GUI3DTransition
{
	public Vector3 StartScale = new Vector3(0f, 0f, 1f);

	public Vector3 EndScale = new Vector3(1f, 1f, 1f);

	public float MaxScaleSpeed = 15f;

	public float MaxAcceleration = 50f;

	public float BounceFactor = 0.2f;

	private Vector3 origScale;

	private Vector3 speed;

	private Vector3 scale;

	private Vector3 difference;

	private Vector3 lastDifference;

	private Vector3 direction;

	private int samePositionFrames;

	private bool adjusted;

	private float factor = 1f;

	protected override void Awake()
	{
		base.Awake();
	}

	private void AdjustScale()
	{
		if (adjusted)
		{
			return;
		}
		adjusted = true;
		origScale = EndScale;
		if (panel != null && base.transform.parent.tag == "GUI" && panel.AutoAdjustScale != GUI3DAdjustScale.None && (Screen.width != panel.ReferenceScreenWidth || Screen.height != panel.ReferenceScreenHeight))
		{
			Vector3 endScale = EndScale;
			float num = 0f;
			float num2 = 0f;
			switch (panel.AutoAdjustScale)
			{
			case GUI3DAdjustScale.Stretch:
				endScale.x = endScale.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				endScale.y = endScale.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				MaxScaleSpeed = MaxScaleSpeed / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				MaxAcceleration = MaxAcceleration / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				break;
			case GUI3DAdjustScale.StretchHorizontal:
				endScale.x = endScale.x / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				MaxScaleSpeed = MaxScaleSpeed / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				MaxAcceleration = MaxAcceleration / (float)panel.ReferenceScreenWidth * (float)Screen.width;
				break;
			case GUI3DAdjustScale.StretchVertical:
				endScale.y = endScale.y / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				MaxScaleSpeed = MaxScaleSpeed / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				MaxAcceleration = MaxAcceleration / (float)panel.ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchAverageToFitAspect:
				num = (float)(panel.ReferenceScreenWidth + panel.ReferenceScreenHeight) / 2f;
				num2 = (float)(Screen.width + Screen.height) / 2f;
				endScale.x = endScale.x / num * num2;
				endScale.y = endScale.y / num * num2;
				endScale.z = endScale.z / num * num2;
				MaxScaleSpeed = MaxScaleSpeed / num * num2;
				MaxAcceleration = MaxAcceleration / num * num2;
				break;
			case GUI3DAdjustScale.StretchMaxToFitAspect:
				num = Mathf.Max(panel.ReferenceScreenWidth, panel.ReferenceScreenHeight);
				num2 = Mathf.Max(Screen.width, Screen.height);
				endScale.x = endScale.x / num * num2;
				endScale.y = endScale.y / num * num2;
				endScale.z = endScale.z / num * num2;
				MaxScaleSpeed = MaxScaleSpeed / num * num2;
				MaxAcceleration = MaxAcceleration / num * num2;
				break;
			case GUI3DAdjustScale.StretchMinToFitAspect:
				num = Mathf.Min(panel.ReferenceScreenWidth, panel.ReferenceScreenHeight);
				num2 = Mathf.Min(Screen.width, Screen.height);
				endScale.x = endScale.x / num * num2;
				endScale.y = endScale.y / num * num2;
				endScale.z = endScale.z / num * num2;
				MaxScaleSpeed = MaxScaleSpeed / num * num2;
				MaxAcceleration = MaxAcceleration / num * num2;
				break;
			}
			EndScale = (origScale = endScale);
		}
	}

	protected override void OnResetTransition()
	{
		AdjustScale();
		if (CurrentState != States.Show)
		{
			scale = StartScale;
			CurrentState = States.Hide;
		}
		else
		{
			scale = origScale;
		}
		if (Application.isPlaying)
		{
			base.transform.localScale = scale;
		}
	}

	protected override void OnStartTransition()
	{
		if (CurrentState == States.Hide || CurrentState == States.Outro)
		{
			CurrentState = States.Intro;
			scale = StartScale;
			difference = (lastDifference = origScale - scale);
			direction = difference.normalized;
			speed = direction * MaxScaleSpeed * factor;
			base.transform.localScale = scale;
		}
		else if (CurrentState == States.Show || CurrentState == States.Intro)
		{
			CurrentState = States.Outro;
			scale = origScale;
			difference = (lastDifference = StartScale - scale);
			direction = difference.normalized;
			speed = direction * MaxScaleSpeed * factor;
			base.transform.localScale = scale;
		}
		samePositionFrames = 0;
	}

	protected override void OnStopTransition()
	{
		if (CurrentState == States.Intro)
		{
			scale = origScale;
			CurrentState = States.Show;
		}
		else if (CurrentState == States.Outro)
		{
			scale = StartScale;
			CurrentState = States.Hide;
		}
		base.transform.localScale = scale;
		samePositionFrames = 0;
	}

	protected override void OnUpdate()
	{
		switch (CurrentState)
		{
		case States.Intro:
			PoppingIn();
			break;
		case States.Outro:
			PoppingOut();
			break;
		}
	}

	private void PoppingIn()
	{
		speed += direction * MaxAcceleration * deltaTime * factor;
		if (speed.sqrMagnitude > MaxScaleSpeed * MaxScaleSpeed * factor)
		{
			speed = speed.normalized * MaxScaleSpeed * factor;
		}
		scale += speed * deltaTime * factor;
		difference = origScale - scale;
		if (difference.sqrMagnitude >= lastDifference.sqrMagnitude && speed.normalized == direction)
		{
			scale = origScale;
			speed *= 0f - BounceFactor;
			difference = origScale - scale;
		}
		lastDifference = difference;
		base.transform.localScale = scale;
		if (difference.sqrMagnitude <= 0f)
		{
			samePositionFrames++;
			if (samePositionFrames >= 5)
			{
				StopTransition();
			}
		}
		else
		{
			samePositionFrames = 0;
		}
	}

	private void PoppingOut()
	{
		speed += direction * MaxAcceleration * deltaTime * factor;
		if (speed.sqrMagnitude > MaxScaleSpeed * MaxScaleSpeed * factor)
		{
			speed = speed.normalized * MaxScaleSpeed * factor;
		}
		scale += speed * deltaTime * factor;
		difference = StartScale - scale;
		if (difference.sqrMagnitude > lastDifference.sqrMagnitude)
		{
			base.transform.localScale = (scale = EndScale);
			StopTransition();
		}
		else
		{
			lastDifference = difference;
			base.transform.localScale = scale;
		}
	}
}
