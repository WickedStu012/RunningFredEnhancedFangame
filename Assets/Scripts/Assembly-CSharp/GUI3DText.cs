using System;
using System.Collections.Generic;
using System.Text;
using BmFont;
using UnityEngine;

public class GUI3DText : MonoBehaviour, IGUI3DObject
{
	public struct CharMetrics
	{
		public string Line;

		public char[] LineChars;

		public float X;

		public float Y;

		public float Space;

		public int IdNextLine;
	}

	public bool AutoAdjustPosition = true;

	public GUI3DAdjustScale AutoAdjustScale = GUI3DAdjustScale.StretchMinToFitAspect;

	public string Text = string.Empty;

	public int Number;

	public string Font = string.Empty;

	public string Style = string.Empty;

	public Color Color = Color.white;

	public Color StrokeColor = Color.black;

	public float Threshold = 0.5f;

	public float StrokeSize = 0.5f;

	public float Gamma = 0.02f;

	public float StrokeGamma;

	public float ShadowThreshold = 0.5f;

	public float ShadowSoftness = 0.5f;

	public bool IsDynamicColor;

	public GUI3DTextJustify Justify;

	public GUI3DTextAlign Align;

	public float TextBoxWidth = 400f;

	public float TextBoxHeight = 200f;

	public Material ImageMaterial;

	public bool DynamicText;

	public bool NumericField;

	public int NumericFieldFixedDigits;

	public bool UseCustomXML;

	public string CustomXML = string.Empty;

	public bool UseCustomUniqueId;

	public string CustomUniqueId = string.Empty;

	public bool cleanTextures;

	public Vector2 ShadowDistance = new Vector2(0f, 0f);

	public Color ShadowColor = Color.black;

	public float CustomLineSpacing;

	public bool ForcePopupMaterial;

	private GUI3DTextJustify lastJustify;

	private string lastText = string.Empty;

	private int lastNumber;

	private char[] numberChars;

	private FontFile fontDesc;

	private FontChar[] chars;

	private bool loaded;

	private string font = string.Empty;

	private MeshRenderer meshRenderer;

	private MeshFilter meshFilter;

	private Mesh mesh;

	private Texture texture;

	[HideInInspector]
	public float textBoxWidth = 400f;

	[HideInInspector]
	public float textBoxHeight = 200f;

	[HideInInspector]
	public float TextWidth;

	[HideInInspector]
	public float TextHeight;

	[HideInInspector]
	public string TempText = string.Empty;

	private Vector3[] vertices;

	private Vector2[] uv;

	private Vector2[] uv2;

	private int[] triangles;

	private GUI3DPanel panel;

	private GUI3D gui3d;

	private bool visible = true;

	private Transform trans;

	private string dynamicTextFormat = string.Empty;

	private string textId = string.Empty;

	private object[] dynamicVars;

	private StringBuilder currentLine = new StringBuilder();

	private static List<CharMetrics> cachedMetrics = new List<CharMetrics>(2048);

	private GUI3DTextAlign lastAlign;

	public string DynamicTextFormat
	{
		get
		{
			if (string.IsNullOrEmpty(dynamicTextFormat))
			{
				return "{0}";
			}
			return dynamicTextFormat;
		}
		set
		{
			dynamicTextFormat = value;
		}
	}

	public string LocaleXML
	{
		get
		{
			if (UseCustomXML)
			{
				return CustomXML;
			}
			if (GetGUI() != null)
			{
				return GetGUI().LocaleXML;
			}
			return string.Empty;
		}
	}

	public void SetVisible(bool visible)
	{
		if (this.visible != visible)
		{
			if ((bool)this)
			{
				base.gameObject.SetActive(visible);
			}
			this.visible = visible;
		}
	}

	public bool IsVisible()
	{
		return visible;
	}

