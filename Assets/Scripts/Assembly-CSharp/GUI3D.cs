using System;
using System.Collections.Generic;
using UnityEngine;

public class GUI3D : MonoBehaviour
{
	public delegate void OnEvent(GUI3DEvent evt);

	public Camera GUICamera;

	public Camera[] GUICameras;

	public bool Visible;

	public bool CheckEvents = true;

	public bool AlwaysVisible;

	public bool AutoAdjustPosition = true;

	public GUI3DAdjustScale AutoAdjustScale = GUI3DAdjustScale.StretchMinToFitAspect;

	public int ReferenceScreenWidth = 1024;

	public int ReferenceScreenHeight = 768;

	private Ray ray;

	private RaycastHit hit;

	private Collider currentCollider;

	private IGUI3DPanel currentPanel;

	private IGUI3DPanel currentPressedPanel;

	private GUI3DPanel[] panels;

	private static bool isOverGUI;

	private Dictionary<string, List<OnEvent>> listeners = new Dictionary<string, List<OnEvent>>();

	private GUI3DOnActivate onActivate = new GUI3DOnActivate();

	private GUI3DOnDeactivate onDeactivate = new GUI3DOnDeactivate();

	public string LocaleXML
	{
		get
		{
			return DedalordLoadLevel.GetCurrentScene() + "_" + base.name;
		}
	}

	public static bool IsOverGUI()
	{
		return isOverGUI;
	}

	protected virtual void Awake()
	{
		isOverGUI = false;
		panels = base.gameObject.GetComponentsInChildren<GUI3DPanel>();
		if (GUICamera == null)
		{
			GUICamera = GetComponentInChildren<Camera>();
		}
		GUICamera.gameObject.layer = 17;
		GUI3DPanel[] array = panels;
		foreach (GUI3DPanel gUI3DPanel in array)
		{
			gUI3DPanel.GUI3D = this;
			Justify(gUI3DPanel);
		}
		if (AutoAdjustPosition)
		{
			Vector3 localPosition = GUICamera.transform.localPosition;
			localPosition.x = localPosition.x / (float)ReferenceScreenWidth * (float)Screen.width;
			localPosition.y = localPosition.y / (float)ReferenceScreenHeight * (float)Screen.height;
			GUICamera.transform.localPosition = localPosition;
			GUICamera.orthographicSize = GUICamera.orthographicSize / (float)ReferenceScreenHeight * (float)Screen.height;
		}
	}

	private void Start()
	{
		if (GUI3DManager.Instance != null)
		{
			GUI3DManager.Instance.AddGUI3D(this);
		}
	}

	public void ApplyAll()
	{
		panels = base.gameObject.GetComponentsInChildren<GUI3DPanel>();
		GUI3DPanel[] array = panels;
		foreach (GUI3DPanel gUI3DPanel in array)
		{
			gUI3DPanel.AutoAdjustPosition = AutoAdjustPosition;
			gUI3DPanel.AutoAdjustScale = AutoAdjustScale;
			gUI3DPanel.ReferenceScreenWidth = ReferenceScreenWidth;
			gUI3DPanel.ReferenceScreenHeight = ReferenceScreenHeight;
			gUI3DPanel.ApplyAll();
		}
	}

	public void Justify(GUI3DPanel panel)
	{
		if (panel.JustifyHorizontal != GUI3DJustifyHorizontal.JustifyNone || panel.JustifyVertical != GUI3DJustifyVertical.JustifyNone)
		{
			Vector3 position = GUICamera.WorldToScreenPoint(panel.transform.position);
			BoxCollider boxCollider = (BoxCollider)panel.GetComponent<Collider>();
			Vector3 size = boxCollider.size;
			Vector3 zero = Vector3.zero;
			if (panel.JustifyHorizontal == GUI3DJustifyHorizontal.JustifyLeft)
			{
				position.x = 0f;
				zero.x = size.x / 2f;
			}
			else if (panel.JustifyHorizontal == GUI3DJustifyHorizontal.JustifyRight)
			{
				position.x = Screen.width;
				zero.x = (0f - size.x) / 2f;
			}
			else if (panel.JustifyHorizontal == GUI3DJustifyHorizontal.JustifyCenter)
			{
				position.x = Screen.width / 2;
			}
			if (panel.JustifyVertical == GUI3DJustifyVertical.JustifyTop)
			{
				position.y = Screen.height;
				zero.y = (0f - size.y) / 2f;
			}
			else if (panel.JustifyVertical == GUI3DJustifyVertical.JustifyBottom)
			{
				position.y = 0f;
				zero.y = size.y / 2f;
			}
			else if (panel.JustifyVertical == GUI3DJustifyVertical.JustifyCenter)
			{
				position.y = Screen.height / 2;
			}
			panel.transform.position = GUICamera.ScreenToWorldPoint(position) + zero - boxCollider.center;
		}
	}

