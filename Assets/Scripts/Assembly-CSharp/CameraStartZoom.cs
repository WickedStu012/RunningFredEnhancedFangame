using UnityEngine;

public class CameraStartZoom : MonoBehaviour
{
	public GameObject cam;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider col)
	{
		LookAtPlayerForTrailer component = cam.GetComponent<LookAtPlayerForTrailer>();
		component.StartZoom();
	}
}
