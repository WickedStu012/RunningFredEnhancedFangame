using UnityEngine;

public class LevelTutorialAssets : MonoBehaviour
{
	public GameObject corridor;

	public GameObject corridorWithDoor;

	public GameObject steering;

	public GameObject columns;

	public GameObject obstaclesToJump;

	public GameObject accelerators;

	public GameObject ender;

	public GameObject GetCorridor()
	{
		return Object.Instantiate(corridor) as GameObject;
	}

	public GameObject GetCorridorWithDoor()
	{
		return Object.Instantiate(corridorWithDoor) as GameObject;
	}

	public GameObject GetSteering()
	{
		return Object.Instantiate(steering) as GameObject;
	}

	public GameObject GetColumns()
	{
		return Object.Instantiate(columns) as GameObject;
	}

	public GameObject GetObstaclesToJump()
	{
		return Object.Instantiate(obstaclesToJump) as GameObject;
	}

	public GameObject GetAccelerators()
	{
		return Object.Instantiate(accelerators) as GameObject;
	}

	public GameObject GetEnder()
	{
		return Object.Instantiate(ender) as GameObject;
	}
}
