using UnityEngine;

public class AlignWithText : MonoBehaviour
{
	public enum Align
	{
		Left = 0,
		Right = 1
	}

	public GUI3DText AlignedText;

	public GUI3DObject GuiObject;

	public GUI3DText Text;

	public Align Aligned;

	public float DistanceFromText = -25f;

	public bool UpdateRegularly;

	private string lastText;

	private void Awake()
	{
		Vector3 localPosition = Vector3.zero;
		if (GuiObject != null)
		{
			localPosition = GuiObject.transform.localPosition;
		}
		else if (AlignedText != null)
		{
			localPosition = AlignedText.transform.localPosition;
		}
		localPosition.x = GetAlignPos();
		if (AlignedText != null)
		{
			AlignedText.transform.localPosition = localPosition;
		}
		if (GuiObject != null)
		{
			GuiObject.transform.localPosition = localPosition;
		}
		if (!UpdateRegularly)
		{
			base.enabled = false;
		}
		lastText = Text.Text;
	}

	private void OnDrawGizmos()
	{
		if (Text == null)
		{
			Text = GetComponent<GUI3DText>();
		}
		Vector3 localPosition = Vector3.zero;
		if (GuiObject != null)
		{
			localPosition = GuiObject.transform.localPosition;
		}
		else if (AlignedText != null)
		{
			localPosition = AlignedText.transform.localPosition;
		}
		localPosition.x = GetAlignPos();
		if (AlignedText != null)
		{
			AlignedText.transform.localPosition = localPosition;
		}
		if (GuiObject != null)
		{
			GuiObject.transform.localPosition = localPosition;
		}
		Update();
		if (Text != null)
		{
			lastText = Text.Text;
		}
	}

	private void Update()
	{
		if (Text != null && (UpdateRegularly || Text.Text != lastText))
		{
			Vector3 localPosition = Vector3.zero;
			if (AlignedText != null)
			{
				localPosition = AlignedText.transform.localPosition;
			}
			if (GuiObject != null)
			{
				localPosition = GuiObject.transform.localPosition;
			}
			localPosition.x = GetAlignPos();
			if (AlignedText != null)
			{
				AlignedText.transform.localPosition = localPosition;
			}
			if (GuiObject != null)
			{
				GuiObject.transform.localPosition = localPosition;
			}
			lastText = Text.Text;
		}
	}

	private float GetAlignPos()
	{
		if (Text == null)
		{
			return 0f;
		}
		if (GuiObject != null)
		{
			if (Aligned == Align.Left)
			{
				if (Text.Justify == GUI3DTextJustify.JustifyCenter)
				{
					return Text.transform.localPosition.x - Text.TextWidth / 2f - GuiObject.ObjectSize.x * GuiObject.transform.localScale.x / 2f + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyLeft || Text.Justify == GUI3DTextJustify.JustifyNone)
				{
					return Text.transform.localPosition.x - GuiObject.ObjectSize.x * GuiObject.transform.localScale.x / 2f + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyRight)
				{
					return Text.transform.localPosition.x - Text.TextWidth + DistanceFromText;
				}
			}
			else
			{
				if (Text.Justify == GUI3DTextJustify.JustifyCenter)
				{
					return Text.transform.localPosition.x + Text.TextWidth / 2f + GuiObject.ObjectSize.x * GuiObject.transform.localScale.x / 2f + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyLeft || Text.Justify == GUI3DTextJustify.JustifyNone)
				{
					return Text.transform.localPosition.x + Text.TextWidth + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyRight)
				{
					return Text.transform.localPosition.x + DistanceFromText;
				}
			}
		}
		else if (AlignedText != null)
		{
			if (Aligned == Align.Left)
			{
				if (Text.Justify == GUI3DTextJustify.JustifyCenter)
				{
					return Text.transform.localPosition.x - Text.TextWidth / 2f - AlignedText.TextBoxWidth * AlignedText.transform.localScale.x / 2f + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyLeft || Text.Justify == GUI3DTextJustify.JustifyNone)
				{
					return Text.transform.localPosition.x - AlignedText.TextBoxWidth * AlignedText.transform.localScale.x / 2f + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyRight)
				{
					return Text.transform.localPosition.x - Text.TextWidth + DistanceFromText;
				}
			}
			else
			{
				if (Text.Justify == GUI3DTextJustify.JustifyCenter)
				{
					return Text.transform.localPosition.x + Text.TextWidth / 2f + AlignedText.TextBoxWidth * AlignedText.transform.localScale.x / 2f + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyLeft || Text.Justify == GUI3DTextJustify.JustifyNone)
				{
					return Text.transform.localPosition.x + Text.TextWidth + DistanceFromText;
				}
				if (Text.Justify == GUI3DTextJustify.JustifyRight)
				{
					return Text.transform.localPosition.x + DistanceFromText;
				}
			}
		}
		return 0f;
	}
}
