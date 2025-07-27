using UnityEngine;

public class FollowFredForTrailer : MonoBehaviour
{
	public GameObject player;

	public float distance = 20f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = player.transform.position - new Vector3(0f, 0f, distance);
	}
}
