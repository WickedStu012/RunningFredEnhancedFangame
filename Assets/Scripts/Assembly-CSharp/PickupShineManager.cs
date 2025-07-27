using UnityEngine;

public class PickupShineManager : MonoBehaviour
{
	public float randomness = 0.001f;

	public Material[] pickupMaterials;

	public AnimationCurve intensityCurve;

	public float shineLength = 0.1f;

	private bool shine;

	private float timeCounter;

	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Update()
	{
		if (pickupMaterials.Length <= 0)
		{
			return;
		}
		if (!shine && Random.value < randomness)
		{
			shine = true;
			timeCounter = 0f;
		}
		if (!shine)
		{
			return;
		}
		timeCounter += Time.deltaTime;
		float num = timeCounter / shineLength;
		if (timeCounter > shineLength)
		{
			num = 1f;
			shine = false;
			for (int i = 0; i < pickupMaterials.Length; i++)
			{
				Material material = pickupMaterials[i];
				material.SetFloat("_ShineValue", 0f);
			}
		}
		else
		{
			float value = intensityCurve.Evaluate(num);
			for (int j = 0; j < pickupMaterials.Length; j++)
			{
				Material material2 = pickupMaterials[j];
				material2.SetFloat("_ShineValue", value);
				material2.SetTextureOffset("_ShineTex", new Vector2(0f, num * 2f - 1f));
			}
		}
	}

	public void OnDestroy()
	{
		for (int i = 0; i < pickupMaterials.Length; i++)
		{
			Material material = pickupMaterials[i];
			material.SetFloat("_ShineValue", 0f);
			material.SetTextureOffset("_ShineTex", new Vector2(0f, 0f));
		}
	}
}
