using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public Pool[] Pool;

	private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

	private static PoolManager instance;

	private bool init;

	public static PoolManager Instance
	{
		get
		{
			if (instance != null)
			{
				instance.Init();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		Init();
	}

	private void Init()
	{
		if (init)
		{
			return;
		}
		Pool[] pool = Pool;
		foreach (Pool pool2 in pool)
		{
			if (pools.ContainsKey(pool2.name))
			{
				Debug.LogError("Another pool named " + pool2.name + " already exists.");
			}
			else
			{
				pools[pool2.name] = pool2;
			}
		}
		init = true;
	}

	private void OnEnable()
	{
		instance = this;
		Init();
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public Pool GetPool(string name)
	{
		return pools[name];
	}
}
