using UnityEngine;

public class BarrelTrigger : MonoBehaviour
{
	public enum TriggerType
	{
		MOVEMENT = 0,
		EXPLOSION = 1
	}

	public Barrel[] barrels;

	public TriggerType triggerType;

	private bool collide;

	private float accumTime;

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= 3f)
			{
				accumTime = 0f;
				collide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (barrels == null || barrels.Length == 0)
		{
			Debug.Log("The barrel trigger is not triggering barrels because there aren't reference to barrels specified.");
		}
		else
		{
			for (int i = 0; i < barrels.Length; i++)
			{
				if (barrels[i] != null)
				{
					if (triggerType == TriggerType.MOVEMENT)
					{
						barrels[i].EnableRigidBody();
					}
					else
					{
						barrels[i].Explode();
					}
				}
			}
		}
		collide = true;
	}
}
