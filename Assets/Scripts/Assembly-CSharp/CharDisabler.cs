using UnityEngine;

public class CharDisabler : MonoBehaviour
{
	public GameObject[] goToDisable;

	private void Awake()
	{
		for (int i = 0; i < goToDisable.Length; i++)
		{
			goToDisable[i].SetActive(false);
		}
	}
}
