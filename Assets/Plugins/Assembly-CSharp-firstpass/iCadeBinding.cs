using System.Runtime.InteropServices;
using UnityEngine;

public class iCadeBinding
{
	public static iCadeState state = default(iCadeState);

	[DllImport("__Internal")]
	private static extern void _iCadeSetActive(bool active);

	public static void setActive(bool active)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_iCadeSetActive(active);
		}
	}

	[DllImport("__Internal")]
	private static extern void iCadeUpdateState(ref iCadeState state);

	public static void updateState()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			iCadeUpdateState(ref state);
		}
	}
}
