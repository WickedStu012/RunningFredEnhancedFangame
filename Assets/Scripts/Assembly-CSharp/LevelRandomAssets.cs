using UnityEngine;

public class LevelRandomAssets : MonoBehaviour
{
	private const float MAX_Y_POS = 50f;

	private const float MIN_Y_POS = -50f;

	public GameObject[] connectors;

	public GameObject[] group0;

	public GameObject[] group1;

	public GameObject[] group2;

	public GameObject[] group3;

	public GameObject[] group4;

	public GameObject[] group5;

	public GameObject[] group6;

	public GameObject[] group7;

	public GameObject[] group8;

	public GameObject[] group9;

	public GameObject GetConnector(int connNum)
	{
		int num = Random.Range(0, connectors.Length);
		return Object.Instantiate(connectors[num]) as GameObject;
	}

	public GameObject GetGroupChunk(int groupNum)
	{
		int num = 0;
		GameObject gameObject = null;
		switch (groupNum)
		{
		case 0:
			num = Random.Range(0, group0.Length);
			gameObject = Object.Instantiate(group0[num]) as GameObject;
			break;
		case 1:
			num = Random.Range(0, group1.Length);
			gameObject = Object.Instantiate(group1[num]) as GameObject;
			break;
		case 2:
			num = Random.Range(0, group2.Length);
			gameObject = Object.Instantiate(group2[num]) as GameObject;
			break;
		case 3:
			num = Random.Range(0, group3.Length);
			gameObject = Object.Instantiate(group3[num]) as GameObject;
			break;
		case 4:
			num = Random.Range(0, group4.Length);
			gameObject = Object.Instantiate(group4[num]) as GameObject;
			break;
		case 5:
			num = Random.Range(0, group5.Length);
			gameObject = Object.Instantiate(group5[num]) as GameObject;
			break;
		case 6:
			num = Random.Range(0, group6.Length);
			gameObject = Object.Instantiate(group6[num]) as GameObject;
			break;
		case 7:
			num = Random.Range(0, group7.Length);
			gameObject = Object.Instantiate(group7[num]) as GameObject;
			break;
		case 8:
			num = Random.Range(0, group8.Length);
			gameObject = Object.Instantiate(group8[num]) as GameObject;
			break;
		case 9:
			num = Random.Range(0, group9.Length);
			gameObject = Object.Instantiate(group9[num]) as GameObject;
			break;
		}
		if (!(gameObject != null))
		{
			Debug.LogError(string.Format("Error. Cannot find: Group: {0} Idx: {1}", groupNum, num));
		}
		return gameObject;
	}

	public GameObject GetConnectorTest()
	{
		return Object.Instantiate(group0[9]) as GameObject;
	}
}
