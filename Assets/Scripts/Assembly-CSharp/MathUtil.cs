using System.Collections;
using UnityEngine;

public class MathUtil
{
	public static float GetAverage(float[] arr, int arrMaxNum)
	{
		float num = 0f;
		for (int i = 0; i < arrMaxNum; i++)
		{
			num += arr[i];
		}
		return num / (float)arrMaxNum;
	}

	public static bool IsInsideValues(float min, float max, float numberToCheck)
	{
		if (numberToCheck < min)
		{
			return false;
		}
		if (numberToCheck > max)
		{
			return false;
		}
		return true;
	}

	public static float EaseInOutQuad(float t, float b, float c, float d)
	{
		t /= d / 2f;
		if (t < 1f)
		{
			return c / 2f * t * t + b;
		}
		t -= 1f;
		return (0f - c) / 2f * (t * (t - 2f) - 1f) + b;
	}

	public static void ApplyEase(float start, float end, float duration, ref Queue nextPos)
	{
		float num = 0f;
		nextPos.Clear();
		while (num < duration)
		{
			num += Time.deltaTime;
			nextPos.Enqueue(EaseInOutQuad(num, start, end, duration));
		}
	}

	public static void Swap(ref float n1, ref float n2)
	{
		float num = n2;
		n2 = n1;
		n1 = num;
	}

	public static bool InsideDistance(Vector3 a, Vector3 b, float distance)
	{
		return (b - a).magnitude <= distance;
	}
}
