using UnityEngine;

public class iCadeGUIManager : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float width = ((Screen.width < 960 && Screen.height < 960) ? 200 : 400);
		float num2 = ((Screen.width < 960 && Screen.height < 960) ? 40 : 80);
		float num3 = num2 + 10f;
		if (GUI.Button(new Rect(left, num, width, num2), "Activate"))
		{
			iCadeBinding.setActive(true);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Deactivate"))
		{
			iCadeBinding.setActive(false);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "Update/Print Button State"))
		{
			iCadeBinding.updateState();
		}
	}
}
