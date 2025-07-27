using System.Collections.Generic;
using UnityEngine;

public class GUI3DManager : MonoBehaviour
{
	private class GUIStates
	{
		public GUI3D Gui;

		public bool Active;

		public bool Visible;

		public GUIStates(GUI3D gui, bool active, bool visible)
		{
			Gui = gui;
			Active = active;
			Visible = visible;
		}
	}

	public float ReferenceScreenWidth = 1024f;

	public float ReferenceScreenHeight = 768f;

	public float MaxDeltaTime = 0.025f;

	public string[] PreloadAtlas;

	private static GUI3DManager instance;

	private GUI3D[] GUI3Ds;

	private Stack<List<GUIStates>> lastStates = new Stack<List<GUIStates>>();

	private List<GUI3D> GUI3DList = new List<GUI3D>();

	private Dictionary<string, GUI3D> GUI3DsByName = new Dictionary<string, GUI3D>();

	private float lastTime;

	private float deltaTime;

	private int callActivate;

	private string _guiName;

	private GUI3D _gui;

	private bool _disableOthers;

	private bool _hideOthers;

	public static GUI3DManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType(typeof(GUI3DManager)) as GUI3DManager;
				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "GUI3DManager";
					instance = gameObject.AddComponent<GUI3DManager>();
				}
			}
			return instance;
		}
	}

	public float DeltaTime
	{
		get
		{
			return deltaTime;
		}
	}

	private void Awake()
	{
		instance = this;
		if (PreloadAtlas != null && PreloadAtlas.Length > 0)
		{
			string[] preloadAtlas = PreloadAtlas;
			foreach (string atlas in preloadAtlas)
			{
				GUI3DGlobalParameters.Instance.PreloadAtlas(atlas);
			}
		}
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
		MonoBehaviour[] array = componentsInChildren;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour is IGUI3DInit)
			{
				((IGUI3DInit)monoBehaviour).Init();
			}
		}
	}

	private void OnDestroy()
	{
		instance = null;
		if (base.tag != "LoadingObjs")
		{
			GUI3DGlobalParameters.Instance.OnDisable();
		}
	}

	public void AddGUI3D(GUI3D g)
	{
		if (GUI3DsByName.ContainsKey(g.name))
		{
			Debug.LogError("Another GUI with the same name already exists!");
		}
		if (!GUI3DList.Contains(g))
		{
			GUI3DList.Add(g);
			GUI3Ds = GUI3DList.ToArray();
			GUI3DsByName[g.name] = g;
			g.transform.parent = base.transform;
			if (!(g is GUI3DPopupManager) && !g.Visible)
			{
				g.SetVisible(false);
			}
		}
	}

	public GUI3D Activate(string guiName, bool disableOthers, bool hideOthers)
	{
		if (GUI3DPopupManager.Instance.CurrentPopup != null)
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Cancel, true);
			callActivate = 10;
			_guiName = guiName;
			_disableOthers = disableOthers;
			_hideOthers = hideOthers;
			return null;
		}
		return activateByName(guiName, disableOthers, hideOthers);
	}

	public GUI3D GetGUI3DByName(string guiName)
	{
		if (!GUI3DsByName.ContainsKey(guiName))
		{
			return null;
		}
		return GUI3DsByName[guiName];
	}

	private GUI3D activateByName(string guiName, bool disableOthers, bool hideOthers)
	{
		if (!GUI3DsByName.ContainsKey(guiName))
		{
			Debug.LogError("There's no GUI3D called \"" + guiName + "\"!");
			return null;
		}
		GUI3D gUI3D = GUI3DsByName[guiName];
		activateByGUI(gUI3D, disableOthers, hideOthers);
		return gUI3D;
	}

	private void activateByGUI(GUI3D gui, bool disableOthers, bool hideOthers)
	{
		if (gui != null)
		{
			gui.SetActive(true);
			gui.SetVisible(true);
		}
		if (!disableOthers && !hideOthers)
		{
			return;
		}
		GUI3D[] gUI3Ds = GUI3Ds;
		foreach (GUI3D gUI3D in gUI3Ds)
		{
			if (gUI3D != gui && !(gUI3D is GUI3DPopupManager))
			{
				if (disableOthers)
				{
					gUI3D.SetActive(false);
				}
				if (hideOthers && !gUI3D.AlwaysVisible)
				{
					gUI3D.SetVisible(false);
				}
			}
		}
	}

	public void Activate(GUI3D gui, bool disableOthers, bool hideOthers)
	{
		if (GUI3DPopupManager.Instance.CurrentPopup != null)
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Cancel, true);
		}
		activateByGUI(gui, disableOthers, hideOthers);
	}

	public bool IsActive(string guiName)
	{
		if (!GUI3DsByName.ContainsKey(guiName))
		{
			Debug.LogError(string.Format("Cannot find: {0}", guiName));
		}
		GUI3D gUI3D = GUI3DsByName[guiName];
		if (gUI3D != null)
		{
			return gUI3D.Visible;
		}
		return false;
	}

	public void SaveCurrentState()
	{
		List<GUIStates> list = new List<GUIStates>();
		GUI3D[] gUI3Ds = GUI3Ds;
		foreach (GUI3D gUI3D in gUI3Ds)
		{
			if (!(gUI3D is GUI3DPopupManager))
			{
				list.Add(new GUIStates(gUI3D, gUI3D.CheckEvents, gUI3D.Visible));
			}
		}
		lastStates.Push(list);
	}

	public void RestoreLastState()
	{
		if (lastStates.Count <= 0)
		{
			return;
		}
		List<GUIStates> list = lastStates.Pop();
		foreach (GUIStates item in list)
		{
			item.Gui.SetVisible(item.Visible);
			item.Gui.SetActive(item.Active);
		}
	}

	private void OnEnable()
	{
		lastTime = 0f;
		deltaTime = 0f;
	}

	private void Update()
	{
		if (lastTime != 0f)
		{
			deltaTime = Time.realtimeSinceStartup - lastTime;
		}
		if (deltaTime >= MaxDeltaTime)
		{
			deltaTime = MaxDeltaTime;
		}
		lastTime = Time.realtimeSinceStartup;
		if (callActivate > 0)
		{
			callActivate--;
			if (callActivate == 0)
			{
				activateByName(_guiName, _disableOthers, _hideOthers);
			}
		}
	}
}
