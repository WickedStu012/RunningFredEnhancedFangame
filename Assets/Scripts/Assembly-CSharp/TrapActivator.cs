using UnityEngine;

internal class TrapActivator : MonoBehaviour
{
	public GameObject[] gosToEnable;

	public GameObject[] gosToEnableRec;

	public void EnableTrap()
	{
		base.gameObject.SetActive(true);
		if (gosToEnable != null)
		{
			for (int i = 0; i < gosToEnable.Length; i++)
			{
				if (gosToEnable[i] != null)
				{
					gosToEnable[i].SetActive(true);
				}
			}
		}
		if (gosToEnableRec == null)
		{
			return;
		}
		for (int j = 0; j < gosToEnableRec.Length; j++)
		{
			if (gosToEnableRec[j] != null)
			{
				gosToEnableRec[j].SetActive(true);
			}
		}
	}
}
