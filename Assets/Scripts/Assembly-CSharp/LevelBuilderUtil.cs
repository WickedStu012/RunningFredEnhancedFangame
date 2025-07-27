using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilderUtil
{
	public const float INC_STEP = 4f;

	public const float defLength = 16f;

	public const float defWidth = 16f;

	private static GameObject[][] chunks;

	public static Material defMaterial;

	private static GameObject outerSideWall;

	public static void ConnectFloors(List<GameObject> nodes)
	{
		for (int i = 0; i < nodes.Count - 1; i++)
		{
			ConnectFloor(nodes[i].GetComponent<LevelFloorBase>(), nodes[i + 1].GetComponent<LevelFloorBase>());
		}
	}

	public static void ConnectFloor(LevelFloorBase floorBase, LevelFloorBase nextFloorBase)
	{
		float num = 8f;
		LevelFloor floor = floorBase.floor;
		LevelFloor floor2 = nextFloorBase.floor;
		float num2 = Mathf.Tan(floor.rotation * ((float)Math.PI / 180f)) * num;
		float num3 = floor.transform.position.y - num2;
		float num4 = Mathf.Tan(floor2.rotation * ((float)Math.PI / 180f)) * num;
		float num5 = ((!(floor2.transform.localScale.y > 0f)) ? floor2.endYPos : floor2.startYPos);
		float num6 = ((!(floor.transform.localScale.y > 0f)) ? floor.startYPos : floor.endYPos);
		float num7 = 0f;
		if (nextFloorBase.frontWall != null)
		{
			num7 = (float)nextFloorBase.frontWall.segmentLen * 4f;
		}
		floor2.transform.position = new Vector3(floor2.transform.position.x, num3 - num4 - num5 + num6 + num7, floor2.transform.position.z);
		if (RequireFrontWall(floor, floor2))
		{
			AddFrontWall(nextFloorBase, num7, num4 + num5);
		}
		else if (nextFloorBase.frontWall != null && nextFloorBase.frontWall.GetComponent<Renderer>() != null)
		{
			nextFloorBase.frontWall.GetComponent<Renderer>().enabled = false;
			nextFloorBase.frontWall.GetComponent<Collider>().enabled = false;
		}
		CorrectWallTransform(floorBase, outerSideWall);
	}

	public static bool RequireFrontWall(LevelFloor lf, LevelFloor lf2)
	{
		bool flag = lf.transform.localScale.y < 0f;
		bool flag2 = lf2.transform.localScale.y < 0f;
		if ((lf2.transform.position.y > lf.transform.position.y && ((!flag && lf2.generateFrontWall) || (flag && lf2.generateFrontWallInverse))) || (lf2.transform.position.y < lf.transform.position.y && ((flag2 && lf.generateFrontWall) || (!flag2 && lf.generateFrontWallInverse))))
		{
			return true;
		}
		return false;
	}

	public static void Init(Material defMat)
	{
		defMaterial = defMat;
		loadOutSideWallPrefab();
	}

	public static void InitIfNecessary(Material defMat)
	{
		defMaterial = defMat;
		if (outerSideWall == null)
		{
			loadOutSideWallPrefab();
		}
	}

	public static LevelTileTex GetTexFlavorId(GameObject[][] chunks, GameObject floorGO)
	{
		LevelBuilderUtil.chunks = chunks;
		MeshFilter component = floorGO.GetComponent<MeshFilter>();
		LevelTileTex flavorId;
		getChunkIdx(component.sharedMesh, out flavorId);
		return flavorId;
	}

	public static LevelTileChunk GetChunkId(GameObject[][] chunks, GameObject floorGO)
	{
		LevelBuilderUtil.chunks = chunks;
		MeshFilter component = floorGO.GetComponent<MeshFilter>();
		LevelTileTex flavorId;
		return getChunkIdx(component.sharedMesh, out flavorId);
	}

	public static int GetNodeIndex(List<GameObject> chunkNodes, GameObject floorGO)
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

	public static LevelFloor GetLevelFloor(GameObject goFloorBase)
	{
		return goFloorBase.GetComponent<LevelFloorBase>().floor;
	}

	private static LevelTileChunk getChunkIdx(Mesh mf, out LevelTileTex flavorId)
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

	public static void PlacePlayerOnFloor()
	{
		GameObject gameObject = GameObject.FindWithTag("Player");
		RaycastHit hitInfo;
		if (Physics.Raycast(new Vector3(gameObject.transform.position.x, 10000f, gameObject.transform.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 8704))
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, hitInfo.transform.position.y, gameObject.transform.position.z);
		}
	}

	public static void SetInvisible(GameObject floorGO, Material invisibleMat)
	{
		LevelFloor component = floorGO.GetComponent<LevelFloor>();
		if (component != null)
		{
			component.visible = false;
			component.GetComponent<Renderer>().material = invisibleMat;
		}
	}

	public static void ChangeParentingToThisTrap(List<GameObject> chunks, GameObject trap)
	{
		if (chunks == null)
		{
			return;
		}
		float num = 8f;
		if (trap.transform.position.z < chunks[0].transform.position.z - num)
		{
			trap.transform.parent = chunks[0].transform;
			LevelFloorBase component = chunks[0].transform.GetComponent<LevelFloorBase>();
			if (component.traps == null)
			{
				component.traps = new List<GameObject>();
			}
			if (!component.traps.Contains(trap))
			{
				component.traps.Add(trap);
			}
		}
		for (int i = 0; i < chunks.Count; i++)
		{
			if (chunks[i].transform.position.z - num <= trap.transform.position.z && trap.transform.position.z <= chunks[i].transform.position.z + num)
			{
				trap.transform.parent = chunks[i].transform;
				LevelFloorBase component2 = chunks[i].transform.GetComponent<LevelFloorBase>();
				if (component2.traps == null)
				{
					component2.traps = new List<GameObject>();
				}
				if (!component2.traps.Contains(trap))
				{
					component2.traps.Add(trap);
				}
			}
		}
		if (chunks[chunks.Count - 1].transform.position.z + num < trap.transform.position.z)
		{
			trap.transform.parent = chunks[chunks.Count - 1].transform;
			LevelFloorBase component3 = chunks[chunks.Count - 1].transform.GetComponent<LevelFloorBase>();
			if (component3.traps == null)
			{
				component3.traps = new List<GameObject>();
			}
			if (!component3.traps.Contains(trap))
			{
				component3.traps.Add(trap);
			}
		}
	}

	public static void ChangeParentingToThisDecoration(List<GameObject> chunks, GameObject decoration)
	{
		if (chunks == null)
		{
			return;
		}
		float num = 8f;
		if (decoration.transform.position.z < chunks[0].transform.position.z - num)
		{
			decoration.transform.parent = chunks[0].transform;
			LevelFloorBase component = chunks[0].transform.GetComponent<LevelFloorBase>();
			if (component.decorations == null)
			{
				component.decorations = new List<GameObject>();
			}
			if (!component.decorations.Contains(decoration))
			{
				component.decorations.Add(decoration);
			}
		}
		for (int i = 0; i < chunks.Count; i++)
		{
			if (chunks[i].transform.position.z - num <= decoration.transform.position.z && decoration.transform.position.z <= chunks[i].transform.position.z + num)
			{
				decoration.transform.parent = chunks[i].transform;
				LevelFloorBase component2 = chunks[i].transform.GetComponent<LevelFloorBase>();
				if (component2.decorations == null)
				{
					component2.decorations = new List<GameObject>();
				}
				if (!component2.decorations.Contains(decoration))
				{
					component2.decorations.Add(decoration);
				}
			}
		}
		if (chunks[chunks.Count - 1].transform.position.z + num < decoration.transform.position.z)
		{
			decoration.transform.parent = chunks[chunks.Count - 1].transform;
			LevelFloorBase component3 = chunks[chunks.Count - 1].transform.GetComponent<LevelFloorBase>();
			if (component3.decorations == null)
			{
				component3.decorations = new List<GameObject>();
			}
			if (!component3.decorations.Contains(decoration))
			{
				component3.decorations.Add(decoration);
			}
		}
	}

	public static void ChangeParentingToThisPlatform(List<GameObject> chunks, GameObject platform)
	{
		if (chunks == null)
		{
			return;
		}
		float num = 8f;
		if (platform.transform.position.z < chunks[0].transform.position.z - num)
		{
			platform.transform.parent = chunks[0].transform;
			LevelFloorBase component = chunks[0].transform.GetComponent<LevelFloorBase>();
			if (component.platforms == null)
			{
				component.platforms = new List<GameObject>();
			}
			if (!component.platforms.Contains(platform))
			{
				component.platforms.Add(platform);
			}
		}
		for (int i = 0; i < chunks.Count; i++)
		{
			if (chunks[i].transform.position.z - num <= platform.transform.position.z && platform.transform.position.z <= chunks[i].transform.position.z + num)
			{
				platform.transform.parent = chunks[i].transform;
				LevelFloorBase component2 = chunks[i].transform.GetComponent<LevelFloorBase>();
				if (component2.platforms == null)
				{
					component2.platforms = new List<GameObject>();
				}
				if (!component2.platforms.Contains(platform))
				{
					component2.platforms.Add(platform);
				}
			}
		}
		if (chunks[chunks.Count - 1].transform.position.z + num < platform.transform.position.z)
		{
			platform.transform.parent = chunks[chunks.Count - 1].transform;
			LevelFloorBase component3 = chunks[chunks.Count - 1].transform.GetComponent<LevelFloorBase>();
			if (component3.platforms == null)
			{
				component3.platforms = new List<GameObject>();
			}
			if (!component3.platforms.Contains(platform))
			{
				component3.platforms.Add(platform);
			}
		}
	}

	public static void ChangeParentingToThisPickup(List<GameObject> chunks, GameObject pickup)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(new Vector3(0f, 1000f, pickup.transform.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 512))
		{
			pickup.transform.parent = hitInfo.transform.parent;
			if (hitInfo.transform.parent == null)
			{
				Debug.Log(string.Format("name: {0}", hitInfo.transform.name));
			}
			LevelFloorBase component = hitInfo.transform.parent.GetComponent<LevelFloorBase>();
			if (component.pickups == null)
			{
				component.pickups = new List<GameObject>();
			}
			if (!component.pickups.Contains(pickup))
			{
				component.pickups.Add(pickup);
			}
		}
		if (chunks == null)
		{
			return;
		}
		float num = 8f;
		if (pickup.transform.position.z < chunks[0].transform.position.z - num)
		{
			pickup.transform.parent = chunks[0].transform;
			LevelFloorBase component2 = chunks[0].transform.GetComponent<LevelFloorBase>();
			if (component2.pickups == null)
			{
				component2.pickups = new List<GameObject>();
			}
			if (!component2.pickups.Contains(pickup))
			{
				component2.pickups.Add(pickup);
			}
		}
		for (int i = 0; i < chunks.Count; i++)
		{
			if (chunks[i].transform.position.z - num <= pickup.transform.position.z && pickup.transform.position.z <= chunks[i].transform.position.z + num)
			{
				pickup.transform.parent = chunks[i].transform;
				LevelFloorBase component3 = chunks[i].transform.GetComponent<LevelFloorBase>();
				if (component3.pickups == null)
				{
					component3.pickups = new List<GameObject>();
				}
				if (!component3.pickups.Contains(pickup))
				{
					component3.pickups.Add(pickup);
				}
			}
		}
		if (chunks[chunks.Count - 1].transform.position.z + num < pickup.transform.position.z)
		{
			pickup.transform.parent = chunks[chunks.Count - 1].transform;
			LevelFloorBase component4 = chunks[chunks.Count - 1].transform.GetComponent<LevelFloorBase>();
			if (component4.pickups == null)
			{
				component4.pickups = new List<GameObject>();
			}
			if (!component4.pickups.Contains(pickup))
			{
				component4.pickups.Add(pickup);
			}
		}
	}

	public static LevelWall AddWall(GameObject wallPrefab, GameObject go, float width, bool isLeft, bool isInner)
	{
		if (wallPrefab == null)
		{
			Debug.LogError("WallPrefab is null");
			return null;
		}
		float num = 0f;
		GameObject gameObject = UnityEngine.Object.Instantiate(wallPrefab) as GameObject;
		gameObject.name = ((!isLeft) ? "rightWall" : "leftWall");
		gameObject.layer = ((!isLeft) ? 11 : 10);
		gameObject.transform.parent = go.transform;
		LevelFloor floor = go.GetComponent<LevelFloorBase>().floor;
		if (floor != null)
		{
			num = floor.transform.position.y;
		}
		if (isLeft ^ !isInner)
		{
			gameObject.transform.localPosition = new Vector3(isInner ? ((0f - width) / 2f) : 0f, 0f - go.transform.position.y + num, 0f);
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(gameObject.transform.localRotation.eulerAngles.x, gameObject.transform.localRotation.eulerAngles.y, gameObject.transform.localRotation.eulerAngles.z + 180f));
		}
		else
		{
			gameObject.transform.localPosition = new Vector3(isInner ? (width / 2f) : 0f, num, 0f);
			gameObject.transform.localRotation = Quaternion.Euler(new Vector3(gameObject.transform.localRotation.eulerAngles.x, 0f, 0f));
		}
		if (isInner)
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.center = new Vector3(0f, boxCollider.center.y, boxCollider.center.z);
			boxCollider.size = new Vector3(0.5f, boxCollider.size.y, boxCollider.size.z);
		}
		else
		{
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				meshCollider.sharedMesh = component.sharedMesh;
			}
		}
		gameObject.AddComponent<TriggerWall>();
		LevelWall levelWall = gameObject.AddComponent<LevelWall>();
		levelWall.isInner = isInner;
		return levelWall;
	}

	public static void RelocateEnder(ChunkChain cc)
	{
		if (!(cc.ender == null))
		{
			List<GameObject> nodes = cc.GetNodes();
			GameObject gameObject = nodes[nodes.Count - 1];
			LevelFloorBase component = gameObject.GetComponent<LevelFloorBase>();
			LevelEnd component2 = cc.ender.GetComponent<LevelEnd>();
			cc.ender.transform.position = new Vector3(0f, component.floor.transform.position.y, gameObject.transform.position.z) + component2.translationOffset;
			cc.ender.transform.parent = cc.transform;
		}
	}

	public static LevelFrontalWall GetFrontalWall(GameObject goFloorBase)
	{
		return goFloorBase.GetComponent<LevelFloorBase>().frontWall;
	}

	public static void CorrectWallTransform(LevelFloorBase floorBase, GameObject outsideWallPrefab)
	{
		float y = 0f;
		LevelFloor floor = floorBase.floor;
		if (floor != null)
		{
			y = floor.transform.position.y;
		}
		LevelWall leftWall = floorBase.leftWall;
		if (!(leftWall != null))
		{
			return;
		}
		if (!leftWall.isInner)
		{
			leftWall.transform.position = new Vector3(leftWall.transform.position.x, y, leftWall.transform.position.z);
			if (floor != null)
			{
				if (floor.rotation != 0f)
				{
					Mesh mesh = new Mesh();
					Mesh sharedMesh = leftWall.GetComponent<MeshFilter>().sharedMesh;
					float num = Mathf.Tan(floor.rotation * ((float)Math.PI / 180f)) * 8f;
					Vector3[] array = new Vector3[sharedMesh.vertices.Length];
					for (int i = 0; i < sharedMesh.vertices.Length; i++)
					{
						switch (i)
						{
						case 0:
							array[i] = new Vector3(sharedMesh.vertices[i].x, sharedMesh.vertices[i].y, num);
							break;
						case 1:
							array[i] = new Vector3(sharedMesh.vertices[i].x, sharedMesh.vertices[i].y, 0f - num);
							break;
						default:
							array[i] = new Vector3(sharedMesh.vertices[i].x, sharedMesh.vertices[i].y, sharedMesh.vertices[i].z);
							break;
						}
					}
					mesh.vertices = array;
					mesh.uv = sharedMesh.uv;
					mesh.triangles = sharedMesh.triangles;
					mesh.RecalculateNormals();
					mesh.RecalculateBounds();
					leftWall.GetComponent<MeshFilter>().mesh = mesh;
				}
				else
				{
					leftWall.GetComponent<MeshFilter>().sharedMesh = outsideWallPrefab.GetComponent<MeshFilter>().sharedMesh;
				}
			}
		}
		LevelWall rightWall = floorBase.rightWall;
		if (!(rightWall != null) || rightWall.isInner)
		{
			return;
		}
		rightWall.transform.position = new Vector3(rightWall.transform.position.x, y, rightWall.transform.position.z);
		if (!(floor != null))
		{
			return;
		}
		if (floor.rotation != 0f)
		{
			Mesh mesh2 = new Mesh();
			Mesh sharedMesh2 = rightWall.GetComponent<MeshFilter>().sharedMesh;
			float num2 = Mathf.Tan(floor.rotation * ((float)Math.PI / 180f)) * 8f;
			Vector3[] array2 = new Vector3[sharedMesh2.vertices.Length];
			for (int j = 0; j < sharedMesh2.vertices.Length; j++)
			{
				switch (j)
				{
				case 0:
					array2[j] = new Vector3(sharedMesh2.vertices[j].x, sharedMesh2.vertices[j].y, 0f - num2);
					break;
				case 1:
					array2[j] = new Vector3(sharedMesh2.vertices[j].x, sharedMesh2.vertices[j].y, num2);
					break;
				default:
					array2[j] = new Vector3(sharedMesh2.vertices[j].x, sharedMesh2.vertices[j].y, sharedMesh2.vertices[j].z);
					break;
				}
			}
			mesh2.vertices = array2;
			mesh2.uv = sharedMesh2.uv;
			mesh2.triangles = sharedMesh2.triangles;
			mesh2.RecalculateNormals();
			mesh2.RecalculateBounds();
			rightWall.GetComponent<MeshFilter>().mesh = mesh2;
		}
		else
		{
			rightWall.GetComponent<MeshFilter>().sharedMesh = outsideWallPrefab.GetComponent<MeshFilter>().sharedMesh;
		}
	}

	public static void ModifyFrontWallsToClimbeable(List<GameObject> goFloorBases, bool climb)
	{
		for (int i = 0; i < goFloorBases.Count; i++)
		{
			LevelFloorBase component = goFloorBases[i].GetComponent<LevelFloorBase>();
			if (component.frontWall != null)
			{
				ModifyFrontWallToClimbeable(component.frontWall, climb);
			}
		}
	}

	public static void ModifyFrontWallToClimbeable(LevelFrontalWall wall, bool climb)
	{
		wall.climb = climb;
	}

	public static void AddFrontWall(LevelFloorBase floorBase, float heightDif, float dh)
	{
		LevelFrontalWall frontalWall = floorBase.frontWall;
		GameObject gameObject = floorBase.floor.gameObject;
		float num = 0.0625f;
		float minU = 0.25f;
		float maxU = 0.5f;
		float minV = 0f;
		float maxV = num;
		float minV2 = 0.25f;
		float maxV2 = 0.25f + num;
		int num2 = (int)(heightDif / 4f);
		if (num2 == 0)
		{
			if (frontalWall != null)
			{
				UnityEngine.Object.DestroyImmediate(frontalWall.gameObject);
			}
			return;
		}
		int segmentCount = Mathf.Abs(num2);
		Vector3[] verts = getVerts(segmentCount, -8f, 8f, 4f, -8f, num2 > 0);
		Vector2[] array = null;
		array = ((!(frontalWall == null) && !frontalWall.climb) ? getUVs(segmentCount, minU, maxU, minV2, maxV2, num) : getUVs(segmentCount, minU, maxU, minV, maxV, num));
		int[] tris = getTris(segmentCount);
		Mesh mesh = getMesh(verts, array, tris);
		GameObject frontWallGO = getFrontWallGO(floorBase, ref frontalWall);
		updateFrontalWallComponents(frontWallGO, mesh, num2);
		frontWallGO.transform.position = new Vector3(floorBase.transform.position.x, gameObject.transform.position.y - heightDif + dh, floorBase.transform.position.z);
		frontWallGO.tag = Tag.GetStairsStr();
		frontWallGO.layer = 16;
		frontalWall.segmentLen = num2;
		frontalWall.GetComponent<Renderer>().enabled = true;
		frontalWall.GetComponent<Collider>().enabled = true;
		floorBase.GetComponent<LevelFloorBase>().frontWall = frontalWall;
	}

	private static Vector3[] getVerts(int segmentCount, float x1, float x2, float yStep, float z, bool positive)
	{
		Vector3[] array = new Vector3[segmentCount * 4];
		for (int i = 0; i < segmentCount; i++)
		{
			if (positive)
			{
				array[i * 4] = new Vector3(x1, (float)i * yStep, z);
				array[i * 4 + 1] = new Vector3(x2, (float)i * yStep, z);
				array[i * 4 + 2] = new Vector3(x1, (float)i * yStep + yStep, z);
				array[i * 4 + 3] = new Vector3(x2, (float)i * yStep + yStep, z);
			}
			else
			{
				array[i * 4] = new Vector3(x1, (float)(-i) * yStep, z);
				array[i * 4 + 1] = new Vector3(x2, (float)(-i) * yStep, z);
				array[i * 4 + 2] = new Vector3(x1, (float)(-i) * yStep - yStep, z);
				array[i * 4 + 3] = new Vector3(x2, (float)(-i) * yStep - yStep, z);
			}
		}
		return array;
	}

	private static Vector2[] getUVs(int segmentCount, float minU, float maxU, float minV, float maxV, float vInc)
	{
		Vector2[] array = new Vector2[segmentCount * 4];
		for (int i = 0; i < segmentCount; i++)
		{
			array[i * 4] = new Vector2(minU, minV + vInc * ((float)i % 4f));
			array[i * 4 + 1] = new Vector2(maxU, minV + vInc * ((float)i % 4f));
			array[i * 4 + 2] = new Vector2(minU, maxV + vInc * ((float)i % 4f));
			array[i * 4 + 3] = new Vector2(maxU, maxV + vInc * ((float)i % 4f));
		}
		return array;
	}

	private static int[] getTris(int segmentCount)
	{
		int[] array = new int[segmentCount * 6];
		for (int i = 0; i < segmentCount; i++)
		{
			array[i * 6] = i * 4 + 1;
			array[i * 6 + 1] = i * 4;
			array[i * 6 + 2] = i * 4 + 2;
			array[i * 6 + 3] = i * 4 + 2;
			array[i * 6 + 4] = i * 4 + 3;
			array[i * 6 + 5] = i * 4 + 1;
		}
		return array;
	}

	private static Mesh getMesh(Vector3[] verts, Vector2[] uvs, int[] tris)
	{
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;
		return mesh;
	}

	public static void updateFrontalWallComponents(GameObject go, Mesh newMesh, int segments)
	{
		MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = go.AddComponent<MeshRenderer>();
		}
		MeshFilter meshFilter = go.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = go.AddComponent<MeshFilter>();
		}
		BoxCollider boxCollider = go.GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = go.AddComponent<BoxCollider>();
		}
		boxCollider.size = new Vector3(16f, 4f * (float)Mathf.Abs(segments), 0.5f);
		boxCollider.center = new Vector3(0f, 4f * (float)segments / 2f, -8f);
		meshRenderer.material = defMaterial;
		meshFilter.mesh = newMesh;
		meshFilter.mesh.RecalculateNormals();
		meshFilter.mesh.RecalculateBounds();
	}

	private static GameObject getFrontWallGO(LevelFloorBase floorBase, ref LevelFrontalWall frontalWall)
	{
		GameObject gameObject;
		if (frontalWall != null)
		{
			gameObject = frontalWall.gameObject;
		}
		else
		{
			gameObject = new GameObject("frontWall");
			frontalWall = gameObject.AddComponent<LevelFrontalWall>();
			gameObject.transform.parent = floorBase.transform;
		}
		return gameObject;
	}

	private static void loadOutSideWallPrefab()
	{
		outerSideWall = Resources.Load("Levels/Castle/OuterWallSide", typeof(GameObject)) as GameObject;
	}
}
