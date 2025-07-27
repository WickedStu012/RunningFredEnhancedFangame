using UnityEngine;

public class GUI3DObject : MonoBehaviour, IGUI3DObject
{
	public string Tag = string.Empty;

	public bool CreateOwnMesh;

	public Material ImageMaterial;

	public Material NoAlphaImageMaterial;

	public bool DuplicateMaterial;

	public bool NoAlphaMaterial;

	public bool NoAlphaMiddleSeg;

	public string StartSegmentTexName;

	public string TextureName;

	public string EndSegmentTexName;

	public bool VolatileTexture;

	public Vector2 ObjectSize = default(Vector2);

	[HideInInspector]
	public Vector2 TextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 TextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 StartSegmentTexOffset = default(Vector2);

	[HideInInspector]
	public Vector2 StartSegmentTexAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 EndSegmentTexOffset = default(Vector2);

	[HideInInspector]
	public Vector2 EndSegmentTexAtlasSize = default(Vector2);

	public bool InvertHorizontalUV;

	public int ReferenceScreenWidth = 1024;

	public int ReferenceScreenHeight = 768;

	public bool AutoAdjustPosition = true;

	public GUI3DAdjustScale AutoAdjustScale = GUI3DAdjustScale.StretchMinToFitAspect;

	public Color color = Color.white;

	public float TouchableAreaScale = 1f;

	[HideInInspector]
	public bool cleanTextures;

	protected GameObject thisGameObject;

	protected Mesh mesh;

	protected Texture texture;

	private Vector3 lastSize;

	private Vector3 roundedPos;

	private IGUI3DObject parentObject;

	private GUI3DPanel panel;

	private void OnDestroy()
	{
		Object.DestroyImmediate(mesh);
	}

