using UnityEngine;

public class Pool : MonoBehaviour
{
	public GameObject Prefab;

	public int Count = 30;

	private PoolGameObject[] poolObjects;

	private int id;

	private Vector3 defaultPos = new Vector3(0f, 0f, 0f);

	private Vector3 defaultScale = new Vector3(1f, 1f, 1f);

	private bool isDestroyed;

	private GameObject changeParentGO;

	private void Awake()
	{
		if (Count != 0)
		{
			Initialize(Count);
		}
		base.transform.localPosition = Vector3.zero;
		base.transform.position = Vector3.zero;
		isDestroyed = false;
	}

	public void Initialize(int count)
	{
		Count = count;
		poolObjects = new PoolGameObject[Count];
		base.transform.position = Vector3.zero;
		for (int i = 0; i < poolObjects.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(Prefab) as GameObject;
			poolObjects[i] = gameObject.AddComponent<PoolGameObject>();
			poolObjects[i].Pool = this;
			gameObject.SetActive(false);
			gameObject.transform.parent = base.transform;
		}
	}

	private void OnDestroy()
	{
		isDestroyed = true;
	}

	public PoolGameObject Create()
	{
		return Create(defaultPos, defaultScale);
	}

	public PoolGameObject Create(Vector3 position)
	{
		return Create(position, defaultScale);
	}

	public PoolGameObject Create(Vector3 position, Vector3 scale)
	{
		if (poolObjects == null && Count > 0)
		{
			Initialize(Count);
		}
		if (id < poolObjects.Length)
		{
			PoolGameObject poolGameObject = poolObjects[id];
			poolGameObject.OnCreate();
			id++;
			if (poolGameObject == null)
			{
				Debug.LogError("PoolGameObject is null");
			}
			else if (poolGameObject.transform == null)
			{
				Debug.LogError("PoolGameObject transform is null");
			}
			if (poolGameObject != null)
			{
				poolGameObject.transform.position = position;
				poolGameObject.transform.localScale = scale;
				poolGameObject.gameObject.SetActive(true);
				return poolGameObject;
			}
		}
		return null;
	}

	public void Recycle(PoolGameObject go)
	{
		Recycle(go, false);
	}

	public void Recycle(PoolGameObject go, bool changeParentImmediately)
	{
		if (id > 0)
		{
			id--;
			poolObjects[id] = go;
			if (poolObjects[id].IsDestroyed)
			{
				return;
			}
			go.Owner = null;
			go.gameObject.SetActive(false);
			if (!isDestroyed)
			{
				changeParentGO = go.gameObject;
				if (changeParentImmediately)
				{
					changeGameObjectParent();
				}
				else
				{
					Invoke("changeGameObjectParent", 0f);
				}
			}
		}
		else
		{
			Debug.LogError("No more room for recycling!!!");
		}
	}

	public bool IsEmpty()
	{
		return id == poolObjects.Length;
	}

	private void changeGameObjectParent()
	{
		if (changeParentGO != null)
		{
			changeParentGO.transform.parent = base.transform;
		}
	}
}
