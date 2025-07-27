using UnityEngine;

public class PuncherTrigger : MonoBehaviour
{
	public Puncher[] punchers;

	private bool collide;

	private float accumTime;

	private void Start()
	{
	}

	private void Update()
	{
		if (collide)
		{
			accumTime += Time.deltaTime;
			if (accumTime > 3f)
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
		if (punchers != null)
		{
			for (int i = 0; i < punchers.Length; i++)
			{
				if (punchers[i] != null)
				{
					punchers[i].Trigger();
				}
				else
				{
					Debug.LogError("PuncherTrigger: A puncher element is null. Please, remove nulls from the array.");
				}
			}
		}
		collide = true;
	}
}
