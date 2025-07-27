using UnityEngine;

public class CharBuilderHelper
{
	private static GameObject goPlayer;

	private static Transform leftLeg;

	private static Transform leftLeg1;

	private static Transform leftLeg2;

	private static Transform leftFoot;

	private static Transform rightLeg;

	private static Transform rightLeg1;

	private static Transform rightLeg2;

	private static Transform rightFoot;

	private static Transform torso1;

	private static Transform torso2;

	private static Transform head;

	private static Transform leftArm;

	private static Transform leftArm1;

	private static Transform leftHand;

	private static Transform rightArm;

	private static Transform rightArm1;

	private static Transform rightHand;

	private static Transform meshHead;

	private static Transform meshHips;

	private static Transform meshLeftArm;

	private static Transform meshLeftFoot;

	private static Transform meshLeftHand;

	private static Transform meshLeftLeg1;

	private static Transform meshLeftLeg2;

	private static Transform meshRightArm;

	private static Transform meshRightFoot;

	private static Transform meshRightHand;

	private static Transform meshRightLeg1;

	private static Transform meshRightLeg2;

	private static Transform meshTorso;

	private static Material characterMaterial;

	private static Material characterMaterialGore;

	public static void BuildChar(CharDef charDef)
	{
		DestroyChar();
		GameObject gameObject = Resources.Load(string.Format("Characters/OriginalMeshes/{0}", charDef.basePrefab), typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError(string.Format("Cannot find the base prefab: {0}", charDef.basePrefab));
			return;
		}
		goPlayer = Object.Instantiate(gameObject) as GameObject;
		goPlayer.tag = "Player";
		goPlayer.name = charDef.charName;
		LoadMaterial(charDef.matName);
		characterMaterialGore = Resources.Load(string.Format("Characters/Materials/{0}", charDef.matGoreName), typeof(Material)) as Material;
		RelocateSkeleton();
		setMaterial();
		tagBones();
		applyLayerToBones();
		addRagdoll();
		addCharacterController();
		addOtherBehaviours();
		replaceHeadWithAnimatedHead();
		addAttachments(charDef);
		hideParts(charDef);
		addSkinned(charDef);
		addCharred(charDef);
		addBones(charDef);
		addGore(charDef);
		addSkin(charDef);
		addBloodSpurrPoints();
		goPlayer.layer = 8;
	}

	public static Material LoadMaterial(string matName)
	{
		characterMaterial = Resources.Load(string.Format("Characters/Materials/{0}", matName), typeof(Material)) as Material;
		return characterMaterial;
	}

	public static void DestroyChar()
	{
		GameObject gameObject = GameObject.FindWithTag("Player");
		if (gameObject != null)
		{
			Object.DestroyImmediate(gameObject);
		}
	}

	public static void RelocateSkeleton()
	{
		if (goPlayer == null)
		{
			goPlayer = CharHelper.GetPlayer();
		}
		leftLeg = getTransformByName(goPlayer, "leftLeg");
		leftLeg1 = getTransformByName(goPlayer, "leftLeg1");
		leftLeg2 = getTransformByName(goPlayer, "leftLeg2");
		leftFoot = getTransformByName(goPlayer, "leftFoot");
		rightLeg = getTransformByName(goPlayer, "rightLeg");
		rightLeg1 = getTransformByName(goPlayer, "rightLeg1");
		rightLeg2 = getTransformByName(goPlayer, "rightLeg2");
		rightFoot = getTransformByName(goPlayer, "rightFoot");
		torso1 = getTransformByName(goPlayer, "torso1");
		torso2 = getTransformByName(goPlayer, "torso2");
		head = getTransformByName(goPlayer, "head");
		leftArm = getTransformByName(goPlayer, "leftArm");
		leftArm1 = getTransformByName(goPlayer, "leftArm1");
		leftHand = getTransformByName(goPlayer, "leftHand");
		rightArm = getTransformByName(goPlayer, "rightArm");
		rightArm1 = getTransformByName(goPlayer, "rightArm1");
		rightHand = getTransformByName(goPlayer, "rightHand");
		relocateMeshes();
		if (meshLeftHand != null)
		{
			meshLeftHand.parent = leftHand;
		}
		if (meshRightHand != null)
		{
			meshRightHand.parent = rightHand;
		}
		if (meshLeftArm != null)
		{
			meshLeftArm.parent = leftArm1;
		}
		if (meshRightArm != null)
		{
			meshRightArm.parent = rightArm1;
		}
		if (meshTorso != null)
		{
			meshTorso.parent = torso1;
		}
		if (meshHips != null)
		{
			meshHips.parent = torso2;
		}
		if (meshLeftLeg1 != null)
		{
			meshLeftLeg1.parent = leftLeg1;
		}
		if (meshLeftLeg2 != null)
		{
			meshLeftLeg2.parent = leftLeg2;
		}
		if (meshLeftFoot != null)
		{
			meshLeftFoot.parent = leftFoot;
		}
		if (meshRightLeg1 != null)
		{
			meshRightLeg1.parent = rightLeg1;
		}
		if (meshRightLeg2 != null)
		{
			meshRightLeg2.parent = rightLeg2;
		}
		if (meshRightFoot != null)
		{
			meshRightFoot.parent = rightFoot;
		}
		if (meshHead != null)
		{
			meshHead.parent = head;
		}
	}

