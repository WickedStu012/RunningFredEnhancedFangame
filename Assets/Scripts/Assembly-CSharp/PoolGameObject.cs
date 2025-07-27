using UnityEngine;

public class PoolGameObject : MonoBehaviour
{
	public bool IsDestroyed;

	public GameObject Owner;

	public Pool Pool;

	public Material Material;

	private bool recycled = true;

	private void Awake()
	{
		IsDestroyed = false;
		recycled = true;
	}

	private void OnEnable()
	{
		if (Material == null && base.GetComponent<Renderer>() != null)
		{
			Material = base.GetComponent<Renderer>().material;
		}
	}

	public void OnCreate()
	{
		recycled = false;
	}

	private void OnDestroy()
	{
		IsDestroyed = true;
		if (Material != null)
		{
			Object.DestroyImmediate(Material);
		}
	}

	public void Recycle()
	{
		if (!recycled)
		{
			Pool.Recycle(this);
			recycled = true;
		}
	}

	public void Recycle(bool changeParentImmediately)
	{
		if (!recycled)
		{
			Pool.Recycle(this, changeParentImmediately);
			recycled = true;
		}
	}
}
