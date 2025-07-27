using UnityEngine;

public class SkeletonBuilderHelper
{
	private static GameObject goSkeleton;

	private static Transform leftLeg1;

	private static Transform leftLeg2;

	private static Transform leftFoot;

	private static Transform rightLeg1;

	private static Transform rightLeg2;

	private static Transform rightFoot;

	private static Transform torso1;

	private static Transform torso2;

	private static Transform head;

	private static Transform leftArm1;

	private static Transform leftHand;

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

	public static void BuildChar()
	{
		GameObject gameObject = Resources.Load(string.Format("Decorations/Skeleton/Skeleton"), typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError(string.Format("Cannot find the skeleton prefab"));
			return;
		}
		goSkeleton = Object.Instantiate(gameObject) as GameObject;
		goSkeleton.name = gameObject.name;
		characterMaterial = Resources.Load(string.Format("Characters/Materials/FredGore"), typeof(Material)) as Material;
		relocateSkeleton();
		setMaterial();
		addRagdoll();
	}

	private static void relocateSkeleton()
	{
		leftLeg1 = getTransformByName(goSkeleton, "leftLeg1");
		leftLeg2 = getTransformByName(goSkeleton, "leftLeg2");
		leftFoot = getTransformByName(goSkeleton, "leftFoot");
		rightLeg1 = getTransformByName(goSkeleton, "rightLeg1");
		rightLeg2 = getTransformByName(goSkeleton, "rightLeg2");
		rightFoot = getTransformByName(goSkeleton, "rightFoot");
		torso1 = getTransformByName(goSkeleton, "torso1");
		torso2 = getTransformByName(goSkeleton, "torso2");
		head = getTransformByName(goSkeleton, "head");
		leftArm1 = getTransformByName(goSkeleton, "leftArm1");
		leftHand = getTransformByName(goSkeleton, "leftHand");
		rightArm1 = getTransformByName(goSkeleton, "rightArm1");
		rightHand = getTransformByName(goSkeleton, "rightHand");
		relocateMeshes();
		meshLeftHand.parent = leftHand;
		meshRightHand.parent = rightHand;
		meshLeftArm.parent = leftArm1;
		meshRightArm.parent = rightArm1;
		meshTorso.parent = torso1;
		meshHips.parent = torso2;
		meshLeftLeg1.parent = leftLeg1;
		meshLeftLeg2.parent = leftLeg2;
		meshLeftFoot.parent = leftFoot;
		meshRightLeg1.parent = rightLeg1;
		meshRightLeg2.parent = rightLeg2;
		meshRightFoot.parent = rightFoot;
		meshHead.parent = head;
	}

	private static void relocateMeshes()
	{
		meshHead = getTransformByName(goSkeleton, "meshHead");
		meshHips = getTransformByName(goSkeleton, "meshHips");
		meshLeftArm = getTransformByName(goSkeleton, "meshLeftArm1");
		meshLeftFoot = getTransformByName(goSkeleton, "meshLeftFoot");
		meshLeftHand = getTransformByName(goSkeleton, "meshLeftHand");
		meshLeftLeg1 = getTransformByName(goSkeleton, "meshLeftLeg1");
		meshLeftLeg2 = getTransformByName(goSkeleton, "meshLeftLeg2");
		meshRightArm = getTransformByName(goSkeleton, "meshRightArm1");
		meshRightFoot = getTransformByName(goSkeleton, "meshRightFoot");
		meshRightHand = getTransformByName(goSkeleton, "meshRightHand");
		meshRightLeg1 = getTransformByName(goSkeleton, "meshRightLeg1");
		meshRightLeg2 = getTransformByName(goSkeleton, "meshRightLeg2");
		meshTorso = getTransformByName(goSkeleton, "meshTorso");
	}

	private static void setMaterial()
	{
		meshHead.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshHips.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshLeftArm.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshLeftFoot.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshLeftHand.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshLeftLeg1.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshLeftLeg2.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshRightArm.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshRightFoot.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshRightHand.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshRightLeg1.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshRightLeg2.GetComponent<Renderer>().sharedMaterial = characterMaterial;
		meshTorso.GetComponent<Renderer>().sharedMaterial = characterMaterial;
	}

	public static void SetMaterial(Material mat)
	{
		if (goSkeleton == null)
		{
			goSkeleton = CharHelper.GetPlayer();
		}
		if (meshHead == null)
		{
			relocateMeshes();
		}
		meshHead.GetComponent<Renderer>().sharedMaterial = mat;
		meshHips.GetComponent<Renderer>().sharedMaterial = mat;
		meshLeftArm.GetComponent<Renderer>().sharedMaterial = mat;
		meshLeftFoot.GetComponent<Renderer>().sharedMaterial = mat;
		meshLeftHand.GetComponent<Renderer>().sharedMaterial = mat;
		meshLeftLeg1.GetComponent<Renderer>().sharedMaterial = mat;
		meshLeftLeg2.GetComponent<Renderer>().sharedMaterial = mat;
		meshRightArm.GetComponent<Renderer>().sharedMaterial = mat;
		meshRightFoot.GetComponent<Renderer>().sharedMaterial = mat;
		meshRightHand.GetComponent<Renderer>().sharedMaterial = mat;
		meshRightLeg1.GetComponent<Renderer>().sharedMaterial = mat;
		meshRightLeg2.GetComponent<Renderer>().sharedMaterial = mat;
		meshTorso.GetComponent<Renderer>().sharedMaterial = mat;
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
		addJoint(torso1, torso2, new Vector3(0f, 1f, 0f), new Vector3(-1f, 0f, 0f), -20f, 20f, 10f, 15f);
		addJoint(head, torso1, new Vector3(1f, 1f, 0f), new Vector3(1f, 0f, 0f), -40f, 25f, 25f, 55f);
		addJoint(rightArm1, torso1, new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), -70f, 10f, 50f, 15f);
		addJoint(rightHand, rightArm1, new Vector3(0f, 0f, -1f), new Vector3(1f, 1f, 0f), -90f, 0f, 0f, 15f);
		addJoint(leftArm1, torso1, new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), -70f, 10f, 50f, 15f);
		addJoint(leftHand, leftArm1, new Vector3(0f, 0f, -1f), new Vector3(1f, 1f, 0f), -90f, 0f, 0f, 15f);
		addJoint(rightLeg1, torso2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -20f, 70f, 30f, 15f);
		addJoint(rightLeg2, rightLeg1, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, 15f);
		addJoint(rightFoot, rightLeg2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, 15f);
		addJoint(leftLeg1, torso2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -20f, 70f, 30f, 15f);
		addJoint(leftLeg2, leftLeg1, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, 15f);
		addJoint(leftFoot, leftLeg2, new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f), -80f, 0f, 0f, 15f);
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

	private static Transform getTransformByName(GameObject goSkeleton, string boneName)
	{
		Transform[] componentsInChildren = goSkeleton.GetComponentsInChildren<Transform>();
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

	private static Rigidbody addRigidBody(Transform tranf, float mass)
	{
		Rigidbody rigidbody = tranf.gameObject.GetComponent<Rigidbody>();
		if (rigidbody == null)
		{
			rigidbody = tranf.gameObject.AddComponent<Rigidbody>();
		}
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
}
