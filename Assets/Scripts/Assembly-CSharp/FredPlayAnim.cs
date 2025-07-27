using UnityEngine;

public class FredPlayAnim : MonoBehaviour
{
	private void Awake()
	{
		if (base.GetComponent<Animation>() != null)
		{
			base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().Play("Sprint2");
		}
	}
}
