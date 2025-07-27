using UnityEngine;

public class CheckParamsTabs : MonoBehaviour
{
	private void Start()
	{
		if (!SceneParamsManager.Instance.IsEmpty)
		{
			string tabname = (string)SceneParamsManager.Instance.Pop();
			GUI3DTabControl component = GetComponent<GUI3DTabControl>();
			if (component != null)
			{
				component.SwitchToTab(tabname);
			}
			else
			{
				Debug.Log("Can't find TabControl");
			}
		}
	}
}
