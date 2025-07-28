using UnityEngine;

public class SurvivalSkulliesGUI : MonoBehaviour
{
	public GUI3DText SkulliesText;
	
	private int lastSkulliesCount = -1;

	private void Awake()
	{
		if (SkulliesText == null)
		{
			SkulliesText = GetComponent<GUI3DText>();
		}
	}

	private void OnEnable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.MoneyChangeEvent += OnSkulliesChange;
			UpdateSkulliesDisplay();
		}
	}

	private void OnDisable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.MoneyChangeEvent -= OnSkulliesChange;
		}
	}

	private void Update()
	{
		// Only update in survival mode
		if (PlayerAccount.Instance != null && PlayerAccount.Instance.CurrentGameMode == PlayerAccount.GameMode.Survival)
		{
			UpdateSkulliesDisplay();
		}
	}

	private void OnSkulliesChange(int skullies)
	{
		UpdateSkulliesDisplay();
	}

	private void UpdateSkulliesDisplay()
	{
		if (PlayerAccount.Instance != null && SkulliesText != null)
		{
			int currentSkullies = PlayerAccount.Instance.RetrieveMoney();
			
			// Only update if the count has changed
			if (currentSkullies != lastSkulliesCount)
			{
				lastSkulliesCount = currentSkullies;
				SkulliesText.SetDynamicText(StringUtil.FormatNumbers(currentSkullies));
			}
		}
	}
} 