	private void Update()
	{
		if (!CheckEvents)
		{
			currentPressedPanel = null;
			return;
		}
		if (Input.GetKeyUp(KeyCode.Escape) || (MogaInput.Instance.IsConnected() && MogaInput.Instance.GetButtonBDown()))
		{
			GUI3DPanel[] array = panels;
			foreach (GUI3DPanel gUI3DPanel in array)
			{
				if (gUI3DPanel.CheckEvents)
				{
					gUI3DPanel.OnEscape();
				}
			}
		}
		bool button = Input.GetButton("Fire1");
		Vector3 position;
		IGUI3DPanel panel;
		isOverGUI = CheckForPanel(Input.mousePosition, out position, out panel);
		if ((Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) || button)
		{
			if (isOverGUI)
			{
				if (panel != currentPanel)
				{
					if (currentPanel != null)
					{
						currentPanel.OnMouseOut();
					}
					currentPanel = panel;
				}
				if (currentPressedPanel == null && button)
				{
					currentPressedPanel = currentPanel;
				}
				currentPanel.OnMouseIn(position);
			}
			else
			{
				if (currentPressedPanel != null && !button)
				{
					currentPressedPanel.OnMouseReleaseOutside();
					currentPressedPanel = null;
				}
				if (currentPanel != null)
				{
					currentPanel.OnMouseOut();
					currentPanel = null;
				}
			}
		}
		if (currentPressedPanel == null || button)
		{
			return;
		}
		if (isOverGUI)
		{
			if (panel == currentPressedPanel)
			{
				currentPressedPanel.OnMouseRelease();
			}
			else
			{
				currentPressedPanel.OnMouseReleaseOutside();
			}
		}
		else
		{
			currentPressedPanel.OnMouseReleaseOutside();
		}
		currentPressedPanel = null;
	}

	private bool CheckForPanel(Vector3 mousePosition, out Vector3 position, out IGUI3DPanel panel)
	{
		position = Vector3.zero;
		panel = null;
		if (CheckForPanel(GUICamera, mousePosition, out position, out panel))
		{
			return true;
		}
		Camera[] gUICameras = GUICameras;
		foreach (Camera cam in gUICameras)
		{
			if (CheckForPanel(cam, mousePosition, out position, out panel))
			{
				return true;
			}
		}
		return panel != null;
	}

	private bool CheckForPanel(Camera cam, Vector3 mousePosition, out Vector3 position, out IGUI3DPanel panel)
	{
		ray = cam.ScreenPointToRay(mousePosition);
		position = Vector3.zero;
		panel = null;
		currentCollider = null;
		RaycastHit[] array = Physics.RaycastAll(ray, 1000f, 1 << cam.gameObject.layer);
		int num = 0;
		int num2 = 0;
		while (array != null && num2 < array.Length)
		{
			hit = array[num2];
			if (hit.collider.tag == "GUI3DPanel" && (currentCollider == null || currentCollider.transform.position.z > hit.collider.transform.position.z) && (currentCollider != hit.collider || panel == null))
			{
				currentCollider = hit.collider;
				GUI3DPanel component = currentCollider.gameObject.GetComponent<GUI3DPanel>();
				if (component.CheckEvents)
				{
					panel = component;
					num = num2;
				}
			}
			num2++;
		}
		if (panel != null)
		{
			hit = array[num];
			Vector3 point = hit.point;
			point -= hit.collider.transform.position;
			point = hit.collider.transform.InverseTransformDirection(point);
			point.z = 0f;
			position = point;
			return true;
		}
		return false;
	}

	private void DebugDraw(GUI3DPanel panel)
	{
		Vector3 size = panel.GetComponent<Collider>().bounds.size;
		Vector3 vector = panel.transform.position - Vector3.right * size.x * 0.5f + Vector3.up * size.y * 0.5f;
		Vector3 vector2 = panel.transform.position + Vector3.right * size.x * 0.5f + Vector3.up * size.y * 0.5f;
		Vector3 vector3 = panel.transform.position + Vector3.right * size.x * 0.5f - Vector3.up * size.y * 0.5f;
		Vector3 vector4 = panel.transform.position - Vector3.right * size.x * 0.5f - Vector3.up * size.y * 0.5f;
		Debug.DrawLine(vector, vector2, Color.yellow);
		Debug.DrawLine(vector2, vector3, Color.yellow);
		Debug.DrawLine(vector3, vector4, Color.yellow);
		Debug.DrawLine(vector4, vector, Color.yellow);
	}

	private void OnDrawGizmos()
	{
		panels = base.gameObject.GetComponentsInChildren<GUI3DPanel>();
		if (panels == null)
		{
			return;
		}
		GUI3DPanel[] array = panels;
		foreach (GUI3DPanel gUI3DPanel in array)
		{
			if (gUI3DPanel.ChangedJustify)
			{
				Justify(gUI3DPanel);
			}
		}
	}

	public void SetActive(bool active)
	{
		if (CheckEvents != active)
		{
			if (active)
			{
				Dispatch(onActivate);
			}
			else
			{
				Dispatch(onDeactivate);
			}
		}
		CheckEvents = active;
		base.enabled = active;
	}

	public void SetVisible(bool visible)
	{
		if (visible != base.gameObject.activeInHierarchy)
		{
			if (visible)
			{
				base.gameObject.SetActive(true);
				Dispatch(onActivate);
			}
			else
			{
				Dispatch(onDeactivate);
				base.gameObject.SetActive(false);
			}
			Visible = visible;
		}
	}

	public void Dispatch(GUI3DEvent evt)
	{
		if (!listeners.ContainsKey(evt.Name))
		{
			return;
		}
		evt.Target = this;
		foreach (OnEvent item in listeners[evt.Name])
		{
			item(evt);
		}
	}

	public void AddListener(OnEvent listener, string evt)
	{
		if (!listeners.ContainsKey(evt))
		{
			listeners[evt] = new List<OnEvent>();
		}
		listeners[evt].Add(listener);
	}

	public void RemoveListener(OnEvent listener, string evt)
	{
		if (listeners.ContainsKey(evt))
		{
			listeners[evt].Remove(listener);
		}
	}

	public static IGUI3DObject CreateObject(Type type)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "GUI3DObject";
		gameObject.tag = "GUI";
		gameObject.layer = LayerMask.NameToLayer("GUI");
		return (IGUI3DObject)gameObject.AddComponent(type);
	}
}
