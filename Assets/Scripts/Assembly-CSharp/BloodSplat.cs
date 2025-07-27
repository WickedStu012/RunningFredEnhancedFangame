using UnityEngine;

public class BloodSplat : MonoBehaviour
{
	public float AliveTime = 2f;

	private Color color;

	private float alpha = 1f;

	private PoolGameObject poolGameObject;

	private Material material;

	private void Awake()
	{
		poolGameObject = GetComponent<PoolGameObject>();
	}

	private void OnEnable()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			alpha = 1f;
			if (material == null)
			{
				material = base.GetComponent<Renderer>().material;
			}
			color = material.GetColor("_TintColor");
			color.a = alpha;
			material.SetColor("_TintColor", color);
		}
	}

	private void Update()
	{
		if (poolGameObject == null)
		{
			poolGameObject = GetComponent<PoolGameObject>();
		}
		alpha -= Time.deltaTime / AliveTime;
		if (alpha < 0f)
		{
			alpha = 0f;
		}
		color.a = alpha;
		if (material != null)
		{
			material.SetColor("_TintColor", color);
		}
		if (alpha == 0f && poolGameObject != null)
		{
			poolGameObject.Recycle();
		}
	}
}
