using UnityEngine;

public class DetonatorManager : MonoBehaviour
{
	private static DetonatorManager instance;

	public GameObject explosion1Prefab;

	private GameObject explosion;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		explosion = Object.Instantiate(explosion1Prefab) as GameObject;
		explosion.SetActive(false);
	}

	public static GameObject GetDetonator()
	{
		return instance.explosion;
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
