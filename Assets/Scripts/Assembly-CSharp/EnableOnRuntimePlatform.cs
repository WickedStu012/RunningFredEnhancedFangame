using UnityEngine;

public class EnableOnRuntimePlatform : MonoBehaviour
{
	public RuntimePlatform[] Platforms;

	public bool enableOnEditor = true;

	public bool VisibleForKindle;

	private GUI3DObject guiObject;

	private GUI3DText gui3DText;

	private void OnEnable()
	{
		if (guiObject == null)
		{
			guiObject = GetComponent<GUI3DObject>();
		}
		bool flag = false;
		if (ConfigParams.isKindle)
		{
			flag = VisibleForKindle;
		}
		else
		{
			RuntimePlatform[] platforms = Platforms;
			foreach (RuntimePlatform runtimePlatform in platforms)
			{
				if (runtimePlatform == Application.platform || enableOnEditor)
				{
					flag = true;
					break;
				}
			}
		}
		if (guiObject != null)
		{
			if (!flag)
			{
				guiObject.CreateOwnMesh = false;
				if (guiObject.GetComponent<Renderer>() != null)
				{
					guiObject.GetComponent<Renderer>().enabled = false;
				}
			}
			else
			{
				guiObject.CreateOwnMesh = true;
				if (guiObject.GetComponent<Renderer>() != null)
				{
					guiObject.GetComponent<Renderer>().enabled = true;
				}
			}
		}
		else if (!flag)
		{
			Object.Destroy(base.gameObject);
		}
		if (gui3DText == null)
		{
			gui3DText = GetComponent<GUI3DText>();
		}
		if (gui3DText != null && !flag)
		{
			gui3DText.SetDynamicText(string.Empty);
		}
	}
}
