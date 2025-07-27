using UnityEngine;

public class RockTrigger : MonoBehaviour
{
	public Rock[] rocks;

	private bool collide;

	private float accumTime;

	private void OnEnable()
	{
		collide = false;
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 1f)
			{
				collide = false;
				accumTime = 0f;
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c))
		{
			return;
		}
		if (rocks == null || rocks.Length == 0)
		{
			Debug.Log("The rock trigger is not triggering rocks because there aren't reference to rocks specified.");
		}
		else
		{
			for (int i = 0; i < rocks.Length; i++)
			{
				if (rocks[i] != null)
				{
					rocks[i].EnableRigidBody();
				}
			}
		}
		collide = true;
	}
}
