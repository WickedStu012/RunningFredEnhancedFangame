using UnityEngine;

public class CharIgnoreColliderDef : MonoBehaviour
{
	private void Awake()
	{
		getTransformByName(base.gameObject, "leftLeg");
		Transform transformByName = getTransformByName(base.gameObject, "leftLeg1");
		Transform transformByName2 = getTransformByName(base.gameObject, "leftLeg2");
		Transform transformByName3 = getTransformByName(base.gameObject, "leftFoot");
		getTransformByName(base.gameObject, "rightLeg");
		Transform transformByName4 = getTransformByName(base.gameObject, "rightLeg1");
		Transform transformByName5 = getTransformByName(base.gameObject, "rightLeg2");
		Transform transformByName6 = getTransformByName(base.gameObject, "rightFoot");
		Transform transformByName7 = getTransformByName(base.gameObject, "torso1");
		Transform transformByName8 = getTransformByName(base.gameObject, "torso2");
		Transform transformByName9 = getTransformByName(base.gameObject, "head");
		getTransformByName(base.gameObject, "leftArm");
		Transform transformByName10 = getTransformByName(base.gameObject, "leftArm1");
		Transform transformByName11 = getTransformByName(base.gameObject, "leftHand");
		getTransformByName(base.gameObject, "rightArm");
		Transform transformByName12 = getTransformByName(base.gameObject, "rightArm1");
		Transform transformByName13 = getTransformByName(base.gameObject, "rightHand");
		Physics.IgnoreCollision(transformByName3.GetComponent<Collider>(), transformByName2.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName2.GetComponent<Collider>(), transformByName.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName.GetComponent<Collider>(), transformByName8.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName6.GetComponent<Collider>(), transformByName5.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName5.GetComponent<Collider>(), transformByName4.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName4.GetComponent<Collider>(), transformByName8.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName7.GetComponent<Collider>(), transformByName8.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName11.GetComponent<Collider>(), transformByName10.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName13.GetComponent<Collider>(), transformByName12.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName2.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName3.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName4.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName5.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName6.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName7.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName8.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		if (transformByName9.GetComponent<Collider>() != null)
		{
			Physics.IgnoreCollision(transformByName9.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		}
		Physics.IgnoreCollision(transformByName10.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName11.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName12.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
		Physics.IgnoreCollision(transformByName13.GetComponent<Collider>(), base.gameObject.GetComponent<Collider>());
	}

	private Transform getTransformByName(GameObject goPlayer, string boneName)
	{
		Transform[] componentsInChildren = goPlayer.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(boneName, componentsInChildren[i].name, true) == 0)
			{
				return componentsInChildren[i];
			}
		}
		Debug.Log(string.Format("Cannot find the bone {0}", boneName));
		return null;
	}
}
