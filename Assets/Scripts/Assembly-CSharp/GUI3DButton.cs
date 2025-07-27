using UnityEngine;

public class GUI3DButton : GUI3DInteractiveObject
{
	public string StartSegRollOverTexture;

	public string RollOverTexture;

	public string EndSegRollOverTexture;

	protected Vector2 RollOverTextureOffset = default(Vector2);

	protected Vector2 RollOverTextureAtlasSize = default(Vector2);

	protected Vector2 StartSegRollOverTextureOffset = default(Vector2);

	protected Vector2 StartSegRollOverTextureAtlasSize = default(Vector2);

	protected Vector2 EndSegRollOverTextureOffset = default(Vector2);

	protected Vector2 EndSegRollOverTextureAtlasSize = default(Vector2);

	public string StartSegPressedTexture;

	public string PressedTexture;

	public string EndSegPressedTexture;

	public float RollOverScale = 1.2f;

	public float PressScale = 0.8f;

	public GUI3DTransition ActivateTransition;

	public GUI3D SwitchToGUI;

	public bool DeactivateOnChangeGUI = true;

	public bool HideOnChangeGUI;

	public bool SaveCurrentState;

	public bool RestoreLastGUIState;

	public bool CancelActions;

	public string LoadScene = string.Empty;

	private Vector3 originalScale;

	protected Vector2 PressedTextureOffset = default(Vector2);

	protected Vector2 PressedTextureAtlasSize = default(Vector2);

	protected Vector2 StartSegPressedTextureOffset = default(Vector2);

	protected Vector2 StartSegPressedTextureAtlasSize = default(Vector2);

	protected Vector2 EndSegPressedTextureOffset = default(Vector2);

	protected Vector2 EndSegPressedTextureAtlasSize = default(Vector2);

	public Vector3 TopLeftDragLimit = new Vector3(-400f, 350f, 0f);

	public Vector3 BottomRightDragLimit = new Vector3(400f, -350f, 0f);

	public bool IsBackButton;

	public bool ShowBackButton;

	private bool rolledOver;

	private Vector2[] normalUV;

	private Vector2[] pressedUV;

	private Vector2[] rollOverUV;

	protected override void Awake()
	{
		base.Awake();
		originalScale = base.transform.localScale;
		RefreshUVs();
	}

