using UnityEngine;

public class GUI3DCheckbox : GUI3DInteractiveObject
{
	public delegate void OnCheckboxChangeEvent(GUI3DOnCheckboxChangeEvent evt);

	public bool StartCheckStatus;

	public bool CanUncheck = true;

	public string UncheckedRollOverTexture;

	protected Vector2 UncheckedRollOverOffset = default(Vector2);

	protected Vector2 UncheckedRollOverAtlasSize = default(Vector2);

	public string CheckedTexture;

	protected Vector2 CheckedTextureOffset = default(Vector2);

	protected Vector2 CheckedTextureAtlasSize = default(Vector2);

	public string CheckedRollOverTexture;

	protected Vector2 CheckedRollOverTextureOffset = default(Vector2);

	protected Vector2 CheckedRollOverTextureAtlasSize = default(Vector2);

	private GUI3DOnCheckboxChangeEvent onCheckboxChangeEvent = new GUI3DOnCheckboxChangeEvent();

	private bool check;

	public bool Checked
	{
		get
		{
			return check;
		}
		set
		{
			check = value;
			if (this.CheckboxChangeEvent != null)
			{
				onCheckboxChangeEvent.Checked = check;
				onCheckboxChangeEvent.Target = this;
				this.CheckboxChangeEvent(onCheckboxChangeEvent);
			}
			UpdateState();
		}
	}

	public event OnCheckboxChangeEvent CheckboxChangeEvent;

	protected override void Awake()
	{
		base.Awake();
		if (UncheckedRollOverTexture != null && UncheckedRollOverTexture != string.Empty)
		{
			Vector2[] uV = GUI3DGlobalParameters.Instance.GetUV(UncheckedRollOverTexture);
			if (uV != null)
			{
				UncheckedRollOverOffset = uV[0];
				UncheckedRollOverAtlasSize = uV[1];
			}
		}
		if (CheckedTexture != null && CheckedTexture != string.Empty)
		{
			Vector2[] uV2 = GUI3DGlobalParameters.Instance.GetUV(CheckedTexture);
			if (uV2 != null)
			{
				CheckedTextureOffset = uV2[0];
				CheckedTextureAtlasSize = uV2[1];
			}
		}
		if (CheckedRollOverTexture != null && CheckedRollOverTexture != string.Empty)
		{
			Vector2[] uV3 = GUI3DGlobalParameters.Instance.GetUV(CheckedRollOverTexture);
			if (uV3 != null)
			{
				CheckedRollOverTextureOffset = uV3[0];
				CheckedRollOverTextureAtlasSize = uV3[1];
			}
		}
		Checked = StartCheckStatus;
	}

	private void Start()
	{
		if (Checked)
		{
			mesh.uv = GetUV(CheckedTextureOffset, CheckedTextureAtlasSize);
		}
		else
		{
			mesh.uv = GetUV(TextureOffset, TextureAtlasSize);
		}
	}

	public override void CleanTextures()
	{
		base.CleanTextures();
	}

	public override void OnMouseOver()
	{
		base.OnMouseOver();
		if (!(mesh != null))
		{
			return;
		}
		if (Checked)
		{
			if (CheckedRollOverTexture != string.Empty)
			{
				mesh.uv = GetUV(CheckedRollOverTextureOffset, CheckedRollOverTextureAtlasSize);
			}
		}
		else if (UncheckedRollOverTexture != string.Empty)
		{
			mesh.uv = GetUV(UncheckedRollOverOffset, UncheckedRollOverAtlasSize);
		}
	}

	public override void OnMouseOut()
	{
		base.OnMouseOut();
		if (mesh != null)
		{
			if (Checked)
			{
				mesh.uv = GetUV(CheckedTextureOffset, CheckedTextureAtlasSize);
			}
			else
			{
				mesh.uv = GetUV(TextureOffset, TextureAtlasSize);
			}
		}
	}

	public override void OnClick(Vector3 position)
	{
		base.OnClick(position);
		if (CanUncheck || !Checked)
		{
			Checked = !Checked;
		}
	}

	public void UpdateState()
	{
		if (mesh != null)
		{
			if (Checked)
			{
				mesh.uv = GetUV(CheckedTextureOffset, CheckedTextureAtlasSize);
			}
			else
			{
				mesh.uv = GetUV(TextureOffset, TextureAtlasSize);
			}
		}
	}
}
