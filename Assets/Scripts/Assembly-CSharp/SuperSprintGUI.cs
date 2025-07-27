using UnityEngine;

public class SuperSprintGUI : MonoBehaviour
{
	public GUI3DProgressBar SprintBar;

	private CharProps charProps;

	private void OnEnable()
	{
		if (SprintBar == null)
		{
			SprintBar = GetComponent<GUI3DProgressBar>();
		}
		if (charProps == null && CharHelper.GetProps() != null)
		{
			charProps = CharHelper.GetProps();
			SprintBar.Progress = (int)Mathf.Ceil((float)charProps.SuperSprintsLeft / (float)charProps.MaxSuperSprintsCount * 100f);
			GameEventDispatcher.AddListener("CharChangeState", OnStateChange);
		}
	}

	private void OnDisable()
	{
		charProps = null;
	}

	private void FixedUpdate()
	{
		if (charProps == null && CharHelper.GetProps() != null)
		{
			charProps = CharHelper.GetProps();
			SprintBar.Progress = (int)Mathf.Ceil((float)charProps.SuperSprintsLeft / (float)charProps.MaxSuperSprintsCount * 100f);
			GameEventDispatcher.AddListener("CharChangeState", OnStateChange);
		}
	}

	private void OnStateChange(object sender, GameEvent e)
	{
		CharChangeState charChangeState = (CharChangeState)e;
		if (charChangeState.CurrentState.GetState() == ActionCode.SUPER_SPRINT)
		{
			Debug.Log("Super Sprints left: " + charProps.SuperSprintsLeft);
			SprintBar.Progress = (int)Mathf.Ceil((float)charProps.SuperSprintsLeft / (float)charProps.MaxSuperSprintsCount * 100f);
			Debug.Log("SprintBar.Progress: " + SprintBar.Progress);
		}
	}
}
