using UnityEngine;

public class CharDebug : MonoBehaviour
{
	private CharStateMachine sm;

	private void Start()
	{
		sm = CharHelper.GetCharStateMachine();
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0f, 0f, Screen.width, 22f), string.Format("state: {0}", sm.GetCurrentState()));
	}
}
