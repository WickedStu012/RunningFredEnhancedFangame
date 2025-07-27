using System.Collections.Generic;
using UnityEngine;

public class LevelMaterialDebug : MonoBehaviour
{
	private List<string> matNames;

	private void Start()
	{
		getMatNames();
	}

	private void OnGUI()
	{
		for (int i = 0; i < matNames.Count; i++)
		{
			GUILayout.Label(matNames[i]);
		}
		if (GUILayout.Button("Refresh"))
		{
			getMatNames();
		}
	}

	private void getMatNames()
	{
		matNames = new List<string>();
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].sharedMaterial == null)
			{
				Debug.Log(string.Format("mrs[i].sharedMaterial is null"));
			}
			else if (!matNames.Contains(componentsInChildren[i].sharedMaterial.name))
			{
				matNames.Add(componentsInChildren[i].sharedMaterial.name);
			}
		}
	}
}