	public void RefreshUVs()
	{
		int num = 4;
		if (StartSegmentTexName != string.Empty)
		{
			num += 4;
		}
		if (EndSegmentTexName != string.Empty || StartSegmentTexName != string.Empty)
		{
			num += 4;
		}
		normalUV = new Vector2[num];
		int num2 = 0;
		Vector2[] uV;
		if (StartSegmentTexName != null && StartSegmentTexName != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(StartSegmentTexName);
			if (uV != null)
			{
				StartSegmentTexOffset = uV[0];
				StartSegmentTexAtlasSize = uV[1];
				EndSegmentTexOffset = uV[0];
				EndSegmentTexAtlasSize = uV[1];
			}
			uV = GetUV(StartSegmentTexOffset, StartSegmentTexAtlasSize);
			for (int i = 0; i < uV.Length; i++)
			{
				normalUV[num2++] = uV[i];
			}
		}
		if (TextureName != null && TextureName != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(TextureName);
			if (uV != null)
			{
				TextureOffset = uV[0];
				TextureAtlasSize = uV[1];
			}
			uV = GetUV(TextureOffset, TextureAtlasSize);
			for (int j = 0; j < uV.Length; j++)
			{
				normalUV[num2++] = uV[j];
			}
		}
		if (EndSegmentTexName != null && EndSegmentTexName != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(EndSegmentTexName);
			if (uV != null)
			{
				EndSegmentTexOffset = uV[0];
				EndSegmentTexAtlasSize = uV[1];
			}
			uV = GetUV(EndSegmentTexOffset, EndSegmentTexAtlasSize);
			for (int k = 0; k < uV.Length; k++)
			{
				normalUV[num2++] = uV[k];
			}
		}
		else if (StartSegmentTexName != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(StartSegmentTexName);
			uV = GetUV(StartSegmentTexOffset, StartSegmentTexAtlasSize);
			normalUV[num2++] = uV[1];
			normalUV[num2++] = uV[0];
			normalUV[num2++] = uV[3];
			normalUV[num2++] = uV[2];
		}
		if (mesh != null)
		{
			mesh.uv = normalUV;
		}
		if (RollOverTexture != null && RollOverTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(RollOverTexture);
			if (uV != null)
			{
				RollOverTextureOffset = uV[0];
				RollOverTextureAtlasSize = uV[1];
			}
		}
		if (PressedTexture != null && PressedTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(PressedTexture);
			if (uV != null)
			{
				PressedTextureOffset = uV[0];
				PressedTextureAtlasSize = uV[1];
			}
		}
		int num3 = 4;
		if (StartSegPressedTexture != null && StartSegPressedTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(StartSegPressedTexture);
			if (uV != null)
			{
				StartSegPressedTextureOffset = uV[0];
				StartSegPressedTextureAtlasSize = uV[1];
				EndSegPressedTextureOffset = uV[0];
				EndSegPressedTextureAtlasSize = uV[1];
				EndSegPressedTextureOffset.x += EndSegPressedTextureAtlasSize.x;
				EndSegPressedTextureAtlasSize.x = 0f - EndSegPressedTextureAtlasSize.x;
			}
			num3 += 4;
		}
		if (EndSegPressedTexture != null && EndSegPressedTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(EndSegPressedTexture);
			if (uV != null)
			{
				EndSegPressedTextureOffset = uV[0];
				EndSegPressedTextureAtlasSize = uV[1];
			}
			num3 += 4;
		}
		else if (StartSegPressedTexture != string.Empty)
		{
			num3 += 4;
		}
		pressedUV = new Vector2[num3];
		num3 = 4;
		if (StartSegRollOverTexture != null && StartSegRollOverTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(StartSegRollOverTexture);
			if (uV != null)
			{
				StartSegRollOverTextureOffset = uV[0];
				StartSegRollOverTextureAtlasSize = uV[1];
				EndSegRollOverTextureOffset = uV[0];
				EndSegRollOverTextureAtlasSize = uV[1];
				EndSegRollOverTextureOffset.x += EndSegRollOverTextureAtlasSize.x;
				EndSegRollOverTextureAtlasSize.x = 0f - EndSegRollOverTextureAtlasSize.x;
			}
			num3 += 4;
		}
		if (EndSegRollOverTexture != null && EndSegRollOverTexture != string.Empty)
		{
			uV = GUI3DGlobalParameters.Instance.GetUV(EndSegRollOverTexture);
			if (uV != null)
			{
				EndSegRollOverTextureOffset = uV[0];
				EndSegRollOverTextureAtlasSize = uV[1];
			}
			num3 += 4;
		}
		else if (StartSegRollOverTexture != string.Empty)
		{
			num3 += 4;
		}
		rollOverUV = new Vector2[num3];
		num2 = 0;
		if (StartSegPressedTexture != string.Empty)
		{
			uV = GetUV(StartSegPressedTextureOffset, StartSegPressedTextureAtlasSize);
			for (int l = 0; l < uV.Length; l++)
			{
				pressedUV[num2++] = uV[l];
			}
		}
		uV = GetUV(PressedTextureOffset, PressedTextureAtlasSize);
		for (int m = 0; m < uV.Length; m++)
		{
			pressedUV[num2++] = uV[m];
		}
		if (EndSegPressedTexture != string.Empty)
		{
			uV = GetUV(EndSegPressedTextureOffset, EndSegPressedTextureAtlasSize);
			for (int n = 0; n < uV.Length; n++)
			{
				pressedUV[num2++] = uV[n];
			}
		}
		else if (StartSegPressedTexture != string.Empty)
		{
			uV = GetUV(StartSegPressedTextureOffset, StartSegPressedTextureAtlasSize);
			pressedUV[num2++] = uV[1];
			pressedUV[num2++] = uV[0];
			pressedUV[num2++] = uV[3];
			pressedUV[num2++] = uV[2];
			EndSegPressedTextureAtlasSize = StartSegPressedTextureAtlasSize;
		}
		num2 = 0;
		if (StartSegRollOverTexture != string.Empty)
		{
			uV = GetUV(StartSegRollOverTextureOffset, StartSegRollOverTextureAtlasSize);
			for (int num4 = 0; num4 < uV.Length; num4++)
			{
				rollOverUV[num2++] = uV[num4];
			}
		}
		uV = GetUV(RollOverTextureOffset, RollOverTextureAtlasSize);
		for (int num5 = 0; num5 < uV.Length; num5++)
		{
			rollOverUV[num2++] = uV[num5];
		}
		if (EndSegRollOverTexture != string.Empty)
		{
			uV = GetUV(EndSegRollOverTextureOffset, EndSegRollOverTextureAtlasSize);
			for (int num6 = 0; num6 < uV.Length; num6++)
			{
				rollOverUV[num2++] = uV[num6];
			}
		}
		else if (StartSegRollOverTexture != string.Empty)
		{
			uV = GetUV(StartSegRollOverTextureOffset, StartSegRollOverTextureAtlasSize);
			rollOverUV[num2++] = uV[1];
			rollOverUV[num2++] = uV[0];
			rollOverUV[num2++] = uV[3];
			rollOverUV[num2++] = uV[2];
			EndSegRollOverTextureAtlasSize = StartSegRollOverTextureAtlasSize;
		}
	}

