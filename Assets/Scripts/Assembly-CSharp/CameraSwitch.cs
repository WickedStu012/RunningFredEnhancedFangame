using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
	public GameObject disableCam;

	public GameObject enableCam;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider col)
	{
		enableCam.SetActive(true);
		disableCam.GetComponent<Camera>().enabled = false;
	}
}
