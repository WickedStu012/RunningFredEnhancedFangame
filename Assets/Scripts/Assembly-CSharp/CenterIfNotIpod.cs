using UnityEngine;

public class CenterIfNotIpod : MonoBehaviour
{
	private void Awake()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = 0f;
			base.transform.localPosition = localPosition;
		}
	}
}
