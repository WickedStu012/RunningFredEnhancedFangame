using UnityEngine;

[ExecuteInEditMode]
public class RandomColor : MonoBehaviour
{
	public Color color1 = Color.white;

	public Color color2 = Color.white;

	public float minInterpolationVariation;

	public float maxInterpolationVariation;

	public int timeToInterpolate = 1000;

	public int timeInterpolationVariation;

	public string colorToSet = "_Color";

	public Material originalMaterial;

	private bool goingUp;

	private int lastTimetoInterpolate;

	private float lastMinInterpolation;

	private float lastMaxInterpolation;

	private float remainingTime;

	private void Start()
	{
		if (originalMaterial != null)
		{
			base.GetComponent<Renderer>().material = originalMaterial;
		}
	}

	private void Update()
	{
		if (remainingTime <= 0f)
		{
			SwitchDirection();
		}
		else
		{
			remainingTime -= Time.deltaTime * 1000f;
		}
		float num = 100f - remainingTime * 100f / (float)lastTimetoInterpolate;
		num /= 100f;
		if (colorToSet != null && base.GetComponent<Renderer>() != null)
		{
			if (goingUp)
			{
				base.GetComponent<Renderer>().material.SetColor(colorToSet, Color.Lerp(color1, color2, num));
			}
			else
			{
				base.GetComponent<Renderer>().material.SetColor(colorToSet, Color.Lerp(color2, color1, num));
			}
		}
	}

	private void SwitchDirection()
	{
		goingUp = !goingUp;
		lastMinInterpolation = Random.Range(0f, minInterpolationVariation);
		lastMaxInterpolation = Random.Range(0f, maxInterpolationVariation);
		remainingTime = timeToInterpolate - Random.Range(0, timeInterpolationVariation);
		if (goingUp)
		{
			remainingTime *= 1f - Random.Range(0f, lastMaxInterpolation);
		}
		else
		{
			remainingTime *= 1f - Random.Range(0f, lastMinInterpolation);
		}
		lastTimetoInterpolate = (int)remainingTime;
	}
}
