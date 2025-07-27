using System.Collections.Generic;
using UnityEngine;

public class Spline
{
	private Transform[] point;

	private float[] length;

	private int prevCount = -1;

	public float splineSteps = 3f;

	public float lengthTotal;

	private float calc1;

	private float calc2;

	private float calc3;

	private float calc4;

	private float px;

	private float py;

	private float pz;

	public void Update(Transform[] pnts)
	{
		point = pnts;
		if (length == null || length.Length != pnts.Length)
		{
			length = new float[pnts.Length];
		}
		if (point.Length <= 1)
		{
			return;
		}
		if (point.Length != prevCount)
		{
			length[0] = 0f;
			length[1] = 0f;
			length[point.Length - 1] = 0f;
			for (int i = 2; i < point.Length - 1; i++)
			{
				CalculateLengthInSegment(i);
			}
			prevCount = point.Length;
		}
		CalculateLength();
	}

	public void Update(List<GameObject> pnts)
	{
		point = new Transform[pnts.Count];
		for (int i = 0; i < point.Length; i++)
		{
			point[i] = pnts[i].transform;
		}
		if (length == null || length.Length != pnts.Count)
		{
			length = new float[pnts.Count];
		}
		if (point.Length <= 1)
		{
			return;
		}
		if (point.Length != prevCount)
		{
			length[0] = 0f;
			length[1] = 0f;
			length[point.Length - 1] = 0f;
			for (int j = 2; j < point.Length - 1; j++)
			{
				CalculateLengthInSegment(j);
			}
			prevCount = point.Length;
		}
		CalculateLength();
	}

	public float CalculateLength()
	{
		float num = 10f;
		Vector3 vector = GetPoint(2, 0f);
		lengthTotal = 0f;
		for (int i = 2; i < point.Length - 1; i++)
		{
			for (int j = 1; (float)j <= num; j++)
			{
				Vector3 vector2 = GetPoint(i, (float)j / num);
				lengthTotal += (vector2 - vector).magnitude;
				vector = vector2;
			}
		}
		return lengthTotal;
	}

	private float CalculateLengthInSegment(int index)
	{
		if (index < 2)
		{
			return -1f;
		}
		if (index > point.Length - 1)
		{
			return -2f;
		}
		float num = 10f;
		Vector3 vector = GetPoint(index, 0f);
		length[index] = 0f;
		for (int i = 1; (float)i <= num; i++)
		{
			Vector3 vector2 = GetPoint(index, (float)i / num);
			Vector3 vector3 = vector2 - vector;
			length[index] += vector3.magnitude;
			vector = vector2;
		}
		return length[index];
	}

	public Vector3 GetPoint(int i, float t)
	{
		if (point == null || point.Length < 4)
		{
			return Vector3.zero;
		}
		calc1 = (((0f - t + 3f) * t - 3f) * t + 1f) / 6f;
		calc2 = ((3f * t - 6f) * t * t + 4f) / 6f;
		calc3 = (((-3f * t + 3f) * t + 3f) * t + 1f) / 6f;
		calc4 = t * t * t / 6f;
		px = calc1 * point[i - 2].position.x;
		py = calc1 * point[i - 2].position.y;
		pz = calc1 * point[i - 2].position.z;
		px += calc2 * point[i - 1].position.x;
		py += calc2 * point[i - 1].position.y;
		pz += calc2 * point[i - 1].position.z;
		px += calc3 * point[i].position.x;
		py += calc3 * point[i].position.y;
		pz += calc3 * point[i].position.z;
		px += calc4 * point[i + 1].position.x;
		py += calc4 * point[i + 1].position.y;
		pz += calc4 * point[i + 1].position.z;
		return new Vector3(px, py, pz);
	}

	public Vector3 GetPointAtDistance(float distance, out int val)
	{
		int num = 2;
		int num2 = -1;
		bool flag = false;
		float num3 = length[num];
		val = -1;
		if (distance < 0f)
		{
			flag = true;
			val = 0;
			return GetPoint(2, 0f);
		}
		while (!flag)
		{
			if (num >= point.Length - 1)
			{
				flag = true;
			}
			else if (distance > num3)
			{
				num++;
				num3 += length[num];
			}
			else
			{
				flag = true;
				num2 = num;
			}
		}
		if (num2 != -1)
		{
			float num4 = length[num2] - (num3 - distance);
			float t = num4 / length[num2];
			return GetPoint(num2, t);
		}
		val = 1;
		return GetPoint(point.Length - 2, 1f);
	}
}
