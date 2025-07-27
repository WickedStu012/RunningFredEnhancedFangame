using UnityEngine;

public class EnableOnPlatform : MonoBehaviour
{
	public bool HighPerformance = true;

	private GUI3DObject guiObject;

	private void OnEnable()
	{
		if (guiObject == null)
		{
			guiObject = GetComponent<GUI3DObject>();
		}
		if (!(guiObject != null))
		{
			return;
		}
		if ((HighPerformance && Profile.GreaterThan(PerformanceScore.AVERAGE)) || (!HighPerformance && Profile.LessOrEqualTo(PerformanceScore.AVERAGE)))
		{
			guiObject.CreateOwnMesh = true;
			if (guiObject.GetComponent<Renderer>() != null)
			{
				guiObject.GetComponent<Renderer>().enabled = true;
			}
		}
		else
		{
			guiObject.CreateOwnMesh = false;
			if (guiObject.GetComponent<Renderer>() != null)
			{
				guiObject.GetComponent<Renderer>().enabled = false;
			}
		}
	}
}
