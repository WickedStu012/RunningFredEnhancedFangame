using UnityEngine;

[ExecuteInEditMode]
public class WayPath : MonoBehaviour
{
	public enum CameraLook
	{
		UsePointRotation = 0,
		LookAtPath = 1,
		LookAtObject = 2,
		LookAtEndPoint = 3
	}

	public enum TransfInterpType
	{
		Absolute = 0,
		Relative = 1
	}

	public enum BehaveAtEnd
	{
		StayAtLastNode = 0,
		Loop = 1,
		ReturnToFirstNode = 2
	}

	public bool drawOrigin = true;

	public bool drawLines = true;

	public CameraLook camLookAt;

	public GameObject objectToTransform;

	public TransfInterpType moveType;

	public TransfInterpType rotType;

	public BehaveAtEnd behaveAtEnd;

	public GameObject objectToLook;

	public Color lineColor = Color.blue;

	public bool restrictRotationsToYAxisOnly = true;

	protected Transform[] point;

	protected Vector3 originalObjToTransfPos;

	protected Quaternion originalObjToTransRot;

	protected Vector3 _translationDir;

	protected Color splineColor = Color.red;

	protected int currentNode;

	protected float accumTime;

	protected float speed = 1f;

	protected bool isPlaying;

	protected bool isPaused;

	protected void Awake()
	{
		isPlaying = false;
		isPaused = false;
		updatePoints();
		if (objectToTransform != null)
		{
			originalObjToTransfPos = objectToTransform.transform.position;
			originalObjToTransRot = objectToTransform.transform.rotation;
		}
	}

	protected void Update()
	{
		if (!Application.isPlaying)
		{
			updatePoints();
		}
		else if (isPlaying)
		{
			UpdateObjectPosition();
		}
		drawDebugLines();
	}

	private void updatePoints()
	{
		WayPathNode[] componentsInChildren = GetComponentsInChildren<WayPathNode>(true);
		if (point == null || point.Length != componentsInChildren.Length)
		{
			point = new Transform[componentsInChildren.Length];
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			WayPathNode wayPathNode = componentsInChildren[i];
			wayPathNode.name = string.Format("node{0}", i);
			point[i] = wayPathNode.gameObject.transform;
		}
	}

