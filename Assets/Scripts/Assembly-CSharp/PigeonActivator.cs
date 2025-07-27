using UnityEngine;

public class PigeonActivator : MonoBehaviour
{
	public Pigeon pigeon;

	private void OnBecameVisible()
	{
		pigeon.Think(true);
	}

	private void OnBecameInvisible()
	{
		pigeon.Think(false);
	}
}
