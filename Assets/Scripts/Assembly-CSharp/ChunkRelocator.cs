using System.Collections.Generic;
using UnityEngine;

public class ChunkRelocator : MonoBehaviour
{
	private const float CHECK_TIME = 0.1f;

	public int VISIBLE_CHUNK_NUMBER = 16;

	public int VISIBLE_FLOORS_AND_PLATFORMS_NUMBER = 16;

	public int VISIBLE_WALLS_NUMBER = 16;

	public int VISIBLE_DECORATIONS_NUMBER = 10;

	public int VISIBLE_TRAPS_NUMBER = 10;

	public int VISIBLE_PICKUPS_NUMBER = 10;

	public float averageProfileK = 0.8f;

	public float goodProfileK = 1f;

	public float greatProfileK = 1f;

	private int CUR_VISIBLE_CHUNK_NUMBER;

	private int CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER;

	private int CUR_VISIBLE_WALLS_NUMBER;

	private int CUR_VISIBLE_DECORATIONS_NUMBER;

	private int CUR_VISIBLE_TRAPS_NUMBER;

	private int CUR_VISIBLE_PICKUPS_NUMBER;

	public bool destroyPreviousChunks;

	private List<GameObject> chunkNodes;

	private List<LevelFloorBase> chunkNodesBase;

	private GameObject player;

	private float accumTime;

	private int firstActiveChunk;

	private float defLength;

	private ILevelChunkContainer cc;

	private bool changeVisibility = true;

	private List<Mesh> meshesToDestroy = new List<Mesh>();

	private static ChunkRelocator instance;

	private bool randomMode;

	private Transform nextClosestTransform;

	private float nextClosestDistance = -1f;

	private bool chunkHasFadedIn;

