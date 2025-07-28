using UnityEngine;

public class SurvivalSkulliesDebug : MonoBehaviour
{
	public GUI3DText DebugText;
	
	private void Start()
	{
		if (DebugText == null)
		{
			DebugText = GetComponent<GUI3DText>();
		}
	}

	private void Update()
	{
		if (PlayerAccount.Instance != null && DebugText != null)
		{
			int currentMoney = PlayerAccount.Instance.RetrieveMoney();
			string debugInfo = $"Skullies: {currentMoney}\nMode: {PlayerAccount.Instance.CurrentGameMode}";
			DebugText.SetDynamicText(debugInfo);
		}
	}
} 