	public override void OnMouseOver()
	{
		base.OnMouseOver();
		rolledOver = true;
		if (mesh != null && RollOverTexture != string.Empty)
		{
			mesh.uv = rollOverUV;
		}
		else
		{
			base.transform.localScale = originalScale * RollOverScale;
		}
	}

	public override void OnMouseOut()
	{
		base.OnMouseOut();
		rolledOver = false;
		if (mesh != null)
		{
			mesh.uv = normalUV;
		}
		base.transform.localScale = originalScale;
	}

	public override void OnClick(Vector3 position)
	{
		if (mesh != null && PressedTexture != string.Empty)
		{
			mesh.uv = pressedUV;
		}
		base.OnClick(position);
	}

	public override void OnPress(Vector3 position)
	{
		if (mesh != null && PressedTexture != string.Empty)
		{
			mesh.uv = pressedUV;
		}
		else
		{
			base.transform.localScale = originalScale * PressScale;
		}
		base.OnPress(position);
	}

	public override void OnRelease()
	{
		if (!rolledOver || RollOverTexture == string.Empty)
		{
			if (mesh != null)
			{
				mesh.uv = normalUV;
			}
			base.transform.localScale = originalScale;
		}
		else if (mesh != null)
		{
			mesh.uv = rollOverUV;
		}
		if (!CancelActions)
		{
			if (ActivateTransition != null)
			{
				if (SwitchToGUI != null && SaveCurrentState)
				{
					GUI3DManager.Instance.SaveCurrentState();
				}
				ActivateTransition.StartTransition();
				ActivateTransition.TransitionEndEvent += OnEndTransition;
			}
			if (ActivateTransition == null)
			{
				if (LoadScene != string.Empty)
				{
					DedalordLoadLevel.LoadLevel(LoadScene);
				}
				else if (SwitchToGUI != null)
				{
					if (SaveCurrentState)
					{
						GUI3DManager.Instance.SaveCurrentState();
					}
					GUI3DManager.Instance.Activate(SwitchToGUI, DeactivateOnChangeGUI, HideOnChangeGUI);
				}
				else if (RestoreLastGUIState)
				{
					GUI3DManager.Instance.RestoreLastState();
				}
			}
		}
		base.OnRelease();
	}

