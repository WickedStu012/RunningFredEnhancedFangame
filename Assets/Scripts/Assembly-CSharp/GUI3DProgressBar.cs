using UnityEngine;

public class GUI3DProgressBar : MonoBehaviour, IGUI3DObject
{
	public int ReferenceScreenWidth = 1024;

	public int ReferenceScreenHeight = 768;

	public bool AutoAdjustPosition = true;

	public GUI3DAdjustScale AutoAdjustScale = GUI3DAdjustScale.Stretch;

	public Vector2 ObjectSize = default(Vector2);

	public Material ImageMaterial;

	public string StartSegmentTexName;

	public string MiddleSegmentTexName;

	public string EndSegmentTexName;

	public Vector2 StartSegmentTextOffset = default(Vector2);

	public Vector2 StartSegmentTextAtlasSize = default(Vector2);

	public Vector2 MiddleSegmentTextOffset = default(Vector2);

	public Vector2 MiddleSegmentTextAtlasSize = default(Vector2);

	public Vector2 EndSegmentTextOffset = default(Vector2);

	public Vector2 EndSegmentTextAtlasSize = default(Vector2);

	public float StartSegmentSizeX = 1f;

	public float EndSegmentSizeX = 1f;

	public float Spacing;

	public int TestProgress;

	private int lastTestProgress;

	public int SegmentsCount = 100;

	public bool Invert;

	public bool cleanTextures = true;

	private int progress = 100;

	private int lastSegmentsCount;

	protected Mesh mesh;

	protected Texture texture;

	private Vector3[] vertices;

	private Vector2[] uv;

	private int[] triangles;

	private Vector2 startSegmentTextOffset = default(Vector2);

	private Vector2 startSegmentTextAtlasSize = default(Vector2);

	private Vector2 middleSegmentTextOffset = default(Vector2);

	private Vector2 middleSegmentTextAtlasSize = default(Vector2);

	private Vector2 endSegmentTextOffset = default(Vector2);

	private Vector2 endSegmentTextAtlasSize = default(Vector2);

	private float startSegmentSizeX = 1f;

	private float endSegmentSizeX = 1f;

	private GUI3DPanel panel;

	public int Progress
	{
		get
		{
			return progress;
		}
		set
		{
			progress = value;
			if (texture != null)
			{
				RefreshMesh();
			}
		}
	}

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

	public Vector3 RealPosition()
	{
		return base.transform.position;
	}

	private void Awake()
	{
		CreateMesh(false);
	}

