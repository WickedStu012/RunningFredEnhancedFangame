using System;
using UnityEngine;

public class CheatConsole : MonoBehaviour
{
	public Camera cam;

	private Vector2 mousePos1;

	private Vector2 mousePos2;

	private float accel;

	private DateTime time1;

	private CheatConsoleWheel wheel;

	private float moveMouseY;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 vector = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
			Vector3 vector2 = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.farClipPlane));
			Vector3 direction = vector2 - vector;
			direction.Normalize();
			RaycastHit hitInfo;
			if (Physics.Raycast(vector, direction, out hitInfo, float.PositiveInfinity))
			{
				wheel = hitInfo.transform.gameObject.GetComponentInChildren<CheatConsoleWheel>();
				mousePos1.x = Input.mousePosition.x;
				mousePos1.y = Input.mousePosition.y;
				mousePos2.x = Input.mousePosition.x;
				mousePos2.y = Input.mousePosition.y;
				moveMouseY = Input.mousePosition.y;
				time1 = DateTime.Now;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			mousePos2.x = Input.mousePosition.x;
			mousePos2.y = Input.mousePosition.y;
			if (Mathf.Abs(mousePos2.y - mousePos1.y) < 5f)
			{
				accel = 0f;
			}
			else
			{
				mousePos2.x = Input.mousePosition.x;
				mousePos2.y = Input.mousePosition.y;
				Vector2 vector3 = mousePos2 - mousePos1;
				TimeSpan timeSpan = DateTime.Now - time1;
				accel = vector3.magnitude / (float)timeSpan.TotalMilliseconds * 100f;
				if (mousePos2.y < mousePos1.y)
				{
					accel *= -1f;
				}
			}
			if (wheel != null)
			{
				if (accel != 0f)
				{
					wheel.ApplyImpulse(accel);
				}
				wheel.FixAngle();
			}
			wheel = null;
		}
		if (Input.GetMouseButton(0) && wheel != null)
		{
			float ang = (Input.mousePosition.y - moveMouseY) * 0.5f;
			wheel.Move(ang);
			moveMouseY = Input.mousePosition.y;
		}
	}
}
