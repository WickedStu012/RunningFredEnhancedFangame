using UnityEngine;

public class CameraSetOrthoSize : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Camera>().orthographicSize = Screen.height / 4;
	}
}
