using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class PickupPlacer : MonoBehaviour
{
	private const string PICKUP_PLACER_NAME = "PickupPlacer";

	private static PickupPlacer instance;

	public string levelName;

	public float placePickupsBetween = 0.5f;

	public float splineStep = 0.05f;

	public bool useSplines = true;

	public bool showTotalNumCoins = true;

	private bool record;

	private List<List<Vector3>> paths;

	private List<Spline> splines;

	private float accumTime;

	private Transform playerT;

	private int lastPointIdx;

	private int recCount;

	private Texture recImageNotifier;

	private int goldCount;

	private int silverCount;

	private void Awake()
	{
		instance = this;
		if (levelName == null || levelName == string.Empty)
		{
			levelName = Application.loadedLevelName;
		}
	}

	private void Start()
	{
		recImageNotifier = Resources.Load("PickupPaths/PathRecEditorImage", typeof(Texture)) as Texture;
		recCount = 0;
	}

	private void OnDisable()
	{
		if (Application.isEditor && Application.isPlaying && recCount > 0)
		{
			savePathsToFile();
		}
	}

	private void OnGUI()
	{
		if (record)
		{
			GUI.DrawTexture(new Rect(10f, 100f, recImageNotifier.width, recImageNotifier.height), recImageNotifier);
		}
		if (Application.isEditor && !showTotalNumCoins)
		{
		}
	}

	private void Update()
	{
		UpdateCointCount();
		if (!Application.isEditor)
		{
			return;
		}
		if (Application.isPlaying)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				record = !record;
				if (record)
				{
					startRecording();
				}
			}
			if (record)
			{
				accumTime += Time.deltaTime;
				if (accumTime >= placePickupsBetween)
				{
					addPoint();
					accumTime %= placePickupsBetween;
				}
			}
		}
		else
		{
			drawPaths();
		}
	}

	public void UpdateCointCount()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Pickup");
		goldCount = 0;
		silverCount = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.StartsWith("Gold"))
			{
				goldCount++;
			}
			else if (array[i].name.StartsWith("Silver"))
			{
				silverCount++;
			}
		}
	}

	private void startRecording()
	{
		recCount++;
		if (paths == null)
		{
			paths = new List<List<Vector3>>();
		}
		paths.Add(new List<Vector3>());
		accumTime = 0f;
		addPoint();
	}

	private void addPoint()
	{
		if (playerT == null)
		{
			playerT = CharHelper.GetPlayerTransform();
		}
		paths[paths.Count - 1].Add(playerT.position);
	}

	public static void SavePathsToFile()
	{
		findInstance();
		instance.savePathsToFile();
	}

	private void savePathsToFile()
	{
		StreamWriter streamWriter = new StreamWriter(string.Format("Assets/Resources/PickupPaths/{0}.txt", levelName));
		for (int i = 0; i < paths.Count; i++)
		{
			streamWriter.WriteLine(string.Format("Path{0}", i));
			for (int j = 0; j < paths[i].Count; j++)
			{
				streamWriter.WriteLine(string.Format("{0},{1},{2}", paths[i][j].x, paths[i][j].y, paths[i][j].z));
			}
		}
		streamWriter.Close();
	}

	public static void UpdatePathStructureFromGOs()
	{
		findInstance();
		instance.updatePathStructureFromGOs();
	}

	private void updatePathStructureFromGOs()
	{
		PickupPath[] componentsInChildren = GetComponentsInChildren<PickupPath>();
		paths = new List<List<Vector3>>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			paths.Add(new List<Vector3>());
			Transform[] componentsInChildren2 = componentsInChildren[i].gameObject.GetComponentsInChildren<Transform>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren2[j].name.StartsWith("Point"))
				{
					paths[paths.Count - 1].Add(componentsInChildren2[j].position);
				}
			}
		}
	}

	public static void LoadPickupPathsFromFile()
	{
		findInstance();
		instance.loadPickupPathsFromFile();
	}

	private static void findInstance()
	{
		if (instance == null)
		{
			instance = GameObject.Find("PickupPlacer").GetComponent<PickupPlacer>();
			if (instance == null)
			{
				Debug.LogError(string.Format("Cannot find {0} game object on scene.", "PickupPlacer"));
			}
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void loadPickupPathsFromFile()
	{
		GameObject gameObject = null;
		List<Vector3> list = null;
		paths = new List<List<Vector3>>();
		splines = new List<Spline>();
		StreamReader streamReader = new StreamReader(string.Format("Assets/Resources/PickupPaths/{0}.txt", levelName));
		string text = streamReader.ReadLine();
		List<GameObject> list2 = null;
		while (text != null)
		{
			if (text.StartsWith("Path"))
			{
				list = new List<Vector3>();
				paths.Add(list);
				Spline spline = new Spline();
				splines.Add(spline);
				gameObject = new GameObject(string.Format("Path{0}", paths.Count - 1));
				gameObject.transform.parent = base.transform;
				PickupPath pickupPath = gameObject.AddComponent<PickupPath>();
				pickupPath.path = list;
				pickupPath.spline = spline;
				list2 = new List<GameObject>();
			}
			else
			{
				string[] array = text.Split(',');
				Vector3 vector = new Vector3(Convert.ToSingle(array[0]), Convert.ToSingle(array[1]), Convert.ToSingle(array[2]));
				list.Add(vector);
				GameObject gameObject2 = new GameObject(string.Format("Point{0}", list.Count - 1));
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.position = vector;
				gameObject2.transform.rotation = Quaternion.identity;
				list2.Add(gameObject2);
				splines[instance.paths.Count - 1].Update(list2);
			}
			text = streamReader.ReadLine();
		}
		streamReader.Close();
	}

	public static void ClearGOs()
	{
		findInstance();
		instance.clearGOs();
	}

	private void clearGOs()
	{
		List<GameObject> list = new List<GameObject>();
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name.StartsWith("Path"))
			{
				list.Add(componentsInChildren[i].gameObject);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			UnityEngine.Object.DestroyImmediate(list[j]);
		}
		paths = null;
		splines = null;
	}

	private void drawPaths()
	{
		if (paths == null)
		{
			return;
		}
		for (int i = 0; i < paths.Count; i++)
		{
			if (useSplines)
			{
				int val;
				Vector3 start = splines[i].GetPointAtDistance(0f, out val);
				float num = splines[i].CalculateLength();
				for (float num2 = splineStep; num2 <= num; num2 += splineStep)
				{
					Vector3 pointAtDistance = splines[i].GetPointAtDistance(num2, out val);
					Debug.DrawLine(start, pointAtDistance, Color.green);
					start = pointAtDistance;
				}
			}
			else
			{
				for (int j = 0; j < paths[i].Count - 1; j++)
				{
					Debug.DrawLine(paths[i][j], paths[i][j + 1], Color.green);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (paths == null)
		{
			return;
		}
		for (int i = 0; i < paths.Count; i++)
		{
			for (int j = 0; j < paths[i].Count - 1; j++)
			{
				Gizmos.DrawCube(paths[i][j], new Vector3(0.1f, 0.1f, 0.1f));
			}
		}
	}
}
