using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChunkChain : MonoBehaviour, ILevelChunkContainer
{
	private static ChunkChain instance;

	public float minHeight = -25f;

	public float maxHeight = 25f;

	public float defLength = 16f;

	public float defWidth = 16f;

	[HideInInspector]
	public GameObject[][] chunks;

	public Material mat;

	[HideInInspector]
	public GameObject tallWallRight;

	[HideInInspector]
	public GameObject outerSideWall;

	[HideInInspector]
	public GameObject outerFrontWall;

	public GameObject ender;

	private List<GameObject> chunkNodes = new List<GameObject>();

	private List<LevelFloorBase> chunkNodeBases = new List<LevelFloorBase>();

	private LevelTileChunk curChunk;

	private LevelTileTex curFlavor;

	private float[] floorYPos;

	private void Awake()
	{
		instance = this;
		LevelFloor[] componentsInChildren = base.gameObject.GetComponentsInChildren<LevelFloor>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			chunkNodes.Add(componentsInChildren[i].transform.parent.gameObject);
		}
		for (int j = 0; j < chunkNodes.Count; j++)
		{
			chunkNodeBases.Add(chunkNodes[j].GetComponent<LevelFloorBase>());
		}
		createFloorYPosArray();
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
	{
		float num = 0f;
		if (chunkNodes == null)
		{
			return;
		}
		for (int i = 0; i < chunkNodes.Count; i++)
		{
			if (chunkNodes[i] != null)
			{
				float num2 = chunkNodes[i].transform.position.z;
				if (i > 0 && num2 < num)
				{
					num2 = num;
				}
				chunkNodes[i].transform.position = new Vector3(0f, Mathf.Clamp(chunkNodes[i].transform.position.y, minHeight, maxHeight), (float)i * defLength);
				num = num2;
			}
		}
	}

	public List<LevelFloorBase> GetNodeBases()
	{
		return chunkNodeBases;
	}

	public float GetDefLength()
	{
		return defLength;
	}

	public GameObject GetEnder()
	{
		return ender;
	}

	public Material GetMaterial()
	{
		return mat;
	}

	public static ChunkChain GetInstance()
	{
		return instance;
	}

	public void AddMany(int num, bool leftIsInner, bool rightIsInner)
	{
		for (int i = 0; i < num; i++)
		{
			AddNewOne(leftIsInner, rightIsInner);
		}
	}

	public void AddNewOne(bool leftIsInner, bool rightIsInner)
	{
		if (chunkNodes == null)
		{
			chunkNodes = new List<GameObject>();
		}
		GameObject gameObject = new GameObject(string.Format("Chunk{0}", chunkNodes.Count));
		LevelFloorBase levelFloorBase = gameObject.AddComponent<LevelFloorBase>();
		GameObject gameObject2 = new GameObject("floor");
		gameObject2.transform.parent = gameObject.transform;
		LevelFloor levelFloor = gameObject2.AddComponent<LevelFloor>();
		MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = chunks[0][0].GetComponent<MeshFilter>().sharedMesh;
		gameObject2.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
		MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
		meshRenderer.material = mat;
		MeshCollider meshCollider = gameObject2.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = chunks[0][0].GetComponent<MeshFilter>().sharedMesh;
		LevelFloorProps component = chunks[0][0].GetComponent<LevelFloorProps>();
		if (component != null)
		{
			levelFloor.startYPos = component.startYPos;
			levelFloor.endYPos = component.endYPos;
			levelFloor.generateFrontWall = component.generateFrontWall;
			levelFloor.generateFrontWallInverse = component.generateFrontWallInverse;
		}
		else
		{
			levelFloor.startYPos = 0f;
			levelFloor.endYPos = 0f;
			levelFloor.generateFrontWall = true;
			levelFloor.generateFrontWallInverse = true;
		}
		gameObject.transform.parent = base.transform;
		gameObject2.layer = 9;
		if (chunkNodes.Count > 0)
		{
			gameObject.transform.position = new Vector3(0f, 0f, chunkNodes[chunkNodes.Count - 1].transform.position.z + defLength);
		}
		levelFloorBase.floor = levelFloor;
		levelFloorBase.rightWall = LevelBuilderUtil.AddWall(rightIsInner ? tallWallRight : outerSideWall, gameObject, defWidth, false, rightIsInner);
		levelFloorBase.leftWall = LevelBuilderUtil.AddWall(leftIsInner ? tallWallRight : outerSideWall, gameObject, defWidth, true, leftIsInner);
		chunkNodes.Add(gameObject);
	}

	public void Clear()
	{
		List<GameObject> list = new List<GameObject>();
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name.StartsWith("Chunk"))
			{
				list.Add(componentsInChildren[i].gameObject);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			UnityEngine.Object.DestroyImmediate(list[j]);
		}
		if (ender != null)
		{
			UnityEngine.Object.DestroyImmediate(ender);
			ender = null;
		}
		chunkNodes = new List<GameObject>();
		curChunk = LevelTileChunk.PLANE;
		curFlavor = LevelTileTex.FLAVOR1;
	}

	public List<GameObject> GetNodes()
	{
		return chunkNodes;
	}

	public void ChangeChunk(GameObject floorGO, int relativeMoveTo)
	{
		if (relativeMoveTo == -1)
		{
			if (curChunk == LevelTileChunk.PLANE)
			{
				curChunk = (LevelTileChunk)(chunks.Length - 1);
			}
			else
			{
				curChunk--;
			}
		}
		else if (curChunk == (LevelTileChunk)(chunks.Length - 1))
		{
			curChunk = LevelTileChunk.PLANE;
		}
		else
		{
			curChunk++;
		}
		ChangeChunkTo(floorGO, curChunk, LevelTileTex.FLAVOR1);
	}

	public void ChangeChunkTo(GameObject floorGO, LevelTileChunk chunkId, LevelTileTex flavorId)
	{
		LevelFloor component = floorGO.GetComponent<LevelFloor>();
		if (component == null)
		{
			return;
		}
		curChunk = chunkId;
		curFlavor = flavorId;
		MeshFilter component2 = floorGO.GetComponent<MeshFilter>();
		component2.sharedMesh = chunks[(int)curChunk][(int)curFlavor].GetComponent<MeshFilter>().sharedMesh;
		LevelFloorProps component3 = chunks[(int)curChunk][(int)curFlavor].GetComponent<LevelFloorProps>();
		if (component3 != null)
		{
			component.startYPos = component3.startYPos;
			component.endYPos = component3.endYPos;
			component.generateFrontWall = component3.generateFrontWall;
			component.generateFrontWallInverse = component3.generateFrontWallInverse;
			if (component3.meshCollider != null)
			{
				floorGO.GetComponent<MeshCollider>().sharedMesh = component3.meshCollider;
			}
			else
			{
				floorGO.GetComponent<MeshCollider>().sharedMesh = chunks[(int)curChunk][(int)curFlavor].GetComponent<MeshFilter>().sharedMesh;
			}
			Transform[] componentsInChildren = floorGO.gameObject.GetComponentsInChildren<Transform>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (floorGO.transform != componentsInChildren[i])
					{
						UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
					}
				}
			}
			if (component3.subGOs != null)
			{
				for (int j = 0; j < component3.subGOs.Length; j++)
				{
					GameObject gameObject = new GameObject(component3.subGOs[j].name);
					gameObject.transform.parent = floorGO.transform;
					gameObject.transform.localPosition = component3.subGOs[j].transform.localPosition;
					gameObject.transform.localRotation = component3.subGOs[j].transform.localRotation;
					gameObject.transform.localScale = component3.subGOs[j].transform.localScale;
					MeshCollider component4 = component3.subGOs[j].GetComponent<MeshCollider>();
					if (component4 != null)
					{
						MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
						meshCollider.sharedMesh = component4.sharedMesh;
					}
				}
			}
		}
		else
		{
			component.startYPos = 0f;
			component.endYPos = 0f;
			component.generateFrontWall = true;
			component.generateFrontWallInverse = true;
			floorGO.GetComponent<MeshCollider>().sharedMesh = chunks[(int)curChunk][(int)curFlavor].GetComponent<MeshFilter>().sharedMesh;
			Transform[] componentsInChildren2 = floorGO.gameObject.GetComponentsInChildren<Transform>();
			if (componentsInChildren2 != null)
			{
				for (int k = 0; k < componentsInChildren2.Length; k++)
				{
					if (floorGO.transform != componentsInChildren2[k])
					{
						UnityEngine.Object.DestroyImmediate(componentsInChildren2[k].gameObject);
					}
				}
			}
		}
		ObstacleFallBack component5 = chunks[(int)curChunk][(int)curFlavor].GetComponent<ObstacleFallBack>();
		if (component5 != null)
		{
			ObstacleFallBack obstacleFallBack = floorGO.GetComponent<ObstacleFallBack>();
			if (obstacleFallBack == null)
			{
				obstacleFallBack = floorGO.AddComponent<ObstacleFallBack>();
			}
			obstacleFallBack.duckeable = component5.duckeable;
			obstacleFallBack.canTrip = component5.canTrip;
			obstacleFallBack.canWalkOver = component5.canWalkOver;
			obstacleFallBack.canWalkOnWalls = component5.canWalkOnWalls;
			obstacleFallBack.addForceToObjOnHit = component5.addForceToObjOnHit;
			obstacleFallBack.canClimbEdge = component5.canClimbEdge;
		}
		else
		{
			ObstacleFallBack component6 = floorGO.GetComponent<ObstacleFallBack>();
			if (component6 != null)
			{
				UnityEngine.Object.DestroyImmediate(component6);
			}
		}
	}

	public void ChangeChunkTo(GameObject floorGO, LevelTileChunk chunkId, LevelTileTex flavorId, bool invertH, bool invertV)
	{
		ChangeChunkTo(floorGO, chunkId, flavorId);
		floorGO.transform.localScale = new Vector3(floorGO.transform.localScale.x * (float)((!invertH) ? 1 : (-1)), floorGO.transform.localScale.y * (float)((!invertV) ? 1 : (-1)), floorGO.transform.localScale.z);
	}

	public void ChangeChunkTexTo(GameObject floorGO, LevelTileTex flavorId)
	{
		MeshFilter component = floorGO.GetComponent<MeshFilter>();
		curChunk = getChunkIdx(component.sharedMesh, out curFlavor);
		curFlavor = flavorId;
		int num = (int)curChunk;
		int num2 = (int)curFlavor;
		if (num2 < chunks[num].Length)
		{
			component.sharedMesh = chunks[num][num2].GetComponent<MeshFilter>().sharedMesh;
			floorGO.GetComponent<MeshCollider>().sharedMesh = chunks[num][num2].GetComponent<MeshFilter>().sharedMesh;
		}
		else
		{
			Debug.Log(string.Format("There isn't flavor {0} for chunk {1}", num2, num));
		}
	}

	private LevelTileChunk getChunkIdx(Mesh mf, out LevelTileTex flavorId)
	{
		for (int i = 0; i < chunks.Length; i++)
		{
			for (int j = 0; j < chunks[i].Length; j++)
			{
				if (chunks[i][j].GetComponent<MeshFilter>().sharedMesh == mf)
				{
					flavorId = (LevelTileTex)j;
					return (LevelTileChunk)i;
				}
			}
		}
		flavorId = LevelTileTex.FLAVOR1;
		return LevelTileChunk.UNKNOWN;
	}

	public void ConnectFloors()
	{
		float num = defLength / 2f;
		for (int i = 0; i < chunkNodes.Count - 1; i++)
		{
			if (chunkNodes[i] == null)
			{
				Debug.Log(string.Format("node {0} is null", i));
			}
			LevelFloor levelFloor = LevelBuilderUtil.GetLevelFloor(chunkNodes[i]);
			LevelFloor levelFloor2 = LevelBuilderUtil.GetLevelFloor(chunkNodes[i + 1]);
			LevelFrontalWall frontalWall = LevelBuilderUtil.GetFrontalWall(chunkNodes[i]);
			float num2 = ((!(frontalWall != null)) ? 0f : ((float)frontalWall.segmentLen * 4f));
			float num3 = Mathf.Tan(levelFloor.rotation * ((float)Math.PI / 180f)) * num;
			float num4 = levelFloor.transform.position.y - num3;
			float num5 = Mathf.Tan(levelFloor2.rotation * ((float)Math.PI / 180f)) * num;
			float num6 = ((!(levelFloor2.transform.localScale.y > 0f)) ? levelFloor2.endYPos : levelFloor2.startYPos);
			float num7 = ((!(levelFloor.transform.localScale.y > 0f)) ? levelFloor.startYPos : (levelFloor.endYPos - num2));
			levelFloor.transform.position = new Vector3(levelFloor.transform.position.x, num2, levelFloor.transform.position.z);
			levelFloor2.transform.position = new Vector3(levelFloor2.transform.position.x, num4 - num5 - num6 + num7, levelFloor2.transform.position.z);
		}
	}

	public int GetTileIndex(GameObject floorGO)
	{
		for (int i = 0; i < chunkNodes.Count; i++)
		{
			if (chunkNodes[i].GetComponent<LevelFloorBase>().floor.gameObject == floorGO)
			{
				return i;
			}
		}
		return -1;
	}

	public GameObject GetFloorGOByIndex(int idx)
	{
		return chunkNodes[idx].GetComponent<LevelFloorBase>().floor.gameObject;
	}

	private void createFloorYPosArray()
	{
		if (chunkNodes == null)
		{
			Debug.Log("chunkNodes is null");
			return;
		}
		floorYPos = new float[chunkNodes.Count];
		LevelFloorBase component = chunkNodes[0].GetComponent<LevelFloorBase>();
		for (int i = 0; i < chunkNodes.Count - 1; i++)
		{
			LevelFloorBase component2 = chunkNodes[i + 1].GetComponent<LevelFloorBase>();
			component = chunkNodes[i].GetComponent<LevelFloorBase>();
			floorYPos[i] = Mathf.Min(component.floor.transform.position.y, component2.floor.transform.position.y);
			component = component2;
		}
		floorYPos[chunkNodes.Count - 1] = chunkNodes[chunkNodes.Count - 1].transform.position.y;
	}

	private float getMin(float y1, float y2, float y3)
	{
		if (y1 < y2)
		{
			return (!(y1 < y3)) ? y3 : y1;
		}
		return (!(y2 < y3)) ? y3 : y2;
	}
}
