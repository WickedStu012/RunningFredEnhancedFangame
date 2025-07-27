using UnityEngine;

public class Plank : MonoBehaviour
{
	public GameObject normal;

	public GameObject broken;

	private bool collide;

	private void Start()
	{
		normal.SetActive(true);
		broken.SetActive(false);
		collide = false;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!collide && CharHelper.IsColliderFromPlayer(c))
		{
			if (CharHelper.GetCharStateMachine().GetCurrentState() != ActionCode.DIVE)
			{
				base.gameObject.GetComponent<Collider>().isTrigger = false;
			}
			else
			{
				normal.SetActive(false);
				broken.SetActive(true);
				SoundManager.PlaySound(7);
			}
			collide = true;
		}
	}
}
