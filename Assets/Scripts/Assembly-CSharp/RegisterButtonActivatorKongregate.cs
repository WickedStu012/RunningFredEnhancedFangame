using UnityEngine;

public class RegisterButtonActivatorKongregate : MonoBehaviour
{
	public GUI3D gui3D;

	private bool guiEnabled;

	private void Update()
	{
		if (!guiEnabled)
		{
			checkUserIsGuest();
		}
	}

	private void checkUserIsGuest()
	{
		if (KongregateAPI.IsGuest())
		{
			guiEnabled = true;
			gui3D.Visible = true;
			gui3D.CheckEvents = true;
			gui3D.gameObject.SetActive(true);
		}
	}
}
