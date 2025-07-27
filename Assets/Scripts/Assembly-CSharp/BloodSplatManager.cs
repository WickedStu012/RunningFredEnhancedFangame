using UnityEngine;

public class BloodSplatManager : MonoBehaviour
{
	public string PoolName = "BloodSplatPool";

	public Vector3 MaxScale = new Vector3(7f, 1f, 7f);

	public float MaxMovement = 10f;

	public int MaxDecalsCount = 7;

	public float MaxForce = 40f;

	public float MinForce = 25f;

	public float ScaleFactor = 0.7f;

	private static BloodSplatManager instance;

	private Pool bloodSplatPool;

	public static BloodSplatManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void OnEnable()
	{
		if (PoolManager.Instance != null)
		{
			bloodSplatPool = PoolManager.Instance.GetPool(PoolName);
		}
		instance = this;
	}

	private void OnDisable()
	{
		instance = null;
	}

	public void Create(float impactForce)
	{
		if (bloodSplatPool == null)
		{
			if (!(PoolManager.Instance != null))
			{
				return;
			}
			bloodSplatPool = PoolManager.Instance.GetPool(PoolName);
		}
		float num = MaxMovement;
		float num2 = Mathf.Min(impactForce - MinForce, MaxForce - MinForce) / (MaxForce - MinForce);
		int num3 = Mathf.FloorToInt((float)MaxDecalsCount * num2);
		Vector3 localScale = MaxScale * num2;
		Vector3 position = default(Vector3);
		for (int i = 0; i < num3; i++)
		{
			PoolGameObject poolGameObject = bloodSplatPool.Create();
			if (!(poolGameObject == null))
			{
				position.x = Random.value - Random.value;
				position.y = Random.value - Random.value;
				position.z = 0f;
				position.Normalize();
				position *= num;
				num *= ScaleFactor;
				Transform transform = poolGameObject.transform;
				transform.localScale = localScale;
				transform.position = position;
				transform.rotation *= Quaternion.AngleAxis(Random.value * 360f, Vector3.forward);
				localScale *= ScaleFactor;
			}
		}
	}
}