	public GUI3D GetGUI()
	{
		if (gui3d == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null && (gui3d == null || !(gui3d is GUI3D)))
			{
				gui3d = parent.GetComponent<GUI3D>();
				parent = parent.parent;
			}
		}
		return gui3d;
	}

	public GUI3DPanel GetPanel()
	{
		if (panel == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null && (panel == null || !(panel is GUI3DPanel)))
			{
				panel = parent.GetComponent<GUI3DPanel>();
				parent = parent.parent;
			}
		}
		if (panel != null && gui3d == null)
		{
			gui3d = panel.GUI3D;
			if (gui3d == null && panel.transform.parent != null)
			{
				gui3d = panel.transform.parent.GetComponent<GUI3D>();
			}
		}
		return panel;
	}

	public void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(mesh);
	}

	public void SetPanel(GUI3DPanel panel)
	{
		this.panel = panel;
	}

	public virtual Vector3 RealPosition()
	{
		return base.transform.position;
	}

	public void RefreshId()
	{
		textId = GetUniqueId();
	}

	public void RefreshGUI()
	{
		gui3d = null;
		GetGUI();
	}

	public string GetUniqueId()
	{
		if (UseCustomUniqueId)
		{
			return CustomUniqueId;
		}
		string text = base.name;
		Transform parent = base.transform.parent;
		while (parent != null && parent.parent != null)
		{
			text = parent.name + "." + text;
			parent = parent.parent;
		}
		return text;
	}

	public virtual void Awake()
	{
		Init();
	}

	private void Init()
	{
		InitComponents();
		LoadFont(Font);
		CreateMesh(Application.isEditor && !Application.isPlaying);
		if (!NumericField)
		{
			if (textId == null || textId == string.Empty)
			{
				textId = GetUniqueId();
			}
			if (textId != null && textId != string.Empty)
			{
				string empty = string.Empty;
				empty = ((!UseCustomXML) ? LocaleXML : CustomXML);
				if (empty != string.Empty)
				{
					MonoBehaviorSingleton<GUI3DLocalization>.Instance.Load(empty);
					if (NumericField)
					{
						SetNumber(Number);
					}
					else if (DynamicText)
					{
						dynamicTextFormat = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText(empty, textId, "!BAD TEXT!");
						if (dynamicVars != null)
						{
							SetDynamicText(dynamicVars);
						}
					}
					else
					{
						SetText(empty, textId);
					}
				}
			}
		}
		if (base.transform.parent != null)
		{
			GUI3DButton component = base.transform.parent.GetComponent<GUI3DButton>();
			if (component != null && component.IsBackButton && !component.ShowBackButton)
			{
				Text = "Back";
				RefreshText();
			}
		}
	}

	private void InitComponents()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		}
		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		trans = base.transform;
	}

	public void OnEnable()
	{
		if (meshRenderer != null)
		{
			meshRenderer.enabled = true;
		}
	}

	public virtual Vector3 GetObjectSize()
	{
		Vector3 result = new Vector3(TextBoxWidth * base.transform.localScale.x, TextBoxHeight * base.transform.localScale.y);
		return result;
	}

	private bool LoadFont(string font)
	{
		return LoadFont(font, false);
	}

	private bool LoadFont(string font, bool editor)
	{
		loaded = true;
		this.font = font;
		FontDesc fontDesc = null;
		if ((!cleanTextures || !editor) && GUI3DGlobalParameters.Instance != null)
		{
			fontDesc = GUI3DGlobalParameters.Instance.GetFont(font);
		}
		if (fontDesc != null)
		{
			if (!cleanTextures || !editor)
			{
				ImageMaterial = GUI3DGlobalParameters.Instance.GetFontMaterial(Font, Color);
			}
			if (ImageMaterial != null && meshRenderer != null)
			{
				texture = ImageMaterial.mainTexture;
				meshRenderer.sharedMaterial = ImageMaterial;
			}
			this.fontDesc = fontDesc.fontFile;
			chars = fontDesc.Chars;
			return true;
		}
		return false;
	}

	private int GetHash()
	{
		string str = string.Concat(Color.ToString(), StrokeColor.ToString(), ShadowColor.ToString(), StrokeColor, Threshold, StrokeSize, Gamma, StrokeGamma, StrokeColor, ShadowThreshold, ShadowSoftness);
		return Jenkins(str);
	}

	private int Jenkins(string str)
	{
		int i;
		int num = (i = 0);
		for (; i < str.Length; i++)
		{
			num += str[i];
			num += num << 10;
			num ^= num >> 6;
		}
		num += num << 3;
		num ^= num >> 11;
		return num + (num << 15);
	}

	private void CreateMesh(bool editor)
	{
		if (fontDesc != null)
		{
			if (editor)
			{
				InitComponents();
			}
			mesh = new Mesh();
			if (Font != null && Font != string.Empty)
			{
				ImageMaterial = GUI3DGlobalParameters.Instance.GetFontMaterial(Font, Color);
			}
			if (ImageMaterial != null)
			{
				texture = ImageMaterial.mainTexture;
				ImageMaterial.color = Color;
				meshRenderer.sharedMaterial = ImageMaterial;
			}
			if (NumericField || (Text != null && Text.Length > 0))
			{
				RefreshMesh(editor);
			}
			meshFilter.sharedMesh = mesh;
		}
	}

	private void RefreshMesh()
	{
		RefreshMesh(false);
	}

	private void RefreshMesh(bool editor)
	{
		if (fontDesc == null || (!NumericField && Text == null))
		{
			return;
		}
		if (NumericField)
		{
			int num = Math.Abs(Number);
			bool flag = Number < 0;
			int num2;
			if (NumericFieldFixedDigits > 0)
			{
				num2 = NumericFieldFixedDigits;
			}
			else
			{
				num2 = 1;
				int num3 = num / 10;
				while (num3 != 0)
				{
					num3 /= 10;
					num2++;
				}
				if (flag)
				{
					num2++;
				}
			}
			if (numberChars == null || numberChars.Length != num2)
			{
				numberChars = new char[num2];
			}
			int num4 = 1;
			int num5 = 0;
			if (flag)
			{
				numberChars[0] = '-';
				num5 = 1;
			}
			for (int i = 0; i < num2 - num5; i++)
			{
				numberChars[num2 - 1 - i] = (char)(48 + num / num4 % 10);
				num4 *= 10;
			}
		}
		if (NumericField)
		{
			if (vertices == null || uv == null || triangles == null || triangles.Length != numberChars.Length * 6)
			{
				if (vertices == null || vertices.Length < numberChars.Length * 4)
				{
					vertices = new Vector3[numberChars.Length * 4];
					uv = new Vector2[numberChars.Length * 4];
					uv2 = new Vector2[numberChars.Length * 4];
				}
				triangles = new int[numberChars.Length * 6];
			}
		}
		else if (vertices == null || uv == null || triangles == null || triangles.Length != Text.Length * 6 || editor)
		{
			if (editor || lastText == null || Text.Length > lastText.Length)
			{
				vertices = new Vector3[Text.Length * 4];
				uv = new Vector2[Text.Length * 4];
				uv2 = new Vector2[Text.Length * 4];
			}
			triangles = new int[Text.Length * 6];
		}
		lastText = Text;
		lastNumber = Number;
		TextWidth = 0f;
		TextHeight = 0f;
		float num6 = (float)fontDesc.Common.LineHeight + CustomLineSpacing;
		if (NumericField || Text.Length != 0)
		{
			if (NumericField)
			{
				cachedMetrics = CalculateCharMetrics(numberChars, cachedMetrics);
			}
			else
			{
				cachedMetrics = CalculateCharMetrics(Text, cachedMetrics);
			}
			float num7 = 0f;
			int num8 = 0;
			float num9 = 0f;
			switch (Align)
			{
			case GUI3DTextAlign.JustifyTop:
				num9 = 0f;
				break;
			case GUI3DTextAlign.JustifyCenter:
				num9 = ((float)fontDesc.Common.LineHeight + CustomLineSpacing / 2f) * (float)cachedMetrics.Count / 2f;
				break;
			case GUI3DTextAlign.JustifyBottom:
				num9 = ((float)fontDesc.Common.LineHeight + CustomLineSpacing / 2f) * (float)cachedMetrics.Count;
				break;
			}
			TextHeight = num6 * (float)cachedMetrics.Count;
			for (int j = 0; j < cachedMetrics.Count; j++)
			{
				CharMetrics charMetrics = cachedMetrics[j];
				num7 = 0f;
				float num10 = 0f;
				int num11 = ((charMetrics.LineChars == null) ? charMetrics.Line.Length : charMetrics.LineChars.Length);
				for (int k = 0; k < num11; k++)
				{
					char c = ((charMetrics.LineChars == null) ? charMetrics.Line[k] : charMetrics.LineChars[k]);
					FontChar fontChar = chars[(uint)c];
					if (c != ' ')
					{
						float num12 = fontChar.Width;
						float num13 = fontChar.Height;
						float num14 = fontChar.XOffset;
						float num15 = fontChar.YOffset;
						vertices[num8 * 4] = new Vector3(charMetrics.X + num14 + num7, num9 + charMetrics.Y - num15, 0f);
						vertices[num8 * 4 + 1] = new Vector3(charMetrics.X + num14 + num7 + num12, num9 + charMetrics.Y - num15, 0f);
						vertices[num8 * 4 + 2] = new Vector3(charMetrics.X + num14 + num7, num9 + charMetrics.Y - num15 - num13, 0f);
						vertices[num8 * 4 + 3] = new Vector3(charMetrics.X + num14 + num7 + num12, num9 + charMetrics.Y - num15 - num13, 0f);
						triangles[num8 * 6 + 2] = num8 * 4 + 2;
						triangles[num8 * 6 + 1] = num8 * 4 + 1;
						triangles[num8 * 6] = num8 * 4;
						triangles[num8 * 6 + 5] = num8 * 4 + 3;
						triangles[num8 * 6 + 4] = num8 * 4 + 1;
						triangles[num8 * 6 + 3] = num8 * 4 + 2;
						if (texture != null)
						{
							uv[num8 * 4].x = (float)fontChar.X / (float)fontDesc.Common.ScaleW;
							uv[num8 * 4].y = 1f - (float)fontChar.Y / (float)fontDesc.Common.ScaleH;
							uv[num8 * 4 + 1].x = ((float)fontChar.X + num12) / (float)fontDesc.Common.ScaleW;
							uv[num8 * 4 + 1].y = 1f - (float)fontChar.Y / (float)fontDesc.Common.ScaleH;
							uv[num8 * 4 + 2].x = (float)fontChar.X / (float)fontDesc.Common.ScaleW;
							uv[num8 * 4 + 2].y = 1f - ((float)fontChar.Y + num13) / (float)fontDesc.Common.ScaleH;
							uv[num8 * 4 + 3].x = ((float)fontChar.X + num12) / (float)fontDesc.Common.ScaleW;
							uv[num8 * 4 + 3].y = 1f - ((float)fontChar.Y + num13) / (float)fontDesc.Common.ScaleH;
						}
						uv2[num8 * 4] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						uv2[num8 * 4 + 1] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						uv2[num8 * 4 + 2] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						uv2[num8 * 4 + 3] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						num7 += (float)fontChar.XAdvance;
						num8++;
					}
					else
					{
						vertices[num8 * 4] = Vector3.zero;
						vertices[num8 * 4 + 1] = Vector3.zero;
						vertices[num8 * 4 + 2] = Vector3.zero;
						vertices[num8 * 4 + 3] = Vector3.zero;
						triangles[num8 * 6 + 2] = num8 * 4 + 2;
						triangles[num8 * 6 + 1] = num8 * 4 + 1;
						triangles[num8 * 6] = num8 * 4;
						triangles[num8 * 6 + 5] = num8 * 4 + 3;
						triangles[num8 * 6 + 4] = num8 * 4 + 1;
						triangles[num8 * 6 + 3] = num8 * 4 + 2;
						uv[num8 * 4] = Vector2.one;
						uv[num8 * 4 + 1] = Vector2.one;
						uv[num8 * 4 + 2] = Vector2.one;
						uv[num8 * 4 + 3] = Vector2.one;
						uv2[num8 * 4] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						uv2[num8 * 4 + 1] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						uv2[num8 * 4 + 2] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						uv2[num8 * 4 + 3] = new Vector2(ShadowDistance.x, ShadowDistance.y);
						num8++;
						num7 = ((charMetrics.Space != 0f) ? (num7 + charMetrics.Space) : (num7 + (float)fontChar.XAdvance));
					}
					num10 = num7;
				}
				if (num10 > TextWidth)
				{
					TextWidth = num10;
				}
			}
		}
		TextWidth *= trans.localScale.x;
		TextHeight *= trans.localScale.y;
		if (mesh != null)
		{
			if (vertices != null)
			{
				mesh.vertices = vertices;
			}
			if (uv != null)
			{
				mesh.uv = uv;
			}
			if (uv2 != null)
			{
				mesh.uv2 = uv2;
			}
			if (triangles != null)
			{
				mesh.triangles = triangles;
			}
		}
	}

	public void RefreshShadow()
	{
		if (uv2 != null && uv2.Length > 0)
		{
			for (int i = 0; i < uv2.Length; i++)
			{
				uv2[i] = new Vector2(ShadowDistance.x, ShadowDistance.y);
			}
			if (mesh != null && uv2 != null)
			{
				mesh.uv2 = uv2;
			}
		}
	}

	private List<CharMetrics> CalculateCharMetrics(string text, List<CharMetrics> metrics)
	{
		if (fontDesc == null)
		{
			return null;
		}
		if (metrics == null)
		{
			metrics = new List<CharMetrics>(2048);
		}
		else
		{
			metrics.Clear();
		}
		float num = 0f;
		float x = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float space = 0f;
		int num5 = -1;
		int length = -1;
		int num6 = 0;
		float num7 = (float)fontDesc.Common.LineHeight + CustomLineSpacing;
		currentLine.Length = 0;
		CharMetrics item;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != '\n' && (text[i] >= chars.Length || chars[(uint)text[i]] == null))
			{
				continue;
			}
			FontChar fontChar = null;
			if (text[i] != '\n')
			{
				fontChar = chars[(uint)text[i]];
				num2 += (float)fontChar.XAdvance;
			}
			if (num2 < textBoxWidth && text[i] != '\n' && text[i] != '\r')
			{
				currentLine.Append(text[i]);
				if (text[i] == ' ')
				{
					num6++;
					num5 = i;
					length = currentLine.Length - 1;
					num4 = 0f;
				}
				else if (fontChar != null)
				{
					num3 += (float)fontChar.XAdvance;
					num4 += (float)fontChar.XAdvance;
				}
				continue;
			}
			if (text[i] != ' ' && num5 != -1 && text[i] != '\n' && text[i] != '\r' && fontChar != null)
			{
				num3 += (float)fontChar.XAdvance;
				num4 += (float)fontChar.XAdvance;
				num2 -= num4;
				num3 -= num4;
				i = num5;
				currentLine.Length = length;
				num6--;
			}
			while (currentLine.Length > 0 && currentLine[currentLine.Length - 1] == ' ')
			{
				num2 -= (float)chars[32].XAdvance;
				num6--;
				currentLine.Length--;
			}
			switch (Justify)
			{
			case GUI3DTextJustify.JustifyCenter:
				x = (0f - num2) / 2f;
				break;
			case GUI3DTextJustify.JustifyRight:
				x = 0f - num2;
				break;
			case GUI3DTextJustify.JustifyExpand:
				space = (((float)num6 == 0f) ? 0f : ((textBoxWidth - num3) / (float)num6));
				break;
			}
			item = CreateMetric(currentLine.ToString(), x, num * num7, space);
			metrics.Add(item);
			num -= 1f;
			space = 0f;
			num6 = 0;
			num2 = 0f;
			num3 = 0f;
			length = 0;
			num4 = 0f;
			num5 = -1;
			currentLine.Length = 0;
			if ((Mathf.Abs(num) - 1f) * num7 >= textBoxHeight)
			{
				break;
			}
		}
		switch (Justify)
		{
		case GUI3DTextJustify.JustifyCenter:
			x = (0f - num2) / 2f;
			break;
		case GUI3DTextJustify.JustifyRight:
			x = 0f - num2;
			break;
		}
		item = ((currentLine.Length != text.Length) ? CreateMetric(currentLine.ToString(), x, num * num7, space) : CreateMetric(text, x, num * num7, space));
		metrics.Add(item);
		return metrics;
	}

	private List<CharMetrics> CalculateCharMetrics(char[] textChars, List<CharMetrics> metrics)
	{
		if (fontDesc == null)
		{
			return null;
		}
		if (metrics == null)
		{
			metrics = new List<CharMetrics>(2048);
		}
		else
		{
			metrics.Clear();
		}
		float num = 0f;
		float x = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float space = 0f;
		int num5 = -1;
		int length = -1;
		int num6 = 0;
		float num7 = (float)fontDesc.Common.LineHeight + CustomLineSpacing;
		currentLine.Length = 0;
		CharMetrics item;
		for (int i = 0; i < textChars.Length; i++)
		{
			char c = textChars[i];
			if (c != '\n' && (c >= chars.Length || chars[(uint)c] == null))
			{
				continue;
			}
			FontChar fontChar = null;
			if (c != '\n')
			{
				fontChar = chars[(uint)c];
				num2 += (float)fontChar.XAdvance;
			}
			if (num2 < textBoxWidth && c != '\n' && c != '\r')
			{
				currentLine.Append(c);
				if (c == ' ')
				{
					num6++;
					num5 = i;
					length = currentLine.Length - 1;
					num4 = 0f;
				}
				else if (fontChar != null)
				{
					num3 += (float)fontChar.XAdvance;
					num4 += (float)fontChar.XAdvance;
				}
				continue;
			}
			if (c != ' ' && num5 != -1 && c != '\n' && c != '\r' && fontChar != null)
			{
				num3 += (float)fontChar.XAdvance;
				num4 += (float)fontChar.XAdvance;
				num2 -= num4;
				num3 -= num4;
				i = num5;
				currentLine.Length = length;
				num6--;
			}
			while (currentLine.Length > 0 && currentLine[currentLine.Length - 1] == ' ')
			{
				num2 -= (float)chars[32].XAdvance;
				num6--;
				currentLine.Length--;
			}
			switch (Justify)
			{
			case GUI3DTextJustify.JustifyCenter:
				x = (0f - num2) / 2f;
				break;
			case GUI3DTextJustify.JustifyRight:
				x = 0f - num2;
				break;
			case GUI3DTextJustify.JustifyExpand:
				space = (((float)num6 == 0f) ? 0f : ((textBoxWidth - num3) / (float)num6));
				break;
			}
			item = CreateMetric(currentLine.ToString(), x, num * num7, space);
			metrics.Add(item);
			num -= 1f;
			space = 0f;
			num6 = 0;
			num2 = 0f;
			num3 = 0f;
			length = 0;
			num4 = 0f;
			num5 = -1;
			currentLine.Length = 0;
			if ((Mathf.Abs(num) - 1f) * num7 >= textBoxHeight)
			{
				break;
			}
		}
		switch (Justify)
		{
		case GUI3DTextJustify.JustifyCenter:
			x = (0f - num2) / 2f;
			break;
		case GUI3DTextJustify.JustifyRight:
			x = 0f - num2;
			break;
		}
		item = ((currentLine.Length != textChars.Length) ? CreateMetric(currentLine.ToString(), x, num * num7, space) : CreateMetric(textChars, x, num * num7, space));
		metrics.Add(item);
		return metrics;
	}

	private CharMetrics CreateMetric(string line, float x, float y, float space)
	{
		return new CharMetrics
		{
			Line = line.Trim(),
			X = x,
			Y = y,
			Space = space
		};
	}

	private CharMetrics CreateMetric(char[] line, float x, float y, float space)
	{
		return new CharMetrics
		{
			LineChars = line,
			X = x,
			Y = y,
			Space = space
		};
	}

	public virtual Vector3 Position()
	{
		Vector3 localPosition = base.transform.localPosition;
		float referenceScreenWidth = GUI3DManager.Instance.ReferenceScreenWidth;
		float referenceScreenHeight = GUI3DManager.Instance.ReferenceScreenHeight;
		if (AutoAdjustPosition)
		{
			localPosition.x = localPosition.x / referenceScreenWidth * (float)Screen.width;
			localPosition.y = localPosition.y / referenceScreenHeight * (float)Screen.height;
		}
		return localPosition;
	}

	public virtual Vector3 Size()
	{
		Vector3 localScale = base.transform.localScale;
		float referenceScreenWidth = GUI3DManager.Instance.ReferenceScreenWidth;
		float referenceScreenHeight = GUI3DManager.Instance.ReferenceScreenHeight;
		if (AutoAdjustScale != GUI3DAdjustScale.None && ((float)Screen.width != referenceScreenWidth || (float)Screen.height != referenceScreenHeight))
		{
			float num = 0f;
			float num2 = 0f;
			switch (AutoAdjustScale)
			{
			case GUI3DAdjustScale.Stretch:
				localScale.x = localScale.x / referenceScreenWidth * (float)Screen.width;
				localScale.y = localScale.y / referenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchHorizontal:
				localScale.x = localScale.x / referenceScreenWidth * (float)Screen.width;
				break;
			case GUI3DAdjustScale.StretchVertical:
				localScale.y = localScale.y / referenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchAverageToFitAspect:
				num = (referenceScreenWidth + referenceScreenHeight) / 2f;
				num2 = (float)(Screen.width + Screen.height) / 2f;
				localScale.x = localScale.x / num * num2;
				localScale.y = localScale.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMaxToFitAspect:
				num = Mathf.Max(referenceScreenWidth, referenceScreenHeight);
				num2 = Mathf.Max(Screen.width, Screen.height);
				localScale.x = localScale.x / num * num2;
				localScale.y = localScale.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMinToFitAspect:
				num = Mathf.Min(referenceScreenWidth, referenceScreenHeight);
				num2 = Mathf.Min(Screen.width, Screen.height);
				localScale.x = localScale.x / num * num2;
				localScale.y = localScale.y / num * num2;
				break;
			}
		}
		return localScale;
	}

	public string GetText()
	{
		if (DynamicText)
		{
			return dynamicTextFormat;
		}
		return Text;
	}

	public string GetVisibleText()
	{
		return Text;
	}

	public void SetText(string textId)
	{
		SetText(LocaleXML, textId);
	}

	public void SetText(string xml, string textId)
	{
		if (xml != string.Empty)
		{
			MonoBehaviorSingleton<GUI3DLocalization>.Instance.Load(xml);
			if (DynamicText)
			{
				Text = string.Empty;
				dynamicTextFormat = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText(xml, textId, "!BAD TEXT!");
			}
			else
			{
				dynamicTextFormat = string.Empty;
				Text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText(xml, textId, "!BAD TEXT!");
			}
			this.textId = textId;
		}
		if (fontDesc != null && !NumericField && Text != lastText)
		{
			RefreshMesh();
		}
	}

	public void ForceSetText(string text)
	{
		Text = text;
	}

	public void SetDynamicText(params object[] vars)
	{
		dynamicVars = vars;
		if (Application.isEditor && (!DynamicText || NumericField))
		{
			Text = string.Format("{0}", vars);
		}
		if (!string.IsNullOrEmpty(dynamicTextFormat))
		{
			Text = string.Format("{0}", vars);
		}
		if (fontDesc != null && !NumericField && Text != lastText)
		{
			RefreshMesh();
		}
	}

	public void LoadScale()
	{
		string empty = string.Empty;
		empty = ((!UseCustomXML) ? LocaleXML : CustomXML);
		LoadScale(empty);
	}

	public void LoadScale(string xml)
	{
		base.transform.localScale = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetTextScale(xml, textId, base.transform.localScale);
	}

	public void SetNumber(int number)
	{
		Number = number;
		if (Application.isEditor && !NumericField)
		{
			Debug.LogError("SetNumber() : Non-number GUI3DText", this);
		}
		if (fontDesc != null && NumericField && Number != lastNumber)
		{
			RefreshMesh();
		}
	}

	private void DestroyMesh()
	{
		meshFilter = GetComponent<MeshFilter>();
		if ((bool)meshFilter)
		{
			UnityEngine.Object.DestroyImmediate(meshFilter);
		}
		meshRenderer = GetComponent<MeshRenderer>();
		if ((bool)meshRenderer)
		{
			UnityEngine.Object.DestroyImmediate(meshRenderer);
		}
		mesh = null;
		meshFilter = null;
		meshRenderer = null;
	}

	public virtual void CleanTextures()
	{
		ImageMaterial = null;
		if (meshRenderer == null)
		{
			meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		}
		if (meshRenderer != null)
		{
			meshRenderer.sharedMaterial = null;
		}
		DestroyMesh();
		cleanTextures = true;
	}

	public virtual void ShowTextures()
	{
		cleanTextures = false;
		if (Font != null && Font != string.Empty)
		{
			LoadFont(Font);
			DestroyMesh();
			CreateMesh(true);
			RefreshMesh(true);
			RefreshText(true);
		}
	}

	public void RefreshText(bool editor = false, bool force = false)
	{
		if (editor)
		{
			InitComponents();
		}
		textBoxWidth = TextBoxWidth / base.transform.localScale.x;
		textBoxHeight = TextBoxHeight / base.transform.localScale.y;
		if (font != Font || !loaded)
		{
			if (LoadFont(Font, true))
			{
				CreateMesh(true);
			}
		}
		else if (fontDesc != null)
		{
			if (mesh == null || meshFilter.sharedMesh != mesh)
			{
				CreateMesh(true);
			}
			if (force || (NumericField && Number != lastNumber) || (!NumericField && Text != lastText) || lastJustify != Justify || lastAlign != Align)
			{
				lastAlign = Align;
				lastJustify = Justify;
				RefreshMesh(editor);
			}
		}
		if (ImageMaterial != null)
		{
			ImageMaterial.color = Color;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (TempText != Text)
		{
			Text = TempText;
			RefreshText();
			RefreshMesh();
		}
		Vector3 size = new Vector3(TextBoxWidth, TextBoxHeight, 0f);
		Vector3 position = base.transform.position;
		position.y -= TextBoxHeight / 2f;
		switch (Justify)
		{
		case GUI3DTextJustify.JustifyNone:
		case GUI3DTextJustify.JustifyLeft:
			position.x += size.x / 2f;
			break;
		case GUI3DTextJustify.JustifyRight:
			position.x -= size.x / 2f;
			break;
		}
		switch (Align)
		{
		case GUI3DTextAlign.JustifyBottom:
			position.y += size.y;
			break;
		case GUI3DTextAlign.JustifyCenter:
			position.y += size.y / 2f;
			break;
		}
		Gizmos.DrawWireCube(position, size);
	}
}
