using UnityEngine;

public class Column : MonoBehaviour
{
	private bool collide;

	private GameObject player;

	private void Start()
	{
		collide = false;
		player = CharHelper.GetPlayer();
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.transform.gameObject.layer == 9 || c.transform.gameObject.layer == 13 || collide || !CharHelper.IsColliderFromPlayer(c))
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
			if (Random.Range(1, 10) < 5)
			{
				CharHelper.GetTransformByName("torso2").GetComponent<Rigidbody>().AddForce(new Vector3(-1f, 0f, -1f) * 800f);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.STAGGER);
			}
			else
			{
				CharHelper.GetTransformByName("torso2").GetComponent<Rigidbody>().AddForce(new Vector3(1f, 0f, -1f) * 800f);
				CharHelper.GetCharStateMachine().SwitchTo(ActionCode.STAGGER);
			}
		}
		else
		{
			Debug.LogError("Column cannot find the Player gameObject. Is there a character tagged as player in scene?");
		}
	}
}
