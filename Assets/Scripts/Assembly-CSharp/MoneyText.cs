using UnityEngine;

public class MoneyText : MonoBehaviour
{
	public GUI3DText Text;

	private void Awake()
	{
		if (Text == null)
		{
			Text = GetComponent<GUI3DText>();
		}
		if (PlayerAccount.Instance != null)
		{
			Text.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
		}
	}

	private void OnEnable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.MoneyChangeEvent += OnMoneyChange;
			Text.SetDynamicText(StringUtil.FormatNumbers(PlayerAccount.Instance.RetrieveMoney()));
		}
	}

	private void OnDisable()
	{
		if (PlayerAccount.Instance != null)
		{
			PlayerAccount.Instance.MoneyChangeEvent -= OnMoneyChange;
		}
	}

	private void OnMoneyChange(int money)
	{
		Text.SetDynamicText(StringUtil.FormatNumbers(money));
	}
}
