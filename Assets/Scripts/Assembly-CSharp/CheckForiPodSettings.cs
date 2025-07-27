using UnityEngine;

public class CheckForiPodSettings : MonoBehaviour
{
	public GameObject[] iPodObjects;

	private void OnEnable()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer && iPodObjects != null)
		{
			GameObject[] array = iPodObjects;
			foreach (GameObject obj in array)
			{
				Object.DestroyImmediate(obj, true);
			}
			iPodObjects = null;
		}
	}
}
