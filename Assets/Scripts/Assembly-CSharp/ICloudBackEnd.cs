using UnityEngine;

public class ICloudBackEnd : iBackEnd
{
	private const float NEXT_UPDATE_INTERVAL = 1f;

	private static string UbiquityIdentityToken;

	private OnNewDataArrivedFromBackend onNewDataArrived;

	private static bool isiCloudKeyEmpty;

	private static bool DebugMode;

	private static float nextUpdateTimeSet;

	private static int updateCount;

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void Update()
	{
	}

	public void ReadData(BackendRes ber)
	{
		Debug.Log("iCloud is not supported in this platform.");
	}

	public void WriteData(string data, BackendRes ber)
	{
		Debug.Log("iCloud is not supported in this platform.");
	}

	public bool IsAvailable()
	{
		return false;
	}

	private string GetUbiquityIdentityToken()
	{
		return UbiquityIdentityToken;
	}

	public void RemoveData(BackendRes ber)
	{
		Debug.Log("iCloud is not supported in this platform.");
	}

	public bool IsUserAuthenticated()
	{
		return false;
	}

	public void SetTestUserName(string un)
	{
	}

	public void SetNewDataArrivedFromBackendCallback(OnNewDataArrivedFromBackend cb)
	{
		if (DebugMode)
		{
			Debug.Log("------> iCloud. SetNewDataArrivedFromBackendCallback");
		}
		onNewDataArrived = cb;
	}
}
