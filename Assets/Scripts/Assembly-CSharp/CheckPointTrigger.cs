using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
	public bool reportYPosition;

	private bool collide;

	private void Start()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	private void OnTriggerEnter(Collider c)
	{
		if (collide || !CharHelper.IsColliderFromPlayer(c) || GameManager.IsFredDead())
		{
			return;
		}
		CheckPointManager.CheckPoint(new Vector3(base.transform.position.x, (!reportYPosition) ? 0f : base.transform.position.y, base.transform.position.z));
		GameEventDispatcher.Dispatch(this, new OnPlayerCheckpoint());
		if (CharHelper.GetProps().Lives > 0)
		{
			SoundManager.PlaySound(48);
			if (GUI3DPopupManager.Instance != null)
			{
				GUI3DPopupManager.Instance.ShowPopup("Checkpoint", "CHECKPOINT!", false);
			}
		}
		collide = true;
	}
}
