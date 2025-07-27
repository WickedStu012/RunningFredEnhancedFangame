using UnityEngine;

public class Moon : MonoBehaviour
{
	public Transform cameraT;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = new Vector3(base.transform.position.x, cameraT.position.y + 15f, cameraT.position.z + 400f);
		base.transform.LookAt(cameraT.position);
	}
}