	private void OnEnable()
	{
		if (parentObject != null || !(base.transform.parent != null) || !(base.transform.parent.tag == "GUI"))
		{
			return;
		}
		MonoBehaviour[] components = base.transform.parent.GetComponents<MonoBehaviour>();
		MonoBehaviour[] array = components;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is IGUI3DObject)
			{
				parentObject = (IGUI3DObject)monoBehaviour;
				break;
			}
		}
	}

	protected virtual bool IsObjectVisible()
	{
		return true;
	}

	public GUI3DPanel GetPanel()
	{
		return panel;
	}

	public void SetPanel(GUI3DPanel panel)
	{
		this.panel = panel;
	}

	public Vector3 RealPosition()
	{
		if (this != null)
		{
			return base.transform.position;
		}
		return Vector3.zero;
	}

	protected virtual void Awake()
	{
		thisGameObject = base.gameObject;
		if (CreateOwnMesh && IsObjectVisible())
		{
			CreateMesh();
		}
		else if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = false;
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

	public void CreateMesh()
	{
		CreateMesh(false);
	}

	private void CreateMesh(bool editor)
	{
		if (GUI3DGlobalParameters.Instance == null)
		{
			return;
		}
		MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = (MeshFilter)base.gameObject.AddComponent(typeof(MeshFilter));
		}
		mesh = new Mesh();
		meshFilter.sharedMesh = mesh;
		MeshRenderer meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		}
		if (TextureName != string.Empty && (!cleanTextures || !editor))
		{
			if (NoAlphaMaterial)
			{
				ImageMaterial = GUI3DGlobalParameters.Instance.GetNoAlphaMaterial(TextureName, VolatileTexture);
			}
			else
			{
				ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(TextureName, VolatileTexture);
			}
			if (NoAlphaMiddleSeg)
			{
				NoAlphaImageMaterial = GUI3DGlobalParameters.Instance.GetNoAlphaMaterial(TextureName, VolatileTexture);
			}
			if (DuplicateMaterial)
			{
				ImageMaterial = new Material(ImageMaterial);
				if (NoAlphaImageMaterial != null)
				{
					NoAlphaImageMaterial = new Material(NoAlphaImageMaterial);
				}
			}
			if (TextureName != null && TextureName != string.Empty)
			{
				Vector2[] uV = GUI3DGlobalParameters.Instance.GetUV(TextureName);
				if (uV != null)
				{
					TextureOffset = uV[0];
					TextureAtlasSize = uV[1];
				}
			}
			if (StartSegmentTexName != null && StartSegmentTexName != string.Empty)
			{
				Vector2[] uV2 = GUI3DGlobalParameters.Instance.GetUV(StartSegmentTexName);
				if (uV2 != null)
				{
					StartSegmentTexOffset = uV2[0];
					StartSegmentTexAtlasSize = uV2[1];
					EndSegmentTexOffset = uV2[0];
					EndSegmentTexAtlasSize = uV2[1];
				}
			}
			if (EndSegmentTexName != null && EndSegmentTexName != string.Empty)
			{
				Vector2[] uV3 = GUI3DGlobalParameters.Instance.GetUV(EndSegmentTexName);
				if (uV3 != null)
				{
					EndSegmentTexOffset = uV3[0];
					EndSegmentTexAtlasSize = uV3[1];
				}
			}
		}
		int num = 4;
		if (StartSegmentTexName != string.Empty)
		{
			num += 4;
		}
		if (EndSegmentTexName != string.Empty || StartSegmentTexName != string.Empty)
		{
			num += 4;
		}
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		if (ImageMaterial == null)
		{
			ImageMaterial = new Material(Shader.Find("GUI/AlphaSelfIllum"));
		}
		if (NoAlphaMiddleSeg && NoAlphaImageMaterial == null)
		{
			NoAlphaImageMaterial = new Material(Shader.Find("GUI/NoAlphaSelfIllum"));
		}
		if (ImageMaterial != null)
		{
			ImageMaterial.color = color;
			if (NoAlphaMiddleSeg)
			{
				NoAlphaImageMaterial.color = color;
			}
			if (NoAlphaMiddleSeg)
			{
				meshRenderer.sharedMaterial = null;
				meshRenderer.sharedMaterials = new Material[2] { ImageMaterial, NoAlphaImageMaterial };
			}
			else
			{
				meshRenderer.sharedMaterials = new Material[1];
				meshRenderer.sharedMaterial = ImageMaterial;
			}
			texture = ImageMaterial.mainTexture;
			if (texture != null)
			{
				int num2 = 0;
				Vector2[] uV4;
				if (StartSegmentTexName != string.Empty)
				{
					uV4 = GetUV(StartSegmentTexOffset, StartSegmentTexAtlasSize);
					for (int i = 0; i < uV4.Length; i++)
					{
						array2[num2++] = uV4[i];
					}
				}
				uV4 = GetUV(TextureOffset, TextureAtlasSize);
				for (int j = 0; j < uV4.Length; j++)
				{
					array2[num2++] = uV4[j];
				}
				if (EndSegmentTexName != string.Empty)
				{
					uV4 = GetUV(EndSegmentTexOffset, EndSegmentTexAtlasSize);
					for (int k = 0; k < uV4.Length; k++)
					{
						array2[num2++] = uV4[k];
					}
				}
				else if (StartSegmentTexName != string.Empty)
				{
					uV4 = GetUV(StartSegmentTexOffset, StartSegmentTexAtlasSize);
					array2[num2++] = uV4[1];
					array2[num2++] = uV4[0];
					array2[num2++] = uV4[3];
					array2[num2++] = uV4[2];
				}
			}
		}
		if (ObjectSize.x == 0f && ObjectSize.y == 0f && (ImageMaterial != null || TextureAtlasSize.x != 0f || TextureAtlasSize.y != 0f))
		{
			if (TextureAtlasSize.x != 0f || TextureAtlasSize.y != 0f)
			{
				ObjectSize = new Vector2(TextureAtlasSize.x, TextureAtlasSize.y);
			}
			else if (ImageMaterial != null && ImageMaterial.mainTexture != null)
			{
				ObjectSize = new Vector2(ImageMaterial.mainTexture.width, ImageMaterial.mainTexture.height);
			}
		}
		int num3 = 0;
		if (StartSegmentTexName != string.Empty)
		{
			array[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f, ObjectSize.y * 0.5f, 0f);
			array[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
			array[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f, (0f - ObjectSize.y) * 0.5f, 0f);
			array[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
		}
		array[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
		array[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
		array[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
		if (EndSegmentTexName != string.Empty || StartSegmentTexName != string.Empty)
		{
			array[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
			array[num3++] = new Vector3(ObjectSize.x * 0.5f, ObjectSize.y * 0.5f, 0f);
			array[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
			array[num3++] = new Vector3(ObjectSize.x * 0.5f, (0f - ObjectSize.y) * 0.5f, 0f);
		}
		mesh.vertices = array;
		mesh.uv = array2;
		int num4 = 6;
		if (NoAlphaMiddleSeg && !NoAlphaMaterial)
		{
			num4 = 0;
		}
		if (StartSegmentTexName != string.Empty)
		{
			num4 += 6;
		}
		if (EndSegmentTexName != string.Empty || StartSegmentTexName != string.Empty)
		{
			num4 += 6;
		}
		if (num4 == 0)
		{
			num4 = 6;
		}
		int[] array3 = new int[num4];
		int num5 = 0;
		array3[num5++] = 0;
		array3[num5++] = 1;
		array3[num5++] = 2;
		array3[num5++] = 2;
		array3[num5++] = 1;
		array3[num5++] = 3;
		if (!NoAlphaMiddleSeg)
		{
			mesh.subMeshCount = 1;
			if (num4 > 6)
			{
				array3[num5++] = 4;
				array3[num5++] = 5;
				array3[num5++] = 6;
				array3[num5++] = 6;
				array3[num5++] = 5;
				array3[num5++] = 7;
			}
			if (num4 > 12)
			{
				array3[num5++] = 8;
				array3[num5++] = 9;
				array3[num5++] = 10;
				array3[num5++] = 10;
				array3[num5++] = 9;
				array3[num5++] = 11;
			}
			mesh.SetTriangles(array3, 0);
		}
		else
		{
			mesh.subMeshCount = 2;
			if (num4 > 6)
			{
				array3[num5++] = 8;
				array3[num5++] = 9;
				array3[num5++] = 10;
				array3[num5++] = 10;
				array3[num5++] = 9;
				array3[num5++] = 11;
			}
			mesh.SetTriangles(array3, 0);
			if (num4 > 6)
			{
				array3 = new int[6];
				num5 = 0;
				array3[num5++] = 4;
				array3[num5++] = 5;
				array3[num5++] = 6;
				array3[num5++] = 6;
				array3[num5++] = 5;
				array3[num5++] = 7;
				mesh.SetTriangles(array3, 1);
			}
		}
		lastSize = ObjectSize;
	}

	public void RefreshMaterial(string name)
	{
		TextureName = name;
		if (NoAlphaMaterial)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetNoAlphaMaterial(TextureName);
		}
		else
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(TextureName);
		}
		if (NoAlphaMiddleSeg)
		{
			NoAlphaImageMaterial = GUI3DGlobalParameters.Instance.GetNoAlphaMaterial(TextureName);
		}
		if (DuplicateMaterial)
		{
			ImageMaterial = new Material(ImageMaterial);
			if (NoAlphaMiddleSeg)
			{
				NoAlphaImageMaterial = new Material(NoAlphaImageMaterial);
			}
		}
		if (base.GetComponent<Renderer>() != null)
		{
			if (NoAlphaMiddleSeg)
			{
				base.GetComponent<Renderer>().sharedMaterial = null;
				base.GetComponent<Renderer>().sharedMaterials = new Material[2] { ImageMaterial, NoAlphaImageMaterial };
			}
			else
			{
				base.GetComponent<Renderer>().sharedMaterials = new Material[1];
				base.GetComponent<Renderer>().sharedMaterial = ImageMaterial;
			}
		}
		RefreshUV();
	}

	public void RefreshUV()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		if (component == null)
		{
			return;
		}
		if (TextureName != string.Empty)
		{
			if (TextureName != null && TextureName != string.Empty)
			{
				Vector2[] uV = GUI3DGlobalParameters.Instance.GetUV(TextureName);
				if (uV != null)
				{
					TextureOffset = uV[0];
					TextureAtlasSize = uV[1];
				}
			}
			if (StartSegmentTexName != null && StartSegmentTexName != string.Empty)
			{
				Vector2[] uV2 = GUI3DGlobalParameters.Instance.GetUV(StartSegmentTexName);
				if (uV2 != null)
				{
					StartSegmentTexOffset = uV2[0];
					StartSegmentTexAtlasSize = uV2[1];
					EndSegmentTexOffset = uV2[0];
					EndSegmentTexAtlasSize = uV2[1];
				}
			}
			if (EndSegmentTexName != null && EndSegmentTexName != string.Empty)
			{
				Vector2[] uV3 = GUI3DGlobalParameters.Instance.GetUV(EndSegmentTexName);
				if (uV3 != null)
				{
					EndSegmentTexOffset = uV3[0];
					EndSegmentTexAtlasSize = uV3[1];
				}
			}
		}
		int num = 4;
		if (StartSegmentTexName != string.Empty)
		{
			num += 4;
		}
		if (EndSegmentTexName != string.Empty || StartSegmentTexName != string.Empty)
		{
			num += 4;
		}
		Vector2[] array = new Vector2[num];
		if (ImageMaterial != null)
		{
			ImageMaterial.color = color;
			if (NoAlphaMiddleSeg)
			{
				NoAlphaImageMaterial.color = color;
			}
			texture = ImageMaterial.mainTexture;
			if (texture != null)
			{
				int num2 = 0;
				Vector2[] uV4;
				if (StartSegmentTexName != string.Empty)
				{
					uV4 = GetUV(StartSegmentTexOffset, StartSegmentTexAtlasSize);
					for (int i = 0; i < uV4.Length; i++)
					{
						array[num2++] = uV4[i];
					}
				}
				uV4 = GetUV(TextureOffset, TextureAtlasSize);
				for (int j = 0; j < uV4.Length; j++)
				{
					array[num2++] = uV4[j];
				}
				if (EndSegmentTexName != string.Empty)
				{
					uV4 = GetUV(EndSegmentTexOffset, EndSegmentTexAtlasSize);
					for (int k = 0; k < uV4.Length; k++)
					{
						array[num2++] = uV4[k];
					}
				}
				else if (StartSegmentTexName != string.Empty)
				{
					uV4 = GetUV(StartSegmentTexOffset, StartSegmentTexAtlasSize);
					array[num2++] = uV4[1];
					array[num2++] = uV4[0];
					array[num2++] = uV4[3];
					array[num2++] = uV4[2];
				}
			}
		}
		component.sharedMesh.uv = array;
		if (ObjectSize.x == 0f || ObjectSize.y == 0f)
		{
			if (TextureAtlasSize.x != 0f || TextureAtlasSize.y != 0f)
			{
				ObjectSize = new Vector2(TextureAtlasSize.x, TextureAtlasSize.y);
			}
			else if (ImageMaterial != null && ImageMaterial.mainTexture != null)
			{
				ObjectSize = new Vector2(ImageMaterial.mainTexture.width, ImageMaterial.mainTexture.height);
			}
			Vector3[] vertices = component.sharedMesh.vertices;
			int num3 = 0;
			if (StartSegmentTexName != string.Empty)
			{
				vertices[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f, ObjectSize.y * 0.5f, 0f);
				vertices[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
				vertices[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f, (0f - ObjectSize.y) * 0.5f, 0f);
				vertices[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
			}
			vertices[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
			vertices[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
			vertices[num3++] = new Vector3((0f - ObjectSize.x) * 0.5f + StartSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
			vertices[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
			if (EndSegmentTexName != string.Empty || StartSegmentTexName != string.Empty)
			{
				vertices[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, ObjectSize.y * 0.5f, 0f);
				vertices[num3++] = new Vector3(ObjectSize.x * 0.5f, ObjectSize.y * 0.5f, 0f);
				vertices[num3++] = new Vector3(ObjectSize.x * 0.5f - EndSegmentTexAtlasSize.x, (0f - ObjectSize.y) * 0.5f, 0f);
				vertices[num3++] = new Vector3(ObjectSize.x * 0.5f, (0f - ObjectSize.y) * 0.5f, 0f);
			}
			component.sharedMesh.vertices = vertices;
		}
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
				array[0].x = (textureOffset.x + textureAtlasSize.x) / (float)texture.width - 1f / (float)texture.width;
				array[0].y = ((float)texture.height - textureOffset.y) / (float)texture.height - 1f / (float)texture.height;
				array[1].x = textureOffset.x / (float)texture.width + 1f / (float)texture.width;
				array[1].y = ((float)texture.height - textureOffset.y) / (float)texture.height - 1f / (float)texture.height;
				array[2].x = (textureOffset.x + textureAtlasSize.x) / (float)texture.width - 1f / (float)texture.width;
				array[2].y = ((float)texture.height - (textureOffset.y + textureAtlasSize.y)) / (float)texture.height + 1f / (float)texture.height;
				array[3].x = textureOffset.x / (float)texture.width + 1f / (float)texture.width;
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
		return localScale * TouchableAreaScale;
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
		if (ImageMaterial != null && ImageMaterial.mainTexture != null)
		{
			ImageMaterial = null;
			NoAlphaImageMaterial = null;
			if (base.gameObject.GetComponent<MeshRenderer>() != null)
			{
				base.GetComponent<Renderer>().sharedMaterials = new Material[1];
				base.GetComponent<Renderer>().sharedMaterial = null;
			}
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
			RefreshUV();
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
