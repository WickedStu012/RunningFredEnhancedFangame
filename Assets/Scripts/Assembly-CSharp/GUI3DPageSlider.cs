using System;
using System.Collections.Generic;
using UnityEngine;

public class GUI3DPageSlider : GUI3DInteractiveObject
{
	public delegate void OnPageChange(GUI3DOnPageChange evt);

	public float EaseSpeed = 3000f;

	public float Scaling;

	public float SpaceBetweenPages = 1024f;

	[NonSerialized]
	public float DragUntilTurnPage = 60f;

	public int ItemsCountPerWidth = 3;

	public int ItemsCountPerHeight = 2;

	public Vector2 SpaceBetweenItems = new Vector2(5f, 0f);

	public bool AllowSlideWhenOnePage = true;

	private int currentPage;

	private float currentSpeed;

	private List<GUI3DPage> pagesList = new List<GUI3DPage>();

	private GUI3DPage[] Pages;

	private List<GUI3DObject> items = new List<GUI3DObject>();

	private bool dragging;

	private bool refresh;

	private bool resetVisibility;

	private int currentAddPage;

	private Vector3 position;

	private Vector3 rounded;

	private Vector3 originalPosition;

	private GUI3DOnPageChange pageChangeEvent = new GUI3DOnPageChange();

	public int CurrentPage
	{
		get
		{
			return currentPage;
		}
		set
		{
			if (Pages != null && currentPage != value && value >= 0 && value < Pages.Length && !Pages[value].IsEmpty())
			{
				if (currentPage > 0)
				{
					Pages[currentPage - 1].gameObject.SetActive(false);
				}
				if (Pages[currentPage] != null)
				{
					Pages[currentPage].gameObject.SetActive(false);
				}
				if (currentPage < Pages.Length - 1)
				{
					Pages[currentPage + 1].gameObject.SetActive(false);
				}
				if (value > 0 && value > currentPage)
				{
					Pages[value - 1].gameObject.SetActive(true);
				}
				if (Pages[value] != null)
				{
					Pages[value].gameObject.SetActive(true);
				}
				if (value < Pages.Length - 1 && value < currentPage)
				{
					Pages[value + 1].gameObject.SetActive(true);
				}
				currentPage = value;
				pageChangeEvent.PageNum = currentPage;
				pageChangeEvent.Page = Pages[currentPage];
				if (this.PageChangeEvent != null)
				{
					this.PageChangeEvent(pageChangeEvent);
				}
			}
		}
	}

	public event OnPageChange PageChangeEvent;

	protected override void Awake()
	{
		base.Awake();
		Draggeable = true;
		pageChangeEvent.Target = this;
	}

	private void OnEnable()
	{
		Refresh();
		ResetVisibility();
		position = base.transform.localPosition;
	}

	public void Clear()
	{
		if (pagesList != null)
		{
			pagesList.Clear();
		}
		if (items != null)
		{
			items.Clear();
		}
		if (Pages != null)
		{
			for (int i = 0; i < Pages.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(Pages[i]);
			}
		}
		Pages = null;
		currentPage = 0;
		currentAddPage = 0;
	}

	public void Refresh()
	{
		if (Pages == null || refresh)
		{
			Pages = GetComponentsInChildren<GUI3DPage>(true);
			if (Pages != null)
			{
				GUI3DPage[] pages = Pages;
				foreach (GUI3DPage item in pages)
				{
					pagesList.Add(item);
				}
			}
			else if (pagesList != null)
			{
				pagesList.Clear();
			}
		}
		refresh = false;
		if (Pages != null && Pages.Length > 0)
		{
			OrderPagesById(Pages);
			ObjectSize.x = SpaceBetweenPages * (float)Pages.Length * 2f;
		}
	}

	public void ResetVisibility()
	{
		resetVisibility = false;
		if (Pages == null)
		{
			return;
		}
		for (int i = 0; i < Pages.Length; i++)
		{
			if (i == currentPage)
			{
				if (Pages[i] != null)
				{
					Pages[i].gameObject.SetActive(true);
				}
			}
			else if (Pages[i] != null)
			{
				Pages[i].gameObject.SetActive(false);
			}
			if (Pages[i] != null)
			{
				Pages[i].transform.localPosition = Vector3.right * SpaceBetweenPages * i;
			}
		}
	}

	public bool IsLastPage()
	{
		if (Pages == null)
		{
			return true;
		}
		return currentPage >= Pages.Length - 1;
	}

	public bool IsFirstPage()
	{
		return currentPage <= 0;
	}

