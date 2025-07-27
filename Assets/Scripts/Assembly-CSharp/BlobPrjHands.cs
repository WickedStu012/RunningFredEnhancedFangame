using UnityEngine;

public class BlobPrjHands : MonoBehaviour
{
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
	}
}
