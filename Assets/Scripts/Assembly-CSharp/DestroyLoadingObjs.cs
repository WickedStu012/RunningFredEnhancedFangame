using UnityEngine;

public class DestroyLoadingObjs : MonoBehaviour
{
	private void Start()
	{
		GameObject gameObject = GameObject.FindWithTag("LoadingObjs");
		if (gameObject != null)
		{
			Object.Destroy(gameObject);
			Object.Destroy(this);
		}
	}
}
