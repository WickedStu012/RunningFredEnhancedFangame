using UnityEngine;

[RequireComponent(typeof(GameAchievements))]
public class BeLordInitializer : MonoBehaviour
{
	public BeLordBackend backendToUseIPhone;

	public BeLordBackend backendToUseAndroid;

	public BeLordBackend backendToUseKindle;

	public BeLordBackend backendToUseMac;

	public BeLordBackend backendToUseEditor;

	private BeLordBackend backendToUse;

	private bool isEnabled;

	private static BeLordInitializer instance;

	public static BeLordInitializer Instance
	{
		get
		{
			return instance;
		}
	}

	private void OnEnable()
	{
		instance = this;
		if (Application.isEditor)
		{
			backendToUse = backendToUseEditor;
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			backendToUse = backendToUseIPhone;
		}
		else if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			backendToUse = backendToUseMac;
		}
		else if (Application.platform == RuntimePlatform.Android && !ConfigParams.isKindle)
		{
			backendToUse = backendToUseAndroid;
		}
		else if (Application.platform == RuntimePlatform.Android && ConfigParams.isKindle)
		{
			backendToUse = backendToUseKindle;
		}
		else
		{
			backendToUse = BeLordBackend.NONE;
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
		isEnabled = (BeLord.Enable = ConfigParams.useLeaderboardAndAchievements);
	}

	private void OnDestroy()
	{
		if (BeLordInApp.Instance != null)
		{
			BeLordInApp.Instance.UnInit();
		}
	}

	private void Update()
	{
		if (isEnabled != BeLord.Enable)
		{
			if (BeLord.Enable)
			{
				Initialize();
			}
			isEnabled = BeLord.Enable;
		}
		BeLord.Update();
	}

	public void Initialize()
	{
		if (BeLord.Enable && !BeLord.IsLoggedIn() && backendToUse != BeLordBackend.NONE)
		{
			BeLord.Login(backendToUse, GetComponent<GameInfo>(), GetComponent<GameAchievements>(), GetComponent<GameLeaderboards>());
		}
	}
}
