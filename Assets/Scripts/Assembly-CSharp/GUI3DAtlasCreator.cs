using System.Collections.Generic;
using UnityEngine;

public class GUI3DAtlasCreator
{
	private class Node
	{
		public Node[] child;

		public Rect rc;

		public Texture2D Image;

		public Node Insert(Texture2D image)
		{
			if (!IsLeaf())
			{
				Node node = null;
				if (child[0] != null)
				{
					node = child[0].Insert(image);
				}
				if (node == null && child[1] != null)
				{
					node = child[1].Insert(image);
				}
				if (node != null)
				{
					return node;
				}
				return null;
			}
			if (Image != null)
			{
				return null;
			}
			if (image.width > (int)rc.width || image.height > (int)rc.height)
			{
				return null;
			}
			if (image.width == (int)rc.width && image.height == (int)rc.height)
			{
				Image = image;
				return this;
			}
			child = new Node[2];
			child[0] = new Node();
			child[1] = new Node();
			int num = (int)rc.width - image.width;
			int num2 = (int)rc.height - image.height;
			if (num > num2)
			{
				child[0].rc = new Rect(rc.x, rc.y, image.width, rc.height);
				child[1].rc = new Rect(rc.x + (float)image.width + 2f, rc.y, rc.width - (float)image.width - 2f, rc.height);
			}
			else
			{
				child[0].rc = new Rect(rc.x, rc.y, rc.width, image.height);
				child[1].rc = new Rect(rc.x, rc.y + (float)image.height + 2f, rc.width, rc.height - (float)image.height - 2f);
			}
			return child[0].Insert(image);
		}

		private bool IsLeaf()
		{
			return child == null;
		}
	}

	private GUI3DAtlas atlas;

	private Node TextureTree;

	public GUI3DAtlas GenerateTexture(string texName, Object[] obj, bool isVolatile, int width, int height)
	{
		atlas = new GUI3DAtlas();
		atlas.Volatile = isVolatile;
		atlas.AtlasName = texName;
		OrderTexturesBySize(obj);
		atlas.Texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		atlas.Texture.filterMode = FilterMode.Point;
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Color(0f, 0f, 0f, 0f);
		}
		atlas.Texture.SetPixels(array);
		TextureTree = null;
		List<string> list = new List<string>();
		for (int j = 0; j < obj.Length; j++)
		{
			Texture2D texture2D = (Texture2D)obj[j];
			if (texture2D == null)
			{
				continue;
			}
			string text = texture2D.name.ToLower();
			if (!text.Contains("font"))
			{
				if (!Insert(texture2D))
				{
					break;
				}
				list.Add(texture2D.name);
			}
		}
		atlas.TexNames = list.ToArray();
		atlas.Texture.Apply();
		atlas.SaveCoords(texName);
		return atlas;
	}

	private bool Insert(Texture2D tex)
	{
		if (TextureTree == null)
		{
			TextureTree = new Node();
			TextureTree.rc.x = 0f;
			TextureTree.rc.y = 0f;
			TextureTree.rc.width = atlas.Texture.width;
			TextureTree.rc.height = atlas.Texture.height;
		}
		Node node = TextureTree.Insert(tex);
		if (node != null)
		{
			atlas.Texture.SetPixels((int)node.rc.x, atlas.Texture.height - (int)(node.rc.y + node.rc.height), (int)node.rc.width, (int)node.rc.height, node.Image.GetPixels());
			if (!atlas.TexCoords.ContainsKey(tex.name))
			{
				atlas.TexCoords[tex.name] = new Vector2[2];
			}
			atlas.TexCoords[tex.name][0] = new Vector2(node.rc.x, node.rc.y);
			atlas.TexCoords[tex.name][1] = new Vector2(node.rc.width, node.rc.height);
			return true;
		}
		Debug.LogError("Couldn't fit texture: Atlas: " + atlas.AtlasName + " - Tex: " + tex.name);
		return false;
	}

	private void OrderTexturesBySize(Object[] texs)
	{
		OrderTexturesBySize(texs, 0, texs.Length - 1);
	}

	private void OrderTexturesBySize(Object[] texs, int lo, int hi)
	{
		int num = (lo + hi) / 2;
		int num2 = (((Texture2D)texs[num]).width + ((Texture2D)texs[num]).height) / 2;
		int num3 = lo;
		int num4 = hi;
		while (true)
		{
			if (num3 < num && (((Texture2D)texs[num3]).width + ((Texture2D)texs[num3]).height) / 2 > num2)
			{
				num3++;
				continue;
			}
			while (num4 > num && (((Texture2D)texs[num4]).width + ((Texture2D)texs[num4]).height) / 2 < num2)
			{
				num4--;
			}
			if (num3 <= num4)
			{
				Object obj = texs[num3];
				texs[num3] = texs[num4];
				texs[num4] = obj;
				num3++;
				num4--;
			}
			if (num3 > num4)
			{
				break;
			}
		}
		if (lo < num4)
		{
			OrderTexturesBySize(texs, lo, num4);
		}
		if (hi > num3)
		{
			OrderTexturesBySize(texs, num3, hi);
		}
	}
}
