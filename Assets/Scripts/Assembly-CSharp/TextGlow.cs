using UnityEngine;

public class TextGlow : MonoBehaviour
{
	public Color Color1;

	public Color Color2;

	public float Timer = 2f;

	private Color sourceColor;

	private Color destColor;

	private float currentTimer;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		sourceColor = Color1;
		destColor = Color2;
	}

	private void FixedUpdate()
	{
		currentTimer += GUI3DManager.Instance.DeltaTime;
		if (currentTimer >= Timer)
		{
			currentTimer = 0f;
			base.GetComponent<Renderer>().material.color = destColor;
			if (sourceColor == Color1)
			{
				sourceColor = Color2;
				destColor = Color1;
			}
			else
			{
				sourceColor = Color1;
				destColor = Color2;
			}
		}
		else
		{
			base.GetComponent<Renderer>().material.color = Color.Lerp(sourceColor, destColor, currentTimer / Timer);
		}
	}
}
