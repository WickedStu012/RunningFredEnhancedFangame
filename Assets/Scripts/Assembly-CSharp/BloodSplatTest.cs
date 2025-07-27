using UnityEngine;

public class BloodSplatTest : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 200f, 75f), "Create Bloodsplat"))
		{
			BloodSplatManager.Instance.Create(Random.Range(25, 40));
		}
	}
}
