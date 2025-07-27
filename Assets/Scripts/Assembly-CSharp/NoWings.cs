using UnityEngine;

public class NoWings : MonoBehaviour
{
	public GameObject editorPlane;

	private float zPos;

	private bool wingsWereRemoved;

	private void Start()
	{
		zPos = base.transform.position.z;
		if (editorPlane != null)
		{
			Object.Destroy(editorPlane);
		}
	}

	private void Update()
	{
		if (wingsWereRemoved)
		{
			return;
		}
		Transform playerTransform = CharHelper.GetPlayerTransform();
		if (playerTransform != null && playerTransform.position.z >= zPos)
		{
			wingsWereRemoved = true;
			if (WingsLeftTime.Instance != null)
			{
				WingsLeftTime.Instance.StartWingsOutTimer(RemoveWings);
			}
			else
			{
				RemoveWings();
			}
		}
	}

	private void RemoveWings()
	{
		CharHelper.GetCharStateMachine().RemoveWings(true, true);
		CharHelper.GetCharStateMachine().ShowJetpack();
	}
}
