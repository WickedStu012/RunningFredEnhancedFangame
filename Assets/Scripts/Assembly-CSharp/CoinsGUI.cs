using UnityEngine;

public class CoinsGUI : MonoBehaviour
{
	public GUI3DObject Coin;

	public GUI3DText CoinText;

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

	private float coinsPerDistance;

	private int lastCoinsEarned;

	private float multiplier;

	private void Awake()
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			GameEventDispatcher.AddListener("OnPlayerBonus", OnBonus);
			GameEventDispatcher.AddListener("OnCoinPerDistance", OnCoinEarnedPerDistance);
			GameEventDispatcher.AddListener("OnEndLessReset", EndLessReset);
		}
	}

	private void OnBonus(object sender, GameEvent evt)
	{
		OnPlayerBonus onPlayerBonus = (OnPlayerBonus)evt;
		multiplier = onPlayerBonus.Multiplier;
	}

	private void OnCoinEarnedPerDistance(object sender, GameEvent evt)
	{
		coinsPerDistance += multiplier;
		if (lastCoinsEarned != (int)coinsPerDistance)
		{
			lastCoinsEarned = (int)coinsPerDistance;
			changeLives = true;
			scale = Vector3.zero;
			scale.z = 1f;
			difference = (lastDifference = Vector3.one - scale);
			direction = difference.normalized;
			speed = direction * MaxScaleSpeed * factor;
			Coin.transform.localScale = scale;
			samePositionFrames = 0;
			CoinText.SetDynamicText(((int)coinsPerDistance).ToString());
		}
	}

	private void Update()
	{
		if (PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival && changeLives)
		{
			PoppingIn();
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
		Coin.transform.localScale = scale;
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

	private void EndLessReset(object sender, GameEvent evt)
	{
		coinsPerDistance = 0f;
		lastCoinsEarned = 0;
		CoinText.SetDynamicText("0");
	}
}
