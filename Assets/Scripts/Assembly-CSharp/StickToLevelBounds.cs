using UnityEngine;

[ExecuteInEditMode]
public class StickToLevelBounds : MonoBehaviour
{
	public bool stickToFloor = true;

	public bool stickToWall;

	public float offsetX;

	public float offsetY;

	public Vector3 offsetAngle;

	public float rayOrigin = 100f;

	public bool useFloorNormal;

	private void Start()
	{
		if (Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			Snap();
		}
	}

	public void Snap()
	{
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
		RaycastHit hitInfo;
		if (stickToFloor && Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + rayOrigin, base.transform.position.z), Vector3.down, out hitInfo, float.PositiveInfinity, 8704))
		{
			base.transform.position = new Vector3(base.transform.position.x, hitInfo.point.y + offsetY, base.transform.position.z);
			if (useFloorNormal)
			{
				base.transform.localRotation = Quaternion.Euler(-90f + hitInfo.normal.z * 57.29578f + offsetAngle.x, offsetAngle.y, offsetAngle.z);
			}
		}
		if (stickToWall)
		{
			if (base.transform.position.x < 0f)
			{
				if (Physics.Raycast(new Vector3(rayOrigin, base.transform.position.y, base.transform.position.z), Vector3.right * -1f, out hitInfo, float.PositiveInfinity, 1024))
				{
					base.transform.position = new Vector3(hitInfo.point.x + offsetX, base.transform.position.y, base.transform.position.z);
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x + offsetAngle.x, 180f + offsetAngle.y, base.transform.rotation.eulerAngles.z + offsetAngle.z);
				}
			}
			else if (Physics.Raycast(new Vector3(0f - rayOrigin, base.transform.position.y, base.transform.position.z), Vector3.right, out hitInfo, float.PositiveInfinity, 2048))
			{
				base.transform.position = new Vector3(hitInfo.point.x - offsetX, base.transform.position.y, base.transform.position.z);
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x + offsetAngle.x, offsetAngle.y, base.transform.rotation.eulerAngles.z + offsetAngle.z);
			}
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = true;
		}
	}
}
