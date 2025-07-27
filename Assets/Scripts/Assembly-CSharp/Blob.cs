using UnityEngine;

public class Blob : MonoBehaviour
{
	public float offsetY = 0.1f;

	public float offsetZ = -0.13f;

	private Vector3 offset;

	private Transform playerT;

	private void Start()
	{
		offset = new Vector3(0f, offsetY, offsetZ);
	}

	private void Update()
	{
		if (playerT == null)
		{
			playerT = CharHelper.GetPlayerTransform();
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(playerT.position + offset, Vector3.down, out hitInfo, 100f, 4207104))
		{
			base.transform.position = hitInfo.point + offset;
			float num = Vector3.Distance(playerT.position, base.transform.position);
			if (num < 1f)
			{
				base.transform.localScale = new Vector3(2f, 1f, 2f);
			}
			else if (1f <= num && num <= 6f)
			{
				float num2 = (5f - (num - 1f)) / 5f;
				base.transform.localScale = new Vector3(2f * num2, num2, 2f * num2);
			}
			else
			{
				base.transform.localScale = Vector3.zero;
			}
			float x = Mathf.Atan(hitInfo.normal.z) * 57.29578f;
			float num3 = Mathf.Atan(1f / hitInfo.normal.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(new Vector3(x, 0f, (!(num3 > 80f) && !(num3 < -80f)) ? (num3 * -1f) : 0f));
			base.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = false;
		}
	}
}
