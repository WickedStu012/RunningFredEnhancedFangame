using UnityEngine;

public class GetPayLoadLocalTester : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUILayout.Button("Get Payload"))
		{
			Debug.Log(GetPayLoadLocal.GetRes("com.dedalord.runningfred.valuepack1"));
		}
	}
}
