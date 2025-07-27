using UnityEngine;

public class MoveUpIfGoreDisabled : MonoBehaviour
{
	public float Ammount = 110f;

	private void Awake()
	{
		if (ConfigParams.DeactivateGore)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.y += Ammount;
			base.transform.localPosition = localPosition;
		}
	}
}
