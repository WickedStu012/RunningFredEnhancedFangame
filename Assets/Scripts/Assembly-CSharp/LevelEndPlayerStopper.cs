using UnityEngine;

public class LevelEndPlayerStopper : MonoBehaviour
{
	private bool stopperReached;

	private void Awake()
	{
		stopperReached = false;
	}

	private void OnTriggerEnter(Collider col)
	{
		if (!stopperReached && CharHelper.IsColliderFromPlayer(col))
		{
			GameEventDispatcher.Dispatch(this, new PlayerReachStopper());
			CharHelper.DisablePlayer();
			stopperReached = true;
		}
	}
}