	public override void OnDrag(Vector3 relativePosition)
	{
		Vector3 vector = new Vector3(1f, 1f, 1f);
		if (AutoAdjustScale != GUI3DAdjustScale.None && (Screen.width != ReferenceScreenWidth || Screen.height != ReferenceScreenHeight))
		{
			float num = 0f;
			float num2 = 0f;
			switch (AutoAdjustScale)
			{
			case GUI3DAdjustScale.Stretch:
				vector.x = vector.x / (float)ReferenceScreenWidth * (float)Screen.width;
				vector.y = vector.y / (float)ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchHorizontal:
				vector.x = vector.x / (float)ReferenceScreenWidth * (float)Screen.width;
				break;
			case GUI3DAdjustScale.StretchVertical:
				vector.y = vector.y / (float)ReferenceScreenHeight * (float)Screen.height;
				break;
			case GUI3DAdjustScale.StretchAverageToFitAspect:
				num = (float)(ReferenceScreenWidth + ReferenceScreenHeight) / 2f;
				num2 = (float)(Screen.width + Screen.height) / 2f;
				vector.x = vector.x / num * num2;
				vector.y = vector.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMaxToFitAspect:
				num = Mathf.Max(ReferenceScreenWidth, ReferenceScreenHeight);
				num2 = Mathf.Max(Screen.width, Screen.height);
				vector.x = vector.x / num * num2;
				vector.y = vector.y / num * num2;
				break;
			case GUI3DAdjustScale.StretchMinToFitAspect:
				num = Mathf.Min(ReferenceScreenWidth, ReferenceScreenHeight);
				num2 = Mathf.Min(Screen.width, Screen.height);
				vector.x = vector.x / num * num2;
				vector.y = vector.y / num * num2;
				break;
			}
		}
		relativePosition.x /= vector.x;
		relativePosition.y /= vector.y;
		Vector3 topLeftDragLimit = TopLeftDragLimit;
		Vector3 bottomRightDragLimit = BottomRightDragLimit;
		Vector3 localPosition = base.transform.localPosition;
		localPosition += relativePosition;
		if (localPosition.x < topLeftDragLimit.x)
		{
			localPosition.x = topLeftDragLimit.x;
		}
		if (localPosition.y > topLeftDragLimit.y)
		{
			localPosition.y = topLeftDragLimit.y;
		}
		if (localPosition.x > bottomRightDragLimit.x)
		{
			localPosition.x = bottomRightDragLimit.x;
		}
		if (localPosition.y < bottomRightDragLimit.y)
		{
			localPosition.y = bottomRightDragLimit.y;
		}
		relativePosition.x = localPosition.x - base.transform.localPosition.x;
		relativePosition.y = localPosition.y - base.transform.localPosition.y;
		base.transform.localPosition = localPosition;
		base.OnDrag(relativePosition);
	}

	private void OnEndTransition(GUI3DOnTransitionEndEvent evt)
	{
		ActivateTransition.TransitionEndEvent -= OnEndTransition;
		if (LoadScene != string.Empty)
		{
			DedalordLoadLevel.LoadLevel(LoadScene);
		}
		else if (SwitchToGUI != null)
		{
			GUI3DManager.Instance.Activate(SwitchToGUI, DeactivateOnChangeGUI, HideOnChangeGUI);
		}
		else if (RestoreLastGUIState)
		{
			GUI3DManager.Instance.RestoreLastState();
		}
	}

	public override bool CheckEventsEnabled()
	{
		if (!ConfigParams.isKindle && IsBackButton && !ShowBackButton)
		{
			return true;
		}
		return base.CheckEventsEnabled();
	}

	protected override bool IsObjectVisible()
	{
		if (!ConfigParams.isKindle && IsBackButton && !ShowBackButton)
		{
			return true;
		}
		return base.IsObjectVisible();
	}
}
