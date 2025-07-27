using UnityEngine;

public class Carltrop : MonoBehaviour
{
	private bool collide;

	private GameObject player;

	private CharStateMachine sm;

	private void Start()
	{
		collide = false;
		player = CharHelper.GetPlayer();
		if (player != null)
		{
			sm = player.GetComponent<CharStateMachine>();
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		collide = true;
		if (player == null)
		{
			player = CharHelper.GetPlayer();
		}
		if (player != null)
		{
			if (sm == null)
			{
				sm = player.GetComponent<CharStateMachine>();
			}
			sm.SwitchTo(ActionCode.CARLTROP);
		}
	}
}
