using System;
using UnityEngine;

public class LevelRandomGroup : MonoBehaviour
{
	private int chunkCount;

	private LevelFloorBase[] bases;

	private FallTrigger[] fts;

	private void Start()
	{
		getBases();
	}

	private void getBases()
	{
		if (bases == null)
		{
			bases = GetComponentsInChildren<LevelFloorBase>();
			chunkCount = bases.Length;
		}
	}

	public int GetChunkCount()
	{
		getBases();
		return chunkCount;
	}

	public float GetLastPosY()
	{
		return bases[chunkCount - 1].transform.position.y;
	}

	public float GetLastPosZ()
	{
		return bases[chunkCount - 1].transform.position.z;
	}

	public LevelFloorBase GetChunkBase(int chunkNum)
	{
		return bases[chunkNum];
	}

	public GameObject GetChunkGO(int chunkNum)
	{
		return bases[chunkNum].gameObject;
	}

	public void RandomizeSlope(SlopeDir slopeDir)
	{
		float num = 0f;
		switch (slopeDir)
		{
		case SlopeDir.ANY:
			num = UnityEngine.Random.Range(-4, 8) * 5;
			break;
		case SlopeDir.UP:
			num = UnityEngine.Random.Range(-4, 0) * 5;
			break;
		case SlopeDir.DOWN:
			num = UnityEngine.Random.Range(0, 8) * 5;
			break;
		}
		getBases();
		if (bases.Length <= 1)
		{
			return;
		}
		for (int i = 1; i < bases.Length; i++)
		{
			if (bases[i].floor != null && bases[i].floor.canChangeSlopeAtRT)
			{
				rotateFloor(bases[i].floor, bases[i].floor.rotation + num);
			}
		}
		for (int j = 0; j < bases.Length - 1; j++)
		{
			LevelBuilderUtil.ConnectFloor(bases[j], bases[j + 1]);
		}
	}

	public void EnableFallTriggers()
	{
		if (fts == null)
		{
			fts = GetComponentsInChildren<FallTrigger>();
		}
		if (fts != null)
		{
			for (int i = 0; i < fts.Length; i++)
			{
				fts[i].gameObject.SetActive(true);
			}
		}
	}

	public FallTrigger[] GetFallTriggers()
	{
		if (fts == null)
		{
			fts = GetComponentsInChildren<FallTrigger>();
		}
		return fts;
	}

	public void ReconnectChunkElements(Material mat)
	{
		getBases();
		if (bases.Length <= 1)
		{
			return;
		}
		LevelBuilderUtil.defMaterial = mat;
		float heightDif = 0f;
		for (int i = 0; i < bases.Length - 1; i++)
		{
			LevelFloorBase levelFloorBase = bases[i + 1];
			if (levelFloorBase.frontWall != null)
			{
				heightDif = (float)levelFloorBase.frontWall.segmentLen * 4f;
			}
			if (bases[i + 1].floor.generateFrontWall)
			{
				LevelBuilderUtil.AddFrontWall(bases[i + 1], heightDif, 0f);
			}
		}
	}

	private void rotateFloor(LevelFloor floor, float angle)
	{
		floor.rotation = angle;
		floor.transform.localRotation = Quaternion.Euler(new Vector3(270f + floor.rotation, 0f, 0f));
		float num = 8f;
		float num2 = Mathf.Cos(floor.rotation * ((float)Math.PI / 180f)) * num;
		float num3 = num / num2;
		floor.transform.localScale = new Vector3(floor.transform.localScale.x, num3 * Mathf.Sign(floor.transform.localScale.y), floor.transform.localScale.z);
	}
}
