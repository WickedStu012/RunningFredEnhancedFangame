using UnityEngine;

public class SmoothFollowPlayer : MonoBehaviour
{
	public float distance = 10f;

	public float height = 5f;

	public float heightDamping = 2f;

	public float rotationDamping = 3f;

	private Transform player;

	private GameObject cameraTarget;

	private Transform cameraTargetT;

	private void Start()
	{
		cameraTarget = new GameObject("CameraTarget");
		cameraTargetT = cameraTarget.transform;
		player = getPlayerTransform();
		updateCameraTargetPos();
	}

	private void updateCameraTargetPos()
	{
		if (player != null)
		{
			cameraTargetT.position = player.transform.position + new Vector3(0f, 0f, 10f);
		}
	}

	private void LateUpdate()
	{
		if (player == null)
		{
			player = getPlayerTransform();
			if (player == null)
			{
				return;
			}
		}
		updateCameraTargetPos();
		float b = 0f;
		float to = cameraTargetT.position.y + height;
		float y = base.transform.eulerAngles.y;
		float y2 = base.transform.position.y;
		y = Mathf.LerpAngle(y, b, rotationDamping * Time.deltaTime);
		y2 = Mathf.Lerp(y2, to, heightDamping * Time.deltaTime);
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		base.transform.position = cameraTargetT.position + new Vector3(0f, 0f, -10f);
		base.transform.position -= quaternion * Vector3.forward * distance;
		base.transform.position = new Vector3(base.transform.position.x, y2, base.transform.position.z);
		base.transform.LookAt(cameraTargetT);
		base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f - MovementHelper.GetRotationAngle());
	}

	private Transform getPlayerTransform()
	{
		GameObject gameObject = GameObject.FindWithTag("Player");
		if (gameObject != null)
		{
			return gameObject.transform;
		}
		return null;
	}
}