	public Vector3 GetObjectSize()
	{
		return ObjectSize;
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
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = null;
		}
		cleanTextures = true;
	}

	public virtual void ShowTextures()
	{
		if (MiddleSegmentTexName != string.Empty)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(MiddleSegmentTexName);
		}
		else if (StartSegmentTexName != string.Empty)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(StartSegmentTexName);
		}
		else if (EndSegmentTexName != string.Empty)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(EndSegmentTexName);
		}
		if (ImageMaterial != null && ImageMaterial.mainTexture != null)
		{
			texture = ImageMaterial.mainTexture;
			if (StartSegmentTexName != null && StartSegmentTexName != string.Empty)
			{
				Vector2[] uV = GUI3DGlobalParameters.Instance.GetUV(StartSegmentTexName);
				if (uV != null)
				{
					StartSegmentTextOffset = uV[0];
					StartSegmentTextAtlasSize = uV[1];
					StartSegmentSizeX = uV[1].x;
				}
			}
			if (MiddleSegmentTexName != null && MiddleSegmentTexName != string.Empty)
			{
				Vector2[] uV2 = GUI3DGlobalParameters.Instance.GetUV(MiddleSegmentTexName);
				if (uV2 != null)
				{
					MiddleSegmentTextOffset = uV2[0];
					MiddleSegmentTextAtlasSize = uV2[1];
				}
			}
			if (EndSegmentTexName != null && EndSegmentTexName != string.Empty)
			{
				Vector2[] uV3 = GUI3DGlobalParameters.Instance.GetUV(EndSegmentTexName);
				if (uV3 != null)
				{
					EndSegmentTextOffset = uV3[0];
					EndSegmentTextAtlasSize = uV3[1];
					EndSegmentSizeX = uV3[1].x;
				}
			}
		}
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = ImageMaterial;
		}
		CreateMesh(true);
		cleanTextures = false;
	}

	public virtual Vector3 Position()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (AutoAdjustPosition)
		{
			float num = 0f;
			float num2 = 0f;
			switch (AutoAdjustScale)
			{
			case GUI3DAdjustScale.Stretch:
				localPosition.x = localPosition.x / (float)ReferenceScreenWidth * (float)Screen.width;
				localPosition.y = localPosition.y / (float)ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchHorizontal:
				localPosition.x = localPosition.x / (float)ReferenceScreenWidth * (float)Screen.width;
				break;
			case GUI3DAdjustScale.StretchVertical:
				localPosition.y = localPosition.y / (float)ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchAverageToFitAspect:
				num = ReferenceScreenHeight;
				num2 = Screen.height;
				localPosition.x = localPosition.x / num * num2;
				localPosition.y = localPosition.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMaxToFitAspect:
				num = ReferenceScreenWidth;
				num2 = Screen.width;
				localPosition.x = localPosition.x / num * num2;
				localPosition.y = localPosition.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMinToFitAspect:
				num = ReferenceScreenHeight;
				num2 = Screen.height;
				localPosition.x = localPosition.x / num * num2;
				localPosition.y = localPosition.y / num * num2;
				break;
			}
		}
		return localPosition;
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

	private void CreateMesh(bool editor)
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		if (component != null)
		{
			Object.DestroyImmediate(component);
		}
		component = (MeshFilter)base.gameObject.AddComponent(typeof(MeshFilter));
		if (component.sharedMesh == null)
		{
			component.sharedMesh = new Mesh();
		}
		mesh = component.sharedMesh;
		MeshRenderer component2 = base.gameObject.GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			Object.DestroyImmediate(component2);
		}
		component2 = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		if (cleanTextures && editor)
		{
			return;
		}
		if (MiddleSegmentTexName != string.Empty)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(MiddleSegmentTexName);
		}
		else if (StartSegmentTexName != string.Empty)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(StartSegmentTexName);
		}
		else if (EndSegmentTexName != string.Empty)
		{
			ImageMaterial = GUI3DGlobalParameters.Instance.GetMaterial(EndSegmentTexName);
		}
		if (!(ImageMaterial != null))
		{
			return;
		}
		texture = ImageMaterial.mainTexture;
		component2.sharedMaterial = ImageMaterial;
		if (StartSegmentTexName != null && StartSegmentTexName != string.Empty)
		{
			Vector2[] uV = GUI3DGlobalParameters.Instance.GetUV(StartSegmentTexName);
			if (uV != null)
			{
				StartSegmentTextOffset = uV[0];
				StartSegmentTextAtlasSize = uV[1];
				StartSegmentSizeX = uV[1].x;
			}
		}
		if (MiddleSegmentTexName != null && MiddleSegmentTexName != string.Empty)
		{
			Vector2[] uV2 = GUI3DGlobalParameters.Instance.GetUV(MiddleSegmentTexName);
			if (uV2 != null)
			{
				MiddleSegmentTextOffset = uV2[0];
				MiddleSegmentTextAtlasSize = uV2[1];
			}
		}
		if (EndSegmentTexName != null && EndSegmentTexName != string.Empty)
		{
			Vector2[] uV3 = GUI3DGlobalParameters.Instance.GetUV(EndSegmentTexName);
			if (uV3 != null)
			{
				EndSegmentTextOffset = uV3[0];
				EndSegmentTextAtlasSize = uV3[1];
				EndSegmentSizeX = uV3[1].x;
			}
		}
		if (texture != null)
		{
			RefreshMesh();
		}
	}

	private void RefreshMesh()
	{
		startSegmentTextOffset = StartSegmentTextOffset;
		middleSegmentTextOffset = MiddleSegmentTextOffset;
		endSegmentTextOffset = EndSegmentTextOffset;
		startSegmentTextAtlasSize = StartSegmentTextAtlasSize;
		middleSegmentTextAtlasSize = MiddleSegmentTextAtlasSize;
		endSegmentTextAtlasSize = EndSegmentTextAtlasSize;
		startSegmentSizeX = StartSegmentSizeX;
		endSegmentSizeX = EndSegmentSizeX;
		float num = (float)Progress / 100f;
		int b = (int)(num * (float)SegmentsCount);
		if ((float)Progress == 0f)
		{
			base.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = true;
		}
		b = Mathf.Max(0, b);
		if (SegmentsCount == 1)
		{
			b = 1;
		}
		if (vertices == null || lastSegmentsCount != SegmentsCount)
		{
			vertices = new Vector3[(SegmentsCount + 2) * 4];
		}
		if (uv == null || lastSegmentsCount != SegmentsCount)
		{
			uv = new Vector2[(SegmentsCount + 2) * 4];
		}
		lastSegmentsCount = SegmentsCount;
		triangles = new int[(b + 2) * 6];
		float num2 = (ObjectSize.x - startSegmentSizeX - endSegmentSizeX) / (float)SegmentsCount;
		if (SegmentsCount == 1)
		{
			num2 *= num;
		}
		Vector3 vector = new Vector3((0f - ObjectSize.x) / 2f, (0f - ObjectSize.y) / 2f, 0f) + ((!Invert) ? Vector3.zero : new Vector3((ObjectSize.x - (startSegmentSizeX + endSegmentSizeX)) * (1f - num), 0f, 0f));
		vertices[0] = new Vector3(0f, 0f, 0f) + vector;
		vertices[1] = new Vector3(startSegmentSizeX + 1f, 0f, 0f) + vector;
		vertices[2] = new Vector3(0f, ObjectSize.y, 0f) + vector;
		vertices[3] = new Vector3(startSegmentSizeX + 1f, ObjectSize.y, 0f) + vector;
		uv[0] = new Vector2(startSegmentTextOffset.x / (float)texture.width + 1f / (float)texture.width, 1f - startSegmentTextOffset.y / (float)texture.height - 1f / (float)texture.height);
		uv[1] = new Vector2((startSegmentTextOffset.x + startSegmentTextAtlasSize.x) / (float)texture.width - 1f / (float)texture.width, 1f - startSegmentTextOffset.y / (float)texture.height - 1f / (float)texture.height);
		uv[2] = new Vector2(startSegmentTextOffset.x / (float)texture.width + 1f / (float)texture.width, 1f - (startSegmentTextOffset.y + startSegmentTextAtlasSize.y) / (float)texture.height + 1f / (float)texture.height);
		uv[3] = new Vector2((startSegmentTextOffset.x + startSegmentTextAtlasSize.x) / (float)texture.width - 1f / (float)texture.width, 1f - (startSegmentTextOffset.y + startSegmentTextAtlasSize.y) / (float)texture.height + 1f / (float)texture.height);
		triangles[0] = 2;
		triangles[1] = 1;
		triangles[2] = 0;
		triangles[3] = 3;
		triangles[4] = 1;
		triangles[5] = 2;
		float num3 = startSegmentSizeX;
		for (int i = 1; i <= b + 1; i++)
		{
			num3 += Spacing;
			vertices[i * 4] = new Vector3(num3 + (float)(i - 1) * num2, 0f, 0f) + vector;
			vertices[i * 4 + 1] = new Vector3(num3 + (float)(i - 1) * num2 + num2, 0f, 0f) + vector;
			vertices[i * 4 + 2] = new Vector3(num3 + (float)(i - 1) * num2, ObjectSize.y, 0f) + vector;
			vertices[i * 4 + 3] = new Vector3(num3 + (float)(i - 1) * num2 + num2, ObjectSize.y, 0f) + vector;
			uv[i * 4] = new Vector2(middleSegmentTextOffset.x / (float)texture.width + 1f / (float)texture.width, 1f - middleSegmentTextOffset.y / (float)texture.height - 1f / (float)texture.height);
			uv[i * 4 + 1] = new Vector2((middleSegmentTextOffset.x + middleSegmentTextAtlasSize.x) / (float)texture.width - 1f / (float)texture.width, 1f - middleSegmentTextOffset.y / (float)texture.height - 1f / (float)texture.height);
			uv[i * 4 + 2] = new Vector2(middleSegmentTextOffset.x / (float)texture.width + 1f / (float)texture.width, 1f - (middleSegmentTextOffset.y + middleSegmentTextAtlasSize.y) / (float)texture.height + 1f / (float)texture.height);
			uv[i * 4 + 3] = new Vector2((middleSegmentTextOffset.x + middleSegmentTextAtlasSize.x) / (float)texture.width - 1f / (float)texture.width, 1f - (middleSegmentTextOffset.y + middleSegmentTextAtlasSize.y) / (float)texture.height + 1f / (float)texture.height);
			triangles[i * 6] = i * 4 + 2;
			triangles[i * 6 + 1] = i * 4 + 1;
			triangles[i * 6 + 2] = i * 4;
			triangles[i * 6 + 3] = i * 4 + 3;
			triangles[i * 6 + 4] = i * 4 + 1;
			triangles[i * 6 + 5] = i * 4 + 2;
		}
		float num4 = Mathf.Max((float)b * num2, 0f) + num3 + Spacing;
		vertices[(b + 1) * 4] = new Vector3(num4 - 1f, 0f, 0f) + vector;
		vertices[(b + 1) * 4 + 1] = new Vector3(num4 + endSegmentSizeX, 0f, 0f) + vector;
		vertices[(b + 1) * 4 + 2] = new Vector3(num4 - 1f, ObjectSize.y, 0f) + vector;
		vertices[(b + 1) * 4 + 3] = new Vector3(num4 + endSegmentSizeX, ObjectSize.y, 0f) + vector;
		uv[(b + 1) * 4] = new Vector2(endSegmentTextOffset.x / (float)texture.width + 1f / (float)texture.width, 1f - endSegmentTextOffset.y / (float)texture.height - 1f / (float)texture.height);
		uv[(b + 1) * 4 + 1] = new Vector2((endSegmentTextOffset.x + endSegmentTextAtlasSize.x) / (float)texture.width - 1f / (float)texture.width, 1f - endSegmentTextOffset.y / (float)texture.height - 1f / (float)texture.height);
		uv[(b + 1) * 4 + 2] = new Vector2(endSegmentTextOffset.x / (float)texture.width + 1f / (float)texture.width, 1f - (endSegmentTextOffset.y + endSegmentTextAtlasSize.y) / (float)texture.height + 1f / (float)texture.height);
		uv[(b + 1) * 4 + 3] = new Vector2((endSegmentTextOffset.x + endSegmentTextAtlasSize.x) / (float)texture.width - 1f / (float)texture.width, 1f - (endSegmentTextOffset.y + endSegmentTextAtlasSize.y) / (float)texture.height + 1f / (float)texture.height);
		triangles[(b + 1) * 6] = (b + 1) * 4 + 2;
		triangles[(b + 1) * 6 + 1] = (b + 1) * 4 + 1;
		triangles[(b + 1) * 6 + 2] = (b + 1) * 4;
		triangles[(b + 1) * 6 + 3] = (b + 1) * 4 + 3;
		triangles[(b + 1) * 6 + 4] = (b + 1) * 4 + 1;
		triangles[(b + 1) * 6 + 5] = (b + 1) * 4 + 2;
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	private void OnDrawGizmos()
	{
		if (TestProgress != lastTestProgress)
		{
			if (mesh == null)
			{
				CreateMesh(true);
			}
			Progress = TestProgress;
			lastTestProgress = TestProgress;
		}
	}
}
