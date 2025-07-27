using System.Collections.Generic;
using UnityEngine;

public class LevelFloorBase : MonoBehaviour
{
	public LevelFloor floor;

	public LevelWall leftWall;

	public LevelWall rightWall;

	public List<GameObject> traps;

	public List<GameObject> decorations;

	public List<GameObject> platforms;

	public List<GameObject> pickups;

	public bool CanBeDisabled = true;

	public LevelFrontalWall frontWall;

	private MeshRenderer[] meshRenderers;

	private bool meshRenderersCached;

	public MeshRenderer[] MeshRenderers
	{
		get
		{
			if (!meshRenderersCached)
			{
				meshRenderers = base.gameObject.GetComponentsInChildren<MeshRenderer>(true);
				meshRenderersCached = true;
			}
			return meshRenderers;
		}
	}
}
