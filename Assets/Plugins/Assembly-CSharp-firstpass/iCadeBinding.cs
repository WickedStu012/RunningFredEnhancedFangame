using System.Runtime.InteropServices;
using UnityEngine;

public class iCadeBinding
{
	public static iCadeState state = default(iCadeState);

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void _iCadeSetActive(bool active);
#endif

	public static void setActive(bool active)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_iCadeSetActive(active);
		}
#endif
	}

#if UNITY_IOS && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void iCadeUpdateState(ref iCadeState state);
#endif

	public static void updateState()
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			iCadeUpdateState(ref state);
		}
#endif
	}
}