	private static void setSkeletonParenting()
	{
		leftFoot.parent = leftLeg2;
		leftLeg2.parent = leftLeg1;
		leftLeg1.parent = leftLeg;
		rightFoot.parent = rightLeg2;
		rightLeg2.parent = rightLeg1;
		rightLeg1.parent = rightLeg;
		head.parent = torso1;
		leftArm.parent = torso1;
		leftArm1.parent = leftArm;
		leftHand.parent = leftArm1;
		rightArm.parent = torso1;
		rightArm1.parent = rightArm;
		rightHand.parent = rightArm1;
		Transform transformByName = getTransformByName(goPlayer, "Armature");
		leftLeg.parent = transformByName;
		rightLeg.parent = transformByName;
		torso1.parent = transformByName;
		torso2.parent = transformByName;
	}

	private static void relocateMeshes()
	{
		meshHead = getTransformByName(goPlayer, "meshHead");
		meshHips = getTransformByName(goPlayer, "meshHips");
		meshLeftArm = getTransformByName(goPlayer, "meshLeftArm1");
		meshLeftFoot = getTransformByName(goPlayer, "meshLeftFoot");
		meshLeftHand = getTransformByName(goPlayer, "meshLeftHand");
		meshLeftLeg1 = getTransformByName(goPlayer, "meshLeftLeg1");
		meshLeftLeg2 = getTransformByName(goPlayer, "meshLeftLeg2");
		meshRightArm = getTransformByName(goPlayer, "meshRightArm1");
		meshRightFoot = getTransformByName(goPlayer, "meshRightFoot");
		meshRightHand = getTransformByName(goPlayer, "meshRightHand");
		meshRightLeg1 = getTransformByName(goPlayer, "meshRightLeg1");
		meshRightLeg2 = getTransformByName(goPlayer, "meshRightLeg2");
		meshTorso = getTransformByName(goPlayer, "meshTorso");
	}

