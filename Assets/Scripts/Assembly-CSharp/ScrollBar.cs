using UnityEngine;

public class ScrollBar : MonoBehaviour
{
	public Vector3 StartPos;

	public Vector3 EndPos;

	public GUI3DListSlider ListSlider;

	public GUI3DObject Background;

	private GUI3DButton button;

	private Vector3 position;

	private Vector3 listSliderPos;

	private float percent;

	private void OnEnable()
	{
		if (button == null)
		{
			button = GetComponent<GUI3DButton>();
		}
		button.DragEvent += OnDrag;
		position = base.transform.localPosition;
		listSliderPos = ListSlider.transform.localPosition;
		ListSlider.Draggeable = false;
		button.CreateOwnMesh = true;
		Background.CreateOwnMesh = true;
	}

	private void OnDisable()
	{
		if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
		{
			if (button == null)
			{
				button = GetComponent<GUI3DButton>();
			}
			button.DragEvent -= OnDrag;
			ListSlider.Draggeable = true;
		}
	}

	private void OnDrag(GUI3DOnDragEvent evt)
	{
		position += evt.RelativePosition;
		if (position.x < StartPos.x)
		{
			position.x = StartPos.x;
		}
		if (position.x > EndPos.x)
		{
			position.x = EndPos.x;
		}
		if (position.y < StartPos.y)
		{
			position.y = StartPos.y;
		}
		if (position.y > EndPos.y)
		{
			position.y = EndPos.y;
		}
		float num = EndPos.x - StartPos.x;
		float num2 = EndPos.y - StartPos.y;
		if (num > num2)
		{
			percent = (position.x - StartPos.x) / num;
			listSliderPos.x = ListSlider.StartPos.x + (ListSlider.EndPos.x - ListSlider.StartPos.x) * percent;
		}
		else
		{
			percent = 1f - (position.y - StartPos.y) / num2;
			listSliderPos.y = ListSlider.StartPos.y + (ListSlider.EndPos.y - ListSlider.StartPos.y) * percent;
		}
		ListSlider.transform.localPosition = listSliderPos;
		base.transform.localPosition = position;
	}
}
