using System.Collections.Generic;
using UnityEngine;

public class GUI3DPanel : MonoBehaviour, IGUI3DPanel
{
	public int MIN_DISTANCE_FOR_DRAG = 10;

	public float ClickTime = 0.2f;

	public bool AutoAdjustPosition = true;

	public GUI3DAdjustScale AutoAdjustScale = GUI3DAdjustScale.StretchMinToFitAspect;

	public int ReferenceScreenWidth = 1024;

	public int ReferenceScreenHeight = 768;

	public bool CheckEvents = true;

	[HideInInspector]
	public GUI3D GUI3D;

	private GUI3DJustifyHorizontal justifyHorizontal;

	private GUI3DJustifyVertical justifyVertical;

	private bool visible = true;

	private List<IGUI3DInteractiveObject> controls;

	private IGUI3DInteractiveObject draggingControl;

	private IGUI3DInteractiveObject currentControl;

	private IGUI3DInteractiveObject mouseOverControl;

	private Vector3 lastPosition;

	private bool isDragging;

	private Vector3 origPosition;

	private bool changedJustify = true;

	private bool lastButtonState;

	public GUI3DJustifyHorizontal JustifyHorizontal
	{
		get
		{
			changedJustify = false;
			return justifyHorizontal;
		}
		set
		{
			justifyHorizontal = value;
			changedJustify = true;
		}
	}

	public GUI3DJustifyVertical JustifyVertical
	{
		get
		{
			changedJustify = false;
			return justifyVertical;
		}
		set
		{
			justifyVertical = value;
			changedJustify = true;
		}
	}

