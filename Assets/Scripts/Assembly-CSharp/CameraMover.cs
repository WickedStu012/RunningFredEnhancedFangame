using UnityEngine;

public class CameraMover : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = new Vector3(base.gameObject.transform.position.x, base.transform.position.y, base.transform.position.z);
	}
}