	private void drawDebugLines()
	{
		Component[] componentsInChildren = GetComponentsInChildren(typeof(WayPathNode), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			WayPathNode wayPathNode = componentsInChildren[i] as WayPathNode;
			if (i > 0 && drawLines)
			{
				WayPathNode wayPathNode2 = componentsInChildren[i - 1] as WayPathNode;
				Debug.DrawLine(wayPathNode2.transform.position, wayPathNode.transform.position, lineColor);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (drawOrigin)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(base.gameObject.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
		}
		if (drawLines)
		{
			Component[] componentsInChildren = GetComponentsInChildren(typeof(WayPathNode), true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				WayPathNode wayPathNode = componentsInChildren[i] as WayPathNode;
				Gizmos.DrawIcon(wayPathNode.transform.position, "waypoint.png");
			}
		}
	}

	private bool isCurrentNodeReached()
	{
		if (currentNode < point.Length)
		{
			if (Vector3.Distance(objectToTransform.transform.position, point[currentNode].position) < 0.3f)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void Pause()
	{
		isPaused = true;
		isPlaying = false;
	}

	public void Resume()
	{
		isPaused = false;
		isPlaying = true;
	}

	public bool IsPaused()
	{
		return isPaused;
	}

	public void Play(GameObject objToTransform, bool walkToFirstNode, float spd)
	{
		isPlaying = true;
		objectToTransform = objToTransform;
		speed = spd;
		if (!walkToFirstNode)
		{
			currentNode = 1;
		}
		else
		{
			currentNode = 0;
		}
		if (moveType == TransfInterpType.Absolute)
		{
			objectToTransform.transform.position = point[0].position;
		}
		switch (camLookAt)
		{
		case CameraLook.UsePointRotation:
			if (currentNode != 0)
			{
				objectToTransform.transform.rotation = point[currentNode - 1].rotation;
			}
			break;
		case CameraLook.LookAtPath:
			objectToTransform.transform.LookAt(point[currentNode].transform);
			break;
		case CameraLook.LookAtObject:
			objectToTransform.transform.LookAt(objectToLook.transform.position);
			break;
		case CameraLook.LookAtEndPoint:
			objectToTransform.transform.LookAt(point[point.Length - 1].transform);
			break;
		}
		if (restrictRotationsToYAxisOnly)
		{
			objectToTransform.transform.eulerAngles = new Vector3(0f, objectToTransform.transform.eulerAngles.y, 0f);
		}
		originalObjToTransfPos = objectToTransform.transform.position;
		originalObjToTransRot = objectToTransform.transform.rotation;
	}

	public void Stop()
	{
		CharacterController characterController = objectToTransform.GetComponent(typeof(CharacterController)) as CharacterController;
		if (characterController != null)
		{
			characterController.SimpleMove(Vector3.zero);
		}
		isPlaying = false;
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}

	public virtual void UpdateObjectPosition()
	{
		accumTime += Time.deltaTime;
		updateTranslationDir();
		if (isCurrentNodeReached())
		{
			if (currentNode < point.Length - 1)
			{
				currentNode++;
			}
			else if (behaveAtEnd == BehaveAtEnd.Loop)
			{
				currentNode = 1;
			}
		}
		if (currentNode < point.Length)
		{
			CharacterController characterController = objectToTransform.GetComponent(typeof(CharacterController)) as CharacterController;
			if (characterController != null)
			{
				characterController.SimpleMove(_translationDir * speed);
			}
		}
		switch (camLookAt)
		{
		case CameraLook.UsePointRotation:
		{
			float num = 0f;
			if (currentNode > 0)
			{
				num = 1f - Vector3.Distance(objectToTransform.transform.position, point[currentNode].position) / Vector3.Distance(point[currentNode - 1].position, point[currentNode].position);
				if (rotType == TransfInterpType.Absolute)
				{
					objectToTransform.transform.rotation = Quaternion.Slerp(point[currentNode - 1].rotation, point[currentNode].rotation, num);
					break;
				}
				Quaternion quaternion = Quaternion.Slerp(point[currentNode - 1].rotation, point[currentNode].rotation, num);
				objectToTransform.transform.rotation = Quaternion.Euler(originalObjToTransRot.eulerAngles.x + (quaternion.eulerAngles.x - base.gameObject.transform.rotation.eulerAngles.x), originalObjToTransRot.eulerAngles.y + (quaternion.eulerAngles.y - base.gameObject.transform.rotation.eulerAngles.y), originalObjToTransRot.eulerAngles.z + (quaternion.eulerAngles.z - base.gameObject.transform.rotation.eulerAngles.z));
			}
			break;
		}
		case CameraLook.LookAtPath:
			if (moveType == TransfInterpType.Absolute)
			{
				objectToTransform.transform.LookAt(point[currentNode].transform);
			}
			else
			{
				objectToTransform.transform.LookAt(objectToTransform.transform.position + _translationDir);
			}
			break;
		case CameraLook.LookAtObject:
			objectToTransform.transform.LookAt(objectToLook.transform.position);
			break;
		case CameraLook.LookAtEndPoint:
			objectToTransform.transform.LookAt(point[point.Length - 1].transform);
			break;
		}
		if (restrictRotationsToYAxisOnly)
		{
			objectToTransform.transform.eulerAngles = new Vector3(0f, objectToTransform.transform.eulerAngles.y, 0f);
		}
	}

	private void updateTranslationDir()
	{
		if (currentNode < point.Length)
		{
			_translationDir = point[currentNode].position - objectToTransform.transform.position;
			_translationDir.Normalize();
		}
	}

	public Vector3 GetNextWayPointPos()
	{
		if (currentNode < point.Length)
		{
			return point[currentNode].position;
		}
		return point[point.Length - 1].position;
	}
}
