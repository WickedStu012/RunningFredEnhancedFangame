using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
	public Material ImageMaterial;

	public Vector2 TextureOffset;

	public Vector2 TextureFrameSize;

	public bool InvertHorizontalUV;

	public bool InvertVerticalUV;

	public bool InvertHorizontalUVRnd;

	public bool InvertVerticalUVRnd;

	public Vector2 ObjectSize;

	public int CurrentFrame;

	public int TotalFrames;

	public string PlayOnStart = string.Empty;

	public bool Billboard = true;

	public bool RotateJustY;

	private int framesPerWidth;

	private Texture2D textureAtlas;

	private Mesh mesh;

	private Vector2[] uv = new Vector2[4];

	private Vector2 textureOffset = default(Vector2);

	private int lastFrame;

	private Dictionary<string, SpriteAnimation> animations = new Dictionary<string, SpriteAnimation>();

	private SpriteAnimation currentAnimation;

	private void Awake()
	{
		if (InvertHorizontalUVRnd)
		{
			InvertHorizontalUV = Random.value >= 0.5f;
		}
		if (InvertVerticalUVRnd)
		{
			InvertVerticalUV = Random.value >= 0.5f;
		}
		CreateMesh(false);
		SpriteAnimation[] components = GetComponents<SpriteAnimation>();
		SpriteAnimation[] array = components;
		foreach (SpriteAnimation spriteAnimation in array)
		{
			if (animations.ContainsKey(spriteAnimation.Name))
			{
				Debug.LogError("Animation name " + spriteAnimation.Name + " already exists!!");
			}
			else
			{
				animations[spriteAnimation.Name] = spriteAnimation;
			}
			spriteAnimation.enabled = false;
		}
		if (PlayOnStart != string.Empty)
		{
			PlayAnimation(PlayOnStart);
		}
		else
		{
			SetFrame(CurrentFrame);
		}
	}

	private void OnDestroy()
	{
		Object.DestroyImmediate(mesh);
		if (base.GetComponent<Renderer>() != null && base.GetComponent<Renderer>().material != null)
		{
			Object.DestroyImmediate(base.GetComponent<Renderer>().material);
		}
	}

	public void CreateMesh(bool editor)
	{
		MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = (MeshFilter)base.gameObject.AddComponent(typeof(MeshFilter));
		}
		mesh = new Mesh();
		if (editor)
		{
			meshFilter.sharedMesh = mesh;
		}
		else
		{
			meshFilter.mesh = mesh;
		}
		MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		}
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		if (ImageMaterial == null)
		{
			ImageMaterial = new Material(Shader.Find("GUI/AlphaSelfIllum"));
		}
		meshRenderer.sharedMaterial = ImageMaterial;
		textureAtlas = (Texture2D)ImageMaterial.mainTexture;
		if (textureAtlas != null)
		{
			ImageMaterial.mainTexture = textureAtlas;
			array2 = GetUV(TextureOffset, TextureFrameSize);
			framesPerWidth = (int)((float)textureAtlas.width / TextureFrameSize.x);
			if (TotalFrames == 0)
			{
				TotalFrames = framesPerWidth;
				TotalFrames *= (int)((float)textureAtlas.height / TextureFrameSize.y);
			}
		}
		if (editor && ObjectSize.x == 0f && ObjectSize.y == 0f)
		{
			ObjectSize = new Vector2(TextureFrameSize.x, TextureFrameSize.y);
			Debug.Log("Setting automatic scale to: " + TextureFrameSize.x + " - " + TextureFrameSize.y);
		}
		array[0] = new Vector3(0f - ObjectSize.x, ObjectSize.y, 0f);
		array[1] = new Vector3(ObjectSize.x, ObjectSize.y, 0f);
		array[2] = new Vector3(0f - ObjectSize.x, 0f - ObjectSize.y, 0f);
		array[3] = new Vector3(ObjectSize.x, 0f - ObjectSize.y, 0f);
		int[] array3 = new int[6];
		array3[2] = 2;
		array3[1] = 1;
		array3[0] = 0;
		array3[5] = 3;
		array3[4] = 1;
		array3[3] = 2;
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	protected Vector2[] GetUV(Vector2 textureOffset, Vector2 textureAtlasSize)
	{
		if (textureAtlas != null)
		{
			if (textureAtlasSize.x == 0f)
			{
				textureAtlasSize.x = textureAtlas.width;
			}
			if (textureAtlasSize.y == 0f)
			{
				textureAtlasSize.y = textureAtlas.height;
			}
			if (InvertHorizontalUV)
			{
				uv[0].x = (textureOffset.x + textureAtlasSize.x) / (float)textureAtlas.width;
				uv[1].x = textureOffset.x / (float)textureAtlas.width;
				uv[2].x = (textureOffset.x + textureAtlasSize.x) / (float)textureAtlas.width;
				uv[3].x = textureOffset.x / (float)textureAtlas.width;
			}
			else
			{
				uv[0].x = textureOffset.x / (float)textureAtlas.width;
				uv[1].x = (textureOffset.x + textureAtlasSize.x) / (float)textureAtlas.width;
				uv[2].x = textureOffset.x / (float)textureAtlas.width;
				uv[3].x = (textureOffset.x + textureAtlasSize.x) / (float)textureAtlas.width;
			}
			if (InvertVerticalUV)
			{
				uv[0].y = ((float)textureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / (float)textureAtlas.height;
				uv[1].y = ((float)textureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / (float)textureAtlas.height;
				uv[2].y = ((float)textureAtlas.height - textureOffset.y) / (float)textureAtlas.height;
				uv[3].y = ((float)textureAtlas.height - textureOffset.y) / (float)textureAtlas.height;
			}
			else
			{
				uv[0].y = ((float)textureAtlas.height - textureOffset.y) / (float)textureAtlas.height;
				uv[1].y = ((float)textureAtlas.height - textureOffset.y) / (float)textureAtlas.height;
				uv[2].y = ((float)textureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / (float)textureAtlas.height;
				uv[3].y = ((float)textureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / (float)textureAtlas.height;
			}
		}
		return uv;
	}

	public void SetFrame(int frame)
	{
		if (mesh != null && textureAtlas != null && frame < TotalFrames)
		{
			CurrentFrame = frame % TotalFrames;
			float num = CurrentFrame % framesPerWidth;
			float num2 = CurrentFrame / framesPerWidth;
			textureOffset.x = TextureOffset.x + TextureFrameSize.x * num;
			textureOffset.y = TextureOffset.y + TextureFrameSize.y * num2;
			uv = GetUV(textureOffset, TextureFrameSize);
			mesh.uv = uv;
		}
	}

	public void PlayAnimation(string name)
	{
		if (animations.ContainsKey(name))
		{
			if (currentAnimation != null)
			{
				currentAnimation.enabled = false;
			}
			currentAnimation = animations[name];
			currentAnimation.enabled = true;
		}
	}

	public void StopCurrentAnimation()
	{
		if (currentAnimation != null)
		{
			currentAnimation.enabled = false;
		}
		currentAnimation = null;
	}

	public bool IsPlayingAnimation()
	{
		if (currentAnimation == null)
		{
			return false;
		}
		return currentAnimation.enabled;
	}

	private void OnDrawGizmos()
	{
		if (CurrentFrame != lastFrame)
		{
			SetFrame(CurrentFrame);
			lastFrame = CurrentFrame;
		}
	}
}
