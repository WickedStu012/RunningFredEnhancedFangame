using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
	private const int barrelPartInstanceCount = 3;

	public int barrelPartsToCache = 3;

	private GameObject barrelParts1Prefab;

	private GameObject barrelParts2Prefab;

	private List<GameObject[]> barrelParts1;

	private List<GameObject[]> barrelParts2;

	private static BarrelManager instance;

	private int part1Ret;

	private int part2Ret;

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Start()
	{
		barrelParts1Prefab = Resources.Load("Traps/BarrelPiece1", typeof(GameObject)) as GameObject;
		barrelParts2Prefab = Resources.Load("Traps/BarrelPiece2", typeof(GameObject)) as GameObject;
		barrelParts1 = new List<GameObject[]>(3);
		for (int i = 0; i < 3; i++)
		{
			barrelParts1.Add(new GameObject[3]);
			barrelParts1[i][0] = Object.Instantiate(barrelParts1Prefab) as GameObject;
			barrelParts1[i][1] = Object.Instantiate(barrelParts1Prefab) as GameObject;
			barrelParts1[i][2] = Object.Instantiate(barrelParts1Prefab) as GameObject;
			for (int j = 0; j < barrelParts1[i].Length; j++)
			{
				barrelParts1[i][j].SetActive(false);
			}
		}
		barrelParts2 = new List<GameObject[]>(3);
		for (int k = 0; k < 3; k++)
		{
			barrelParts2.Add(new GameObject[2]);
			barrelParts2[k][0] = Object.Instantiate(barrelParts2Prefab) as GameObject;
			barrelParts2[k][1] = Object.Instantiate(barrelParts2Prefab) as GameObject;
			for (int l = 0; l < barrelParts2[k].Length; l++)
			{
				barrelParts2[k][l].SetActive(false);
			}
		}
		part1Ret = 0;
		part2Ret = 0;
	}

	public static void CreateIfNecessary()
	{
		GameObject gameObject = GameObject.FindWithTag("Managers");
		if (gameObject != null && gameObject.GetComponent<BarrelManager>() == null)
		{
			gameObject.AddComponent<BarrelManager>();
		}
	}

	public static GameObject[] GetParts1()
	{
		GameObject[] result = instance.barrelParts1[instance.part1Ret];
		instance.part1Ret++;
		if (instance.part1Ret == 3)
		{
			instance.part1Ret = 0;
		}
		return result;
	}

	public static GameObject[] GetParts2()
	{
		GameObject[] result = instance.barrelParts2[instance.part2Ret];
		instance.part2Ret++;
		if (instance.part2Ret == 3)
		{
			instance.part2Ret = 0;
		}
		return result;
	}
}
