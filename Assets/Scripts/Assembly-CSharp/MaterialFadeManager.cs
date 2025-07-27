using System.Collections.Generic;
using UnityEngine;

public class MaterialFadeManager
{
	private const string FADE_SHADER = "RunningFred/DiffuseSimpleFade";

	private static Dictionary<Material, Material> materialToFadeMap;

	private static Dictionary<Material, Material> fadeToMaterialMap;

	private static Shader fadeShader;

	public static void Initialize()
	{
		materialToFadeMap = new Dictionary<Material, Material>();
		fadeToMaterialMap = new Dictionary<Material, Material>();
		if (fadeShader == null)
		{
			fadeShader = Shader.Find("RunningFred/DiffuseSimpleFade");
		}
	}

	public static void SetGlobalTransparency(float t)
	{
		foreach (Material key in fadeToMaterialMap.Keys)
		{
			key.SetFloat("_Alpha", t);
		}
	}

	public static void InitializeFadeMaterial(MeshRenderer[] renderers, bool switchToFade)
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			InitializeFadeMaterial(renderers[i], switchToFade);
		}
	}

	public static void InitializeFadeMaterial(MeshRenderer mr, bool switchToFade)
	{
		if (!mr || !mr.sharedMaterial || materialToFadeMap == null || fadeToMaterialMap == null)
		{
			return;
		}
		if (materialToFadeMap.ContainsKey(mr.sharedMaterial))
		{
			if (switchToFade)
			{
				mr.sharedMaterial = materialToFadeMap[mr.sharedMaterial];
			}
		}
		else if (!materialToFadeMap.ContainsValue(mr.sharedMaterial))
		{
			Material material = new Material(mr.sharedMaterial);
			material.name = "FadeMaterial";
			material.shader = fadeShader;
			materialToFadeMap[mr.sharedMaterial] = material;
			fadeToMaterialMap[material] = mr.sharedMaterial;
			if (switchToFade)
			{
				mr.sharedMaterial = material;
			}
		}
	}

	public static void SwitchToFadeMaterial(MeshRenderer[] renderers)
	{
		if (renderers != null)
		{
			for (int i = 0; i < renderers.Length; i++)
			{
				SwitchToFadeMaterial(renderers[i]);
			}
		}
	}

	public static void SwitchToFadeMaterial(MeshRenderer mr)
	{
		if ((bool)mr && (bool)mr.sharedMaterial && materialToFadeMap != null)
		{
			if (materialToFadeMap.ContainsKey(mr.sharedMaterial))
			{
				mr.sharedMaterial = materialToFadeMap[mr.sharedMaterial];
			}
			else
			{
				InitializeFadeMaterial(mr, true);
			}
		}
	}

	public static void SwitchToSolidMaterial(MeshRenderer[] renderers)
	{
		if (renderers != null)
		{
			for (int i = 0; i < renderers.Length; i++)
			{
				SwitchToSolidMaterial(renderers[i]);
			}
		}
	}

	public static void SwitchToSolidMaterial(MeshRenderer mr)
	{
		if ((bool)mr && (bool)mr.sharedMaterial && fadeToMaterialMap != null && fadeToMaterialMap.ContainsKey(mr.sharedMaterial))
		{
			mr.sharedMaterial = fadeToMaterialMap[mr.sharedMaterial];
		}
	}
}