	public bool ChangedJustify
	{
		get
		{
			return changedJustify;
		}
	}

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value || base.gameObject.activeInHierarchy != value)
			{
				base.gameObject.SetActive(value);
				visible = value;
			}
		}
	}

	protected virtual void Awake()
	{
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>(true);
		RefreshInteractiveObjects();
		if (AutoAdjustPosition)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = localPosition.x / (float)ReferenceScreenWidth * (float)Screen.width;
			localPosition.y = localPosition.y / (float)ReferenceScreenHeight * (float)Screen.height;
			base.transform.localPosition = localPosition;
		}
		if (AutoAdjustScale == GUI3DAdjustScale.None || (Screen.width == ReferenceScreenWidth && Screen.height == ReferenceScreenHeight))
		{
			return;
		}
		Vector3 localScale = base.transform.localScale;
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
			localScale.z = localScale.z / num * num2;
			break;
		case GUI3DAdjustScale.StretchMaxToFitAspect:
			num = Mathf.Max(ReferenceScreenWidth, ReferenceScreenHeight);
			num2 = Mathf.Max(Screen.width, Screen.height);
			localScale.x = localScale.x / num * num2;
			localScale.y = localScale.y / num * num2;
			localScale.z = localScale.z / num * num2;
			break;
		case GUI3DAdjustScale.StretchMinToFitAspect:
			num = Mathf.Min(ReferenceScreenWidth, ReferenceScreenHeight);
			num2 = Mathf.Min(Screen.width, Screen.height);
			localScale.x = localScale.x / num * num2;
			localScale.y = localScale.y / num * num2;
			localScale.z = localScale.z / num * num2;
			break;
		}
		base.transform.localScale = localScale;
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is GUI3DObject)
			{
				GUI3DObject gUI3DObject = (GUI3DObject)monoBehaviour;
				if (gUI3DObject.AutoAdjustScale == GUI3DAdjustScale.None)
				{
					Vector3 localScale2 = gUI3DObject.transform.localScale;
					localScale2.x /= localScale.x;
					localScale2.y /= localScale.y;
					gUI3DObject.transform.localScale = localScale2;
				}
			}
			else if (monoBehaviour is GUI3DText)
			{
				GUI3DText gUI3DText = (GUI3DText)monoBehaviour;
				if (gUI3DText.AutoAdjustScale == GUI3DAdjustScale.None)
				{
					Vector3 localScale3 = gUI3DText.transform.localScale;
					localScale3.x /= localScale.x;
					localScale3.y /= localScale.y;
					gUI3DText.transform.localScale = localScale3;
				}
			}
			else if (monoBehaviour is GUI3DProgressBar)
			{
				GUI3DProgressBar gUI3DProgressBar = (GUI3DProgressBar)monoBehaviour;
				if (gUI3DProgressBar.AutoAdjustScale == GUI3DAdjustScale.None)
				{
					Vector3 localScale4 = gUI3DProgressBar.transform.localScale;
					localScale4.x /= localScale.x;
					localScale4.y /= localScale.y;
					gUI3DProgressBar.transform.localScale = localScale4;
				}
			}
		}
	}

	public void ApplyAll()
	{
		MonoBehaviour[] componentsInChildren = base.gameObject.GetComponentsInChildren<MonoBehaviour>();
		if (componentsInChildren == null)
		{
			return;
		}
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is GUI3DObject)
			{
				GUI3DObject gUI3DObject = (GUI3DObject)monoBehaviour;
				gUI3DObject.AutoAdjustPosition = AutoAdjustPosition;
				gUI3DObject.AutoAdjustScale = AutoAdjustScale;
				gUI3DObject.ReferenceScreenWidth = ReferenceScreenWidth;
				gUI3DObject.ReferenceScreenHeight = ReferenceScreenHeight;
			}
			if (monoBehaviour is GUI3DText)
			{
				GUI3DText gUI3DText = (GUI3DText)monoBehaviour;
				gUI3DText.AutoAdjustPosition = AutoAdjustPosition;
				gUI3DText.AutoAdjustScale = AutoAdjustScale;
			}
			if (monoBehaviour is GUI3DProgressBar)
			{
				GUI3DProgressBar gUI3DProgressBar = (GUI3DProgressBar)monoBehaviour;
				gUI3DProgressBar.AutoAdjustPosition = AutoAdjustPosition;
				gUI3DProgressBar.AutoAdjustScale = AutoAdjustScale;
				gUI3DProgressBar.ReferenceScreenWidth = ReferenceScreenWidth;
				gUI3DProgressBar.ReferenceScreenHeight = ReferenceScreenHeight;
			}
		}
	}

	public void Autosize()
	{
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
		Vector3 zero3 = Vector3.zero;
		MonoBehaviour[] componentsInChildren = base.gameObject.GetComponentsInChildren<MonoBehaviour>();
		List<MonoBehaviour> list = new List<MonoBehaviour>();
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is IGUI3DObject)
			{
				list.Add(monoBehaviour);
			}
		}
		if (list == null || list.Count <= 0)
		{
			return;
		}
		zero = list[0].transform.localPosition - ((IGUI3DObject)list[0]).GetObjectSize() / 2f;
		zero2 = list[0].transform.localPosition + ((IGUI3DObject)list[0]).GetObjectSize() / 2f;
		foreach (MonoBehaviour item in list)
		{
			zero.x = Mathf.Min(zero.x, item.transform.localPosition.x - ((IGUI3DObject)item).GetObjectSize().x / 2f);
			zero.y = Mathf.Min(zero.y, item.transform.localPosition.y - ((IGUI3DObject)item).GetObjectSize().y / 2f);
			zero.z = Mathf.Min(zero.z, item.transform.localPosition.z - ((IGUI3DObject)item).GetObjectSize().z / 2f);
			zero2.x = Mathf.Max(zero2.x, item.transform.localPosition.x + ((IGUI3DObject)item).GetObjectSize().x / 2f);
			zero2.y = Mathf.Max(zero2.y, item.transform.localPosition.y + ((IGUI3DObject)item).GetObjectSize().y / 2f);
			zero2.z = Mathf.Max(zero2.z, item.transform.localPosition.z + ((IGUI3DObject)item).GetObjectSize().z / 2f);
			zero3 += item.transform.localPosition;
		}
		zero3 /= (float)list.Count;
		if (boxCollider != null)
		{
			boxCollider.center = (zero2 + zero) / 2f;
			boxCollider.size = (zero2 - zero) / 2f;
		}
	}

	public void AddGUI3DObject(IGUI3DObject interactiveObject)
	{
		if (interactiveObject is IGUI3DObject)
		{
			interactiveObject.SetPanel(this);
			if (interactiveObject is IGUI3DInteractiveObject)
			{
				for (int i = 0; i < controls.Count; i++)
				{
					if (interactiveObject.RealPosition().z > controls[i].RealPosition().z)
					{
						controls.Insert(i, (IGUI3DInteractiveObject)interactiveObject);
						break;
					}
				}
			}
		}
		MonoBehaviour[] componentsInChildren = ((MonoBehaviour)interactiveObject).GetComponentsInChildren<MonoBehaviour>(true);
		if (componentsInChildren == null || componentsInChildren.Length <= 0)
		{
			return;
		}
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (!(monoBehaviour is IGUI3DInteractiveObject))
			{
				continue;
			}
			bool flag = false;
			IGUI3DInteractiveObject iGUI3DInteractiveObject = (IGUI3DInteractiveObject)monoBehaviour;
			for (int k = 0; k < controls.Count; k++)
			{
				if (iGUI3DInteractiveObject.RealPosition().z > controls[k].RealPosition().z)
				{
					flag = true;
					controls.Insert(k, iGUI3DInteractiveObject);
					break;
				}
			}
			if (!flag)
			{
				controls.Add(iGUI3DInteractiveObject);
			}
			if (iGUI3DInteractiveObject is IGUI3DObject)
			{
				((IGUI3DObject)iGUI3DInteractiveObject).SetPanel(this);
			}
		}
	}

	public void AddGUI3DObject(IGUI3DInteractiveObject interactiveObject)
	{
		AddGUI3DObject((IGUI3DObject)interactiveObject);
	}

	public void RemoveGUI3DObject(IGUI3DObject interactiveObject)
	{
		if (interactiveObject is IGUI3DInteractiveObject)
		{
			controls.Remove((IGUI3DInteractiveObject)interactiveObject);
		}
		MonoBehaviour[] componentsInChildren = ((MonoBehaviour)interactiveObject).GetComponentsInChildren<MonoBehaviour>();
		if (componentsInChildren == null || componentsInChildren.Length <= 0)
		{
			return;
		}
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is IGUI3DInteractiveObject)
			{
				IGUI3DInteractiveObject iGUI3DInteractiveObject = (IGUI3DInteractiveObject)monoBehaviour;
				controls.Remove(iGUI3DInteractiveObject);
				((IGUI3DObject)iGUI3DInteractiveObject).SetPanel(null);
			}
		}
	}

	public void RemoveGUI3DObject(IGUI3DInteractiveObject interactiveObject)
	{
		RemoveGUI3DObject((IGUI3DObject)interactiveObject);
	}

	public void RefreshInteractiveObjects()
	{
		controls = new List<IGUI3DInteractiveObject>();
		MonoBehaviour[] componentsInChildren = base.gameObject.GetComponentsInChildren<MonoBehaviour>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is IGUI3DInteractiveObject)
			{
				controls.Add((IGUI3DInteractiveObject)monoBehaviour);
			}
			if (monoBehaviour is IGUI3DObject)
			{
				((IGUI3DObject)monoBehaviour).SetPanel(this);
			}
		}
		OrderObjects();
	}

	private void OrderObjects()
	{
		IGUI3DInteractiveObject[] array = controls.ToArray();
		OrderByZ(array);
		controls.Clear();
		controls.AddRange(array);
	}

	public void OnEscape()
	{
		for (int i = 0; i < controls.Count; i++)
		{
			IGUI3DInteractiveObject iGUI3DInteractiveObject = controls[i];
			if (iGUI3DInteractiveObject is GUI3DButton)
			{
				GUI3DButton gUI3DButton = (GUI3DButton)iGUI3DInteractiveObject;
				if (gUI3DButton.IsBackButton)
				{
					gUI3DButton.OnRelease();
					break;
				}
			}
		}
	}

	public void OnMouseIn(Vector3 position)
	{
		if (controls == null || controls.Count == 0)
		{
			return;
		}
		bool button = Input.GetButton("Fire1");
		if (button)
		{
			if (!lastButtonState && !isDragging)
			{
				lastPosition = position;
				lastButtonState = button;
			}
			if (isDragging)
			{
				draggingControl.OnDrag(position - lastPosition);
				lastPosition = position;
				return;
			}
		}
		for (int i = 0; i < controls.Count; i++)
		{
			IGUI3DInteractiveObject iGUI3DInteractiveObject = controls[i];
			Vector3 vector = iGUI3DInteractiveObject.RealPosition();
			vector -= base.transform.position;
			if (iGUI3DInteractiveObject.CheckEventsEnabled() && Mathf.Abs(position.x - vector.x) <= iGUI3DInteractiveObject.Size().x / 2f && Mathf.Abs(position.y - vector.y) <= iGUI3DInteractiveObject.Size().y / 2f)
			{
				if (button)
				{
					if (lastPosition != position && iGUI3DInteractiveObject.IsDraggeable() && (lastPosition - position).sqrMagnitude > (float)(MIN_DISTANCE_FOR_DRAG * MIN_DISTANCE_FOR_DRAG))
					{
						if (!isDragging)
						{
							origPosition = ((MonoBehaviour)iGUI3DInteractiveObject).transform.localPosition;
						}
						isDragging = true;
						draggingControl = iGUI3DInteractiveObject;
						lastPosition = position;
						return;
					}
					if (currentControl == null && !(iGUI3DInteractiveObject is GUI3DPageSlider) && !(iGUI3DInteractiveObject is GUI3DListSlider))
					{
						currentControl = iGUI3DInteractiveObject;
					}
					iGUI3DInteractiveObject.OnPress(position);
				}
				else
				{
					iGUI3DInteractiveObject.OnMouseOver();
					mouseOverControl = iGUI3DInteractiveObject;
				}
			}
			else if (iGUI3DInteractiveObject.IsRolledOver() || currentControl == iGUI3DInteractiveObject)
			{
				if (currentControl == iGUI3DInteractiveObject)
				{
					currentControl = null;
				}
				iGUI3DInteractiveObject.OnMouseOut();
				if (iGUI3DInteractiveObject == mouseOverControl)
				{
					mouseOverControl = null;
				}
			}
		}
		lastPosition = position;
	}

	public void OnMouseRelease()
	{
		if (currentControl != null)
		{
			if (!isDragging)
			{
				currentControl.OnClick(lastPosition);
			}
			currentControl.OnRelease();
		}
		if (draggingControl != null)
		{
			draggingControl.OnRelease();
			draggingControl = null;
		}
		isDragging = false;
		currentControl = null;
		lastButtonState = false;
	}

	public void OnMouseReleaseOutside()
	{
		if (currentControl != null)
		{
			currentControl.OnRelease();
			currentControl = null;
		}
		if (isDragging)
		{
			lastPosition = origPosition;
			if (draggingControl != null)
			{
				((MonoBehaviour)draggingControl).transform.localPosition = origPosition;
				draggingControl.OnCancelDrag();
				draggingControl = null;
			}
		}
		isDragging = false;
	}

	public void OnMouseOut()
	{
		if (mouseOverControl != null)
		{
			if (!Input.GetButton("Fire1"))
			{
				mouseOverControl.OnMouseOut();
			}
			mouseOverControl = null;
		}
		if (!isDragging)
		{
			currentControl = null;
		}
	}

	private void OrderByZ(IGUI3DInteractiveObject[] objects)
	{
		if (objects != null && objects.Length != 0)
		{
			OrderByZ(objects, 0, objects.Length - 1);
		}
	}

	private void OrderByZ(IGUI3DInteractiveObject[] objects, int lo, int hi)
	{
		int num = (lo + hi) / 2;
		float z = objects[num].RealPosition().z;
		int num2 = lo;
		int num3 = hi;
		while (true)
		{
			if (objects[num2].RealPosition().z < z)
			{
				num2++;
				continue;
			}
			while (objects[num3].RealPosition().z > z)
			{
				num3--;
			}
			if (num2 <= num3)
			{
				IGUI3DInteractiveObject iGUI3DInteractiveObject = objects[num2];
				objects[num2] = objects[num3];
				objects[num3] = iGUI3DInteractiveObject;
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
			OrderByZ(objects, lo, num3);
		}
		if (hi > num2)
		{
			OrderByZ(objects, num2, hi);
		}
	}
}
