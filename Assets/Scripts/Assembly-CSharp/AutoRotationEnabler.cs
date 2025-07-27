using UnityEngine;

public class AutoRotationEnabler : MonoBehaviour
{
	private void Start()
	{
		OrientationChanger.SetFreeze(false);
	}
}