	private void Update()
	{
		if (Pages == null)
		{
			Debug.Log("Pages is null");
			return;
		}
		if (refresh)
		{
			Refresh();
		}
		if (resetVisibility)
		{
			ResetVisibility();
		}
		if (Pages == null || Pages.Length == 0)
		{
			return;
		}
		Transform transform = base.transform;
		float num = (float)(-CurrentPage) * SpaceBetweenPages;
		float num2 = ((!(position.x < num)) ? (-1f) : 1f);
		if (Pages[currentPage] == null)
		{
			return;
		}
		Vector3 localScale = Pages[currentPage].transform.localScale;
		localScale.x = (localScale.y = 1f - Scaling * Mathf.Abs((num - position.x) / SpaceBetweenPages));
		localScale.z = 1f;
		Pages[currentPage].transform.localScale = localScale;
		if (dragging || transform.localPosition.x == (float)currentPage * SpaceBetweenPages)
		{
			return;
		}
		currentSpeed = EaseSpeed * (num - position.x) / SpaceBetweenPages;
		position.x += currentSpeed * GUI3DManager.Instance.DeltaTime;
		if (num2 > 0f)
		{
			if (Mathf.Round(position.x) >= num)
			{
				position.x = num;
				if (currentPage > 0 && Pages[currentPage - 1].gameObject.activeInHierarchy)
				{
					Pages[currentPage - 1].gameObject.SetActive(false);
				}
				if (currentPage < Pages.Length - 1 && Pages[currentPage + 1].gameObject.activeInHierarchy)
				{
					Pages[currentPage + 1].gameObject.SetActive(false);
				}
			}
		}
		else if (Mathf.Round(position.x) <= num)
		{
			position.x = num;
			if (currentPage > 0 && Pages[currentPage - 1].gameObject.activeInHierarchy)
			{
				Pages[currentPage - 1].gameObject.SetActive(false);
			}
			if (currentPage < Pages.Length - 1 && Pages[currentPage + 1].gameObject.activeInHierarchy)
			{
				Pages[currentPage + 1].gameObject.SetActive(false);
			}
		}
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		base.transform.localPosition = rounded;
	}

	public override void OnDrag(Vector3 relativePosition)
	{
		if (!AllowSlideWhenOnePage && (Pages == null || Pages.Length <= 1))
		{
			return;
		}
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
		relativePosition.y = 0f;
		if (!dragging)
		{
			originalPosition = position;
		}
		position += relativePosition;
		relativePosition.x -= base.transform.localPosition.x;
		base.OnDrag(relativePosition);
		rounded.x = Mathf.Round(position.x);
		rounded.y = Mathf.Round(position.y);
		rounded.z = Mathf.Round(position.z);
		base.transform.localPosition = rounded;
		if (currentPage > 0)
		{
			if (originalPosition.x > position.x)
			{
				if (Pages[currentPage - 1].gameObject.activeInHierarchy)
				{
					Pages[currentPage - 1].gameObject.SetActive(false);
				}
			}
			else if (!Pages[currentPage - 1].gameObject.activeInHierarchy)
			{
				Pages[currentPage - 1].gameObject.SetActive(true);
			}
		}
		if (currentPage < Pages.Length - 1)
		{
			if (originalPosition.x > position.x)
			{
				if (!Pages[currentPage + 1].gameObject.activeInHierarchy)
				{
					Pages[currentPage + 1].gameObject.SetActive(true);
				}
			}
			else if (Pages[currentPage + 1].gameObject.activeInHierarchy)
			{
				Pages[currentPage + 1].gameObject.SetActive(false);
			}
		}
		dragging = true;
	}

	public override void OnRelease()
	{
		base.OnRelease();
		if (dragging && Mathf.Abs(Mathf.Abs(position.x) - (float)CurrentPage * SpaceBetweenPages) >= DragUntilTurnPage)
		{
			if (position.x > (float)(-CurrentPage) * SpaceBetweenPages)
			{
				CurrentPage--;
			}
			else if (position.x < (float)(-CurrentPage) * SpaceBetweenPages)
			{
				CurrentPage++;
			}
		}
		dragging = false;
	}

	public override void OnCancelDrag()
	{
		base.OnCancelDrag();
		position = originalPosition;
	}

	private void OrderPagesById(GUI3DPage[] objects)
	{
		OrderPagesById(objects, 0, objects.Length - 1);
	}

	private void OrderPagesById(GUI3DPage[] objects, int lo, int hi)
	{
		int num = (lo + hi) / 2;
		int id = objects[num].Id;
		int num2 = lo;
		int num3 = hi;
		while (true)
		{
			if (objects[num2].Id < id)
			{
				num2++;
				continue;
			}
			while (objects[num3].Id > id)
			{
				num3--;
			}
			if (num2 <= num3)
			{
				GUI3DPage gUI3DPage = objects[num2];
				objects[num2] = objects[num3];
				objects[num3] = gUI3DPage;
				num2++;
				num3--;
			}
			if (num2 > num3)
			{
				break;
			}
		}
		if (lo < num3)
		{
			OrderPagesById(objects, lo, num3);
		}
		if (hi > num2)
		{
			OrderPagesById(objects, num2, hi);
		}
	}

	public void AddItem(GUI3DObject item)
	{
		addItem(item);
		items.Add(item);
	}

