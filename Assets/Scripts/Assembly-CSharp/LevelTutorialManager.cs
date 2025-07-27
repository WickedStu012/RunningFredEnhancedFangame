using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorialManager : MonoBehaviour, ILevelChunkContainer
{
	private const float CHECK_TIME = 1f;

	private const int MIN_CHUNK_COUNT = 7;

	public const float defLength = 16f;

	public Material mat;

	public LevelTutorialState tutState;

	private LevelTutorialAssets tutAssets;

	private List<GameObject> nodes = new List<GameObject>();

	private List<LevelFloorBase> chunkNodeBases = new List<LevelFloorBase>();

	private int lastChunkNum;

	private float lastZPos;

	private float accumTime;

	private void Awake()
	{
		tutAssets = GetComponent<LevelTutorialAssets>();
		lastChunkNum = 1;
		lastZPos = 16f * (float)(lastChunkNum + 1);
		LevelFloorBase[] componentsInChildren = GetComponentsInChildren<LevelFloorBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			chunkNodeBases.Add(componentsInChildren[i]);
			nodes.Add(componentsInChildren[i].gameObject);
		}
		GameEventDispatcher.AddListener("OnTutorialObjectiveComplete", OnObjectiveComplete);
		GameEventDispatcher.AddListener("OnTutorialObjectiveFail", OnObjectiveFail);
	}

	public List<GameObject> GetNodes()
	{
		return nodes;
	}

	public List<LevelFloorBase> GetNodeBases()
	{
		return chunkNodeBases;
	}

	public float GetDefLength()
	{
		return 16f;
	}

	public GameObject GetEnder()
	{
		return null;
	}

	public Material GetMaterial()
	{
		return mat;
	}

	private void putCorridor()
	{
		Debug.LogError("DEPRECATED. This method should not be called.");
	}

	private void putCorridorWithDoor()
	{
		putGO(tutAssets.GetCorridorWithDoor(), true);
	}

	private void putSteering()
	{
		putGO(tutAssets.GetSteering(), true);
	}

	private void putColumns()
	{
		putGO(tutAssets.GetColumns(), true);
	}

	private void putObstaclesToJump()
	{
		putGO(tutAssets.GetObstaclesToJump(), true);
	}

	private void putAccelerators()
	{
		putGO(tutAssets.GetAccelerators(), true);
	}

	private void putEnder()
	{
		putGO(tutAssets.GetEnder(), true);
	}

	private void putGO(GameObject go, bool canChangeSlope)
	{
		float num = 0f;
		float num2 = 0f;
		LevelRandomGroup component = go.GetComponent<LevelRandomGroup>();
		LevelFloor floor = chunkNodeBases[chunkNodeBases.Count - 1].floor;
		float num3 = floor.transform.position.y;
		if (floor != null)
		{
			if (floor.rotation != 0f)
			{
				num = Mathf.Tan(floor.rotation * ((float)Math.PI / 180f)) * 8f;
			}
			num3 = ((!(floor.transform.localScale.y > 0f)) ? (num3 + (floor.startYPos + num)) : (num3 + (floor.endYPos - num)));
		}
		for (int i = 0; i < component.GetChunkCount(); i++)
		{
			LevelFloorBase chunkBase = component.GetChunkBase(i);
			chunkBase.gameObject.name = string.Format("Chunk{0}", lastChunkNum++);
			chunkBase.transform.parent = base.transform;
			chunkBase.transform.position = new Vector3(chunkBase.transform.position.x, chunkBase.transform.position.y, lastZPos);
			if (chunkBase.floor != null)
			{
				if (i == 0 && chunkBase.floor.rotation != 0f)
				{
					num2 = Mathf.Tan(chunkBase.floor.rotation * ((float)Math.PI / 180f)) * 8f;
				}
				chunkBase.floor.transform.position = new Vector3(chunkBase.floor.transform.position.x, chunkBase.floor.transform.position.y - num2 + num3, chunkBase.floor.transform.position.z);
				if (chunkBase.traps != null)
				{
					for (int j = 0; j < chunkBase.traps.Count; j++)
					{
						chunkBase.traps[j].transform.position = new Vector3(chunkBase.traps[j].transform.position.x, chunkBase.traps[j].transform.position.y - num2 + num3, chunkBase.traps[j].transform.position.z);
					}
				}
				if (chunkBase.decorations != null)
				{
					for (int k = 0; k < chunkBase.decorations.Count; k++)
					{
						chunkBase.decorations[k].transform.position = new Vector3(chunkBase.decorations[k].transform.position.x, chunkBase.decorations[k].transform.position.y - num2 + num3, chunkBase.decorations[k].transform.position.z);
					}
				}
				if (chunkBase.platforms != null)
				{
					for (int l = 0; l < chunkBase.platforms.Count; l++)
					{
						chunkBase.platforms[l].transform.position = new Vector3(chunkBase.platforms[l].transform.position.x, chunkBase.platforms[l].transform.position.y - num2 + num3, chunkBase.platforms[l].transform.position.z);
					}
				}
				if (chunkBase.pickups != null)
				{
					for (int m = 0; m < chunkBase.pickups.Count; m++)
					{
						chunkBase.pickups[m].transform.position = new Vector3(chunkBase.pickups[m].transform.position.x, chunkBase.pickups[m].transform.position.y - num2 + num3, chunkBase.pickups[m].transform.position.z);
					}
				}
			}
			lastZPos += 16f;
			chunkNodeBases.Add(chunkBase);
			nodes.Add(chunkBase.gameObject);
		}
		UnityEngine.Object.Destroy(component.gameObject);
	}

	private float getLastYPos()
	{
		LevelFloor floor = chunkNodeBases[chunkNodeBases.Count - 1].floor;
		if (floor != null)
		{
			return floor.transform.position.y;
		}
		return 0f;
	}

	private void checkForChunkCreation()
	{
		switch (tutState)
		{
		case LevelTutorialState.PHASE_1:
			putSteering();
			putCorridorWithDoor();
			break;
		case LevelTutorialState.PHASE_2:
			putColumns();
			putCorridorWithDoor();
			break;
		case LevelTutorialState.PHASE_3:
			putObstaclesToJump();
			putCorridorWithDoor();
			break;
		case LevelTutorialState.PHASE_4:
			putAccelerators();
			putCorridorWithDoor();
			break;
		case LevelTutorialState.ENDER:
			putEnder();
			break;
		}
	}

	private void OnObjectiveComplete(object sender, GameEvent e)
	{
		OnTutorialObjectiveComplete onTutorialObjectiveComplete = e as OnTutorialObjectiveComplete;
		Debug.Log(string.Format("OnObjectiveComplete: {0}", onTutorialObjectiveComplete.objType));
	}

	private void OnObjectiveFail(object sender, GameEvent e)
	{
		OnTutorialObjectiveFail onTutorialObjectiveFail = e as OnTutorialObjectiveFail;
		Debug.Log(string.Format("OnObjectiveFail: {0}", onTutorialObjectiveFail.objType));
	}

	public void SetState(LevelTutorialState state)
	{
		tutState = state;
		checkForChunkCreation();
	}
}
