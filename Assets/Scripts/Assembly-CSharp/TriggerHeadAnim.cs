using UnityEngine;

public class TriggerHeadAnim : MonoBehaviour
{
	public GameObject head;

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Play Head Anim"))
		{
			head.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			head.GetComponent<Animation>().Play("Dying");
		}
	}
}