	private void addItem(GUI3DObject item)
	{
		refresh = true;
		GUI3DPage gUI3DPage;
		if (Pages == null || Pages.Length == 0 || currentAddPage >= Pages.Length || Pages[currentAddPage].IsFull())
		{
			gUI3DPage = (GUI3DPage)GUI3D.CreateObject(typeof(GUI3DPage));
			gUI3DPage.gameObject.name = "Page" + (pagesList.Count + 1);
			gUI3DPage.transform.parent = base.transform;
			gUI3DPage.AutoAdjustScale = AutoAdjustScale;
			Vector3 localPosition = gUI3DPage.transform.localPosition + Vector3.forward * base.transform.position.z + Vector3.right * SpaceBetweenPages * pagesList.Count;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			localPosition.z = Mathf.Round(localPosition.z);
			gUI3DPage.transform.localPosition = localPosition;
			gUI3DPage.transform.localScale = new Vector3(1f, 1f, 1f);
			gUI3DPage.Id = pagesList.Count;
			gUI3DPage.ItemsCountPerWidth = ItemsCountPerWidth;
			gUI3DPage.ItemsCountPerHeight = ItemsCountPerHeight;
			gUI3DPage.SpaceBetweenItems = SpaceBetweenItems;
			pagesList.Add(gUI3DPage);
			Pages = pagesList.ToArray();
		}
		gUI3DPage = Pages[currentAddPage];
		item.gameObject.SetActive(true);
		item.transform.position = Vector3.zero;
		Vector3 localScale = item.transform.localScale;
		gUI3DPage.AddGUIObject(item);
		item.transform.localScale = localScale;
		item.transform.localPosition = Vector3.zero - Vector3.forward;
		gUI3DPage.Refresh();
		if ((item is IGUI3DInteractiveObject || item is IGUI3DObject) && GetPanel() != null)
		{
			GetPanel().AddGUI3DObject(item);
		}
		if (gUI3DPage.IsFull())
		{
			currentAddPage++;
		}
		resetVisibility = true;
	}

	public void SetFilter(string filter)
	{
		CurrentPage = 0;
		currentAddPage = 0;
		string[] array = filter.Split(';');
		if (items.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < items.Count; i++)
		{
			GUI3DObject gUI3DObject = items[i];
			if (GetPanel() != null)
			{
				GetPanel().RemoveGUI3DObject(gUI3DObject);
			}
			if (gUI3DObject.transform.parent != null)
			{
				GUI3DPage component = gUI3DObject.transform.parent.GetComponent<GUI3DPage>();
				component.RemoveGUIObject(gUI3DObject);
			}
			gUI3DObject.gameObject.SetActive(false);
		}
		for (int j = 0; j < items.Count; j++)
		{
			GUI3DObject gUI3DObject2 = items[j];
			if (gUI3DObject2.Tag == string.Empty)
			{
				addItem(gUI3DObject2);
				continue;
			}
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (gUI3DObject2.Tag == text)
				{
					addItem(gUI3DObject2);
					break;
				}
			}
		}
	}

	public void ReplaceItem(GUI3DObject item, GUI3DObject newItem)
	{
		newItem.transform.position = Vector3.zero;
		GUI3DPage component = item.transform.parent.gameObject.GetComponent<GUI3DPage>();
		component.AddGUIObject(newItem);
		newItem.transform.localPosition = item.transform.localPosition;
		newItem.transform.localScale = item.transform.localScale;
		component.RemoveGUIObject(item);
		if ((newItem is IGUI3DInteractiveObject || newItem is IGUI3DObject) && GetPanel() != null)
		{
			GetPanel().AddGUI3DObject(newItem);
		}
		if ((item is IGUI3DInteractiveObject || item is IGUI3DObject) && GetPanel() != null)
		{
			GetPanel().RemoveGUI3DObject(item);
		}
		items.Add(newItem);
		items.Remove(item);
		newItem.gameObject.SetActive(true);
		item.transform.parent = null;
		item.gameObject.SetActive(false);
		resetVisibility = true;
	}

	public void RemoveItem(GUI3DObject item)
	{
		if (item == null)
		{
			Debug.Log("item is null");
			return;
		}
		if (item.transform == null)
		{
			Debug.Log("item.transform is null");
			return;
		}
		if (item.transform.parent == null)
		{
			Debug.Log("item.transform.parent is null");
			return;
		}
		GUI3DPage component = item.transform.parent.gameObject.GetComponent<GUI3DPage>();
		Vector3 localScale = item.transform.localScale;
		component.RemoveGUIObject(item);
		if ((item is IGUI3DInteractiveObject || item is IGUI3DObject) && GetPanel() != null)
		{
			GetPanel().RemoveGUI3DObject(item);
		}
		items.Remove(item);
		item.transform.parent = null;
		item.gameObject.SetActive(false);
		resetVisibility = true;
		foreach (GUI3DObject item2 in items)
		{
			GUI3DPage component2 = item2.transform.parent.gameObject.GetComponent<GUI3DPage>();
			component2.RemoveGUIObject(item2);
			component2.Refresh();
		}
		currentAddPage = 0;
		foreach (GUI3DObject item3 in items)
		{
			item3.transform.localScale = localScale;
			addItem(item3);
		}
	}
}
