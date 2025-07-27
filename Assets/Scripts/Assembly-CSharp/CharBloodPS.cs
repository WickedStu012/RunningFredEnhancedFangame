using System.Collections.Generic;
using UnityEngine;

public class CharBloodPS : MonoBehaviour
{
	private Dictionary<string, CharBloodPoolGenerator> poolGens;

	private void Start()
	{
		poolGens = new Dictionary<string, CharBloodPoolGenerator>();
		CharBloodPoolGenerator[] componentsInChildren = GetComponentsInChildren<CharBloodPoolGenerator>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			poolGens.Add(componentsInChildren[i].name, componentsInChildren[i]);
		}
	}

	public void StartPoolCreationOn(string boneName)
	{
		if (poolGens.ContainsKey(boneName))
		{
			poolGens[boneName].StartPoolCreation();
		}
	}
}
