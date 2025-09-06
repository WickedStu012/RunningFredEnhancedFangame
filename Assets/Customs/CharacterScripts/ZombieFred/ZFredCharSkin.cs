using System.Collections.Generic;
using UnityEngine;

namespace ZFred
{
	
}

public class ZFredCharSkin : MonoBehaviour
{
	public CharSkinType defSkin;

	public List<GameObject> normal;

	public List<GameObject> skinned;

	public List<GameObject> charred;

	public List<GameObject> bones;

	public List<GameObject> gore;

	public string matName;

	public GameObject bloodDamagePrefab;

	public GameObject bloodSpurrPrefab;

	private List<Renderer> blinkObjs;

	private CharSkinType curSkin;

	private float timeToBlink;

	private float accumTimeBlinking;

	private float accumTime;

	private bool isBlinking;

	public Dictionary<string, GameObject> normalDic;

	public Dictionary<string, GameObject> goreDic;

	private GameObject bloodDmg;

	private GameObject bloodDmg2;

	private GameObject bloodDmg3;

	private GameObject bloodSpurr;

	private ParticleSystem[] particles;

	private bool deactivateParticles;

	private float accumTimePartDeactivation;

	private Vector3[] speedAtStartOfDeactivation;

	private Wings wings;

	private Jetpack jetpack;

