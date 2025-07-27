using UnityEngine;

public class AutoRotationDisabler : MonoBehaviour
{
	private void Start()
	{
		OrientationChanger.SetFreeze(true);
	}
}
