using UnityEngine;

public class Gift : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube(base.transform.position, new Vector3(1f, 1f, 1f));
	}
}