	private List<GameObject> chunkGOToDestroy;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		switch (Profile.Score)
		{
		case PerformanceScore.AVERAGE:
			CUR_VISIBLE_CHUNK_NUMBER = (int)((float)VISIBLE_CHUNK_NUMBER * averageProfileK);
			CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER = (int)((float)VISIBLE_FLOORS_AND_PLATFORMS_NUMBER * averageProfileK);
			CUR_VISIBLE_WALLS_NUMBER = (int)((float)VISIBLE_WALLS_NUMBER * averageProfileK);
			CUR_VISIBLE_DECORATIONS_NUMBER = (int)((float)VISIBLE_DECORATIONS_NUMBER * averageProfileK);
			CUR_VISIBLE_PICKUPS_NUMBER = (int)((float)VISIBLE_PICKUPS_NUMBER * averageProfileK);
			CUR_VISIBLE_TRAPS_NUMBER = (int)((float)VISIBLE_TRAPS_NUMBER * averageProfileK);
			break;
		case PerformanceScore.GOOD:
			CUR_VISIBLE_CHUNK_NUMBER = (int)((float)VISIBLE_CHUNK_NUMBER * goodProfileK);
			CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER = (int)((float)VISIBLE_FLOORS_AND_PLATFORMS_NUMBER * goodProfileK);
			CUR_VISIBLE_WALLS_NUMBER = (int)((float)VISIBLE_WALLS_NUMBER * goodProfileK);
			CUR_VISIBLE_DECORATIONS_NUMBER = (int)((float)VISIBLE_DECORATIONS_NUMBER * goodProfileK);
			CUR_VISIBLE_PICKUPS_NUMBER = (int)((float)VISIBLE_PICKUPS_NUMBER * goodProfileK);
			CUR_VISIBLE_TRAPS_NUMBER = (int)((float)VISIBLE_TRAPS_NUMBER * goodProfileK);
			break;
		default:
			CUR_VISIBLE_CHUNK_NUMBER = (int)((float)VISIBLE_CHUNK_NUMBER * greatProfileK);
			CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER = (int)((float)VISIBLE_FLOORS_AND_PLATFORMS_NUMBER * greatProfileK);
			CUR_VISIBLE_WALLS_NUMBER = (int)((float)VISIBLE_WALLS_NUMBER * greatProfileK);
			CUR_VISIBLE_DECORATIONS_NUMBER = (int)((float)VISIBLE_DECORATIONS_NUMBER * greatProfileK);
			CUR_VISIBLE_PICKUPS_NUMBER = (int)((float)VISIBLE_PICKUPS_NUMBER * greatProfileK);
			CUR_VISIBLE_TRAPS_NUMBER = (int)((float)VISIBLE_TRAPS_NUMBER * greatProfileK);
			break;
		}
		CUR_VISIBLE_TRAPS_NUMBER = VISIBLE_TRAPS_NUMBER;
		if (CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER < CUR_VISIBLE_TRAPS_NUMBER)
		{
			CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER = CUR_VISIBLE_TRAPS_NUMBER;
		}
		if (CUR_VISIBLE_CHUNK_NUMBER < CUR_VISIBLE_TRAPS_NUMBER)
		{
			CUR_VISIBLE_CHUNK_NUMBER = CUR_VISIBLE_TRAPS_NUMBER;
		}
		if (Application.isPlaying)
		{
			cc = base.gameObject.GetComponent(typeof(ILevelChunkContainer)) as ILevelChunkContainer;
			chunkNodes = cc.GetNodes();
			chunkNodesBase = cc.GetNodeBases();
			defLength = cc.GetDefLength();
			if (chunkNodes == null)
			{
				Debug.LogError("chunkNodes is null");
			}
			GameEventDispatcher.AddListener("PlayerReachGoal", onPlayerReachGoal);
		}
		initVis();
	}

	private void CalculateMinDistance()
	{
		if (!player || chunkNodesBase == null)
		{
			return;
		}
		float num = float.MaxValue;
		if (chunkHasFadedIn)
		{
			int index = -1;
			for (int i = 0; i < chunkNodesBase.Count; i++)
			{
				float num2 = chunkNodesBase[i].transform.position.z - (player.transform.position.z - defLength * 4f);
				if (num2 > 0f && num2 < num)
				{
					num = num2;
					index = i;
				}
			}
			nextClosestDistance = num;
			nextClosestTransform = chunkNodesBase[index].transform;
		}
		else if ((bool)nextClosestTransform)
		{
			num = nextClosestTransform.position.z - (player.transform.position.z - defLength * 4f);
		}
		chunkHasFadedIn = false;
		MaterialFadeManager.SetGlobalTransparency(1f - Mathf.Clamp01(num / nextClosestDistance));
	}

	private void Update()
	{
		CalculateMinDistance();
		if (changeVisibility)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 0.1f)
			{
				modifyChunkVisibity();
				accumTime = 0f;
			}
		}
	}

	private void OnDestroy()
	{
		foreach (Mesh item in meshesToDestroy)
		{
			Object.DestroyImmediate(item);
		}
		foreach (LevelFloorBase item2 in chunkNodesBase)
		{
			Object.Destroy(item2);
		}
		foreach (GameObject chunkNode in chunkNodes)
		{
			Object.Destroy(chunkNode);
		}
		meshesToDestroy.Clear();
		instance = null;
	}

	public void initVis()
	{
		MaterialFadeManager.Initialize();
		cc = base.gameObject.GetComponent(typeof(ILevelChunkContainer)) as ILevelChunkContainer;
		chunkNodes = cc.GetNodes();
		chunkNodesBase = cc.GetNodeBases();
		firstActiveChunk = 0;
		clearFloorBasesRefs();
		changeTrapParenting();
		changeDecorationParenting();
		changePlatformParenting();
		changePickupParenting();
		randomMode = base.gameObject.GetComponent<LevelRandomManager>() != null;
		if (!randomMode)
		{
			combineGeometry();
			setInitialVisibility();
		}
		if (Camera.main != null)
		{
			Camera.main.GetComponent<FredCamera>().SetDistanceToShadowPlane((float)(VISIBLE_WALLS_NUMBER - 2) * defLength);
		}
	}

	public void resetVis()
	{
		firstActiveChunk = 0;
		Debug.Log(string.Format("resetVis. firstActiveChunk: {0} chunkNodes.Count: {1}", firstActiveChunk, chunkNodes.Count));
	}

	private void onPlayerReachGoal(object sender, GameEvent evn)
	{
		changeVisibility = false;
	}

	private void combineGeometry()
	{
		for (int i = 0; i < chunkNodes.Count; i++)
		{
			List<CombineInstance> list = new List<CombineInstance>();
			LevelFloorBase component = chunkNodes[i].GetComponent<LevelFloorBase>();
			if (component.leftWall != null && component.leftWall.visible && component.leftWall.isInner)
			{
				addMeshFilterToCombineList(component.leftWall.gameObject, list);
			}
			if (component.rightWall != null && component.rightWall.visible && component.rightWall.isInner)
			{
				addMeshFilterToCombineList(component.rightWall.gameObject, list);
			}
			if (component.floor.visible)
			{
				addMeshFilterToCombineList(component.floor.gameObject, list);
			}
			if (component.frontWall != null)
			{
			}
			if (component.decorations != null)
			{
				for (int j = 0; j < component.decorations.Count; j++)
				{
					if (component.decorations[j] == null)
					{
						Debug.LogWarning(string.Format("Chunk {0} has a null decoration", component.name));
						continue;
					}
					LevelDecoration component2 = component.decorations[j].GetComponent<LevelDecoration>();
					if (component2 == null || (component2 != null && component2.isStatic))
					{
						addMeshFilterToCombineList(component.decorations[j], list);
					}
				}
			}
			if (component.platforms != null)
			{
				for (int k = 0; k < component.platforms.Count; k++)
				{
					if (component.platforms[k] == null)
					{
						Debug.LogWarning(string.Format("Chunk {0} has a null platform", component.name));
						continue;
					}
					LevelPlatform component3 = component.platforms[k].GetComponent<LevelPlatform>();
					if (component3 == null || (component3 != null && component3.isStatic))
					{
						addMeshFilterToCombineList(component.platforms[k], list);
					}
				}
			}
			CombineInstance[] array = new CombineInstance[list.Count];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = list[l];
			}
			MeshFilter meshFilter = chunkNodes[i].GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = chunkNodes[i].AddComponent<MeshFilter>();
			}
			meshFilter.mesh = new Mesh();
			meshesToDestroy.Add(meshFilter.mesh);
			meshFilter.mesh.CombineMeshes(array);
			MeshRenderer meshRenderer = chunkNodes[i].GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				meshRenderer = chunkNodes[i].AddComponent<MeshRenderer>();
			}
			Material material = cc.GetMaterial();
			if (material == null)
			{
				meshRenderer.sharedMaterial = component.gameObject.GetComponent<Renderer>().sharedMaterial;
			}
			else
			{
				meshRenderer.sharedMaterial = material;
			}
			MaterialFadeManager.InitializeFadeMaterial(meshRenderer, false);
			var o_305_3_638890613168472756 = meshFilter.sharedMesh;
			chunkNodes[i].transform.gameObject.SetActive(true);
			chunkNodes[i].layer = 22;
		}
	}

	private void addMeshFilterToCombineList(GameObject go, List<CombineInstance> combineList)
	{
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (!(component == null))
		{
			CombineInstance item = new CombineInstance
			{
				mesh = component.sharedMesh
			};
			Vector3 position = component.transform.position;
			component.transform.position = new Vector3(component.transform.position.x, component.transform.position.y, component.transform.localPosition.z);
			item.transform = component.transform.localToWorldMatrix;
			component.transform.position = position;
			Object.Destroy(component);
			Object.Destroy(go.GetComponent<MeshRenderer>());
			combineList.Add(item);
		}
	}

	private int[] invertMesh(int[] tris)
	{
		for (int i = 0; i < tris.Length / 3; i++)
		{
			int num = tris[i * 3 + 1];
			tris[i * 3 + 1] = tris[i * 3 + 2];
			tris[i * 3 + 2] = num;
		}
		return tris;
	}

	private void setInitialVisibility()
	{
		if (chunkNodes == null)
		{
			return;
		}
		for (int i = CUR_VISIBLE_CHUNK_NUMBER; i < chunkNodes.Count; i++)
		{
			chunkNodes[i].SetActive(false);
		}
		for (int j = CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER; j < chunkNodes.Count; j++)
		{
			chunkNodesBase[j].floor.gameObject.SetActive(false);
			if (chunkNodesBase[j].platforms != null)
			{
				for (int k = 0; k < chunkNodesBase[j].platforms.Count; k++)
				{
					chunkNodesBase[j].platforms[k].SetActive(false);
				}
			}
		}
		for (int l = CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER; l < chunkNodes.Count; l++)
		{
			if (chunkNodesBase[l].leftWall != null)
			{
				chunkNodesBase[l].leftWall.gameObject.SetActive(false);
			}
			if (chunkNodesBase[l].rightWall != null)
			{
				chunkNodesBase[l].rightWall.gameObject.SetActive(false);
			}
			if (chunkNodesBase[l].frontWall != null)
			{
				chunkNodesBase[l].frontWall.gameObject.SetActive(false);
			}
		}
		for (int m = CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER; m < chunkNodes.Count; m++)
		{
			if (chunkNodesBase[m].decorations == null)
			{
				continue;
			}
			for (int n = 0; n < chunkNodesBase[m].decorations.Count; n++)
			{
				if (chunkNodesBase[m].decorations[n] != null)
				{
					chunkNodesBase[m].decorations[n].SetActive(false);
				}
			}
		}
		for (int num = CUR_VISIBLE_TRAPS_NUMBER; num < chunkNodes.Count; num++)
		{
			if (chunkNodesBase[num].traps == null)
			{
				continue;
			}
			for (int num2 = 0; num2 < chunkNodesBase[num].traps.Count; num2++)
			{
				if (chunkNodesBase[num].traps[num2] != null)
				{
					chunkNodesBase[num].traps[num2].SetActive(false);
				}
			}
		}
		for (int num3 = CUR_VISIBLE_PICKUPS_NUMBER; num3 < chunkNodes.Count; num3++)
		{
			if (chunkNodesBase[num3].pickups == null)
			{
				continue;
			}
			for (int num4 = 0; num4 < chunkNodesBase[num3].pickups.Count; num4++)
			{
				if (chunkNodesBase[num3].pickups[num4] != null)
				{
					chunkNodesBase[num3].pickups[num4].SetActive(false);
				}
			}
		}
		GameObject ender = cc.GetEnder();
		if (ender != null)
		{
			ender.SetActive(false);
		}
		disableNotVisibleWalls();
	}

	private void disableNotVisibleWalls()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		for (int i = 0; i < chunkNodes.Count; i++)
		{
			LevelFloorBase component = chunkNodes[i].GetComponent<LevelFloorBase>();
			if (component.floor != null && !component.floor.visible)
			{
				component.floor.gameObject.SetActive(false);
			}
			if (component.leftWall != null && !component.leftWall.visible)
			{
				component.leftWall.gameObject.SetActive(false);
			}
			if (component.rightWall != null && !component.rightWall.visible)
			{
				component.rightWall.gameObject.SetActive(false);
			}
		}
	}

	private void modifyChunkVisibity()
	{
		if (chunkNodes == null)
		{
			Debug.Log("chunkNodes == null");
		}
		if (player == null)
		{
			player = CharHelper.GetPlayer();
		}
		if (player == null)
		{
			Debug.LogError("@ChunkRelocator. Cannot find the player in scene.");
			return;
		}
		Transform transform = player.transform;
		int count = chunkNodes.Count;
		List<LevelFloorBase> list = null;
		for (int i = firstActiveChunk; i < count; i++)
		{
			if (!(chunkNodes[i].transform.position.z < transform.position.z - defLength * 4f))
			{
				continue;
			}
			if (!destroyPreviousChunks && i < count - 1)
			{
				firstActiveChunk = i + 1;
			}
			if (chunkNodesBase[i].CanBeDisabled)
			{
				if (!destroyPreviousChunks)
				{
					chunkNodesBase[i].gameObject.SetActive(false);
				}
				else
				{
					LevelFloorBase levelFloorBase = chunkNodesBase[i];
					if (levelFloorBase.CanBeDisabled)
					{
						if (list == null)
						{
							list = new List<LevelFloorBase>();
						}
						list.Add(levelFloorBase);
					}
				}
			}
			if (i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1 < count)
			{
				if (!chunkNodes[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1].activeSelf)
				{
					if (!randomMode)
					{
						MaterialFadeManager.SwitchToFadeMaterial(chunkNodesBase[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1].MeshRenderers);
					}
					MaterialFadeManager.SwitchToSolidMaterial(chunkNodesBase[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 2].MeshRenderers);
					MaterialFadeManager.SetGlobalTransparency(0f);
					chunkHasFadedIn = true;
				}
				chunkNodes[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1].SetActive(true);
				LevelFloorBase levelFloorBase2 = chunkNodesBase[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1];
				levelFloorBase2.floor.gameObject.SetActive(levelFloorBase2.floor.visible);
			}
			if (i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1 < count)
			{
				LevelFloorBase levelFloorBase2 = chunkNodesBase[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1];
				if (levelFloorBase2.leftWall != null)
				{
					levelFloorBase2.leftWall.gameObject.SetActive(levelFloorBase2.leftWall.visible);
				}
				if (levelFloorBase2.rightWall != null)
				{
					levelFloorBase2.rightWall.gameObject.SetActive(levelFloorBase2.rightWall.visible);
				}
				if (levelFloorBase2.frontWall != null)
				{
					levelFloorBase2.frontWall.gameObject.SetActive(true);
				}
				if (levelFloorBase2.platforms != null)
				{
					for (int j = 0; j < levelFloorBase2.platforms.Count; j++)
					{
						levelFloorBase2.platforms[j].SetActive(true);
					}
				}
			}
			if (i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1 < chunkNodes.Count)
			{
				LevelFloorBase levelFloorBase2 = chunkNodesBase[i + CUR_VISIBLE_FLOORS_AND_PLATFORMS_NUMBER - 1];
				if (levelFloorBase2.decorations != null)
				{
					for (int k = 0; k < levelFloorBase2.decorations.Count; k++)
					{
						levelFloorBase2.decorations[k].SetActive(true);
					}
				}
			}
			if (i + CUR_VISIBLE_TRAPS_NUMBER - 1 >= 0 && i + CUR_VISIBLE_TRAPS_NUMBER - 1 < chunkNodes.Count)
			{
				LevelFloorBase levelFloorBase2 = chunkNodesBase[i + CUR_VISIBLE_TRAPS_NUMBER - 1];
				if (levelFloorBase2.traps != null)
				{
					for (int l = 0; l < levelFloorBase2.traps.Count; l++)
					{
						TrapActivator component = levelFloorBase2.traps[l].GetComponent<TrapActivator>();
						if (component == null)
						{
							levelFloorBase2.traps[l].SetActive(true);
						}
						else
						{
							component.EnableTrap();
						}
					}
				}
			}
			if (i + CUR_VISIBLE_PICKUPS_NUMBER - 1 >= 0 && i + CUR_VISIBLE_PICKUPS_NUMBER - 1 < chunkNodes.Count)
			{
				LevelFloorBase levelFloorBase2 = chunkNodesBase[i + CUR_VISIBLE_PICKUPS_NUMBER - 1];
				if (levelFloorBase2.pickups != null)
				{
					for (int m = 0; m < levelFloorBase2.pickups.Count; m++)
					{
						if (levelFloorBase2.pickups[m] != null)
						{
							levelFloorBase2.pickups[m].SetActive(true);
						}
					}
				}
			}
			if (!destroyPreviousChunks && i < chunkNodes.Count - 1)
			{
				firstActiveChunk = i + 1;
			}
		}
		if (list != null)
		{
			int count2 = list.Count;
			for (int n = 0; n < count2; n++)
			{
				chunkNodes.Remove(list[n].gameObject);
				chunkNodesBase.Remove(list[n]);
				recycleParticlesOf(list[n].gameObject);
				Object.Destroy(list[n].gameObject);
			}
			list.Clear();
			list = null;
		}
		if (chunkNodes[chunkNodes.Count - 1].activeSelf)
		{
			GameObject ender = cc.GetEnder();
			if (ender != null && !ender.activeSelf)
			{
				ender.SetActive(true);
			}
		}
		disableNotVisibleWalls();
	}

	private void changeTrapParenting()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Trap");
		for (int i = 0; i < array.Length; i++)
		{
			LevelBuilderUtil.ChangeParentingToThisTrap(chunkNodes, array[i]);
		}
	}

	private void changeDecorationParenting()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Decoration");
		for (int i = 0; i < array.Length; i++)
		{
			LevelBuilderUtil.ChangeParentingToThisDecoration(chunkNodes, array[i]);
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("AerealAccelerator");
		for (int j = 0; j < array2.Length; j++)
		{
			LevelBuilderUtil.ChangeParentingToThisDecoration(chunkNodes, array2[j]);
		}
	}

	private void changePlatformParenting()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			LevelBuilderUtil.ChangeParentingToThisPlatform(chunkNodes, array[i]);
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Stairs");
		for (int j = 0; j < array2.Length; j++)
		{
			if (array2[j].layer != 16)
			{
				LevelBuilderUtil.ChangeParentingToThisPlatform(chunkNodes, array2[j]);
			}
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("Bouncer");
		for (int k = 0; k < array3.Length; k++)
		{
			if (array3[k].layer != 16)
			{
				LevelBuilderUtil.ChangeParentingToThisPlatform(chunkNodes, array3[k]);
			}
		}
	}

	private void changePickupParenting()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Pickup");
		for (int i = 0; i < array.Length; i++)
		{
			LevelBuilderUtil.ChangeParentingToThisPickup(chunkNodes, array[i]);
		}
	}

	private void clearFloorBasesRefs()
	{
		for (int i = 0; i < chunkNodes.Count; i++)
		{
			LevelFloorBase component = chunkNodes[i].GetComponent<LevelFloorBase>();
			component.traps = null;
			component.decorations = null;
			component.platforms = null;
			component.pickups = null;
		}
	}

	public static void ModifyChunkVisibility()
	{
		instance.modifyChunkVisibilityFromStart();
	}

	private void modifyChunkVisibilityFromStart()
	{
		if (chunkNodes == null)
		{
			Debug.Log("chunkNodes == null");
			return;
		}
		if (player == null)
		{
			player = CharHelper.GetPlayer();
		}
		if (player == null)
		{
			Debug.LogError("@ChunkRelocator. Cannot find the player in scene.");
			return;
		}
		Transform transform = player.transform;
		int num = chunkNodes.Count;
		for (int i = 0; i < num; i++)
		{
			MaterialFadeManager.SwitchToSolidMaterial(chunkNodesBase[i].MeshRenderers);
			if (chunkNodes[i].transform.position.z < transform.position.z - defLength * 2f)
			{
				if (chunkNodesBase[i].CanBeDisabled && chunkNodesBase[i].gameObject.activeSelf)
				{
					chunkNodesBase[i].gameObject.SetActive(false);
				}
				continue;
			}
			if (num == chunkNodes.Count)
			{
				firstActiveChunk = i;
				num = ((i + CUR_VISIBLE_CHUNK_NUMBER > chunkNodes.Count) ? chunkNodes.Count : (i + CUR_VISIBLE_CHUNK_NUMBER));
			}
			LevelFloorBase levelFloorBase = chunkNodesBase[i];
			chunkNodes[i].SetActive(true);
			levelFloorBase.floor.gameObject.SetActive(levelFloorBase.floor.visible);
			if (levelFloorBase.leftWall != null)
			{
				levelFloorBase.leftWall.gameObject.SetActive(levelFloorBase.leftWall.visible);
			}
			if (levelFloorBase.rightWall != null)
			{
				levelFloorBase.rightWall.gameObject.SetActive(levelFloorBase.rightWall.visible);
			}
			if (levelFloorBase.frontWall != null)
			{
				levelFloorBase.frontWall.gameObject.SetActive(true);
			}
			if (levelFloorBase.platforms != null)
			{
				for (int j = 0; j < levelFloorBase.platforms.Count; j++)
				{
					levelFloorBase.platforms[j].SetActive(true);
				}
			}
			if (levelFloorBase.decorations != null)
			{
				for (int k = 0; k < levelFloorBase.decorations.Count; k++)
				{
					levelFloorBase.decorations[k].SetActive(true);
				}
			}
			if (levelFloorBase.traps != null)
			{
				for (int l = 0; l < levelFloorBase.traps.Count; l++)
				{
					TrapActivator component = levelFloorBase.traps[l].GetComponent<TrapActivator>();
					if (component == null)
					{
						levelFloorBase.traps[l].SetActive(true);
					}
					else
					{
						component.EnableTrap();
					}
				}
			}
			if (levelFloorBase.pickups == null)
			{
				continue;
			}
			for (int m = 0; m < levelFloorBase.pickups.Count; m++)
			{
				if (levelFloorBase.pickups[m] != null)
				{
					levelFloorBase.pickups[m].SetActive(true);
				}
			}
		}
		if (num < chunkNodes.Count)
		{
			for (int n = num; n < chunkNodes.Count; n++)
			{
				chunkNodes[n].SetActive(false);
			}
		}
		GameObject ender = cc.GetEnder();
		if (chunkNodes[chunkNodes.Count - 1].activeSelf)
		{
			if (ender != null && !ender.activeSelf)
			{
				ender.SetActive(true);
			}
		}
		else if (ender != null)
		{
			ender.SetActive(false);
		}
	}

	public static void InitVis()
	{
		instance.initVis();
	}

	public static void ResetVis()
	{
		instance.resetVis();
	}

	public static void SetChangeVisibility(bool val)
	{
		instance.changeVisibility = val;
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
