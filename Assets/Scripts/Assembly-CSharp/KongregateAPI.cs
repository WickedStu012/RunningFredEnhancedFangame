using System.Collections;
using UnityEngine;

public class KongregateAPI : MonoBehaviour
{
	private static KongregateAPI instance;

	private bool initialized;

	private int userId;

	private string userName = "Guest";

	private string gameAuthToken = string.Empty;

	private PurchaseRes_s_kongregate purchaseResult;

	private string marketIdInProcess;

	private bool isGuest;

	private float accumTime;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		instance = this;
		if (Application.isEditor)
		{
			Application.LoadLevel("Startup");
		}
		else
		{
			Application.ExternalEval("if (typeof(kongregateUnitySupport) != 'undefined') { kongregateUnitySupport.initAPI('KongregateAPI', '_onKongregateAPILoaded');};");
		}
		initialized = true;
	}

	private void Update()
	{
		if (isGuest)
		{
			accumTime += Time.deltaTime;
			if (accumTime >= 1f)
			{
				checkIsGuest();
				accumTime = 0f;
			}
		}
	}

	public void _onKongregateAPILoaded(string userInfo)
	{
		string[] array = userInfo.Split('|');
		userId = int.Parse(array[0]);
		userName = array[1];
		gameAuthToken = array[2];
		isGuest = string.Compare(userName, "Guest", true) == 0;
		if (!isGuest)
		{
			Application.LoadLevel("Startup");
		}
	}

	public void _onPurchaseResult(int result)
	{
		Debug.Log(string.Format("KongregateAPI._onPurchaseResult: {0}", result));
		if (purchaseResult != null)
		{
			purchaseResult(result == 1, marketIdInProcess);
		}
	}

	public void _setIsGuest(int isG)
	{
		isGuest = isG == 1;
		if (!isGuest)
		{
			Application.LoadLevel("Startup");
		}
	}

	private void onAuthenticate(bool res, Hashtable ht)
	{
		if (res)
		{
			Application.LoadLevel("Startup");
		}
		else
		{
			Debug.Log("Cannot load. Authenticate fails");
		}
	}

	public static bool IsInitialized()
	{
		return instance.initialized;
	}

	public static void OpenOfferWall()
	{
		Application.ExternalEval("kongregate.mtx.showKredPurchaseDialog(\"offers\");");
	}

	public static void PurchaseOption(string marketId, PurchaseRes_s_kongregate purchaseRes)
	{
		Debug.Log(string.Format("Kongregate Purchase MarketId: {0}", marketId));
		string text = null;
		if (string.Compare(marketId, "com.dedalord.runningfred.sk1") == 0 || string.Compare(marketId, "sk1") == 0)
		{
			text = "sk1";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.sk2") == 0 || string.Compare(marketId, "sk2") == 0)
		{
			text = "sk2";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.sk3") == 0 || string.Compare(marketId, "sk3") == 0)
		{
			text = "sk3";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.sk4") == 0 || string.Compare(marketId, "sk4") == 0)
		{
			text = "sk4";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.sk6") == 0 || string.Compare(marketId, "sk6") == 0)
		{
			text = "sk6";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.sk8") == 0 || string.Compare(marketId, "sk8") == 0)
		{
			text = "sk8";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.sk7") == 0 || string.Compare(marketId, "sk7") == 0)
		{
			text = "sk7";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.valuepack1") == 0 || string.Compare(marketId, "valuepack1") == 0)
		{
			text = "valuepack1";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.valuepack2") == 0 || string.Compare(marketId, "valuepack2") == 0)
		{
			text = "valuepack2";
		}
		else if (string.Compare(marketId, "com.dedalord.runningfred.valuepack3") == 0 || string.Compare(marketId, "valuepack3") == 0)
		{
			text = "valuepack3";
		}
		if (text != null)
		{
			instance.purchaseResult = purchaseRes;
			instance.marketIdInProcess = marketId;
			Application.ExternalEval("var items=[{identifier:\"" + text + "\"}];kongregate.mtx.purchaseItems(items, onPurchaseResult);function onPurchaseResult(result) { if (result.success) {\tkongregateUnitySupport.getUnityObject().SendMessage('KongregateAPI', '_onPurchaseResult', 1);}  else { kongregateUnitySupport.getUnityObject().SendMessage('KongregateAPI', '_onPurchaseResult', 0); }};");
		}
	}

	public static void ReportScore(string lbName, int score)
	{
		if (ConfigParams.IsKongregate())
		{
			Application.ExternalEval("kongregate.stats.submit('" + lbName + "', " + score + ");");
		}
	}

	public static void UnlockAchievement(string achName)
	{
		if (ConfigParams.IsKongregate())
		{
			Application.ExternalEval("kongregate.stats.submit('" + achName + "', 1);");
		}
	}

	public static string GetUserName()
	{
		if (instance != null)
		{
			return instance.userName;
		}
		return string.Empty;
	}

	private void checkIsGuest()
	{
		Application.ExternalEval("var isguest = kongregate.services.isGuest();if (isguest) { kongregateUnitySupport.getUnityObject().SendMessage(\"KongregateAPI\", \"_setIsGuest\", 1); }else { kongregateUnitySupport.getUnityObject().SendMessage(\"KongregateAPI\", \"_setIsGuest\", 0); }");
	}

	public static bool IsGuest()
	{
		return instance.isGuest;
	}

	public static void OpenRegisterDashboard()
	{
		Application.ExternalEval("kongregate.services.showRegistrationBox();");
	}

	public static int GetUserId()
	{
		return instance.userId;
	}

	public static string GetAuthToken()
	{
		return instance.gameAuthToken;
	}

	public static void ReportBadgeHistoricalMoney(int historicalMoney)
	{
		Application.ExternalEval("kongregate.stats.submit('skullies_historical_count', " + historicalMoney + ");");
	}
}
