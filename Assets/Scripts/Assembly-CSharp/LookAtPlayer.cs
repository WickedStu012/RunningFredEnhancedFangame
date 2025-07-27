using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
	private GameObject goPlayer;

	private void Start()
	{
		goPlayer = GameObject.FindWithTag("Player");
	}

	private void Update()
	{
		base.GetComponent<Camera>().transform.LookAt(goPlayer.transform);
	}
}
