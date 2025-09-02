using System.Collections.Generic;
using UnityEngine;

public class CharBloodSplat : MonoBehaviour
{
	private const string GORE_TEX_NAME = "CharacterDamage";

	public Material characterMaterial;

	private Texture2D mainTex;

	private Texture2D goreTex;

	private FredCharacterDamage goreData;

	private bool generateDyn = true;

	private int headDamageLevel;

	private int torsoDamageLevel;

	private int leftArmDamageLevel;

	private int rightArmDamageLevel;

	private int leftLegDamageLevel;

	private int rightLegDamageLevel;

	private Dictionary<string, bool> damageApplied = new Dictionary<string, bool>();

	public Material newCharMaterial;
	
	private void Start()
	{
		string matName = GetComponent<CharSkin>().matName;
		Material source = CharBuilderHelper.LoadMaterial(matName);
		mainTex = new Texture2D(characterMaterial.mainTexture.width, characterMaterial.mainTexture.height, TextureFormat.RGBA32, false);
		mainTex.SetPixels((characterMaterial.mainTexture as Texture2D).GetPixels());
		mainTex.Apply(false);
		newCharMaterial = new Material(source);
		newCharMaterial.name = matName;
		newCharMaterial.mainTexture = mainTex;
		CharBuilderHelper.SetMaterial(newCharMaterial);
		if (generateDyn)
		{
			goreTex = getGoreTex("CharacterDamage");
		}
		else
		{
			goreTex = Resources.Load(string.Format("Gore/Textures/{0}-{1}", matName, "CharacterDamage"), typeof(Texture2D)) as Texture2D;
		}
		Debug.Log(string.Format("Gore texture loaded: {0}x{1}", goreTex.width, goreTex.height));
		// Pass the target texture dimensions (1024x1024) so FredCharacterDamage knows what to scale TO
		// The coordinates in CharacterDamage.txt are for 256x256, but we want them scaled to 1024x1024
		goreData = new FredCharacterDamage(goreTex.width, goreTex.height);
		
		// Debug: Show what parts are available in the dictionary
		Debug.Log("Available gore parts: " + string.Join(", ", goreData.goreTexData.Keys));
	}

	private void OnDesroy()
	{
		if (newCharMaterial != null)
		{
			Object.Destroy(newCharMaterial);
		}
		if (mainTex != null)
		{
			Object.Destroy(mainTex);
		}
	}

	public void SetCharacterMaterialToHead(GameObject head)
	{
		SkinnedMeshRenderer componentInChildren = head.GetComponentInChildren<SkinnedMeshRenderer>();
		if (componentInChildren != null)
		{
			componentInChildren.GetComponent<Renderer>().sharedMaterial = newCharMaterial;
		}
	}

