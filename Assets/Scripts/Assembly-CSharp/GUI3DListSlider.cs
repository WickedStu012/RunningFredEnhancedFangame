using System.Collections.Generic;
using UnityEngine;

public class GUI3DListSlider : GUI3DInteractiveObject
{
	public bool IsVertical;

	public float SpaceBetweenItems = 5f;

	public float AccelFactor = 250f;

	public Vector3 StartPos;

	public Vector3 EndPos;

	public Vector3 VisibleSize;

	private float currentSpeed;

	private List<GUI3DObject> items = new List<GUI3DObject>();

	private bool dragging;

	private Vector3 position;

	private Vector3 rounded;

	private Vector3 originalPosition;

	private Vector3 lastPos;

	protected override void Awake()
	{
		base.Awake();
		originalPosition = (position = base.transform.localPosition);
		base.transform.localPosition = Vector3.zero;
	}

	private void OnEnable()
	{
		position = base.transform.localPosition;
		lastPos = position;
	}

	private void Refresh()
	{
		items.Clear();
		GUI3DObject[] componentsInChildren = GetComponentsInChildren<GUI3DObject>();
		GUI3DObject[] array = componentsInChildren;
		foreach (GUI3DObject item in array)
		{
			AddItem(item);
		}
		RelocateItems();
	}

	private void Update()
	{
		if (!Draggeable)
		{
			return;
		}
		if (!dragging && currentSpeed != 0f)
		{
			if (IsVertical)
			{
				position.y += currentSpeed * GUI3DManager.Instance.DeltaTime;
				if (position.y > EndPos.y)
				{
					position.y = EndPos.y;
				}
				else if (position.y < StartPos.y)
				{
					position.y = StartPos.y;
				}
			}
			else
			{
				position.x += currentSpeed * GUI3DManager.Instance.DeltaTime;
				if (position.x > StartPos.x)
				{
					position.x = StartPos.x;
				}
				else if (position.x < EndPos.x)
				{
					position.x = EndPos.x;
				}
			}
			if (currentSpeed > 0f)
			{
				currentSpeed -= AccelFactor * GUI3DManager.Instance.DeltaTime;
				if (currentSpeed < 0f)
				{
					currentSpeed = 0f;
				}
			}
			else
			{
				currentSpeed += AccelFactor * GUI3DManager.Instance.DeltaTime;
				if (currentSpeed > 0f)
				{
					currentSpeed = 0f;
				}
			}
		}
		else if (dragging)
		{
			float num = 1f;
			Vector3 vector = lastPos - base.transform.localPosition;
			if (IsVertical)
			{
				if (vector.y < 0f)
				{
					num *= -1f;
				}
				currentSpeed = Mathf.Abs(vector.y) * num;
			}
			else
			{
				if (vector.x < 0f)
				{
					num *= -1f;
				}
				currentSpeed = Mathf.Abs(vector.x) * num;
			}
			lastPos = base.transform.localPosition;
		}
		base.transform.localPosition = position;
	}

	public override void OnDrag(Vector3 relativePosition)
	{
		Vector3 vector = new Vector3(1f, 1f, 1f);
		if (IsVertical)
		{
			relativePosition.x = 0f;
			relativePosition.y /= vector.y;
		}
		else
		{
			relativePosition.x /= vector.x;
			relativePosition.y = 0f;
		}
		if (!dragging)
		{
			originalPosition = position;
		}
		position += relativePosition;
		if (IsVertical)
		{
			if (position.y < StartPos.y)
			{
				position.y = StartPos.y;
			}
			else if (position.y > EndPos.y)
			{
				position.y = EndPos.y;
			}
			relativePosition.y -= base.transform.localPosition.y;
		}
		else
		{
			if (position.x < StartPos.x)
			{
				position.x = StartPos.x;
			}
			else if (position.x > EndPos.x)
			{
				position.x = EndPos.x;
			}
			relativePosition.x -= base.transform.localPosition.x;
		}
		base.OnDrag(relativePosition);
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		if (IsVertical)
		{
			if (rounded.y < StartPos.y)
			{
				rounded.y = StartPos.y;
			}
			else if (rounded.y > EndPos.y)
			{
				rounded.y = EndPos.y;
			}
		}
		else if (rounded.x < StartPos.x)
		{
			rounded.x = StartPos.x;
		}
		else if (rounded.x > EndPos.x)
		{
			rounded.x = EndPos.x;
		}
		base.transform.localPosition = rounded;
		float num = 10f;
		currentSpeed = relativePosition.magnitude * num;
		dragging = true;
	}

	public override void OnRelease()
	{
		base.OnRelease();
		dragging = false;
	}

	public override void OnCancelDrag()
	{
		base.OnCancelDrag();
		position = originalPosition;
	}

	public void AddItem(GUI3DObject item)
	{
		if (IsVertical)
		{
			if (ObjectSize.x == 0f)
			{
				ObjectSize.x = item.ObjectSize.x;
			}
			ObjectSize.y = (item.ObjectSize.y * item.transform.localScale.y + SpaceBetweenItems) * (float)(items.Count + 1);
		}
		else
		{
			if (ObjectSize.y == 0f)
			{
				ObjectSize.y = item.ObjectSize.y;
			}
			ObjectSize.x = (item.ObjectSize.x * item.transform.localScale.x + SpaceBetweenItems) * (float)(items.Count + 1);
		}
		if ((item is IGUI3DInteractiveObject || item is IGUI3DObject) && GetPanel() != null)
		{
			GetPanel().AddGUI3DObject(item);
		}
		items.Add(item);
	}

	public void RelocateItems()
	{
		Vector3 zero = Vector3.zero;
		if (IsVertical)
		{
			zero.y = 0f;
		}
		else
		{
			zero.x = (0f - ObjectSize.x) / 2f;
		}
		for (int i = 0; i < items.Count; i++)
		{
			GUI3DObject gUI3DObject = items[i];
			gUI3DObject.transform.parent = base.transform;
			gUI3DObject.transform.localPosition = new Vector3(0f, 0f, -1f);
			gUI3DObject.transform.localScale = Vector3.one;
			zero.z = -1f;
			Vector3 localPosition = zero;
			if (IsVertical)
			{
				localPosition.y -= (gUI3DObject.ObjectSize.y * gUI3DObject.transform.localScale.y + SpaceBetweenItems) * (float)i + gUI3DObject.ObjectSize.y * gUI3DObject.transform.localScale.y / 2f;
			}
			else
			{
				localPosition.x += (gUI3DObject.ObjectSize.x * gUI3DObject.transform.localScale.x + SpaceBetweenItems) * (float)i;
			}
			gUI3DObject.transform.localPosition = localPosition;
		}
		if (IsVertical)
		{
			if (ObjectSize.y > VisibleSize.y)
			{
				StartPos.y = originalPosition.y;
				EndPos.y = ObjectSize.y - VisibleSize.y + originalPosition.y;
			}
			else
			{
				Draggeable = false;
			}
		}
		else if (ObjectSize.x > VisibleSize.x)
		{
			StartPos.x = (ObjectSize.x - VisibleSize.x) / 2f;
			EndPos.x = (0f - (ObjectSize.x - VisibleSize.x)) / 2f;
		}
		else
		{
			Draggeable = false;
		}
		base.transform.localPosition = originalPosition;
		position = base.transform.localPosition;
	}
}
