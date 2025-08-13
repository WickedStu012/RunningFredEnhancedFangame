using UnityEngine;

public class CharHelper
{
	private static GameObject player;

	private static Transform playerT;

	private static CharStateMachine sm;

	private static CharSkin skin;

	private static CharBloodSplat bloodSplat;

	private static CharProps props;

	private static CharEffects effects;

	private static CharacterController cc;

	private static GameObject jetpackGO;

	public static GameObject GetPlayer()
	{
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");
		}
		return player;
	}

	public static Transform GetPlayerTransform()
	{
		if (playerT == null)
		{
			playerT = GetPlayer().transform;
		}
		return playerT;
	}

	public static CharStateMachine GetCharStateMachine()
	{
		if (sm == null)
		{
			sm = GetPlayer().GetComponent<CharStateMachine>();
		}
		return sm;
	}

	public static CharProps GetProps()
	{
		if (props == null && GetPlayer() != null)
		{
			props = GetPlayer().GetComponent<CharProps>();
		}
		return props;
	}

	public static CharEffects GetEffects()
	{
		if (effects == null && GetPlayer() != null)
		{
			effects = GetPlayer().GetComponent<CharEffects>();
		}
		return effects;
	}

	public static CharacterController GetCharacterController()
	{
		if (cc == null && GetPlayer() != null)
		{
			cc = GetPlayer().GetComponent<CharacterController>();
		}
		return cc;
	}

	public static void DisablePlayer()
	{
		GetPlayer().SetActive(false);
	}

	public static GameObject GetPlayerFromBone(GameObject goBone)
	{
		Transform parent = goBone.transform.parent;
		while (parent.tag != "Player" && parent.tag != null)
		{
			parent = parent.transform.parent;
		}
		if (parent != null)
		{
			return parent.gameObject;
		}
		return null;
	}

	public static bool IsColliderFromPlayer(Collider col)
	{
		return col.gameObject.tag.StartsWith("Player");
	}

	public static Transform GetTransformByName(string boneName)
	{
		GameObject goPlayer = GetPlayer();
		return GetTransformByName(goPlayer, boneName);
	}

	public static CharSkin GetCharSkin()
	{
		if (skin == null)
		{
			skin = GetPlayer().GetComponent<CharSkin>();
		}
		return skin;
	}

	public static CharBloodSplat GetCharBloodSplat()
	{
		if (bloodSplat == null)
		{
			bloodSplat = GetPlayer().GetComponent<CharBloodSplat>();
		}
		return bloodSplat;
	}

	public static Transform GetTransformByName(GameObject goPlayer, string boneName)
	{
		Transform[] componentsInChildren = goPlayer.GetComponentsInChildren<Transform>();
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

	public static void PlaceCharacterOnStart()
	{
		GameObject gameObject = GetPlayer();
		RaycastHit hitInfo;
		if (gameObject == null)
		{
			Debug.LogError("Error. Cannot the character in scene.");
		}
		else if (Physics.Raycast(new Vector3(0f, 1000f, 0f), Vector3.down, out hitInfo, float.PositiveInfinity, 8704))
		{
			gameObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
			if (sm == null)
			{
				sm = GetCharStateMachine();
			}
			sm.characterPlaced = true;
		}
		else
		{
			Debug.LogError("Error. Cannot find the floor or platform to place the character");
		}
	}

	public static GameObject AttachJetpack()
	{
		Transform transformByName = GetTransformByName(GetPlayer(), "torso1");
		GameObject gameObject = Resources.Load("Characters/Prefabs/Jetpack/Jetpack_torso1", typeof(GameObject)) as GameObject;
		jetpackGO = Object.Instantiate(gameObject) as GameObject;
		jetpackGO.transform.parent = transformByName;
		jetpackGO.name = gameObject.name;
		jetpackGO.transform.position = gameObject.transform.position;
		jetpackGO.transform.rotation = gameObject.transform.rotation;
		jetpackGO.transform.localScale = gameObject.transform.localScale;
		jetpackGO.GetComponent<Renderer>().sharedMaterial = Resources.Load("Characters/Materials/Jetpack", typeof(Material)) as Material;
		return jetpackGO;
	}

	public static void HideJetpack()
	{
		if (jetpackGO != null)
		{
			jetpackGO.GetComponent<Renderer>().enabled = false;
		}
	}

	public static void ShowJetpack()
	{
		if (jetpackGO != null)
		{
			jetpackGO.GetComponent<Renderer>().enabled = true;
		}
	}

	public static bool IsJetpackVisible()
	{
		if (jetpackGO != null)
		{
			return jetpackGO.GetComponent<Renderer>().enabled;
		}
		return false;
	}

	public static void DetachJetpack()
	{
		if (jetpackGO != null)
		{
			Object.Destroy(jetpackGO);
			jetpackGO = null;
		}
	}

	public static void SetJetpackGO(GameObject jetpack)
	{
		jetpackGO = jetpack;
	}

	/*public static GameObject AttachJetpackIfNecessary()
	{
		if (GameObject.Find("JetpackItem") != null)
		{
			Transform transformByName = GetTransformByName(GetPlayer(), "torso1");
			GameObject gameObject = Resources.Load("Characters/Prefabs/Jetpack/Jetpack_torso1", typeof(GameObject)) as GameObject;
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.parent = transformByName;
			gameObject2.name = gameObject.name;
			gameObject2.transform.position = gameObject.transform.position;
			gameObject2.transform.rotation = gameObject.transform.rotation;
			gameObject2.transform.localScale = gameObject.transform.localScale;
			gameObject2.GetComponent<Renderer>().sharedMaterial = Resources.Load("Characters/Materials/Jetpack", typeof(Material)) as Material;
			return gameObject2;
		}
		return null;
	}
	*/
    public static GameObject AttachJetpackIfNecessary()
    {
        if (JetpackItem.Instance != null || (PlayerAccount.Instance != null && PlayerAccount.Instance.CurrentLevel.IndexOf("Rooftop-Random") != -1))
        {
            Transform transformByName = GetTransformByName(GetPlayer(), "torso1");
            GameObject gameObject = Resources.Load("Characters/Prefabs/Jetpack/Jetpack_torso1", typeof(GameObject)) as GameObject;
            GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
            gameObject2.transform.parent = transformByName;
            gameObject2.name = gameObject.name;
            gameObject2.transform.position = gameObject.transform.position;
            gameObject2.transform.rotation = gameObject.transform.rotation;
            gameObject2.transform.localScale = gameObject.transform.localScale;
            gameObject2.GetComponent<Renderer>().sharedMaterial = Resources.Load("Characters/Materials/Jetpack", typeof(Material)) as Material;
            return gameObject2;
        }
        return null;
    }

    public static GameObject AttachWingsIfNecessary()
	{
		if (WingsItem.Instance != null || (PlayerAccount.Instance != null && PlayerAccount.Instance.CurrentLevel.IndexOf("Rooftop-Random") != -1))
		{
			Transform transformByName = GetTransformByName(GetPlayer(), "torso1");
			GameObject gameObject = Resources.Load("Characters/Prefabs/Wings/Wings_torso1", typeof(GameObject)) as GameObject;
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.parent = transformByName;
			gameObject2.name = gameObject.name;
			gameObject2.transform.position = gameObject.transform.position;
			gameObject2.transform.rotation = gameObject.transform.rotation;
			gameObject2.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
			gameObject2.transform.localScale = gameObject.transform.localScale;
			return gameObject2;
		}
		return null;
	}
}
