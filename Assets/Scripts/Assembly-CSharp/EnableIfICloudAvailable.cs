using UnityEngine;

public class EnableIfICloudAvailable : MonoBehaviour
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
			if (!WebDataStore.IsAvailable())
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
		if (gui3DText == null)
		{
			gui3DText = GetComponent<GUI3DText>();
		}
		if (gui3DText != null && !WebDataStore.IsAvailable())
		{
			gui3DText.SetDynamicText(string.Empty);
		}
	}
}
