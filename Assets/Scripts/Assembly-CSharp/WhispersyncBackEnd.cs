using System;
using UnityEngine;

public class WhispersyncBackEnd : iBackEnd
{
	private enum LastOp
	{
		READ = 0,
		WRITE = 1
	}

	private bool waitingForGameCircleInitialization;

	private BeLordGameCircle blGameCircle;

	private BackendRes readDataCB;

	private BackendRes writeDataCB;

	private LastOp lastOperation;

	private float accumTime;

	private bool isWaitingForResponse;

	private DateTime waitingForResponseTimeStamp;

	public void OnEnable()
	{
		ClearWaitingForResponse();
		registerEvents();
	}

	public void OnDisable()
	{
		unregisterEvents();
	}

	private void registerEvents()
	{
	}

	private void unregisterEvents()
	{
	}

	public void Update()
	{
	}

	public void ReadData(BackendRes ber)
	{
	}

	public void WriteData(string data, BackendRes ber)
	{
	}

	public void RemoveData(BackendRes ber)
	{
	}

	public bool IsAvailable()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	public bool IsUserAuthenticated()
	{
		if (blGameCircle == null)
		{
			blGameCircle = BeLordGameCircle.GetInstance();
		}
		return blGameCircle.IsPlayerAuthenticated();
	}

	public void SetTestUserName(string un)
	{
	}

	private void readData()
	{
	}

	private void SetWaitingForResponse()
	{
		isWaitingForResponse = true;
		waitingForResponseTimeStamp = DateTime.Now;
	}

	private void ClearWaitingForResponse()
	{
		isWaitingForResponse = false;
	}

	private bool IsWaitingForResponse()
	{
		if (isWaitingForResponse && (DateTime.Now - waitingForResponseTimeStamp).TotalSeconds > 10.0)
		{
			isWaitingForResponse = false;
		}
		return isWaitingForResponse;
	}
}
