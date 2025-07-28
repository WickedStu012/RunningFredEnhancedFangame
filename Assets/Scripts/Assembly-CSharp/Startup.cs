using System;
using UnityEngine;

public class Startup : MonoBehaviour
{
	public bool forWebAllowOnlyNaCl = true;

	public bool DeleteAll;

	public Texture texExpired;

	private bool releaseExpired;

	private bool firstUpdate = true;

	private float time;

	private void Awake()
	{
		Profile.CheckPerformance();
		if (DeleteAll)
		{
			PlayerPrefs.DeleteAll();
		}
		ConfigParams.LoadSettings();
		PlayerAccount.Instance.Initialize();
	}

	private void Start()
	{
		Screen.sleepTimeout = -1;
		Store.Instance.InitBeLordInApp();
		if (ConfigParams.IsKongregate())
		{
			KongregateAPI.ReportScore("integration", 1);
		}
	}

	private void OnGUI()
	{
	}

	private void Update()
	{
		if (releaseExpired || (forWebAllowOnlyNaCl && (Application.platform == RuntimePlatform.WebGLPlayer)))
		{
			return;
		}
		time += Time.deltaTime;
		if (firstUpdate && time >= 0.25f)
		{
			firstUpdate = false;
			Debug.Log("Startup.Update() Synchronizing...");
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				PlayerAccount.Instance.Sync(OnResult);
			}
			else
			{
				OnResult(false);
			}
		}
	}

	private void OnResult(bool res)
	{
		Debug.Log("Finish synchronizing... result:" + res);
		PlayerAccount.Instance.RetrieveMoney();
		int pendingPayment = PlayerPrefsWrapper.GetPendingPayment();
		if (pendingPayment != 0)
		{
			PlayerAccount.Instance.AddMoney(pendingPayment);
			PlayerPrefsWrapper.ClearPendings();
		}
		PlayerAccount.Instance.CheckDayEarnings();
		DedalordLoadLevel.LoadLevel(Levels.MainMenu);
	}

	private void checkExpired()
	{
		DateTime dateTime = new DateTime(2014, 7, 1);
		releaseExpired = (DateTime.Now - dateTime).TotalDays > 150.0;
	}

	private void OnApplicationQuit()
	{
		// Cleanup any remaining UnityWebRequest objects to prevent memory leaks
	}
}
