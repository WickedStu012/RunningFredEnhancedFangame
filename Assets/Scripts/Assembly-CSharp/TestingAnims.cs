using UnityEngine;

public class TestingAnims : MonoBehaviour
{
	public GameObject fred;

	private void Start()
	{
		fred.GetComponent<Animation>().Play("Sprint");
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Sprint"))
		{
			fred.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			fred.GetComponent<Animation>().Play("Sprint");
		}
		if (!GUILayout.Button("Enable Ragdoll"))
		{
		}
	}

	private void Update()
	{
	}
}
