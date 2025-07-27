using UnityEngine;

public class Billboard : MonoBehaviour
{
	public bool RotateJustY;

	private void Update()
	{
		Transform playerTransform = CharHelper.GetPlayerTransform();
		if (playerTransform != null)
		{
			if (RotateJustY)
			{
				Vector3 vector = Camera.main.transform.position - base.transform.position;
				vector.x = (vector.z = 0f);
				base.transform.LookAt(playerTransform.position - vector);
			}
			else
			{
				base.transform.LookAt(playerTransform.position);
			}
		}
	}
}
