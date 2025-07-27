using UnityEngine;

public class EnableOnRuntimePlatformRecur : MonoBehaviour
{
	public RuntimePlatform[] Platforms;

	public bool enableOnEditor = true;

	public bool VisibleForKindle;

	private void OnEnable()
	{
		if (enableOnEditor && Application.isEditor)
		{
			base.gameObject.SetActive(true);
			return;
		}
		bool active = false;
		if (ConfigParams.isKindle)
		{
			active = VisibleForKindle;
		}
		else
		{
			RuntimePlatform[] platforms = Platforms;
			foreach (RuntimePlatform runtimePlatform in platforms)
			{
				if (runtimePlatform == Application.platform || enableOnEditor)
				{
					active = true;
					break;
				}
			}
		}
		base.gameObject.SetActive(active);
	}
}
