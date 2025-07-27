using System;
using UnityEngine;

internal class ShineObj
{
	public DateTime dt;

	public GameObject shine;

	public ShineObj(DateTime d, GameObject go)
	{
		dt = d;
		shine = go;
	}
}
