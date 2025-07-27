using UnityEngine;

public class CharOnJointBreak : MonoBehaviour
{
	private void OnJointBreak(float breakForce)
	{
		if (ConfigParams.useGore)
		{
			string part = base.name.ToLower();
			CharHelper.GetCharSkin().ApplyDamageTo(part);
		}
	}
}
