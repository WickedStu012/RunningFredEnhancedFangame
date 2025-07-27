using UnityEngine;

public class LivesGUI : MonoBehaviour
{
	public GUI3DObject Heart;

	public GUI3DText LivesText;

	public float MaxScaleSpeed = 15f;

	public float MaxAcceleration = 50f;

	public float BounceFactor = 0.2f;

	private CharProps charProps;

	private bool changeLives;

	private Vector3 speed;

	private Vector3 scale;

	private Vector3 difference;

	private Vector3 lastDifference;

	private Vector3 direction;

	private int samePositionFrames;

	private float factor = 1f;

	private int currentLives;

	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode != PlayerAccount.GameMode.Survival)
		{
			GameEventDispatcher.AddListener("OnPlayerRespawn", OnLifeLost);
		}
	}

	private void OnLifeLost(object sender, GameEvent evt)
	{
		changeLives = true;
		scale = Vector3.zero;
		scale.z = 1f;
		difference = (lastDifference = Vector3.one - scale);
		direction = difference.normalized;
		speed = direction * MaxScaleSpeed * factor;
		Heart.transform.localScale = scale;
		samePositionFrames = 0;
	}

	private void Update()
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			return;
		}
		if (charProps == null)
		{
			charProps = CharHelper.GetProps();
			if (charProps != null)
			{
				LivesText.SetDynamicText("x" + charProps.Lives);
				currentLives = charProps.Lives;
			}
		}
		if (changeLives)
		{
			if (!PoppingIn())
			{
				return;
			}
			if (charProps != null)
			{
				if (charProps.Lives >= 0)
				{
					LivesText.SetDynamicText("x" + (charProps.Lives - 1));
				}
				currentLives = charProps.Lives;
			}
			changeLives = false;
		}
		else if (currentLives != charProps.Lives)
		{
			LivesText.SetDynamicText("x" + charProps.Lives);
			currentLives = charProps.Lives;
		}
	}

	private bool PoppingIn()
	{
		speed += direction * MaxAcceleration * Time.deltaTime * factor;
		if (speed.sqrMagnitude > MaxScaleSpeed * MaxScaleSpeed * factor)
		{
			speed = speed.normalized * MaxScaleSpeed * factor;
		}
		scale += speed * Time.deltaTime * factor;
		difference = Vector3.one - scale;
		if (difference.sqrMagnitude >= lastDifference.sqrMagnitude && speed.normalized == direction)
		{
			scale = Vector3.one;
			speed *= 0f - BounceFactor;
			difference = Vector3.one - scale;
		}
		lastDifference = difference;
		Heart.transform.localScale = scale;
		if (difference.sqrMagnitude <= 0f)
		{
			samePositionFrames++;
			if (samePositionFrames >= 5)
			{
				return true;
			}
		}
		else
		{
			samePositionFrames = 0;
		}
		return false;
	}
}
