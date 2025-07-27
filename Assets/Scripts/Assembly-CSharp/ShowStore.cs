using UnityEngine;

public class ShowStore : MonoBehaviour
{
	public GameObject store;

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 200f, 200f), "Store"))
		{
			store.SetActive(true);
		}
	}
}
