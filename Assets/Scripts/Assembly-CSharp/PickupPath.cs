using System.Collections.Generic;
using UnityEngine;

public class PickupPath : MonoBehaviour
{
	public List<Vector3> path;

	public Spline spline;

	private float pickupSepDistance;

	public void AddPickups(int count, bool gold, bool useSplines)
	{
		if (spline == null)
		{
			setSpline();
		}
		GameObject gameObject = ((!gold) ? (Resources.Load("Pickups/SilverSkully", typeof(GameObject)) as GameObject) : (Resources.Load("Pickups/GoldSkully", typeof(GameObject)) as GameObject));
		if (useSplines)
		{
			float num = spline.CalculateLength();
			pickupSepDistance = num / (float)count;
			for (float num2 = pickupSepDistance; num2 <= num; num2 += pickupSepDistance)
			{
				int val;
				Vector3 pointAtDistance = spline.GetPointAtDistance(num2, out val);
				GameObject gameObject2 = Object.Instantiate(gameObject, pointAtDistance + Vector3.up, Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;
				gameObject2.name = gameObject.name;
			}
		}
		else
		{
			float z = path[0].z;
			float z2 = path[path.Count - 1].z;
			float num3 = z2 - z;
			pickupSepDistance = num3 / (float)count;
			for (float num4 = z; num4 < z2; num4 += pickupSepDistance)
			{
				Vector3 pointAtDistance = getPathPoint(num4);
				GameObject gameObject3 = Object.Instantiate(gameObject, new Vector3(pointAtDistance.x, pointAtDistance.y, num4) + Vector3.up, Quaternion.Euler(new Vector3(-90f, 0f, 0f))) as GameObject;
				gameObject3.name = gameObject.name;
			}
		}
		base.transform.parent.GetComponent<PickupPlacer>().UpdateCointCount();
	}

	private Vector3 getPathPoint(float zPos)
	{
		float t;
		for (int i = 1; i < path.Count; i++)
		{
			if (zPos < path[i].z)
			{
				t = zPos - path[i - 1].z / (path[i].z - path[i - 1].z);
				return Vector3.Lerp(path[i - 1], path[i], t);
			}
		}
		t = zPos - path[path.Count - 2].z / (path[path.Count - 1].z - path[path.Count - 2].z);
		return Vector3.Lerp(path[path.Count - 2], path[path.Count - 1], t);
	}

	private void setSpline()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		List<GameObject> list = new List<GameObject>();
		path = new List<Vector3>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].name.StartsWith("Path"))
			{
				list.Add(componentsInChildren[i].gameObject);
				path.Add(componentsInChildren[i].position);
			}
		}
		spline = new Spline();
		spline.Update(list);
	}

	public void ClearPickups()
	{
		if (spline == null)
		{
			setSpline();
		}
		float z = path[0].z;
		float z2 = path[path.Count - 1].z;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Pickup");
		for (int i = 0; i < array.Length; i++)
		{
			if (z <= array[i].transform.position.z && array[i].transform.position.z <= z2)
			{
				Object.DestroyImmediate(array[i]);
			}
		}
	}
}
