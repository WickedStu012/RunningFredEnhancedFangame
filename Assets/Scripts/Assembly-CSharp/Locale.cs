using UnityEngine;

public class Locale
{
	public static void _getDecimalSeparator()
	{
	}

	public static void GetDecimalSeparator()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_getDecimalSeparator();
		}
	}
}
