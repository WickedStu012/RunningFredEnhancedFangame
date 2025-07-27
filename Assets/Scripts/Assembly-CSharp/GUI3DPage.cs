using System.Collections.Generic;
using UnityEngine;

public class GUI3DPage : GUI3DObject
{
	public int Id;

	public Vector2 SpaceBetweenItems = new Vector2(5f, 0f);

	public int ItemsCountPerWidth = 3;

	public int ItemsCountPerHeight = 2;

	private int objectsCount;

	private List<GUI3DObject> objects = new List<GUI3DObject>();

	public int ObjectsCount
	{
		get
		{
			return objectsCount;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Refresh();
	}

	public void Refresh()
	{
		GUI3DObject[] componentsInChildren = GetComponentsInChildren<GUI3DObject>(true);
		objects.Clear();
		if (componentsInChildren == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != this && componentsInChildren[i].transform.parent == base.transform)
			{
				objects.Add(componentsInChildren[i]);
				Vector3 localPosition = default(Vector3);
				Vector3 objectSize = componentsInChildren[i].GetObjectSize();
				objectSize.x *= componentsInChildren[i].transform.localScale.x;
				objectSize.y *= componentsInChildren[i].transform.localScale.y;
				Vector3 vector = objectSize;
				vector.x += SpaceBetweenItems.x;
				vector.y += SpaceBetweenItems.y;
				vector.x *= ItemsCountPerWidth - 1;
				vector.y *= ItemsCountPerHeight - 1;
				vector /= 2f;
				localPosition.x = 0f - vector.x + (float)(num % ItemsCountPerWidth) * (objectSize.x + SpaceBetweenItems.x);
				localPosition.y = vector.y - (float)(num / ItemsCountPerWidth) * (objectSize.y + SpaceBetweenItems.y);
				localPosition.z = componentsInChildren[i].transform.localPosition.z;
				componentsInChildren[i].transform.localPosition = localPosition;
				num++;
			}
		}
		objectsCount = num;
	}

	public void AddGUIObject(GUI3DObject obj)
	{
		Vector3 localScale = base.transform.localScale;
		obj.transform.parent = base.transform;
		obj.transform.localScale = localScale;
		obj.AutoAdjustScale = AutoAdjustScale;
		objects.Add(obj);
		objectsCount++;
	}

	public void RemoveGUIObject(GUI3DObject obj)
	{
		obj.transform.parent = null;
		objects.Remove(obj);
		objectsCount--;
	}

	public void RemoveAll()
	{
		objects.Clear();
		objectsCount = 0;
	}

	public GUI3DObject[] GetItems()
	{
		return objects.ToArray();
	}

	public bool IsFull()
	{
		return objectsCount >= ItemsCountPerWidth * ItemsCountPerHeight;
	}

	public bool IsEmpty()
	{
		return objectsCount == 0;
	}
}
