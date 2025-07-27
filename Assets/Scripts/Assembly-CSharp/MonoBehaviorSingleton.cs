using UnityEngine;

public class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviorSingleton<T>
{
	private static MonoBehaviorSingleton<T> instance;

	protected static bool setToNullAfterDestroy = true;

	private static bool destroyed = true;

	private static bool initializedAtLeastOnce;

	private static bool needInitialization = true;

	protected bool enableDuplicateInstanceWarning;

	protected bool Destroyed
	{
		get
		{
			return destroyed;
		}
	}

	public static T Instance
	{
		get
		{
			if (instance == null || destroyed || needInitialization)
			{
				if (instance == null || destroyed)
				{
					MonoBehaviorSingleton<T> monoBehaviorSingleton = Object.FindObjectOfType(typeof(MonoBehaviorSingleton<T>)) as MonoBehaviorSingleton<T>;
					if (monoBehaviorSingleton != null)
					{
						instance = monoBehaviorSingleton;
						destroyed = false;
					}
				}
				if (instance != null && !destroyed)
				{
					if (needInitialization)
					{
						needInitialization = false;
						initializedAtLeastOnce = true;
						instance.Initialize();
					}
				}
				else if (!initializedAtLeastOnce)
				{
					Debug.LogError("Missing Singleton '" + typeof(T).Name + "'");
				}
			}
			return (T)instance;
		}
	}

	public virtual void Awake()
	{
		if (instance == null || destroyed)
		{
			instance = this;
			destroyed = false;
		}
		else if (instance != this)
		{
			if (enableDuplicateInstanceWarning)
			{
				Debug.LogError(string.Concat("Two instances of the same singleton '", this, "' so it will be destroyed"));
			}
			setToNullAfterDestroy = false;
			Object.Destroy(base.gameObject);
			return;
		}
		if (needInitialization)
		{
			needInitialization = false;
			initializedAtLeastOnce = true;
			Initialize();
		}
	}

	public void OnDestroy()
	{
		Destroy();
		destroyed = true;
		needInitialization = true;
		if (setToNullAfterDestroy)
		{
			instance = null;
		}
	}

	public static void Dispose()
	{
		if (instance != null && !destroyed)
		{
			Object.Destroy(instance.gameObject);
			instance = null;
		}
	}

	protected virtual void Initialize()
	{
	}

	protected virtual void Destroy()
	{
	}

	public static bool IsInitialized()
	{
		return instance != null && !destroyed;
	}

	public static bool IsAvailable()
	{
		return IsInitialized() || Object.FindObjectOfType(typeof(MonoBehaviorSingleton<T>)) != null;
	}
}
