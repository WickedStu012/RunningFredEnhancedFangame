using System;
using UnityEngine;

public class DeviceGroup
{
	public static long GetGroupNum()
	{
		long num = 0L;
		try
		{
			num = Convert.ToInt64(SystemInfo.deviceUniqueIdentifier, 10);
		}
		catch
		{
		}
		return num % 4;
	}
}
