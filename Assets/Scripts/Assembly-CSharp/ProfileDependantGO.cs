using UnityEngine;

public class ProfileDependantGO : MonoBehaviour
{
	public PerformanceScore minPerformance;

	private void Start()
	{
		if (Profile.LessThan(minPerformance))
		{
			base.gameObject.SetActive(false);
		}
	}
}
