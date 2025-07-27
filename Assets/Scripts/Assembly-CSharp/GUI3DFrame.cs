using UnityEngine;

public class GUI3DFrame : MonoBehaviour, IGUI3DObject
{
	public bool CreateOwnMesh;

	public Material ImageMaterial;

	public Material CenterImageMaterial;

	public bool CenterNoAlpha = true;

	public string TopCornerSegmentTexName;

	public string TopSegmentTexName;

	public string SideSegmentTexName;

	public string BottomSegmentTexName;

	public string BottomCornerSegmentTexName;

	public string CenterTextureName;

	public Vector2 ObjectSize = default(Vector2);

	[HideInInspector]
	public Vector2 TextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 TextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 TopCornerTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 TopCornerTextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 TopTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 TopTextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 SideTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 SideTextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 BottomTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 BottomTextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 BottomCornerTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 BottomCornerTextureAtlasSize = default(Vector2);

	public bool InvertHorizontalUV;

	public int ReferenceScreenWidth = 1024;

	public int ReferenceScreenHeight = 768;

	public bool AutoAdjustPosition = true;

	public GUI3DAdjustScale AutoAdjustScale = GUI3DAdjustScale.StretchMinToFitAspect;

	[HideInInspector]
	public bool cleanTextures;

	protected GameObject thisGameObject;

	protected Mesh mesh;

	protected Texture texture;

	private Vector3 lastSize;

	private GUI3DPanel panel;

	public GUI3DPanel GetPanel()
	{
		return panel;
	}

	private void OnDestroy()
	{
		Object.DestroyImmediate(mesh);
	}

	public void SetPanel(GUI3DPanel panel)
	{
		this.panel = panel;
	}

	protected virtual void Awake()
	{
		thisGameObject = base.gameObject;
		if (CreateOwnMesh)
		{
			CreateMesh();
		}
		if (mesh == null)
		{
			MeshFilter component = GetComponent<MeshFilter>();
			if (component != null)
			{
				mesh = component.mesh;
			}
		}
	}

	public Vector3 GetObjectSize()
	{
		return ObjectSize;
	}

	private void CreateMesh()
	{
		CreateMesh(false);
	}

