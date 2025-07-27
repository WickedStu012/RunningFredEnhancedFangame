using System.Collections.Generic;
using UnityEngine;

public class CharHeadHelper
{
	private static GameObject headGO;

	public static void SyncHeadAttachs(string charName, Material mat)
	{
		GameObject head = GameObject.FindGameObjectWithTag("CharacterHead");
		List<CharDef> list = CharBuilderFileParser.ParseFile();
		int charDefIndex = getCharDefIndex(charName, list);
		if (charDefIndex != -1)
		{
			CharDef charDef = list[charDefIndex];
			for (int i = 0; i < charDef.parts.Count; i++)
			{
				if (charDef.parts[i].Contains("_head") && !charDef.parts[i].Contains("Head_head"))
				{
					GameObject gameObject = Resources.Load(string.Format("Characters/Prefabs/{0}", charDef.parts[i]), typeof(GameObject)) as GameObject;
					if (gameObject != null)
					{
						locateAttach(gameObject, head, mat);
					}
					else
					{
						Debug.Log(string.Format("Cannot find prefab attach {0} on Resources.", charDef.parts[i]));
					}
				}
			}
		}
		else
		{
			Debug.Log(string.Format("Cannot find char {0} on character list.", charName));
		}
	}

	private static int getCharDefIndex(string charName, List<CharDef> listCharDef)
	{
		for (int i = 0; i < listCharDef.Count; i++)
		{
			if (string.Compare(listCharDef[i].charName, charName) == 0)
			{
				return i;
			}
		}
		return -1;
	}

	private static void locateAttach(GameObject partPrefab, GameObject head, Material mat)
	{
		if (head != null)
		{
			GameObject gameObject = Object.Instantiate(partPrefab) as GameObject;
			Transform transformByName = GetTransformByName(head, "Head_0");
			gameObject.transform.parent = transformByName.transform;
			string strB = partPrefab.name.Substring(0, partPrefab.name.IndexOf("_"));
			Transform playerTransform = CharHelper.GetPlayerTransform();
			MeshFilter[] componentsInChildren = playerTransform.GetComponentsInChildren<MeshFilter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (string.Compare(componentsInChildren[i].name, strB) != 0)
				{
					continue;
				}
				gameObject.transform.localPosition = componentsInChildren[i].transform.localPosition;
				gameObject.transform.localRotation = componentsInChildren[i].transform.localRotation;
				gameObject.transform.localScale = componentsInChildren[i].transform.localScale;
				gameObject.layer = 17;
				if (string.Compare(componentsInChildren[i].name, "FredAssassinHoodie") == 0)
				{
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 0.12f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				else if (string.Compare(componentsInChildren[i].name, "soldierMustache") == 0)
				{
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 0.07f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				else if (string.Compare(componentsInChildren[i].name, "persiaHair") == 0)
				{
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 0.07f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				else if (string.Compare(componentsInChildren[i].name, "FredAviatorHat") == 0)
				{
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 0.02f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
					gameObject.transform.localRotation = Quaternion.Euler(gameObject.transform.localRotation.eulerAngles.x, 280f, 90f);
				}
				else if (string.Compare(componentsInChildren[i].name, "wolverineCap") == 0)
				{
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 0.05f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
				}
				else if (string.Compare(componentsInChildren[i].name, "IronFredHelmet") == 0 || string.Compare(componentsInChildren[i].name, "rocketFredHelmet") == 0)
				{
					SkinnedMeshRenderer componentInChildren = head.transform.GetComponentInChildren<SkinnedMeshRenderer>();
					if (componentInChildren != null)
					{
						componentInChildren.enabled = false;
					}
				}
				break;
			}
			gameObject.GetComponent<Renderer>().sharedMaterial = mat;
		}
		else
		{
			Debug.Log("Cannot find CharacterHead");
		}
	}

	private static Transform GetTransformByName(GameObject head, string boneName)
	{
		Transform[] componentsInChildren = head.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(boneName, componentsInChildren[i].name, true) == 0)
			{
				componentsInChildren[i].name = boneName;
				return componentsInChildren[i];
			}
		}
		Debug.Log(string.Format("Cannot find the bone {0}", boneName));
		return null;
	}

	public static void SetHeadGameObject(GameObject go)
	{
		headGO = go;
	}

	public static GameObject GetHeadGameObject()
	{
		return headGO;
	}
}
