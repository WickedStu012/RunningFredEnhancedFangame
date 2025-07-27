using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelRandomManager : MonoBehaviour, ILevelChunkContainer
{
	private const float CHECK_TIME = 1f;

	private const int METERS_TO_CHANGE_LEVEL = 640;

	private const int MAX_LEVEL_CAP = 10;

	public const float defLength = 16f;

	private int MIN_CHUNK_COUNT = 14;

	public SettingId sid;

	public Material defMaterial;

	private LevelRandomAssets rndAsset;

	private List<GameObject> nodes = new List<GameObject>();

	private List<LevelFloorBase> chunkNodeBases = new List<LevelFloorBase>();

	private int lastChunkNum;

	private float lastZPos;

	private float accumTime;

	private float accumDistance;

	private int curLevelNum;

	private int relLevelNum;

	private int initialLevelNum;

	private LevelRandomPattern pattern;

	private bool shouldInitLevel;

	private float wallYOffset;

	private void Awake()
	{
		pattern = new LevelRandomPattern(sid);
		rndAsset = GetComponent<LevelRandomAssets>();
		initLevel();
	}

	private void initLevel()
	{
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.CurrentLevelNum > 0)
		{
			curLevelNum = (PlayerAccount.Instance.CurrentLevelNum - 1) * 3;
			initialLevelNum = curLevelNum;
		}
		lastChunkNum = 2;
		lastZPos = 16f * (float)(lastChunkNum + 1);
		nodes = new List<GameObject>();
		chunkNodeBases = new List<LevelFloorBase>();
		LevelFloorBase[] componentsInChildren = GetComponentsInChildren<LevelFloorBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			chunkNodeBases.Add(componentsInChildren[i]);
			nodes.Add(componentsInChildren[i].gameObject);
		}
		for (int j = 0; j < 5; j++)
		{
			checkForChunkCreation(false);
		}
		relLevelNum = 0;
		wallYOffset = 0f;
	}

	private void FixedUpdate()
	{
		if (shouldInitLevel)
		{
			initLevel();
			ChunkRelocator.InitVis();
			accumTime = 0f;
			shouldInitLevel = false;
			return;
		}
		accumTime += Time.fixedDeltaTime;
		if (accumTime >= 1f)
		{
			checkForChunkCreation(true);
			accumTime = 0f;
		}
		checkDistanceToLevelUp();
	}

	public void ResetLevel()
	{
		if (chunkNodeBases.Count > 3)
		{
			int count = chunkNodeBases.Count;
			for (int i = 3; i < count; i++)
			{
				LevelFloorBase levelFloorBase = chunkNodeBases[3];
				nodes.Remove(levelFloorBase.gameObject);
				chunkNodeBases.Remove(levelFloorBase);
				recycleParticlesOf(levelFloorBase.gameObject);
				UnityEngine.Object.DestroyImmediate(levelFloorBase.gameObject);
			}
		}
		shouldInitLevel = true;
		wallYOffset = 0f;
	}

	private void checkDistanceToLevelUp()
	{
		int num = relLevelNum;
		if (DistanceManager.Instance != null)
		{
			relLevelNum = DistanceManager.Instance.Distance / 640;
		}
		else
		{
			relLevelNum = 0;
		}
		if (relLevelNum >= 10)
		{
			relLevelNum = 9;
		}
		if (relLevelNum > num)
		{
			curLevelNum = initialLevelNum + relLevelNum;
			GameEventDispatcher.Dispatch(this, new OnLevelRandomRaise(curLevelNum));
		}
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
		return defMaterial;
	}

	private void putConnector(int connNum, bool disableGOs)
	{
		putGO(rndAsset.GetConnector(connNum), disableGOs);
	}

	private void putConnectorTest()
	{
		putGO(rndAsset.GetConnectorTest(), false);
	}

	private void putGroup(int groupNum, bool disableGOs)
	{
		putGO(rndAsset.GetGroupChunk(groupNum), disableGOs);
	}

	private void putGO(GameObject go, bool disableGOs)
	{
		float num = 0f;
		float num2 = 0f;
		LevelRandomGroup component = go.GetComponent<LevelRandomGroup>();
		LevelRandomSet component2 = go.GetComponent<LevelRandomSet>();
		if (component2 != null)
		{
			randomizeRandomSet(component2);
		}
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
		if (component == null)
		{
			Debug.LogError("rndGroupBase == null");
		}
		FallTrigger[] fallTriggers = component.GetFallTriggers();
		for (int i = 0; i < component.GetChunkCount(); i++)
		{
			LevelFloorBase chunkBase = component.GetChunkBase(i);
			if (i == component.GetChunkCount() - 1 && fallTriggers != null)
			{
				for (int j = 0; j < fallTriggers.Length; j++)
				{
					fallTriggers[j].EnableTriggerCheck();
					fallTriggers[j].transform.parent = chunkBase.transform;
				}
			}
			chunkBase.gameObject.name = string.Format("Chunk{0}", lastChunkNum++);
			chunkBase.transform.parent = base.transform;
			chunkBase.transform.position = new Vector3(chunkBase.transform.position.x, chunkBase.transform.position.y, lastZPos);
			if (i == 0 && chunkBase.floor.rotation != 0f)
			{
				num2 = Mathf.Tan(chunkBase.floor.rotation * ((float)Math.PI / 180f)) * 8f;
			}
			if (i == component.GetChunkCount() - 1 && fallTriggers != null)
			{
				for (int k = 0; k < fallTriggers.Length; k++)
				{
					fallTriggers[k].transform.position = new Vector3(fallTriggers[k].transform.position.x, fallTriggers[k].transform.position.y - num2 + num3, fallTriggers[k].transform.position.z);
				}
			}
			if (chunkBase.floor != null)
			{
				chunkBase.floor.transform.position = new Vector3(chunkBase.floor.transform.position.x, chunkBase.floor.transform.position.y - num2 + num3, chunkBase.floor.transform.position.z);
				if (chunkBase.leftWall != null)
				{
					float num4 = chunkBase.floor.transform.position.y - chunkBase.leftWall.transform.position.y;
					wallYOffset = (float)(int)(num4 / 35f) * 35f;
					chunkBase.leftWall.transform.position = new Vector3(chunkBase.leftWall.transform.position.x, wallYOffset, chunkBase.leftWall.transform.position.z);
					if (chunkBase.rightWall != null)
					{
						chunkBase.rightWall.transform.position = new Vector3(chunkBase.rightWall.transform.position.x, wallYOffset, chunkBase.rightWall.transform.position.z);
					}
				}
				if (chunkBase.traps != null)
				{
					for (int l = 0; l < chunkBase.traps.Count; l++)
					{
						if (chunkBase.traps[l] == null)
						{
							Debug.Log(string.Format("Trap is null. At: {0}", chunkBase.name));
						}
						chunkBase.traps[l].transform.position = new Vector3(chunkBase.traps[l].transform.position.x, chunkBase.traps[l].transform.position.y - num2 + num3, chunkBase.traps[l].transform.position.z);
						LevelRandomObjectPlacer component3 = chunkBase.traps[l].GetComponent<LevelRandomObjectPlacer>();
						if (component3 != null)
						{
							component3.locateObj(0f - num2 + num3);
						}
					}
				}
				if (chunkBase.decorations != null)
				{
					for (int m = 0; m < chunkBase.decorations.Count; m++)
					{
						chunkBase.decorations[m].transform.position = new Vector3(chunkBase.decorations[m].transform.position.x, chunkBase.decorations[m].transform.position.y - num2 + num3, chunkBase.decorations[m].transform.position.z);
						LevelRandomObjectPlacer component4 = chunkBase.decorations[m].GetComponent<LevelRandomObjectPlacer>();
						if (component4 != null)
						{
							component4.locateObj(0f - num2 + num3);
						}
					}
				}
				if (chunkBase.platforms != null)
				{
					for (int n = 0; n < chunkBase.platforms.Count; n++)
					{
						chunkBase.platforms[n].transform.position = new Vector3(chunkBase.platforms[n].transform.position.x, chunkBase.platforms[n].transform.position.y - num2 + num3, chunkBase.platforms[n].transform.position.z);
						LevelRandomObjectPlacer component5 = chunkBase.platforms[n].GetComponent<LevelRandomObjectPlacer>();
						if (component5 != null)
						{
							component5.locateObj(0f - num2 + num3);
						}
					}
				}
				if (chunkBase.pickups != null)
				{
					for (int num5 = 0; num5 < chunkBase.pickups.Count; num5++)
					{
						chunkBase.pickups[num5].transform.position = new Vector3(chunkBase.pickups[num5].transform.position.x, chunkBase.pickups[num5].transform.position.y - num2 + num3, chunkBase.pickups[num5].transform.position.z);
					}
				}
			}
			lastZPos += 16f;
			MaterialFadeManager.InitializeFadeMaterial(chunkBase.MeshRenderers, disableGOs);
			chunkNodeBases.Add(chunkBase);
			nodes.Add(chunkBase.gameObject);
		}
		if (disableGOs)
		{
			for (int num6 = 0; num6 < component.GetChunkCount(); num6++)
			{
				LevelFloorBase chunkBase = component.GetChunkBase(num6);
				chunkBase.gameObject.SetActive(false);
			}
		}
		component.EnableFallTriggers();
		component.ReconnectChunkElements(GetMaterial());
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

	private void checkForChunkCreation(bool disableNewGOs)
	{
		float num = nodes[nodes.Count - 1].transform.position.z - CharHelper.GetPlayerTransform().position.z;
		int num2 = (int)(num / 16f);
		if (num2 < MIN_CHUNK_COUNT)
		{
			switch (pattern.GetRandomUnitType(curLevelNum))
			{
			case LevelRandomUnitType.CON0:
				putConnector(0, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP0:
				putGroup(0, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP1:
				putGroup(1, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP2:
				putGroup(2, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP3:
				putGroup(3, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP4:
				putGroup(4, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP5:
				putGroup(5, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP6:
				putGroup(6, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP7:
				putGroup(7, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP8:
				putGroup(8, disableNewGOs);
				break;
			case LevelRandomUnitType.GRP9:
				putGroup(9, disableNewGOs);
				break;
			}
		}
	}

	private void randomizeRandomSet(LevelRandomSet lrs)
	{
		lrs.RandomizeSet();
	}

	private void recycleParticlesOf(GameObject go)
	{
		ParticleSystem[] componentsInChildren = go.GetComponentsInChildren<ParticleSystem>(true);
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].recycleAllParticles(true);
			}
		}
	}
}
