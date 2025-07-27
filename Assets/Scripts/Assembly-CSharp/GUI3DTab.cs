using UnityEngine;

public class GUI3DTab : GUI3DInteractiveObject
{
	public delegate void OnTabChangeEvent(GUI3DOnTabChangeEvent evt);

	public GameObject Content;

	public Color ColorSelected = Color.white;

	public bool ActiveStatusAtStart;

	public string TabActiveStartTexture;

	public string TabActiveCenterTexture;

	public string TabActiveEndTexture;

	[HideInInspector]
	public Vector2 StartSegActiveTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 StartSegActiveTextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 ActiveCenterTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 ActiveCenterTextureAtlasSize = default(Vector2);

	[HideInInspector]
	public Vector2 EndSegActiveTextureOffset = default(Vector2);

	[HideInInspector]
	public Vector2 EndSegActiveTextureAtlasSize = default(Vector2);

	private GUI3DOnTabChangeEvent onTabChangeEvent = new GUI3DOnTabChangeEvent();

	private bool tabActive;

	private Vector2[] activeUV;

	private Vector2[] normalUV;

	public bool TabActive
	{
		get
		{
			return tabActive;
		}
		set
		{
			tabActive = value;
			if (Content != null && Content.activeInHierarchy != value)
			{
				Content.SetActive(value);
			}
			if (this.TabChangeEvent != null)
			{
				onTabChangeEvent.Target = this;
				onTabChangeEvent.Active = value;
				this.TabChangeEvent(onTabChangeEvent);
			}
			UpdateState();
		}
	}

	public event OnTabChangeEvent TabChangeEvent;

	protected override void Awake()
	{
		base.Awake();
		if (mesh != null)
		{
			normalUV = mesh.uv;
		}
		Vector2[] uV;
		if (TabActiveStartTexture != null && TabActiveStartTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(TabActiveStartTexture);
			if (uV != null)
			{
				StartSegActiveTextureOffset = uV[0];
				StartSegActiveTextureAtlasSize = uV[1];
			}
		}
		if (TabActiveCenterTexture != null && TabActiveCenterTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(TabActiveCenterTexture);
			if (uV != null)
			{
				ActiveCenterTextureOffset = uV[0];
				ActiveCenterTextureAtlasSize = uV[1];
			}
		}
		if (TabActiveEndTexture != null && TabActiveEndTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(TabActiveEndTexture);
			if (uV != null)
			{
				EndSegActiveTextureOffset = uV[0];
				EndSegActiveTextureAtlasSize = uV[1];
			}
		}
		int num = 4;
		if (TabActiveStartTexture != null && TabActiveStartTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(TabActiveStartTexture);
			if (uV != null)
			{
				StartSegActiveTextureOffset = uV[0];
				StartSegActiveTextureAtlasSize = uV[1];
			}
			num += 4;
		}
		if (TabActiveEndTexture != null && TabActiveEndTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(TabActiveEndTexture);
			if (uV != null)
			{
				EndSegActiveTextureOffset = uV[0];
				EndSegActiveTextureAtlasSize = uV[1];
			}
			num += 4;
		}
		else if (TabActiveStartTexture != string.Empty)
		{
			num += 4;
		}
		activeUV = new Vector2[num];
		int num2 = 0;
		if (TabActiveStartTexture != string.Empty)
		{
			uV = GetUV(StartSegActiveTextureOffset, StartSegActiveTextureAtlasSize);
			for (int i = 0; i < uV.Length; i++)
			{
				activeUV[num2++] = uV[i];
			}
		}
		uV = GetUV(ActiveCenterTextureOffset, ActiveCenterTextureAtlasSize);
		for (int j = 0; j < uV.Length; j++)
		{
			activeUV[num2++] = uV[j];
		}
		if (TabActiveEndTexture != string.Empty)
		{
			uV = GetUV(EndSegActiveTextureOffset, EndSegActiveTextureAtlasSize);
			for (int k = 0; k < uV.Length; k++)
			{
				activeUV[num2++] = uV[k];
			}
		}
		else if (TabActiveStartTexture != string.Empty)
		{
			uV = GetUV(StartSegActiveTextureOffset, StartSegActiveTextureAtlasSize);
			activeUV[num2++] = uV[1];
			activeUV[num2++] = uV[0];
			activeUV[num2++] = uV[3];
			activeUV[num2++] = uV[2];
			EndSegActiveTextureAtlasSize = StartSegActiveTextureAtlasSize;
		}
	}

	public override void CleanTextures()
	{
		base.CleanTextures();
	}

	public override void OnPress(Vector3 position)
	{
		base.OnPress(position);
		if (!TabActive)
		{
			TabActive = !TabActive;
		}
	}

	public void UpdateState()
	{
		if (!(mesh != null))
		{
			return;
		}
		if (TabActive)
		{
			mesh.uv = activeUV;
			if (base.GetComponent<Renderer>() != null && base.GetComponent<Renderer>().sharedMaterial != null)
			{
				base.GetComponent<Renderer>().sharedMaterial.color = ColorSelected;
			}
		}
		else
		{
			mesh.uv = normalUV;
			if (base.GetComponent<Renderer>() != null && base.GetComponent<Renderer>().sharedMaterial != null)
			{
				base.GetComponent<Renderer>().sharedMaterial.color = color;
			}
		}
	}
}
