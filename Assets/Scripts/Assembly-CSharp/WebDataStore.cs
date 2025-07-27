using System;
using UnityEngine;

public class WebDataStore : MonoBehaviour
{
	public BackEndType EditorBackEndType;

	public BackEndType iOSBackEndType;

	public BackEndType AndroidBackEndType;

	public BackEndType KindleBackEndType;

	public BackEndType WebBackEndType;

	public BackEndType NaClBackEndType;

	public BackEndType MacBackEndType;

	public BackEndType WinBackEndType;

	public string editorTestUserName = "johndoe";

	private static iBackEnd be;

	private static DateTime writeTimestamp;

	private static int bytesWritten;

	private OnNewDataArrivedFromBackend onNewDataArrived;

	private void Start()
	{
		if (be == null)
		{
			createBackEnd();
		}
		writeTimestamp = DateTime.MinValue;
		bytesWritten = 0;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		if (be == null)
		{
			createBackEnd();
		}
		if (be != null)
		{
			be.OnEnable();
		}
	}

	private void OnDisable()
	{
		if (be != null)
		{
			be.OnDisable();
		}
	}

	private void Update()
	{
		if (be != null)
		{
			be.Update();
		}
	}

	private void createBackEnd()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			be = BackEndFactory.Create(EditorBackEndType);
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			be = BackEndFactory.Create(iOSBackEndType);
		}
		else if (Application.platform == RuntimePlatform.Android && !ConfigParams.isKindle)
		{
			be = BackEndFactory.Create(AndroidBackEndType);
		}
		else if (Application.platform == RuntimePlatform.Android && ConfigParams.isKindle)
		{
			be = BackEndFactory.Create(KindleBackEndType);
		}
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			be = BackEndFactory.Create(WebBackEndType);
		}
		else if (Application.platform == RuntimePlatform.NaCl)
		{
			be = BackEndFactory.Create(NaClBackEndType);
		}
		else if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			be = BackEndFactory.Create(MacBackEndType);
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			be = BackEndFactory.Create(WinBackEndType);
		}
		else
		{
			be = null;
		}
		if (be != null)
		{
			be.SetTestUserName(editorTestUserName);
			if (be is ICloudBackEnd)
			{
				(be as ICloudBackEnd).SetNewDataArrivedFromBackendCallback(onNewDataArrivedFromBackend);
			}
		}
	}

	public static void ReadData(BackendRes ber)
	{
		if (be == null)
		{
			if (ber != null)
			{
				ber(false, null);
			}
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			be.ReadData(ber);
		}
		else if (ber != null)
		{
			ber(false, "No network connection");
		}
	}

	public static void WriteData(string data, BackendRes ber)
	{
		if (be == null)
		{
			if (ber != null)
			{
				ber(false, null);
			}
		}
		else if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			bytesWritten += data.Length;
			be.WriteData(data, ber);
		}
		else if (ber != null)
		{
			ber(false, "No network connection");
		}
	}

	private static void WriteData(string data)
	{
		if (be != null && Application.internetReachability != NetworkReachability.NotReachable)
		{
			bytesWritten += data.Length;
			be.WriteData(data, null);
		}
	}

	public static void WriteData(string data, bool force)
	{
		if (be != null)
		{
			if (force)
			{
				WriteData(data);
				writeTimestamp = DateTime.Now;
			}
			else if ((DateTime.Now - writeTimestamp).TotalSeconds > 120.0)
			{
				WriteData(data);
				writeTimestamp = DateTime.Now;
			}
		}
	}

	public static bool IsAvailable()
	{
		if (be == null)
		{
			return false;
		}
		return be.IsAvailable();
	}

	public static bool IsUserAuthenticated()
	{
		if (be == null)
		{
			return false;
		}
		return be.IsUserAuthenticated();
	}

	public static void RemoveData(BackendRes ber)
	{
		if (be == null)
		{
			if (ber != null)
			{
				ber(false, null);
			}
		}
		else
		{
			be.RemoveData(ber);
		}
	}

	public void SetNewDataArrivedFromBackendCallback(OnNewDataArrivedFromBackend cb)
	{
		onNewDataArrived = cb;
	}

	private void onNewDataArrivedFromBackend()
	{
		if (onNewDataArrived != null)
		{
			onNewDataArrived();
		}
	}

	public bool IsType(BackEndType bet)
	{
		if (be != null)
		{
			switch (bet)
			{
			case BackEndType.ICLOUD:
				return be is ICloudBackEnd;
			case BackEndType.GOOGLE_APP_ENGINE:
				return be is GAEBackend;
			}
		}
		return false;
	}
}
