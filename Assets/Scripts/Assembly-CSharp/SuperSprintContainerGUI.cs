using UnityEngine;

public class SuperSprintContainerGUI : MonoBehaviour
{
	public GUI3DProgressBar SprintBar;

	private void OnEnable()
	{
		if (SprintBar == null)
		{
			SprintBar = GetComponent<GUI3DProgressBar>();
		}
	}
}