	private static void setMaterial()
	{
		if (meshHead != null)
		{
			meshHead.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshHips != null)
		{
			meshHips.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshLeftArm != null)
		{
			meshLeftArm.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshLeftFoot != null)
		{
			meshLeftFoot.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshLeftHand != null)
		{
			meshLeftHand.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshLeftLeg1 != null)
		{
			meshLeftLeg1.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshLeftLeg2 != null)
		{
			meshLeftLeg2.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshRightArm != null)
		{
			meshRightArm.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshRightFoot != null)
		{
			meshRightFoot.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshRightHand != null)
		{
			meshRightHand.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshRightLeg1 != null)
		{
			meshRightLeg1.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshRightLeg2 != null)
		{
			meshRightLeg2.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
		if (meshTorso != null)
		{
			meshTorso.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		}
	}

	public static void SetMaterial(Material mat)
	{
		if (goPlayer == null)
		{
			goPlayer = CharHelper.GetPlayer();
		}
		if (meshHead == null)
		{
			relocateMeshes();
		}
		if (meshHead != null)
		{
			meshHead.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshHips != null)
		{
			meshHips.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshLeftArm != null)
		{
			meshLeftArm.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshLeftFoot != null)
		{
			meshLeftFoot.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshLeftHand != null)
		{
			meshLeftHand.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshLeftLeg1 != null)
		{
			meshLeftLeg1.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshLeftLeg2 != null)
		{
			meshLeftLeg2.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshRightArm != null)
		{
			meshRightArm.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshRightFoot != null)
		{
			meshRightFoot.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshRightHand != null)
		{
			meshRightHand.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshRightLeg1 != null)
		{
			meshRightLeg1.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshRightLeg2 != null)
		{
			meshRightLeg2.GetComponent<Renderer>().sharedMaterial = mat;
		}
		if (meshTorso != null)
		{
			meshTorso.GetComponent<Renderer>().sharedMaterial = mat;
		}
	}

	public static Material GetMaterial()
	{
		if (goPlayer == null)
		{
			goPlayer = CharHelper.GetPlayer();
		}
		if (meshHead != null)
		{
			return meshHead.GetComponent<Renderer>().sharedMaterial;
		}
		if (meshTorso != null)
		{
			return meshTorso.GetComponent<Renderer>().sharedMaterial;
		}
		return null;
	}

	private static void tagBones()
	{
		leftLeg.tag = "PlayerBone";
		leftLeg1.tag = "PlayerBone";
		leftLeg2.tag = "PlayerBone";
		leftFoot.tag = "PlayerBone";
		rightLeg.tag = "PlayerBone";
		rightLeg1.tag = "PlayerBone";
		rightLeg2.tag = "PlayerBone";
		rightFoot.tag = "PlayerBone";
		torso1.tag = "PlayerBone";
		torso2.tag = "PlayerBone";
		head.tag = "PlayerBone";
		leftArm.tag = "PlayerBone";
		leftArm1.tag = "PlayerBone";
		leftHand.tag = "PlayerBone";
		rightArm.tag = "PlayerBone";
		rightArm1.tag = "PlayerBone";
		rightHand.tag = "PlayerBone";
	}

	private static void applyLayerToBones()
	{
		leftLeg.gameObject.layer = 19;
		leftLeg1.gameObject.layer = 19;
		leftLeg2.gameObject.layer = 19;
		leftFoot.gameObject.layer = 19;
		rightLeg.gameObject.layer = 19;
		rightLeg1.gameObject.layer = 19;
		rightLeg2.gameObject.layer = 19;
		rightFoot.gameObject.layer = 19;
		torso1.gameObject.layer = 19;
		torso2.gameObject.layer = 19;
		head.gameObject.layer = 19;
		leftArm.gameObject.layer = 19;
		leftArm1.gameObject.layer = 19;
		leftHand.gameObject.layer = 19;
		rightArm.gameObject.layer = 19;
		rightArm1.gameObject.layer = 19;
		rightHand.gameObject.layer = 19;
	}

	private static void addRagdoll()
	{
		addRigidBody(torso1, 0.2f);
		addRigidBody(torso2, 0.1f);
		addRigidBody(head, 0.2f);
		addRigidBody(rightArm1, 0.1f);
		addRigidBody(rightHand, 0.1f);
		addRigidBody(leftArm1, 0.1f);
		addRigidBody(leftHand, 0.1f);
		addRigidBody(rightLeg1, 0.1f);
		addRigidBody(rightLeg2, 0.1f);
		addRigidBody(rightFoot, 0.1f);
		addRigidBody(leftLeg1, 0.1f);
		addRigidBody(leftLeg2, 0.1f);
		addRigidBody(leftFoot, 0.1f);
		addJoint(torso1, torso2, new Vector3(0f, 1f, 0f), new Vector3(-1f, 0f, 0f), -20f, 20f, 10f, float.PositiveInfinity);
		addJoint(head, torso1, new Vector3(1f, 1f, 0f), new Vector3(1f, 0f, 0f), -40f, 25f, 25f, float.PositiveInfinity);
		addJoint(rightArm1, torso1, new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), -70f, 10f, 50f, float.PositiveInfinity);
		addJoint(rightHand, rightArm1, new Vector3(0f, 0f, -1f), new Vector3(1f, 1f, 0f), -90f, 0f, 0f, float.PositiveInfinity);
		addJoint(leftArm1, torso1, new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), -70f, 10f, 50f, float.PositiveInfinity);
		addJoint(leftHand, leftArm1, new Vector3(0f, 0f, -1f), new Vector3(1f, 1f, 0f), -90f, 0f, 0f, float.PositiveInfinity);
		addJoint(rightLeg1, torso2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -20f, 70f, 30f, float.PositiveInfinity);
		addJoint(rightLeg2, rightLeg1, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, float.PositiveInfinity);
		addJoint(rightFoot, rightLeg2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, float.PositiveInfinity);
		addJoint(leftLeg1, torso2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -20f, 70f, 30f, float.PositiveInfinity);
		addJoint(leftLeg2, leftLeg1, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, float.PositiveInfinity);
		addJoint(leftFoot, leftLeg2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, float.PositiveInfinity);
		addBoxCollider(torso1, new Vector3(0.1f, 0f, 0f), new Vector3(0.12f, 0.5f, 0.4f));
		addBoxCollider(torso2, new Vector3(0.21f, 0f, -0.01f), new Vector3(0.45f, 0.5f, 0.4f));
		float radius = 0.07f;
		addCapsuleCollider(rightLeg1, new Vector3(-0.16f, 0f, 0f), radius, 0.17f, 0);
		addCapsuleCollider(rightLeg2, new Vector3(-0.1f, 0f, 0f), radius, 0.17f, 0);
		addCapsuleCollider(rightFoot, new Vector3(-0.18f, 0f, 0f), radius, 0.4f, 0);
		addCapsuleCollider(leftLeg1, new Vector3(-0.16f, 0f, 0f), radius, 0.17f, 0);
		addCapsuleCollider(leftLeg2, new Vector3(-0.1f, 0f, 0f), radius, 0.17f, 0);
		addCapsuleCollider(leftFoot, new Vector3(-0.18f, 0f, 0f), radius, 0.4f, 0);
		addCapsuleCollider(rightArm1, new Vector3(-0.086f, 0f, 0f), radius, 0.2f, 0);
		addCapsuleCollider(rightHand, new Vector3(-0.2f, 0f, 0f), 0.07f, 0.38f, 0);
		addCapsuleCollider(leftArm1, new Vector3(-0.086f, 0f, 0f), radius, 0.2f, 0);
		addCapsuleCollider(leftHand, new Vector3(-0.2f, 0f, 0f), 0.07f, 0.38f, 0);
		addSphereCollider(head, new Vector3(-0.4f, 0f, 0.05f), 0.4f);
	}

	private static void addSkin(CharDef charDef)
	{
		CharSkin charSkin = goPlayer.GetComponent<CharSkin>();
		if (charSkin == null)
		{
			charSkin = goPlayer.AddComponent<CharSkin>();
		}
		charSkin.GetReferences(charDef);
		charSkin.matName = charDef.matName;
		charSkin.SetSkin(CharSkinType.NORMAL);
	}

	private static Transform getTransformByName(GameObject goPlayer, string boneName)
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
		return null;
	}

	private static Rigidbody addRigidBody(Transform tranf, float mass)
	{
		Rigidbody rigidbody = tranf.gameObject.GetComponent<Rigidbody>();
		if (rigidbody == null)
		{
			rigidbody = tranf.gameObject.AddComponent<Rigidbody>();
		}
		rigidbody.isKinematic = true;
		rigidbody.mass = mass;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		return rigidbody;
	}

	private static CharacterJoint addJoint(Transform transfHost, Transform transfJointed, Vector3 axis, Vector3 swingAxis, float lowLimit, float highLimit, float swingLimit, float breakForce)
	{
		CharacterJoint characterJoint = transfHost.gameObject.GetComponent<CharacterJoint>();
		if (characterJoint == null)
		{
			characterJoint = transfHost.gameObject.AddComponent<CharacterJoint>();
		}
		characterJoint.connectedBody = transfJointed.GetComponent<Rigidbody>();
		characterJoint.axis = axis;
		characterJoint.swingAxis = swingAxis;
		characterJoint.lowTwistLimit = new SoftJointLimit
		{
			limit = lowLimit
		};
		characterJoint.highTwistLimit = new SoftJointLimit
		{
			limit = highLimit
		};
		characterJoint.swing1Limit = new SoftJointLimit
		{
			limit = swingLimit
		};
		characterJoint.breakForce = breakForce;
		characterJoint.breakTorque = breakForce;
		if (breakForce != float.PositiveInfinity && transfHost.gameObject.GetComponent<CharOnJointBreak>() == null)
		{
			transfHost.gameObject.AddComponent<CharOnJointBreak>();
		}
		return characterJoint;
	}

	private static void addBoxCollider(Transform transf, Vector3 center, Vector3 size)
	{
		BoxCollider boxCollider = transf.gameObject.GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = transf.gameObject.AddComponent<BoxCollider>();
		}
		boxCollider.center = center;
		boxCollider.size = size;
	}

	private static void addCapsuleCollider(Transform transf, Vector3 center, float radius, float height, int direction)
	{
		CapsuleCollider capsuleCollider = transf.gameObject.GetComponent<CapsuleCollider>();
		if (capsuleCollider == null)
		{
			capsuleCollider = transf.gameObject.AddComponent<CapsuleCollider>();
		}
		capsuleCollider.center = center;
		capsuleCollider.radius = radius;
		capsuleCollider.height = height;
		capsuleCollider.direction = direction;
	}

	private static void addSphereCollider(Transform transf, Vector3 center, float radius)
	{
		SphereCollider sphereCollider = transf.gameObject.GetComponent<SphereCollider>();
		if (sphereCollider == null)
		{
			sphereCollider = transf.gameObject.AddComponent<SphereCollider>();
		}
		sphereCollider.center = center;
		sphereCollider.radius = radius;
	}

	private static void addCharacterController()
	{
		CharacterController characterController = goPlayer.GetComponent<CharacterController>();
		if (characterController == null)
		{
			characterController = goPlayer.AddComponent<CharacterController>();
		}
		characterController.stepOffset = 1f;
		characterController.height = 2.1f;
		characterController.radius = 0.33f;
		characterController.center = new Vector3(0f, 1.1f, 0f);
	}

	private static void addOtherBehaviours()
	{
		CharStateMachine component = goPlayer.GetComponent<CharStateMachine>();
		if (component == null)
		{
			component = goPlayer.AddComponent<CharStateMachine>();
		}
		CharIgnoreColliderDef component2 = goPlayer.GetComponent<CharIgnoreColliderDef>();
		if (component2 == null)
		{
			component2 = goPlayer.AddComponent<CharIgnoreColliderDef>();
		}
		CharBloodPS component3 = goPlayer.GetComponent<CharBloodPS>();
		if (component3 == null)
		{
			component3 = goPlayer.AddComponent<CharBloodPS>();
		}
		CharProps component4 = goPlayer.GetComponent<CharProps>();
		if (component4 == null)
		{
			component4 = goPlayer.AddComponent<CharProps>();
		}
		CharEffects component5 = goPlayer.GetComponent<CharEffects>();
		if (component5 == null)
		{
			component5 = goPlayer.AddComponent<CharEffects>();
		}
		CharBloodSplat component6 = goPlayer.GetComponent<CharBloodSplat>();
		if (component6 == null)
		{
			component6 = goPlayer.AddComponent<CharBloodSplat>();
			component6.characterMaterial = characterMaterial;
		}
	}

	private static void locateAttach(GameObject attach, Material mat)
	{
		string[] array = attach.name.Split('_');
		if (array.Length == 2)
		{
			string text = array[1];
			Transform transformByName = getTransformByName(goPlayer, text);
			if (transformByName == null)
			{
				Debug.LogWarning(string.Format("Cannot find the transform {0} in the character.", text));
			}
			GameObject gameObject = Object.Instantiate(attach) as GameObject;
			gameObject.transform.parent = transformByName;
			gameObject.name = array[0];
			gameObject.transform.position = attach.transform.position;
			gameObject.transform.rotation = attach.transform.localRotation;
			gameObject.transform.localScale = attach.transform.localScale;
			gameObject.GetComponent<Renderer>().sharedMaterial = mat;
		}
	}

	private static void addAttachments(CharDef charDef)
	{
		for (int i = 0; i < charDef.parts.Count; i++)
		{
			GameObject gameObject = Resources.Load(string.Format("Characters/Prefabs/{0}", charDef.parts[i]), typeof(GameObject)) as GameObject;
			if (gameObject == null)
			{
				Debug.Log(string.Format("Cannot find attachment: {0}", charDef.parts[i]));
			}
			else
			{
				locateAttach(gameObject, characterMaterial);
			}
		}
	}

	private static void addSkinned(CharDef charDef)
	{
		for (int i = 0; i < charDef.skinned.Count; i++)
		{
			GameObject gameObject = Resources.Load(string.Format("Characters/Prefabs/{0}", charDef.skinned[i]), typeof(GameObject)) as GameObject;
			if (gameObject == null)
			{
				Debug.Log(string.Format("Cannot find skinned part: {0}", charDef.skinned[i]));
			}
			else
			{
				locateAttach(gameObject, characterMaterialGore);
			}
		}
	}

	private static void addCharred(CharDef charDef)
	{
		for (int i = 0; i < charDef.charred.Count; i++)
		{
			GameObject gameObject = Resources.Load(string.Format("Characters/Prefabs/{0}", charDef.charred[i]), typeof(GameObject)) as GameObject;
			if (gameObject == null)
			{
				Debug.Log(string.Format("Cannot find charred part: {0}", charDef.charred[i]));
			}
			else
			{
				locateAttach(gameObject, characterMaterialGore);
			}
		}
	}

	private static void addBones(CharDef charDef)
	{
		for (int i = 0; i < charDef.bones.Count; i++)
		{
			GameObject gameObject = Resources.Load(string.Format("Characters/Prefabs/{0}", charDef.bones[i]), typeof(GameObject)) as GameObject;
			if (gameObject == null)
			{
				Debug.Log(string.Format("Cannot find bones part: {0}", charDef.bones[i]));
			}
			else
			{
				locateAttach(gameObject, characterMaterialGore);
			}
		}
	}

	private static void addGore(CharDef charDef)
	{
		for (int i = 0; i < charDef.gore.Count; i++)
		{
			GameObject gameObject = Resources.Load(string.Format("Characters/Prefabs/{0}", charDef.gore[i]), typeof(GameObject)) as GameObject;
			if (gameObject == null)
			{
				Debug.Log(string.Format("Cannot find gore part: {0}", charDef.gore[i]));
			}
			else
			{
				locateAttach(gameObject, characterMaterialGore);
			}
		}
	}

	private static void hideParts(CharDef charDef)
	{
		for (int i = 0; i < charDef.hideParts.Count; i++)
		{
			Transform transformByName = CharHelper.GetTransformByName(charDef.hideParts[i]);
			if (transformByName != null)
			{
				Object.DestroyImmediate(transformByName.gameObject);
			}
		}
	}

	public static void RebuildChar()
	{
		goPlayer = CharHelper.GetPlayerTransform().gameObject;
		setSkeletonParenting();
		setMaterial();
		addRagdoll();
		CharHelper.GetCharSkin().SetSkin(CharSkinType.NORMAL, true);
	}

	public static void RebuildChar2()
	{
		addRagdoll();
	}

	private static void addBloodSpurrPoints()
	{
		GameObject gameObject = new GameObject("bloodSpurrNeck");
		gameObject.transform.parent = torso1;
		gameObject.transform.localPosition = new Vector3(-0.4f, 0f, 0f);
		gameObject.AddComponent<BloodSpurrPoint>();
		GameObject gameObject2 = new GameObject("bloodSpurrLeftArm");
		gameObject2.transform.parent = torso1;
		gameObject2.transform.localPosition = new Vector3(-0.3f, -0.26f, 0f);
		gameObject2.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 170f));
		gameObject2.AddComponent<BloodSpurrPoint>();
		GameObject gameObject3 = new GameObject("bloodSpurrRightArm");
		gameObject3.transform.parent = torso1;
		gameObject3.transform.localPosition = new Vector3(-0.3f, 0.26f, 0f);
		gameObject3.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 10f));
		gameObject3.AddComponent<BloodSpurrPoint>();
		GameObject gameObject4 = new GameObject("bloodSpurrTorso1");
		gameObject4.transform.parent = torso1;
		gameObject4.transform.localPosition = new Vector3(0.13f, 0f, 0f);
		gameObject4.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 270f));
		gameObject4.AddComponent<BloodSpurrPoint>();
		GameObject gameObject5 = new GameObject("bloodSpurrLeftLeg");
		gameObject5.transform.parent = torso2;
		gameObject5.transform.localPosition = new Vector3(-0.31f, -0.16f, 0f);
		gameObject5.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
		gameObject5.AddComponent<BloodSpurrPoint>();
		GameObject gameObject6 = new GameObject("bloodSpurrRightLeg");
		gameObject6.transform.parent = torso2;
		gameObject6.transform.localPosition = new Vector3(-0.31f, 0.16f, 0f);
		gameObject6.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
		gameObject6.AddComponent<BloodSpurrPoint>();
		GameObject gameObject7 = new GameObject("bloodSpurrLeftLeg1");
		gameObject7.transform.parent = torso2;
		gameObject7.transform.localPosition = new Vector3(-0.17f, -0.16f, 0f);
		gameObject7.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
		gameObject7.AddComponent<BloodSpurrPoint>();
		GameObject gameObject8 = new GameObject("bloodSpurrRightLeg1");
		gameObject8.transform.parent = torso2;
		gameObject8.transform.localPosition = new Vector3(-0.17f, 0.16f, 0f);
		gameObject8.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
		gameObject8.AddComponent<BloodSpurrPoint>();
	}

	private static void replaceHeadWithAnimatedHead()
	{
	}
}
