using System;
using UnityEngine;

public class IconAnimation : MonoBehaviour
{
	public string[] picture;

	public float[] time;

	public GUI3DObject icon;

	private double accumTime;

	private int curFrame;

	private bool updateAnim = true;

	private DateTime lastTime;

	private void OnEnable()
	{
		updateAnim = string.Compare(icon.TextureName, "Help-AfterBurner-1", true) == 0;
		lastTime = DateTime.Now;
	}

	private void Update()
	{
		if (!updateAnim)
		{
			return;
		}
		accumTime += (DateTime.Now - lastTime).TotalSeconds;
		lastTime = DateTime.Now;
		if (accumTime >= (double)time[curFrame])
		{
			accumTime %= time[curFrame];
			curFrame++;
			if (curFrame == picture.Length)
			{
				curFrame = 0;
			}
			icon.ObjectSize = Vector2.zero;
			icon.RefreshMaterial(picture[curFrame]);
		}
	}
}
