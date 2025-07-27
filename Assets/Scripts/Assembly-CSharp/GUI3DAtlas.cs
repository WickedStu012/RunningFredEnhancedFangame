using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUI3DAtlas
{
	public string[] TexNames;

	public Material Material;

	public Material NoAlphaMaterial;

	public Texture2D Texture;

	public Dictionary<string, Vector2[]> TexCoords = new Dictionary<string, Vector2[]>();

	public bool Volatile;

	public string AtlasName;

	public Vector2[] GetUV(string textureName)
	{
		Vector2[] array = new Vector2[2];
		array[0].x = 0f;
		array[0].y = 0f;
		array[1].x = Texture.width;
		array[1].y = Texture.height;
		if (TexCoords.ContainsKey(textureName))
		{
			array[0] = TexCoords[textureName][0];
			array[1] = TexCoords[textureName][1];
		}
		return array;
	}

	public void SaveCoords(string texName)
	{
		StreamWriter streamWriter = new StreamWriter("Assets/Resources/GUI3D/Atlas/" + texName + ".txt");
		string text = ((!Volatile) ? "0" : "1") + ";";
		text = text + TexNames.Length + ";";
		for (int i = 0; i < TexNames.Length; i++)
		{
			string text2 = TexNames[i];
			if (TexCoords.ContainsKey(text2))
			{
				text = text + text2 + ";";
				text = text + TexCoords[text2][0].x + ";";
				text = text + TexCoords[text2][0].y + ";";
				text = text + TexCoords[text2][1].x + ";";
				text = text + TexCoords[text2][1].y + ";";
			}
		}
		streamWriter.WriteLine(text);
		streamWriter.Close();
	}

	public void LoadCoords(string texName)
	{
		AtlasName = texName;
		TextAsset textAsset = Resources.Load("GUI3D/Atlas/" + texName, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("Coudn't find atlas: " + texName);
			return;
		}
		string[] array = textAsset.text.Split(';');
		int num = 0;
		Volatile = int.Parse(array[num++]) == 1;
		int num2 = int.Parse(array[num++]);
		TexNames = new string[num2];
		for (int i = 0; i < TexNames.Length; i++)
		{
			string text = array[num++];
			TexNames[i] = text;
			TexCoords[text] = new Vector2[2];
			TexCoords[text][0] = default(Vector2);
			TexCoords[text][0].x = float.Parse(array[num++]);
			TexCoords[text][0].y = float.Parse(array[num++]);
			TexCoords[text][1] = default(Vector2);
			TexCoords[text][1].x = float.Parse(array[num++]);
			TexCoords[text][1].y = float.Parse(array[num++]);
		}
	}

	public void LoadTexture()
	{
		Texture = Resources.Load("GUI3D/Atlas/" + AtlasName, typeof(Texture2D)) as Texture2D;
	}
}
