using UnityEngine;

public class TriggerWall : MonoBehaviour
{
	private GameObject player;

	private void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "PlayerBone")
		{
			if (player == null)
			{
				player = getRoot(c.gameObject);
			}
			CharStateMachine component = player.GetComponent<CharStateMachine>();
			component.MoveDirection = new Vector3(0f - component.MoveDirection.x, component.MoveDirection.y * 1.1f, component.MoveDirection.z);
		}
	}

	private GameObject getRoot(GameObject goBone)
	{
		Transform parent = goBone.transform.parent;
		while (parent.tag != "Player" && parent.tag != null)
		{
			parent = parent.transform.parent;
		}
		if (parent != null)
		{
			return parent.gameObject;
		}
		return null;
	}
}