	public void ResetDamage()
	{
		headDamageLevel = 0;
		torsoDamageLevel = 0;
		leftArmDamageLevel = 0;
		rightArmDamageLevel = 0;
		leftLegDamageLevel = 0;
		rightLegDamageLevel = 0;
		damageApplied.Clear();
		mainTex.SetPixels((characterMaterial.mainTexture as Texture2D).GetPixels());
		mainTex.Apply(false);
		newCharMaterial.mainTexture = mainTex;
		CharBuilderHelper.SetMaterial(newCharMaterial);
		GameObject headGameObject = CharHeadHelper.GetHeadGameObject();
		if (headGameObject != null)
		{
			SkinnedMeshRenderer[] componentsInChildren = headGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sharedMaterial = newCharMaterial;
			}
		}
	}

	public void SplatOn(BodyDamage bp, int damageLevel)
	{
		BloodSplatManager.Instance.Create(Random.Range(25, 40));
		switch (bp)
		{
		case BodyDamage.HEAD:
			headDamageLevel = Mathf.Clamp(headDamageLevel + damageLevel, 0, 4);
			applyDamageTo(bp, "eye1", headDamageLevel, 1);
			applyDamageTo(bp, "eye2", headDamageLevel, 1);
			applyDamageTo(bp, "nose", headDamageLevel, 1);
			applyDamageTo(bp, "mouth1", headDamageLevel, 2);
			applyDamageTo(bp, "mouth2", headDamageLevel, 2);
			applyDamageTo(bp, "mouth3", headDamageLevel, 2);
			applyDamageTo(bp, "mouth4", headDamageLevel, 3);
			applyDamageTo(bp, "backeyes", headDamageLevel, 3);
			applyDamageTo(bp, "rhead1", headDamageLevel, 3);
			applyDamageTo(bp, "rhead2", headDamageLevel, 3);
			applyDamageTo(bp, "rhead3", headDamageLevel, 4);
			applyDamageTo(bp, "lhead1", headDamageLevel, 4);
			applyDamageTo(bp, "lhead2", headDamageLevel, 4);
			applyDamageTo(bp, "lhead3", headDamageLevel, 4);
			mainTex.Apply();
			break;
		case BodyDamage.TORSO:
			torsoDamageLevel = Mathf.Clamp(torsoDamageLevel + damageLevel, 0, 10);
			applyDamageTo(bp, "chest", torsoDamageLevel, 2);
			applyDamageTo(bp, "back", torsoDamageLevel, 4);
			applyDamageTo(bp, "side1", torsoDamageLevel, 8);
			applyDamageTo(bp, "side2", torsoDamageLevel, 10);
			mainTex.Apply();
			break;
		case BodyDamage.LEFT_ARM:
			leftArmDamageLevel = Mathf.Clamp(leftArmDamageLevel + damageLevel, 0, 10);
			applyDamageTo(bp, "larm2", leftArmDamageLevel, 1);
			applyDamageTo(bp, "larm4", leftArmDamageLevel, 3);
			applyDamageTo(bp, "larm5", leftArmDamageLevel, 5);
			mainTex.Apply();
			break;
		case BodyDamage.RIGHT_ARM:
			rightArmDamageLevel = Mathf.Clamp(rightArmDamageLevel + damageLevel, 0, 10);
			applyDamageTo(bp, "rarm2", rightArmDamageLevel, 1);
			applyDamageTo(bp, "rarm4", rightArmDamageLevel, 3);
			applyDamageTo(bp, "rarm5", rightArmDamageLevel, 5);
			mainTex.Apply();
			break;
		case BodyDamage.RIGHT_LEG:
			rightLegDamageLevel = Mathf.Clamp(rightLegDamageLevel + damageLevel, 0, 10);
			applyDamageTo(bp, "rleg3", rightLegDamageLevel, 1);
			applyDamageTo(bp, "rleg2", rightLegDamageLevel, 3);
			applyDamageTo(bp, "rleg4", rightLegDamageLevel, 4);
			applyDamageTo(bp, "rleg5", rightLegDamageLevel, 5);
			mainTex.Apply();
			break;
		case BodyDamage.LEFT_LEG:
			leftLegDamageLevel = Mathf.Clamp(leftLegDamageLevel + damageLevel, 0, 10);
			applyDamageTo(bp, "lleg2", leftLegDamageLevel, 1);
			applyDamageTo(bp, "lleg3", leftLegDamageLevel, 3);
			applyDamageTo(bp, "lleg4", leftLegDamageLevel, 4);
			applyDamageTo(bp, "lleg5", leftLegDamageLevel, 5);
			mainTex.Apply();
			break;
		case BodyDamage.LEGS_OFF:
			applyDamageTo(bp, "rlegoff1");
			applyDamageTo(bp, "llegoff1");
			mainTex.Apply();
			break;
		case BodyDamage.LEFT_LEG_OFF:
			applyDamageTo(bp, "llegoff1");
			applyDamageTo(bp, "lfoot1");
			applyDamageTo(bp, "lfoot2");
			mainTex.Apply();
			break;
		case BodyDamage.RIGHT_LEG_OFF:
			applyDamageTo(bp, "rlegoff1");
			applyDamageTo(bp, "rfoot1");
			applyDamageTo(bp, "rfoot2");
			mainTex.Apply();
			break;
		case BodyDamage.HALF_BODY_OFF:
			applyDamageTo(bp, "chest");
			applyDamageTo(bp, "back");
			applyDamageTo(bp, "side1");
			applyDamageTo(bp, "side2");
			applyDamageTo(bp, "middle");
			applyDamageTo(bp, "pelvis");
			mainTex.Apply();
			break;
		case BodyDamage.LEFT_ARM_OFF:
			applyDamageTo(bp, "larmoff");
			applyDamageTo(bp, "side2");
			mainTex.Apply();
			break;
		case BodyDamage.RIGHT_ARM_OFF:
			applyDamageTo(bp, "rarmoff");
			applyDamageTo(bp, "side1");
			mainTex.Apply();
			break;
		case BodyDamage.LEFT_LEG_2_OFF:
			applyDamageTo(bp, "llegoff3");
			mainTex.Apply();
			break;
		case BodyDamage.RIGHT_LEG_2_OFF:
			applyDamageTo(bp, "rlegoff3");
			mainTex.Apply();
			break;
		case BodyDamage.HEAD_OFF:
		case BodyDamage.ARMS_OFF:
			break;
		}
	}

	private void applyDamageTo(BodyDamage bodyPart, string partName, int damageLevel, int reqMinDamageLevel)
	{
		if (damageLevel >= reqMinDamageLevel && !damageApplied.ContainsKey(partName))
		{
			damageApplied.Add(partName, true);
			texSetPixels(partName);
		}
	}

	private void applyDamageTo(BodyDamage bodyPart, string partName)
	{
		if (!damageApplied.ContainsKey(partName))
		{
			damageApplied.Add(partName, true);
			texSetPixels(partName);
		}
	}

	private void texSetPixels(string part)
	{
		if (!goreData.goreTexData.ContainsKey(part))
		{
			Debug.LogError(string.Format("Gore part '{0}' not found! Available parts: {1}", 
				part, string.Join(", ", goreData.goreTexData.Keys)));
			return;
		}
		
		int[] array = goreData.goreTexData[part];
		Debug.Log(string.Format("Applying gore to part {0} at coordinates: x={1}, y={2}, width={3}, height={4}", part, array[0], array[1], array[2], array[3]));
		mainTex.SetPixels(array[0], array[1], array[2], array[3], goreTex.GetPixels(array[0], array[1], array[2], array[3]));
	}

	private Texture2D getGoreTex(string goreTexSrc)
	{
		Material material = Resources.Load(string.Format("Characters/Materials/{0}", characterMaterial.name), typeof(Material)) as Material;
		Texture2D texture2D = material.mainTexture as Texture2D;
		Texture2D texture2D2 = Resources.Load(string.Format("Gore/Textures/{0}", goreTexSrc), typeof(Texture2D)) as Texture2D;
		
		// Use the character texture dimensions instead of gore texture dimensions
		int width = texture2D.width;
		int height = texture2D.height;
		Texture2D texture2D3 = new Texture2D(width, height, TextureFormat.ARGB32, false);
		
		// Scale the gore texture to match the character texture size
		float scaleX = (float)width / (float)texture2D2.width;
		float scaleY = (float)height / (float)texture2D2.height;
		
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				// Sample from the gore texture with proper scaling
				int goreX = (int)(i / scaleX);
				int goreY = (int)(j / scaleY);
				
				// Clamp to gore texture bounds
				goreX = Mathf.Clamp(goreX, 0, texture2D2.width - 1);
				goreY = Mathf.Clamp(goreY, 0, texture2D2.height - 1);
				
				Color pixel = texture2D2.GetPixel(goreX, goreY);
				Color pixel2 = texture2D.GetPixel(i, j);
				Color color = pixel * pixel.a + pixel2 * (1f - pixel.a);
				color.a = 1f;
				texture2D3.SetPixel(i, j, color);
			}
		}
		texture2D3.Apply();
		return texture2D3;
	}
}
