using UnityEngine;

public class DeactivateIfNoGore : MonoBehaviour
{
	private GUI3DObject guiObject;

	private GUI3DText gui3DText;

	private void OnEnable()
	{
		if (guiObject == null)
		{
			guiObject = GetComponent<GUI3DObject>();
		}
		if (guiObject != null)
		{
			if (ConfigParams.DeactivateGore)
			{
				guiObject.CreateOwnMesh = false;
				if (guiObject.GetComponent<Renderer>() != null)
				{
					guiObject.GetComponent<Renderer>().enabled = false;
				}
				if (guiObject is GUI3DInteractiveObject)
				{
					((GUI3DInteractiveObject)guiObject).CheckEvents = false;
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
		if (gui3DText == null)
		{
			gui3DText = GetComponent<GUI3DText>();
		}
		if (gui3DText != null && ConfigParams.DeactivateGore)
		{
			gui3DText.SetDynamicText(string.Empty);
		}
	}
}
