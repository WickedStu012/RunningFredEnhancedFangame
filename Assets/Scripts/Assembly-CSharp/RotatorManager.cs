using UnityEngine;

public class RotatorManager : MonoBehaviour
{
	public static Quaternion angle;

	public int speedRotation = 100;

	private Transform transf;

	private void Start()
	{
		transf = base.transform;
	}

	private void Update()
	{
		transf.Rotate(Vector3.down, Time.deltaTime * (float)speedRotation);
		angle = transf.rotation;
	}
}
