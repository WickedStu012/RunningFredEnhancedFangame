using UnityEngine;

public class ActivateByDevices : MonoBehaviour
{
	public RuntimePlatform[] Platforms;

	private void Awake()
	{
		bool flag = false;
		RuntimePlatform[] platforms = Platforms;
		foreach (RuntimePlatform runtimePlatform in platforms)
		{
			if (runtimePlatform == Application.platform)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Object.DestroyImmediate(base.gameObject);
		}
	}
}
