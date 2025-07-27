using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUI3DGlobalParameters
{
	public string FontFolder = "Fonts";

	public string[] AtlasNames;

	public Shader GUIShader;

	public Shader GUINoAlphaShader;

	private Dictionary<string, GUI3DAtlas> nonVolatileAtlas = new Dictionary<string, GUI3DAtlas>();

	private Dictionary<string, GUI3DAtlas> volatileAtlas = new Dictionary<string, GUI3DAtlas>();

	private Dictionary<string, Texture2D> loadedTexture = new Dictionary<string, Texture2D>();

	private Dictionary<string, Material> materials = new Dictionary<string, Material>();

	private Dictionary<string, Material> noAlphaMaterials = new Dictionary<string, Material>();

	private List<Texture2D> volatileTextures = new List<Texture2D>();

	private Dictionary<string, Dictionary<Color, Material>> fontMaterials = new Dictionary<string, Dictionary<Color, Material>>();

	private List<Material> dynamicColorFontMaterials = new List<Material>();

	private static GUI3DGlobalParameters instance;

	private static bool initialized;

	public static GUI3DGlobalParameters Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GUI3DGlobalParameters();
			}
			if (!initialized && instance != null)
			{
				instance.Init();
			}
			return instance;
		}
	}

	protected GUI3DGlobalParameters()
	{
		instance = this;
		if (!initialized)
		{
			Init();
		}
	}

	public void OnDisable()
	{
		ClearTextures();
	}

	public static void Release()
	{
		if (instance != null)
		{
			instance.ClearTextures();
		}
		instance = null;
		initialized = false;
	}

	public void Init()
	{
		if (initialized)
		{
			return;
		}
		nonVolatileAtlas.Clear();
		volatileAtlas.Clear();
		loadedTexture.Clear();
		materials.Clear();
		noAlphaMaterials.Clear();
		dynamicColorFontMaterials.Clear();
		fontMaterials.Clear();
		instance = this;
		if (AtlasNames == null || AtlasNames.Length == 0)
		{
			LoadAtlasNames();
		}
		string[] atlasNames = AtlasNames;
		foreach (string texName in atlasNames)
		{
			GUI3DAtlas gUI3DAtlas = new GUI3DAtlas();
			gUI3DAtlas.LoadCoords(texName);
			if (gUI3DAtlas.Volatile)
			{
				string[] texNames = gUI3DAtlas.TexNames;
				foreach (string key in texNames)
				{
					volatileAtlas[key] = gUI3DAtlas;
				}
			}
			else
			{
				string[] texNames2 = gUI3DAtlas.TexNames;
				foreach (string key2 in texNames2)
				{
					nonVolatileAtlas[key2] = gUI3DAtlas;
				}
			}
		}
		initialized = true;
	}

	public Material GetMaterial(string filename)
	{
		return GetMaterial(filename, false);
	}

	public Material GetNoAlphaMaterial(string filename)
	{
		return GetNoAlphaMaterial(filename, false);
	}

	public Material GetFontMaterial(string filename, Color color)
	{
		if (filename == null)
		{
			return null;
		}
		if (!initialized)
		{
			Init();
		}
		string text = "GUI3D";
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += filename;
		Material material = null;
		if (filename != null && filename.StartsWith("font"))
		{
			if (fontMaterials.ContainsKey(filename) && fontMaterials[filename] != null && fontMaterials[filename].ContainsKey(color))
			{
				material = fontMaterials[filename][color];
				if (!(material == null))
				{
				}
			}
			else
			{
				material = new Material(Shader.Find("GUI/Text Shader"));
				if (!fontMaterials.ContainsKey(filename) || fontMaterials[filename] == null)
				{
					fontMaterials[filename] = new Dictionary<Color, Material>();
				}
				fontMaterials[filename][color] = material;
			}
			filename = FontFolder + "/" + MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetCurrentLanguage() + "/" + filename;
		}
		if (material == null)
		{
			Debug.Log("---> Will return null material @ GUI3DGlobalParameters.GetFontMaterial");
			return null;
		}
		return SetTexture(filename, material, false);
	}

	public Material GetDynamicColorFontMaterial(string filename)
	{
		if (filename == null)
		{
			return null;
		}
		if (!initialized)
		{
			Init();
		}
		string text = "GUI3D";
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += filename;
		Material material = null;
		if (filename != null && filename.StartsWith("font"))
		{
			material = new Material(Shader.Find("GUI/Text Shader"));
			dynamicColorFontMaterials.Add(material);
			filename = FontFolder + "/" + MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetCurrentLanguage() + "/" + filename;
		}
		if (material == null)
		{
			return null;
		}
		return SetTexture(filename, material, false);
	}

	public Material GetNoAlphaMaterial(string filename, bool isVolatile)
	{
		return GetMaterial(filename, isVolatile, false);
	}

	public Material GetMaterial(string filename, bool isVolatile)
	{
		return GetMaterial(filename, isVolatile, true);
	}

	public Material GetMaterial(string filename, bool isVolatile, bool alpha)
	{
		if (filename == null)
		{
			return null;
		}
		if (GUIShader == null)
		{
			GUIShader = Shader.Find("GUI/AlphaSelfIllum");
		}
		if (GUINoAlphaShader == null)
		{
			GUINoAlphaShader = Shader.Find("GUI/NoAlphaSelfIllum");
		}
		if (!initialized)
		{
			Init();
		}
		string text = "GUI3D";
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += filename;
		Material mat = null;
		GUI3DAtlas gUI3DAtlas = null;
		if (nonVolatileAtlas.ContainsKey(filename))
		{
			gUI3DAtlas = nonVolatileAtlas[filename];
		}
		else if (volatileAtlas.ContainsKey(filename))
		{
			gUI3DAtlas = volatileAtlas[filename];
		}
		if (gUI3DAtlas != null)
		{
			if (gUI3DAtlas.Texture == null)
			{
				gUI3DAtlas.LoadTexture();
			}
			if (alpha)
			{
				if (gUI3DAtlas.Material == null)
				{
					gUI3DAtlas.Material = new Material(GUIShader);
					gUI3DAtlas.Material.mainTexture = gUI3DAtlas.Texture;
				}
				return gUI3DAtlas.Material;
			}
			if (gUI3DAtlas.NoAlphaMaterial == null)
			{
				gUI3DAtlas.NoAlphaMaterial = new Material(GUINoAlphaShader);
				gUI3DAtlas.NoAlphaMaterial.mainTexture = gUI3DAtlas.Texture;
			}
			return gUI3DAtlas.NoAlphaMaterial;
		}
		return SetTexture(filename, mat, isVolatile, alpha);
	}

	private Material SetTexture(string filename, Material mat, bool isVolatile)
	{
		return SetTexture(filename, mat, isVolatile, true);
	}

	private Material SetTexture(string filename, Material mat, bool isVolatile, bool alpha)
	{
		Texture2D texture2D;
		if (loadedTexture.ContainsKey(filename))
		{
			texture2D = loadedTexture[filename];
		}
		else
		{
			string path = "GUI3D/" + filename;
			texture2D = Resources.Load(path, typeof(Texture2D)) as Texture2D;
			if (texture2D == null)
			{
				Debug.LogError(filename + " not found in resources folder.");
			}
			else if (!isVolatile)
			{
				loadedTexture[filename] = texture2D;
			}
			else
			{
				volatileTextures.Add(texture2D);
			}
		}
		if (mat == null)
		{
			if (alpha)
			{
				if (materials.ContainsKey(filename))
				{
					mat = materials[filename];
				}
				else
				{
					mat = new Material(GUIShader);
					mat.mainTexture = texture2D;
					materials[filename] = mat;
				}
			}
			else if (noAlphaMaterials.ContainsKey(filename))
			{
				mat = noAlphaMaterials[filename];
			}
			else
			{
				mat = new Material(GUINoAlphaShader);
				mat.mainTexture = texture2D;
				noAlphaMaterials[filename] = mat;
			}
		}
		else
		{
			mat.mainTexture = texture2D;
		}
		return mat;
	}

	public void PreloadAtlas(string atlas)
	{
		foreach (GUI3DAtlas value in nonVolatileAtlas.Values)
		{
			if (value.AtlasName == atlas)
			{
				value.LoadTexture();
				break;
			}
		}
		foreach (GUI3DAtlas value2 in volatileAtlas.Values)
		{
			if (value2.AtlasName == atlas)
			{
				value2.LoadTexture();
				break;
			}
		}
	}

	public void ClearTextures()
	{
		volatileTextures.Clear();
		foreach (GUI3DAtlas value in volatileAtlas.Values)
		{
			value.Texture = null;
			if (value.Material != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(value.Material);
				}
				else
				{
					Object.DestroyImmediate(value.Material);
				}
			}
			value.Material = null;
		}
		foreach (Material value2 in materials.Values)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value2);
			}
			else
			{
				Object.DestroyImmediate(value2);
			}
		}
		materials.Clear();
		foreach (Material value3 in noAlphaMaterials.Values)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value3);
			}
			else
			{
				Object.DestroyImmediate(value3);
			}
		}
		noAlphaMaterials.Clear();
		foreach (Material dynamicColorFontMaterial in dynamicColorFontMaterials)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(dynamicColorFontMaterial);
			}
			else
			{
				Object.DestroyImmediate(dynamicColorFontMaterial);
			}
		}
		dynamicColorFontMaterials.Clear();
		foreach (string key in fontMaterials.Keys)
		{
			if (fontMaterials[key] == null)
			{
				continue;
			}
			foreach (Color key2 in fontMaterials[key].Keys)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(fontMaterials[key][key2]);
				}
				else
				{
					Object.DestroyImmediate(fontMaterials[key][key2]);
				}
			}
		}
		fontMaterials.Clear();
		loadedTexture.Clear();
	}

	public FontDesc GetFont(string font)
	{
		string text = "GUI3D/";
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += FontFolder;
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetCurrentLanguage();
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += font;
		return GUI3DFontManager.Instance.LoadFont(text);
	}

	public Vector2[] GetUV(string textureName)
	{
		GUI3DAtlas gUI3DAtlas = null;
		Vector2[] array = new Vector2[2];
		array[0].x = 0f;
		array[0].y = 0f;
		array[1].x = 0f;
		array[1].y = 0f;
		if (nonVolatileAtlas.ContainsKey(textureName))
		{
			gUI3DAtlas = nonVolatileAtlas[textureName];
		}
		else if (volatileAtlas.ContainsKey(textureName))
		{
			gUI3DAtlas = volatileAtlas[textureName];
		}
		else if (loadedTexture.ContainsKey(textureName))
		{
			Texture2D texture2D = loadedTexture[textureName];
			array[0].x = 0f;
			array[0].y = 0f;
			array[1].x = texture2D.width;
			array[1].y = texture2D.height;
			return array;
		}
		if (gUI3DAtlas != null && gUI3DAtlas.TexCoords.ContainsKey(textureName))
		{
			array[0] = gUI3DAtlas.TexCoords[textureName][0];
			array[1] = gUI3DAtlas.TexCoords[textureName][1];
		}
		return array;
	}

	public void LoadAtlasNames()
	{
		TextAsset textAsset = Resources.Load("GUI3D/globals", typeof(TextAsset)) as TextAsset;
		string[] array = textAsset.text.Split(';');
		if (array != null && array.Length != 0)
		{
			AtlasNames = new string[array.Length - 1];
			for (int i = 0; i < AtlasNames.Length; i++)
			{
				string text = array[i];
				AtlasNames[i] = text;
			}
		}
	}

	public void SaveAtlasNames()
	{
		StreamWriter streamWriter = new StreamWriter("Assets/Resources/GUI3D/globals.txt");
		string text = string.Empty;
		for (int i = 0; i < AtlasNames.Length; i++)
		{
			string text2 = AtlasNames[i];
			text = text + text2 + ";";
		}
		streamWriter.WriteLine(text);
		streamWriter.Close();
	}
}
