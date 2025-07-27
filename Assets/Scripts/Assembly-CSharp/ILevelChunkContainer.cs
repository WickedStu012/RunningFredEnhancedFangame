using System.Collections.Generic;
using UnityEngine;

public interface ILevelChunkContainer
{
	List<GameObject> GetNodes();

	List<LevelFloorBase> GetNodeBases();

	float GetDefLength();

	GameObject GetEnder();

	Material GetMaterial();
}