	private void CreateMesh(bool editor)
	{
		MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = (MeshFilter)base.gameObject.AddComponent(typeof(MeshFilter));
		}
		meshFilter.sharedMesh = new Mesh();
		mesh = meshFilter.sharedMesh;
		MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		}
		if ((!cleanTextures || !editor) && GUI3DGlobalParameters.Instance != null)
		{
			if (CenterTextureName != string.Empty)
			{
				if (CenterNoAlpha)
				{
					CenterImageMaterial = GUI3DGlobalParameters.Instance.GetNoAlphaMaterial(CenterTextureName);
				}
				else
				{
					ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(CenterTextureName);
				}
			}
			if (TopCornerSegmentTexName != string.Empty)
			{
				ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(TopCornerSegmentTexName);
			}
			else if (BottomCornerSegmentTexName != string.Empty)
			{
				ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(BottomCornerSegmentTexName);
			}
			if (CenterTextureName != null && CenterTextureName != string.Empty)
			{
				Vector2[] uV = GUI3DGlobalParameters.Instance.GetUV(CenterTextureName);
				if (uV != null)
				{
					TextureOffset = uV[0];
					TextureAtlasSize = uV[1];
				}
			}
			if (TopCornerSegmentTexName != null && TopCornerSegmentTexName != string.Empty)
			{
				Vector2[] uV2 = GUI3DGlobalParameters.Instance.GetUV(TopCornerSegmentTexName);
				if (uV2 != null)
				{
					TopCornerTextureOffset = uV2[0];
					TopCornerTextureAtlasSize = uV2[1];
					BottomCornerTextureOffset = uV2[0];
					BottomCornerTextureAtlasSize = uV2[1];
				}
			}
			if (BottomCornerSegmentTexName != null && BottomCornerSegmentTexName != string.Empty)
			{
				Vector2[] uV3 = GUI3DGlobalParameters.Instance.GetUV(BottomCornerSegmentTexName);
				if (uV3 != null)
				{
					BottomCornerTextureOffset = uV3[0];
					BottomCornerTextureAtlasSize = uV3[1];
				}
			}
			if (TopSegmentTexName != null && TopSegmentTexName != string.Empty)
			{
				Vector2[] uV4 = GUI3DGlobalParameters.Instance.GetUV(TopSegmentTexName);
				if (uV4 != null)
				{
					TopTextureOffset = uV4[0];
					TopTextureAtlasSize = uV4[1];
					BottomTextureOffset = uV4[0];
					BottomTextureAtlasSize = uV4[1];
				}
			}
			if (SideSegmentTexName != null && SideSegmentTexName != string.Empty)
			{
				Vector2[] uV5 = GUI3DGlobalParameters.Instance.GetUV(SideSegmentTexName);
				if (uV5 != null)
				{
					SideTextureOffset = uV5[0];
					SideTextureAtlasSize = uV5[1];
				}
			}
			if (BottomSegmentTexName != null && BottomSegmentTexName != string.Empty)
			{
				Vector2[] uV6 = GUI3DGlobalParameters.Instance.GetUV(BottomSegmentTexName);
				if (uV6 != null)
				{
					BottomTextureOffset = uV6[0];
					BottomTextureAtlasSize = uV6[1];
				}
			}
		}
		int num = 36;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		if (ImageMaterial != null)
		{
			if (CenterNoAlpha)
			{
				meshRenderer.sharedMaterial = null;
				meshRenderer.sharedMaterials = new Material[2] { ImageMaterial, CenterImageMaterial };
			}
			else
			{
				if (base.GetComponent<Renderer>().sharedMaterials != null)
				{
					base.GetComponent<Renderer>().sharedMaterials = new Material[mesh.subMeshCount];
				}
				meshRenderer.sharedMaterial = ImageMaterial;
			}
			texture = ImageMaterial.mainTexture;
			if (texture != null)
			{
				int num2 = 0;
				if (TopCornerSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TopCornerTextureOffset, TopCornerTextureAtlasSize);
					for (int i = 0; i < uV7.Length; i++)
					{
						array2[num2++] = uV7[i];
					}
				}
				if (TopSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TopTextureOffset, TopTextureAtlasSize);
					for (int j = 0; j < uV7.Length; j++)
					{
						array2[num2++] = uV7[j];
					}
				}
				if (TopCornerSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TopCornerTextureOffset, TopCornerTextureAtlasSize);
					array2[num2++] = uV7[1];
					array2[num2++] = uV7[0];
					array2[num2++] = uV7[3];
					array2[num2++] = uV7[2];
				}
				if (SideSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(SideTextureOffset, SideTextureAtlasSize);
					for (int k = 0; k < uV7.Length; k++)
					{
						array2[num2++] = uV7[k];
					}
				}
				if (CenterTextureName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TextureOffset, TextureAtlasSize);
					for (int l = 0; l < uV7.Length; l++)
					{
						array2[num2++] = uV7[l];
					}
				}
				if (SideSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(SideTextureOffset, SideTextureAtlasSize);
					array2[num2++] = uV7[1];
					array2[num2++] = uV7[0];
					array2[num2++] = uV7[3];
					array2[num2++] = uV7[2];
				}
				if (BottomCornerSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(BottomCornerTextureOffset, BottomCornerTextureAtlasSize);
					for (int m = 0; m < uV7.Length; m++)
					{
						array2[num2++] = uV7[m];
					}
				}
				else if (TopCornerSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TopCornerTextureOffset, TopCornerTextureAtlasSize);
					array2[num2++] = uV7[2];
					array2[num2++] = uV7[3];
					array2[num2++] = uV7[0];
					array2[num2++] = uV7[1];
				}
				if (BottomSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(BottomTextureOffset, BottomTextureAtlasSize);
					for (int n = 0; n < uV7.Length; n++)
					{
						array2[num2++] = uV7[n];
					}
				}
				else if (TopSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TopTextureOffset, TopTextureAtlasSize);
					array2[num2++] = uV7[2];
					array2[num2++] = uV7[3];
					array2[num2++] = uV7[0];
					array2[num2++] = uV7[1];
				}
				if (BottomCornerSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(BottomCornerTextureOffset, BottomCornerTextureAtlasSize);
					array2[num2++] = uV7[1];
					array2[num2++] = uV7[0];
					array2[num2++] = uV7[3];
					array2[num2++] = uV7[2];
				}
				else if (TopCornerSegmentTexName != string.Empty)
				{
					Vector2[] uV7 = GetUV(TopCornerTextureOffset, TopCornerTextureAtlasSize);
					array2[num2++] = uV7[3];
					array2[num2++] = uV7[2];
					array2[num2++] = uV7[1];
					array2[num2++] = uV7[0];
				}
			}
		}
		if (editor && ObjectSize.x == 0f && ObjectSize.y == 0f && (ImageMaterial != null || TextureAtlasSize.x != 0f || TextureAtlasSize.y != 0f))
		{
			if (TextureAtlasSize.x != 0f || TextureAtlasSize.y != 0f)
			{
				ObjectSize = new Vector2(TextureAtlasSize.x, TextureAtlasSize.y);
			}
			else if (ImageMaterial != null && ImageMaterial.mainTexture != null)
			{
				ObjectSize = new Vector2(ImageMaterial.mainTexture.width, ImageMaterial.mainTexture.height);
			}
			Debug.Log("Setting automatic scale to: " + ObjectSize.x + " - " + ObjectSize.y);
		}
		int num3 = 0;
		Vector2 vector = new Vector2
		{
			x = (0f - ObjectSize.x) * 0.5f,
			y = ObjectSize.y * 0.5f
		};
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x + TopCornerTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, vector.y - TopCornerTextureAtlasSize.y, 0f);
		array[num3++] = new Vector3(vector.x + TopCornerTextureAtlasSize.x, vector.y - TopCornerTextureAtlasSize.y, 0f);
		vector.x = TopCornerTextureAtlasSize.x - ObjectSize.x * 0.5f;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - TopCornerTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, vector.y - TopTextureAtlasSize.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - TopCornerTextureAtlasSize.x, vector.y - TopTextureAtlasSize.y, 0f);
		vector.x = ObjectSize.x * 0.5f - TopCornerTextureAtlasSize.x;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x + TopCornerTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, vector.y - TopCornerTextureAtlasSize.y, 0f);
		array[num3++] = new Vector3(vector.x + TopCornerTextureAtlasSize.x, vector.y - TopCornerTextureAtlasSize.y, 0f);
		vector.x = (0f - ObjectSize.x) * 0.5f;
		vector.y = ObjectSize.y * 0.5f - TopCornerTextureAtlasSize.y;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x + SideTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, (0f - ObjectSize.y) * 0.5f + TopCornerTextureAtlasSize.y, 0f);
		array[num3++] = new Vector3(vector.x + SideTextureAtlasSize.x, (0f - ObjectSize.y) * 0.5f + TopCornerTextureAtlasSize.y, 0f);
		vector.x = TopCornerTextureAtlasSize.x - ObjectSize.x * 0.5f;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - SideTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, (0f - ObjectSize.y) * 0.5f + TopCornerTextureAtlasSize.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - SideTextureAtlasSize.x, (0f - ObjectSize.y) * 0.5f + TopCornerTextureAtlasSize.y, 0f);
		vector.x = ObjectSize.x * 0.5f - SideTextureAtlasSize.x;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, (0f - ObjectSize.y) * 0.5f + TopCornerTextureAtlasSize.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f, (0f - ObjectSize.y) * 0.5f + TopCornerTextureAtlasSize.y, 0f);
		vector.x = (0f - ObjectSize.x) * 0.5f;
		vector.y = (0f - ObjectSize.y) * 0.5f + BottomCornerTextureAtlasSize.y;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x + BottomCornerTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, (0f - ObjectSize.y) * 0.5f, 0f);
		array[num3++] = new Vector3(vector.x + BottomCornerTextureAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
		vector.x = BottomCornerTextureAtlasSize.x - ObjectSize.x * 0.5f;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - BottomCornerTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, (0f - ObjectSize.y) * 0.5f, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - BottomCornerTextureAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
		vector.x = ObjectSize.x * 0.5f - BottomCornerTextureAtlasSize.x;
		array[num3++] = new Vector3(vector.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x + BottomCornerTextureAtlasSize.x, vector.y, 0f);
		array[num3++] = new Vector3(vector.x, (0f - ObjectSize.y) * 0.5f, 0f);
		array[num3++] = new Vector3(vector.x + BottomCornerTextureAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
		mesh.vertices = array;
		mesh.uv = array2;
		int num4 = 48;
		if (CenterNoAlpha)
		{
			mesh.subMeshCount = 2;
			num4 = 48;
		}
		else
		{
			mesh.subMeshCount = 1;
			num4 = 54;
		}
		int[] array3 = new int[num4];
		int num5 = 0;
		array3[num5++] = 0;
		array3[num5++] = 1;
		array3[num5++] = 2;
		array3[num5++] = 2;
		array3[num5++] = 1;
		array3[num5++] = 3;
		array3[num5++] = 4;
		array3[num5++] = 5;
		array3[num5++] = 6;
		array3[num5++] = 6;
		array3[num5++] = 5;
		array3[num5++] = 7;
		array3[num5++] = 8;
		array3[num5++] = 9;
		array3[num5++] = 10;
		array3[num5++] = 10;
		array3[num5++] = 9;
		array3[num5++] = 11;
		array3[num5++] = 12;
		array3[num5++] = 13;
		array3[num5++] = 14;
		array3[num5++] = 14;
		array3[num5++] = 13;
		array3[num5++] = 15;
		if (!CenterNoAlpha)
		{
			array3[num5++] = 16;
			array3[num5++] = 17;
			array3[num5++] = 18;
			array3[num5++] = 18;
			array3[num5++] = 17;
			array3[num5++] = 19;
		}
		array3[num5++] = 20;
		array3[num5++] = 21;
		array3[num5++] = 22;
		array3[num5++] = 22;
		array3[num5++] = 21;
		array3[num5++] = 23;
		array3[num5++] = 24;
		array3[num5++] = 25;
		array3[num5++] = 26;
		array3[num5++] = 26;
		array3[num5++] = 25;
		array3[num5++] = 27;
		array3[num5++] = 28;
		array3[num5++] = 29;
		array3[num5++] = 30;
		array3[num5++] = 30;
		array3[num5++] = 29;
		array3[num5++] = 31;
		array3[num5++] = 32;
		array3[num5++] = 33;
		array3[num5++] = 34;
		array3[num5++] = 34;
		array3[num5++] = 33;
		array3[num5++] = 35;
		if (CenterNoAlpha)
		{
			mesh.SetTriangles(array3, 0);
			num5 = 0;
			num4 = 6;
			array3 = new int[num4];
			array3[num5++] = 16;
			array3[num5++] = 17;
			array3[num5++] = 18;
			array3[num5++] = 18;
			array3[num5++] = 17;
			array3[num5++] = 19;
			mesh.SetTriangles(array3, 1);
		}
		else
		{
			mesh.triangles = array3;
		}
		lastSize = ObjectSize;
	}

	protected Vector2[] GetUV(Vector2 textureOffset, Vector2 textureAtlasSize)
	{
		Vector2[] array = new Vector2[4];
		if (ImageMaterial != null)
		{
			texture = ImageMaterial.mainTexture;
		}
		if (texture != null)
		{
			if (textureAtlasSize.x == 0f)
			{
				textureAtlasSize.x = texture.width;
			}
			if (textureAtlasSize.y == 0f)
			{
				textureAtlasSize.y = texture.height;
			}
			if (InvertHorizontalUV)
			{
				array[0].x = 1f - textureOffset.x / (float)texture.width - 1f / (float)texture.width;
				array[0].y = ((float)texture.height - textureOffset.y) / (float)texture.height - 1f / (float)texture.height;
				array[1].x = 1f - (textureOffset.x + textureAtlasSize.x) / (float)texture.width + 1f / (float)texture.width;
				array[1].y = ((float)texture.height - textureOffset.y) / (float)texture.height - 1f / (float)texture.height;
				array[2].x = 1f - textureOffset.x / (float)texture.width - 1f / (float)texture.width;
				array[2].y = ((float)texture.height - (textureOffset.y + textureAtlasSize.y)) / (float)texture.height + 1f / (float)texture.height;
				array[3].x = 1f - (textureOffset.x + textureAtlasSize.x) / (float)texture.width + 1f / (float)texture.width;
				array[3].y = ((float)texture.height - (textureOffset.y + textureAtlasSize.y)) / (float)texture.height + 1f / (float)texture.height;
			}
			else
			{
				array[0].x = textureOffset.x / (float)texture.width + 1f / (float)texture.width;
				array[0].y = ((float)texture.height - textureOffset.y) / (float)texture.height - 1f / (float)texture.height;
				array[1].x = (textureOffset.x + textureAtlasSize.x) / (float)texture.width - 1f / (float)texture.width;
				array[1].y = ((float)texture.height - textureOffset.y) / (float)texture.height - 1f / (float)texture.height;
				array[2].x = textureOffset.x / (float)texture.width + 1f / (float)texture.width;
				array[2].y = ((float)texture.height - (textureOffset.y + textureAtlasSize.y)) / (float)texture.height + 1f / (float)texture.height;
				array[3].x = (textureOffset.x + textureAtlasSize.x) / (float)texture.width - 1f / (float)texture.width;
				array[3].y = ((float)texture.height - (textureOffset.y + textureAtlasSize.y)) / (float)texture.height + 1f / (float)texture.height;
			}
		}
		return array;
	}

	public void UpdateUV()
	{
		if (mesh != null)
		{
			mesh.uv = GetUV(TextureOffset, TextureAtlasSize);
		}
	}

	public virtual Vector3 Size()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x *= ObjectSize.x;
		localScale.y *= ObjectSize.y;
		if (AutoAdjustScale != GUI3DAdjustScale.None && (Screen.width != ReferenceScreenWidth || Screen.height != ReferenceScreenHeight))
		{
			float num = 0f;
			float num2 = 0f;
			switch (AutoAdjustScale)
			{
			case GUI3DAdjustScale.Stretch:
				localScale.x = localScale.x / (float)ReferenceScreenWidth * (float)Screen.width;
				localScale.y = localScale.y / (float)ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchHorizontal:
				localScale.x = localScale.x / (float)ReferenceScreenWidth * (float)Screen.width;
				break;
			case GUI3DAdjustScale.StretchVertical:
				localScale.y = localScale.y / (float)ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchAverageToFitAspect:
				num = (float)(ReferenceScreenWidth + ReferenceScreenHeight) / 2f;
				num2 = (float)(Screen.width + Screen.height) / 2f;
				localScale.x = localScale.x / num * num2;
				localScale.y = localScale.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMaxToFitAspect:
				num = Mathf.Max(ReferenceScreenWidth, ReferenceScreenHeight);
				num2 = Mathf.Max(Screen.width, Screen.height);
				localScale.x = localScale.x / num * num2;
				localScale.y = localScale.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMinToFitAspect:
				num = Mathf.Min(ReferenceScreenWidth, ReferenceScreenHeight);
				num2 = Mathf.Min(Screen.width, Screen.height);
				localScale.x = localScale.x / num * num2;
				localScale.y = localScale.y / num * num2;
				break;
			}
		}
		return localScale;
	}

	public Vector3 RealPosition()
	{
		return base.transform.position;
	}

	private void DestroyMesh()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		Object.DestroyImmediate(component);
		MeshRenderer component2 = base.gameObject.GetComponent<MeshRenderer>();
		Object.DestroyImmediate(component2);
		mesh = null;
	}

	public virtual void CleanTextures()
	{
		ImageMaterial = null;
		CenterImageMaterial = null;
		if (base.gameObject.GetComponent<MeshRenderer>() != null && base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = null;
			base.GetComponent<Renderer>().sharedMaterials = new Material[2];
		}
		DestroyMesh();
		cleanTextures = true;
	}

	public virtual void ShowTextures()
	{
		cleanTextures = false;
		if (CreateOwnMesh)
		{
			DestroyMesh();
			CreateMesh(true);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if ((mesh == null || lastSize.x != ObjectSize.x || lastSize.y != ObjectSize.y) && CreateOwnMesh)
		{
			CreateMesh(true);
		}
		else if (!CreateOwnMesh && mesh != null)
		{
			DestroyMesh();
		}
		Vector3 vector = new Vector3(ObjectSize.x * base.transform.localScale.x, ObjectSize.y * base.transform.localScale.x);
		Vector3 vector2 = base.transform.position - Vector3.right * vector.x * 0.5f + Vector3.up * vector.y * 0.5f;
		Vector3 vector3 = base.transform.position + Vector3.right * vector.x * 0.5f + Vector3.up * vector.y * 0.5f;
		Vector3 vector4 = base.transform.position + Vector3.right * vector.x * 0.5f - Vector3.up * vector.y * 0.5f;
		Vector3 vector5 = base.transform.position - Vector3.right * vector.x * 0.5f - Vector3.up * vector.y * 0.5f;
		Debug.DrawLine(vector2, vector3, Color.yellow);
		Debug.DrawLine(vector3, vector4, Color.yellow);
		Debug.DrawLine(vector4, vector5, Color.yellow);
		Debug.DrawLine(vector5, vector2, Color.yellow);
	}
}