	private void Awake()
	{
		isBlinking = false;
		normalDic = new Dictionary<string, GameObject>();
		for (int i = 0; i < normal.Count; i++)
		{
			if (normal[i] != null && !normalDic.ContainsKey(normal[i].name))
			{
				normalDic.Add(normal[i].name, normal[i]);
			}
		}
		goreDic = new Dictionary<string, GameObject>();
		for (int j = 0; j < gore.Count; j++)
		{
			if (!goreDic.ContainsKey(gore[j].name))
			{
				goreDic.Add(gore[j].name, gore[j]);
			}
		}
		setEnableNormal(defSkin == CharSkinType.NORMAL);
		setEnableSkinned(defSkin == CharSkinType.SKINNED);
		setEnableCharred(defSkin == CharSkinType.CHARRED);
		setEnableBones(defSkin == CharSkinType.BONES);
		setEnableGore(false);
		bloodDamagePrefab = Resources.Load("Prefabs/BloodDamage", typeof(GameObject)) as GameObject;
		bloodDmg = Object.Instantiate(bloodDamagePrefab) as GameObject;
		bloodDmg.SetActive(false);
		bloodDmg2 = Object.Instantiate(bloodDamagePrefab) as GameObject;
		bloodDmg2.SetActive(false);
		bloodDmg3 = Object.Instantiate(bloodDamagePrefab) as GameObject;
		bloodDmg3.SetActive(false);
		bloodSpurrPrefab = Resources.Load("Prefabs/BloodSpurrParticle", typeof(GameObject)) as GameObject;
		bloodSpurr = Object.Instantiate(bloodSpurrPrefab) as GameObject;
		bloodSpurr.SetActive(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (isBlinking)
		{
			accumTimeBlinking += Time.deltaTime;
			accumTime += Time.deltaTime;
			if (accumTime > 0.05f)
			{
				for (int i = 0; i < blinkObjs.Count; i++)
				{
					blinkObjs[i].enabled = !blinkObjs[i].enabled;
				}
				if (ZFredCharHelper.GetProps().HasWings && wings != null && blinkObjs.Count > 0)
				{
					wings.SetVisibility(!blinkObjs[0].enabled);
				}
				if (ZFredCharHelper.GetProps().HasJetpack && jetpack != null && blinkObjs.Count > 0)
				{
					jetpack.SetVisibility(!blinkObjs[0].enabled);
				}
				accumTime = 0f;
			}
			if (accumTimeBlinking > timeToBlink)
			{
				isBlinking = false;
				for (int j = 0; j < blinkObjs.Count; j++)
				{
					blinkObjs[j].enabled = true;
				}
				if (ZFredCharHelper.GetProps().HasWings && wings != null && blinkObjs.Count > 0)
				{
					wings.SetVisibility(true);
					wings.Fold();
				}
				if (ZFredCharHelper.GetProps().HasJetpack && jetpack != null && blinkObjs.Count > 0)
				{
					jetpack.SetVisibility(true);
				}
			}
		}
		if (deactivateParticles)
		{
			accumTimePartDeactivation += Time.deltaTime;
			for (int k = 0; k < particles.Length; k++)
			{
				particles[k].Speed = speedAtStartOfDeactivation[k] * (1f - accumTimePartDeactivation);
			}
			if (accumTimePartDeactivation >= 1f)
			{
				deactivateParticles = false;
			}
		}
	}

	public void GetReferences(CharDef def)
	{
		normal = new List<GameObject>();
		skinned = new List<GameObject>();
		charred = new List<GameObject>();
		bones = new List<GameObject>();
		gore = new List<GameObject>();
		string[] partNames = getPartNames(def.parts);
		string[] partNames2 = getPartNames(def.skinned);
		string[] partNames3 = getPartNames(def.charred);
		string[] partNames4 = getPartNames(def.bones);
		string[] partNames5 = getPartNames(def.gore);
		SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			normal.Add(componentsInChildren[i].gameObject);
		}
		MeshRenderer[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<MeshRenderer>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j].name.Contains("mesh"))
			{
				normal.Add(componentsInChildren2[j].gameObject);
			}
			else if (containsString(partNames, componentsInChildren2[j].name))
			{
				normal.Add(componentsInChildren2[j].gameObject);
			}
			else if (containsString(partNames2, componentsInChildren2[j].name))
			{
				skinned.Add(componentsInChildren2[j].gameObject);
			}
			else if (containsString(partNames3, componentsInChildren2[j].name))
			{
				charred.Add(componentsInChildren2[j].gameObject);
			}
			else if (containsString(partNames4, componentsInChildren2[j].name))
			{
				bones.Add(componentsInChildren2[j].gameObject);
			}
			else if (containsString(partNames5, componentsInChildren2[j].name))
			{
				gore.Add(componentsInChildren2[j].gameObject);
			}
		}
		curSkin = CharSkinType.NORMAL;
		setEnableNormal(curSkin == CharSkinType.NORMAL);
		setEnableSkinned(curSkin == CharSkinType.SKINNED);
		setEnableCharred(curSkin == CharSkinType.CHARRED);
		setEnableBones(curSkin == CharSkinType.BONES);
		setEnableGore(false);
	}

	private static string[] getPartNames(List<string> list)
	{
		string[] array = new string[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			string fileName = FileNameUtil.GetFileName(list[i]);
			string[] array2 = fileName.Split('_');
			array[i] = array2[0];
		}
		return array;
	}

	private static bool containsString(string[] list, string item)
	{
		for (int i = 0; i < list.Length; i++)
		{
			if (string.Compare(list[i], item, true) == 0)
			{
				return true;
			}
		}
		return false;
	}

	public void SetSkin(CharSkinType skin)
	{
		SetSkin(skin, false);
	}

	public void SetSkin(CharSkinType skin, bool cleanAll)
	{
		if (cleanAll)
		{
			setEnableNormal(false);
			setEnableSkinned(false);
			setEnableCharred(false);
			setEnableBones(false);
		}
		else
		{
			switch (curSkin)
			{
			case CharSkinType.NORMAL:
				setEnableNormal(false);
				break;
			case CharSkinType.SKINNED:
				setEnableSkinned(false);
				break;
			case CharSkinType.CHARRED:
				setEnableCharred(false);
				break;
			case CharSkinType.BONES:
				setEnableBones(false);
				break;
			}
		}
		setEnableGore(false);
		curSkin = skin;
		switch (curSkin)
		{
		case CharSkinType.NORMAL:
			setEnableNormal(true);
			break;
		case CharSkinType.SKINNED:
			setEnableSkinned(true);
			break;
		case CharSkinType.CHARRED:
			setEnableCharred(true);
			break;
		case CharSkinType.BONES:
			setEnableBones(true);
			break;
		}
	}

	private void setEnableNormal(bool en)
	{
		for (int i = 0; i < normal.Count; i++)
		{
			normal[i].SetActive(en);
		}
	}

	private void setEnableSkinned(bool en)
	{
		for (int i = 0; i < skinned.Count; i++)
		{
			skinned[i].SetActive(en);
		}
	}

	private void setEnableCharred(bool en)
	{
		for (int i = 0; i < charred.Count; i++)
		{
			charred[i].SetActive(en);
		}
	}

	private void setEnableBones(bool en)
	{
		for (int i = 0; i < bones.Count; i++)
		{
			bones[i].SetActive(en);
		}
	}

	private void setEnableGore(bool en)
	{
		for (int i = 0; i < gore.Count; i++)
		{
			gore[i].SetActive(en);
		}
	}

	public void ApplyMaterialToNormal(Material mat)
	{
		for (int i = 0; i < normal.Count; i++)
		{
			normal[i].GetComponent<Renderer>().sharedMaterial = mat;
		}
	}

	public void Blink(float time)
	{
		timeToBlink = time;
		isBlinking = true;
		accumTimeBlinking = 0f;
		if (curSkin == CharSkinType.NORMAL)
		{
			blinkObjs = new List<Renderer>(normal.Count);
			for (int i = 0; i < normal.Count; i++)
			{
				blinkObjs.Add(normal[i].GetComponent<Renderer>());
			}
		}
		else if (curSkin == CharSkinType.CHARRED)
		{
			blinkObjs = new List<Renderer>(charred.Count);
			for (int j = 0; j < charred.Count; j++)
			{
				blinkObjs.Add(charred[j].GetComponent<Renderer>());
			}
		}
		else if (curSkin == CharSkinType.SKINNED)
		{
			blinkObjs = new List<Renderer>(skinned.Count);
			for (int k = 0; k < skinned.Count; k++)
			{
				blinkObjs.Add(skinned[k].GetComponent<Renderer>());
			}
		}
		else if (curSkin == CharSkinType.BONES)
		{
			blinkObjs = new List<Renderer>(bones.Count);
			for (int l = 0; l < bones.Count; l++)
			{
				blinkObjs.Add(bones[l].GetComponent<Renderer>());
			}
		}
		if (ZFredCharHelper.GetProps().HasWings && wings == null)
		{
			GameObject gameObject = ZFredCharHelper.GetCharStateMachine().GetWings();
			if (gameObject != null)
			{
				wings = gameObject.GetComponent<Wings>();
			}
		}
		if (ZFredCharHelper.GetProps().HasJetpack && jetpack == null)
		{
			GameObject gameObject2 = ZFredCharHelper.GetCharStateMachine().GetJetpack();
			if (gameObject2 != null)
			{
				jetpack = gameObject2.GetComponent<Jetpack>();
			}
		}
	}

	public bool IsBlinking()
	{
		return isBlinking;
	}

	public void SetGore(string bodyPart)
	{
		ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEGS_OFF, 0);
		normalDic["meshRightFoot"].SetActive(false);
		normalDic["meshLeftFoot"].SetActive(false);
		normalDic["meshRightLeg2"].SetActive(false);
		normalDic["meshLeftLeg2"].SetActive(false);
		normalDic["meshRightLeg1"].SetActive(false);
		normalDic["meshLeftLeg1"].SetActive(false);
		goreDic["goreRightLeg1"].SetActive(true);
		goreDic["goreLeftLeg1"].SetActive(true);
		ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
		Transform transformByName = ZFredCharHelper.GetTransformByName("leftLeg2");
		if (transformByName != null)
		{
			CharacterJoint component = transformByName.GetComponent<CharacterJoint>();
			if (component != null)
			{
				component.connectedBody = null;
				Object.Destroy(component);
			}
			transformByName.parent = null;
			transformByName.GetComponent<Rigidbody>().isKinematic = false;
			transformByName.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
		}
		Transform transformByName2 = ZFredCharHelper.GetTransformByName("rightLeg2");
		if (transformByName2 != null)
		{
			CharacterJoint component2 = transformByName2.GetComponent<CharacterJoint>();
			if (component2 != null)
			{
				component2.connectedBody = null;
				Object.Destroy(component2);
			}
			transformByName2.parent = null;
			transformByName2.GetComponent<Rigidbody>().isKinematic = false;
			transformByName2.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
		}
		Transform transformByName3 = ZFredCharHelper.GetTransformByName("torso1");
		if (transformByName3 != null)
		{
			transformByName3.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 200f, 200f));
			transformByName3.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
		}
		ExplosionManager.Fire2(base.transform.position);
		SoundManager.PlaySound(5);
	}

	public void DismemberRandom()
	{
		if (!ConfigParams.useGore)
		{
			Debug.LogWarning("DismemberRandom was called but useGore is false");
			return;
		}
		string[] array = new string[8] { "head", "leftLeg1", "rightLeg1", "torso1", "leftArm1", "rightArm1", "leftLeg2", "rightLeg2" };
		int num = Random.Range(0, array.Length);
		Dismember(array[num]);
	}

	public void Dismember(string part)
	{
		if (!ConfigParams.useGore)
		{
			return;
		}
		if (string.Compare(part, "head") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD, 10);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			goreDic["goreHead"].SetActive(true);
			Transform transformByName = ZFredCharHelper.GetTransformByName("head");
			if (transformByName != null)
			{
				CharacterJoint component = transformByName.GetComponent<CharacterJoint>();
				if (component != null)
				{
					component.connectedBody = null;
					Object.Destroy(component);
					transformByName.parent = null;
					transformByName.GetComponent<Rigidbody>().isKinematic = false;
					transformByName.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-200, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
				}
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName.position;
				PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso1"), "bloodSpurrNeck");
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
		}
		else if (string.Compare(part, "leftLeg1") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG, 10);
			goreDic["goreLeftLeg1"].SetActive(true);
			Transform transformByName2 = ZFredCharHelper.GetTransformByName("leftLeg1");
			if (transformByName2 != null)
			{
				CharacterJoint component2 = transformByName2.GetComponent<CharacterJoint>();
				if (component2 != null)
				{
					component2.connectedBody = null;
					Object.Destroy(component2);
					transformByName2.parent = null;
					transformByName2.GetComponent<Rigidbody>().isKinematic = false;
					transformByName2.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName2.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
				}
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName2.position;
				PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrLeftLeg1");
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			Transform transformByName3 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName3 != null)
			{
				transformByName3.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 200f);
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName3.position;
			}
		}
		else if (string.Compare(part, "rightLeg1") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG, 10);
			goreDic["goreRightLeg1"].SetActive(true);
			Transform transformByName4 = ZFredCharHelper.GetTransformByName("rightLeg1");
			if (transformByName4 != null)
			{
				CharacterJoint component3 = transformByName4.GetComponent<CharacterJoint>();
				if (component3 != null)
				{
					component3.connectedBody = null;
					Object.Destroy(component3);
					transformByName4.parent = null;
					transformByName4.GetComponent<Rigidbody>().isKinematic = false;
					transformByName4.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName4.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
				}
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName4.position;
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			Transform transformByName5 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName5 != null)
			{
				transformByName5.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 200f);
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName5.position;
			}
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrRightLeg1");
		}
		else if (string.Compare(part, "torso1") == 0 || string.Compare(part, "torso2") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HALF_BODY_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			goreDic["goreTorso"].SetActive(true);
			Transform transformByName6 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName6 != null)
			{
				CharacterJoint component4 = transformByName6.GetComponent<CharacterJoint>();
				if (component4 != null)
				{
					component4.connectedBody = null;
					Object.Destroy(component4);
					transformByName6.parent = null;
					transformByName6.GetComponent<Rigidbody>().isKinematic = false;
					transformByName6.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName6.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
				}
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName6.position;
				bloodSpurr.SetActive(true);
				bloodSpurr.transform.parent = transformByName6;
				bloodSpurr.transform.position = transformByName6.position;
				bloodSpurr.transform.localRotation = Quaternion.Euler(0f, 180f, 90f);
				deactivateParticles = true;
				accumTimePartDeactivation = 0f;
				particles = bloodSpurr.GetComponentsInChildren<ParticleSystem>();
				speedAtStartOfDeactivation = new Vector3[particles.Length];
				if (particles != null)
				{
					for (int i = 0; i < particles.Length; i++)
					{
						speedAtStartOfDeactivation[i] = particles[i].Speed;
					}
				}
			}
			PlaceSpurrOn(transformByName6, "bloodSpurrTorso1");
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			Transform transformByName7 = ZFredCharHelper.GetTransformByName("torso2");
			if (transformByName7 != null)
			{
				transformByName7.GetComponent<Rigidbody>().isKinematic = false;
				transformByName7.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
				transformByName7.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName7.position;
			}
		}
		else if (string.Compare(part, "leftArm1") == 0 || string.Compare(part, "leftHand") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_ARM_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_ARM, 10);
			goreDic["goreLeftArm1"].SetActive(true);
			Transform transformByName8 = ZFredCharHelper.GetTransformByName("leftArm1");
			if (transformByName8 != null)
			{
				CharacterJoint component5 = transformByName8.GetComponent<CharacterJoint>();
				if (component5 != null)
				{
					component5.connectedBody = null;
					Object.Destroy(component5);
					transformByName8.parent = null;
					transformByName8.GetComponent<Rigidbody>().isKinematic = false;
					transformByName8.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName8.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
					bloodDmg.SetActive(true);
					bloodDmg.transform.position = transformByName8.position;
				}
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			Transform transformByName9 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName9 != null)
			{
				transformByName9.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 200f);
			}
			PlaceSpurrOn(transformByName9, "bloodSpurrLeftArm");
		}
		else if (string.Compare(part, "rightArm1") == 0 || string.Compare(part, "rightHand") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_ARM_OFF, 0);
			goreDic["goreRightArm1"].SetActive(true);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_ARM, 10);
			Transform transformByName10 = ZFredCharHelper.GetTransformByName("rightArm1");
			if (transformByName10 != null)
			{
				CharacterJoint component6 = transformByName10.GetComponent<CharacterJoint>();
				if (component6 != null)
				{
					component6.connectedBody = null;
					Object.Destroy(component6);
					transformByName10.parent = null;
					transformByName10.GetComponent<Rigidbody>().isKinematic = false;
					transformByName10.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName10.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
					bloodDmg.SetActive(true);
					bloodDmg.transform.position = transformByName10.position;
				}
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			Transform transformByName11 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName11 != null)
			{
				transformByName11.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 200f);
			}
			PlaceSpurrOn(transformByName11, "bloodSpurrRightArm");
		}
		else if (string.Compare(part, "leftLeg2") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG_2_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG, 10);
			goreDic["goreLeftLeg1"].SetActive(true);
			Transform transformByName12 = ZFredCharHelper.GetTransformByName("leftLeg2");
			if (transformByName12 != null)
			{
				CharacterJoint component7 = transformByName12.GetComponent<CharacterJoint>();
				if (component7 != null)
				{
					component7.connectedBody = null;
					Object.Destroy(component7);
				}
				transformByName12.parent = null;
				transformByName12.GetComponent<Rigidbody>().isKinematic = false;
				transformByName12.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
				transformByName12.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName12.position;
			}
			Transform transformByName13 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName13 != null)
			{
				transformByName13.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 200f);
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName13.position;
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrLeftLeg");
		}
		else if (string.Compare(part, "rightLeg2") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG_2_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG, 10);
			goreDic["goreRightLeg1"].SetActive(true);
			Transform transformByName14 = ZFredCharHelper.GetTransformByName("rightLeg2");
			if (transformByName14 != null)
			{
				CharacterJoint component8 = transformByName14.GetComponent<CharacterJoint>();
				if (component8 != null)
				{
					component8.connectedBody = null;
					Object.Destroy(component8);
					transformByName14.parent = null;
					transformByName14.GetComponent<Rigidbody>().isKinematic = false;
					transformByName14.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200)));
					transformByName14.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
					bloodDmg.SetActive(true);
					bloodDmg.transform.position = transformByName14.position;
				}
			}
			ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
			Transform transformByName15 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName15 != null)
			{
				transformByName15.GetComponent<Rigidbody>().AddForce((Vector3.back + Vector3.up) * 200f);
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName15.position;
			}
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrRightLeg");
		}
		if (Random.Range(0, 2) == 0)
		{
			SoundManager.PlaySound(base.transform.position, 15);
		}
		else
		{
			SoundManager.PlaySound(base.transform.position, 56);
		}
	}

	public void PlaceSpurrOn(Transform basePart, string subPartName)
	{
		if (basePart == null)
		{
			return;
		}
		BloodSpurrPoint bloodSpurrPoint = null;
		BloodSpurrPoint[] componentsInChildren = basePart.GetComponentsInChildren<BloodSpurrPoint>();
		if (componentsInChildren == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (string.Compare(componentsInChildren[i].name, subPartName, true) == 0)
			{
				bloodSpurrPoint = componentsInChildren[i];
				break;
			}
		}
		if (bloodSpurrPoint != null)
		{
			bloodSpurr.SetActive(true);
			bloodSpurr.transform.position = bloodSpurrPoint.transform.position;
			bloodSpurr.transform.rotation = bloodSpurrPoint.transform.rotation;
			bloodSpurr.transform.parent = bloodSpurrPoint.transform;
			ParticleSystem[] componentsInChildren2 = bloodSpurr.GetComponentsInChildren<ParticleSystem>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].Emit = true;
			}
			SoundManager.PlaySound(33);
		}
	}

	public void PlaceSpurrOn(Transform parent, Vector3 pos, Quaternion rot)
	{
		bloodSpurr.SetActive(true);
		bloodSpurr.transform.position = pos;
		bloodSpurr.transform.rotation = rot;
		bloodSpurr.transform.parent = parent;
		ParticleSystem[] componentsInChildren = bloodSpurr.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
		SoundManager.PlaySound(33);
	}

	public void ClearBloodSpurrs()
	{
		bloodSpurr.SetActive(false);
	}

	public void Pierced(Vector3 piercePoint)
	{
		GameEventDispatcher.Dispatch(null, new PlayerWasPierced());
		ZFredCharHelper.GetCharStateMachine().SwitchTo(ActionCode.RAGDOLL);
		if (ConfigParams.useGore)
		{
			CharBloodPS component = GetComponent<CharBloodPS>();
			component.StartPoolCreationOn("torso1");
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso1"), piercePoint, Quaternion.identity);
		}
		SoundManager.PlaySound(6);
		ScreenShaker.Shake(0.5f, 8f);
	}

	public void ApplyDamageTo(string part)
	{
		if (!ConfigParams.useGore)
		{
			return;
		}
		if (string.Compare(part, "head") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HEAD, 10);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			goreDic["goreHead"].SetActive(true);
			Transform transformByName = ZFredCharHelper.GetTransformByName("head");
			if (transformByName != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName.position;
				PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso1"), "bloodSpurrNeck");
			}
		}
		else if (string.Compare(part, "leftLeg1") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG, 10);
			goreDic["goreLeftLeg1"].SetActive(true);
			Transform transformByName2 = ZFredCharHelper.GetTransformByName("leftLeg1");
			if (transformByName2 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName2.position;
				PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrLeftLeg1");
			}
			Transform transformByName3 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName3 != null)
			{
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName3.position;
			}
		}
		else if (string.Compare(part, "rightLeg1") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG, 10);
			goreDic["goreRightLeg1"].SetActive(true);
			Transform transformByName4 = ZFredCharHelper.GetTransformByName("rightLeg1");
			if (transformByName4 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName4.position;
			}
			Transform transformByName5 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName5 != null)
			{
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName5.position;
			}
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrRightLeg1");
		}
		else if (string.Compare(part, "torso1") == 0 || string.Compare(part, "torso2") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.HALF_BODY_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			goreDic["goreTorso"].SetActive(true);
			Transform transformByName6 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName6 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName6.position;
				bloodSpurr.SetActive(true);
				bloodSpurr.transform.parent = transformByName6;
				bloodSpurr.transform.position = transformByName6.position;
				bloodSpurr.transform.localRotation = Quaternion.Euler(0f, 180f, 90f);
				deactivateParticles = true;
				accumTimePartDeactivation = 0f;
				particles = bloodSpurr.GetComponentsInChildren<ParticleSystem>();
				speedAtStartOfDeactivation = new Vector3[particles.Length];
				for (int i = 0; i < particles.Length; i++)
				{
					speedAtStartOfDeactivation[i] = particles[i].Speed;
				}
			}
			Transform transformByName7 = ZFredCharHelper.GetTransformByName("torso2");
			if (transformByName7 != null)
			{
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName7.position;
			}
			PlaceSpurrOn(transformByName6, "bloodSpurrTorso1");
		}
		else if (string.Compare(part, "leftArm1") == 0 || string.Compare(part, "leftHand") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_ARM_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_ARM, 10);
			goreDic["goreLeftArm1"].SetActive(true);
			Transform transformByName8 = ZFredCharHelper.GetTransformByName("leftArm1");
			if (transformByName8 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName8.position;
			}
			Transform transformByName9 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName9 != null)
			{
				PlaceSpurrOn(transformByName9, "bloodSpurrLeftArm");
			}
		}
		else if (string.Compare(part, "rightArm1") == 0 || string.Compare(part, "rightHand") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_ARM_OFF, 0);
			goreDic["goreRightArm1"].SetActive(true);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.TORSO, 10);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_ARM, 10);
			Transform transformByName10 = ZFredCharHelper.GetTransformByName("rightArm1");
			if (transformByName10 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName10.position;
			}
			Transform transformByName11 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName11 != null)
			{
				PlaceSpurrOn(transformByName11, "bloodSpurrRightArm");
			}
		}
		else if (string.Compare(part, "leftLeg2") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG_2_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.LEFT_LEG, 10);
			goreDic["goreLeftLeg1"].SetActive(true);
			Transform transformByName12 = ZFredCharHelper.GetTransformByName("leftLeg2");
			if (transformByName12 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName12.position;
			}
			Transform transformByName13 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName13 != null)
			{
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName13.position;
			}
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrLeftLeg");
		}
		else if (string.Compare(part, "rightLeg2") == 0)
		{
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG_2_OFF, 0);
			ZFredCharHelper.GetCharBloodSplat().SplatOn(BodyDamage.RIGHT_LEG, 10);
			goreDic["goreRightLeg1"].SetActive(true);
			Transform transformByName14 = ZFredCharHelper.GetTransformByName("rightLeg2");
			if (transformByName14 != null)
			{
				bloodDmg.SetActive(true);
				bloodDmg.transform.position = transformByName14.position;
			}
			Transform transformByName15 = ZFredCharHelper.GetTransformByName("torso1");
			if (transformByName15 != null)
			{
				bloodDmg2.SetActive(true);
				bloodDmg2.transform.position = transformByName15.position;
			}
			PlaceSpurrOn(ZFredCharHelper.GetTransformByName("torso2"), "bloodSpurrRightLeg");
		}
		if (Random.Range(0, 2) == 0)
		{
			SoundManager.PlaySound(base.transform.position, 15);
		}
		else
		{
			SoundManager.PlaySound(base.transform.position, 56);
		}
	}
}
