using UnityEngine;

public class LevelEnd : MonoBehaviour
{
	public Vector3 translationOffset;

	public float stopCameraAtDistance = 32f;

	protected bool goalReached;

	protected Transform playerT;

	protected void Awake()
	{
		goalReached = false;
		playerT = CharHelper.GetPlayerTransform();
	}

	protected void Start()
	{
	}

	protected void Update()
	{
		if (playerT == null)
		{
			playerT = CharHelper.GetPlayerTransform();
		}
	}

	protected virtual void PlayerReachGoal()
	{
		CharHelper.GetCharStateMachine().SwitchTo(ActionCode.RUNNING_TO_GOAL);
		ActRunningToGoal actRunningToGoal = CharHelper.GetCharStateMachine().GetCurrentAction() as ActRunningToGoal;
		if (actRunningToGoal != null)
		{
			actRunningToGoal.SetTargetPoint(new Vector3(0f, base.transform.position.y, base.transform.position.z - translationOffset.z));
		}
	}

	protected void EnderFinished()
	{
		GameEventDispatcher.Dispatch(this, new EndDoorOpened());
	}

	private void OnTriggerEnter(Collider c)
	{
		if (CharHelper.IsColliderFromPlayer(c) && !GameManager.IsFredDead() && !goalReached)
		{
			GUI3DPopupManager.Instance.CloseCurrentPopup(GUI3DPopupManager.PopupResult.Cancel, true);
			PlayerReachGoal();
			goalReached = true;
			GameEventDispatcher.Dispatch(this, new PlayerReachGoal());
		}
	}
}
